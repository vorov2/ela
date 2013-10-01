using System;
using System.Collections.Generic;
using System.Text;
using Ela.CodeModel;

namespace Ela.Compilation
{
    //This part is responsible for generating errors, messages and hints.
    internal sealed partial class Builder
    {
        //Shown hints (not to show the same hint twice).
        private Dictionary<ElaCompilerHint,ElaCompilerHint> shownHints = new Dictionary<ElaCompilerHint,ElaCompilerHint>();

        //A list of all messages, including errors, warnings and hints.
        private FastList<ElaMessage> _errors = new FastList<ElaMessage>();
        internal FastList<ElaMessage> Errors
        {
            get { return _errors; }
        }
        
        private void AddError(ElaCompilerError error, ElaExpression exp, params object[] args)
        {
            AddError(error, exp.Line, exp.Column, args);
        }
        
        private void AddError(ElaCompilerError error, int line, int col, params object[] args)
        {
            Errors.Add(new ElaMessage(Strings.GetError(error, args), MessageType.Error,
                (Int32)error, line, col));
            Success = false;

            //This is an ad-hoc error limit
            if (Errors.Count >= 101)
            {
                //We generate a 'Too many errors' message and terminate compilation.
                Errors.Add(new ElaMessage(Strings.GetError(ElaCompilerError.TooManyErrors), MessageType.Error,
                    (Int32)ElaCompilerError.TooManyErrors, line, col));
                throw new TerminationException();
            }
        }

        private void AddWarning(ElaCompilerWarning warning, ElaExpression exp, params object[] args)
        {
            AddWarning(warning, exp.Line, exp.Column, args);
        }

        //Warnings can be ignored or generated as errors
        private void AddWarning(ElaCompilerWarning warning, int line, int col, params object[] args)
        {
            if (options.WarningsAsErrors)
                AddError((ElaCompilerError)warning, line, col, args);
            else if (!options.NoWarnings)
                Errors.Add(new ElaMessage(Strings.GetWarning(warning, args), MessageType.Warning,
                    (Int32)warning, line, col));
        }

        private void AddHint(ElaCompilerHint hint, ElaExpression exp, params object[] args)
        {
            AddHint(hint, exp.Line, exp.Column, args);
        }

        //Hints can be ignored, also a single hint is shown just once
        private void AddHint(ElaCompilerHint hint, int line, int col, params object[] args)
        {
            //Only show the same hint once
            if (options.ShowHints && !options.NoWarnings && !shownHints.ContainsKey(hint))
            {
                Errors.Add(new ElaMessage(Strings.GetHint(hint, args), MessageType.Hint,
                    (Int32)hint, line, col));
                shownHints.Add(hint, hint);
            }
        }
        
        //This method generate a warning and hint and when a value is not used and 
        //pops this value from the top of the stack.
        private void AddValueNotUsed(ElaExpression exp)
        {
            AddWarning(ElaCompilerWarning.ValueNotUsed, exp, FormatNode(exp));
            AddHint(ElaCompilerHint.UseIgnoreToPop, exp, FormatNode(exp, true));
            cw.Emit(Op.Pop);
        }

        //The same as FormatNode(ElaExpression,bool) but always formats AST not in non-compact manner.
        private string FormatNode(ElaExpression exp)
        {
            return FormatNode(exp, false);
        }

        //Used to format an AST node to incorporate its textual representation into an error message.
        //Parameter 'compact' is used to specify whether line brakes and parens should be added around output.
        private string FormatNode(ElaExpression exp, bool compact)
        {
            var str = exp.ToString();
            str = str.Length > 100 ? str.Substring(0, 100) + " ..." : str;

            if (!compact && (str.IndexOf('\n') != -1 || str.Length > 40))
                str = "\r\n    " + str + "\r\n";
            else if (!compact && str.Length > 0 && str[0] != '(' && str[0] != '"' && str[0] != '\'' && str[0] != '[')
                str = "(" + str + ")";

            return str;
        }
    }
}

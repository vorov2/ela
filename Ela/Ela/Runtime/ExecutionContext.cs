using System;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime
{
	public sealed class ExecutionContext
	{
		#region Construction
        public ExecutionContext()
		{
            
		}
		#endregion


		#region Methods
        public void NoOverload(string expected, string got, string function)
        {
            Fail(ElaRuntimeError.NoOverload, function, expected, got);
        }

        public void NoOverload(string got, string function)
        {
            NoOverload("?", got, function);
        }

        public void NoContext(string function)
        {
            Fail(ElaRuntimeError.NoContext, function);
        }

		public void InvalidFormat(string format, ElaValue value)
		{
			Fail(ElaRuntimeError.InvalidFormat, format, value.ToString(), value.GetTypeName());
		}


		public void DivideByZero(ElaValue value)
		{
			Fail(ElaRuntimeError.DivideByZero, value.ToString(), value.GetTypeName());
		}


		public void InvalidIndexType(ElaValue index)
		{
			Fail(ElaRuntimeError.InvalidIndexType, index.GetTypeName());
		}

        public void NotAlgebraicType(ElaValue val)
        {
            Fail(ElaRuntimeError.NotAlgebraicType, val.GetTypeName());
        }
        

		public void IndexOutOfRange(ElaValue index, ElaValue obj)
		{
			Fail(ElaRuntimeError.IndexOutOfRange, index.ToString(), index.GetTypeName(),
				obj.ToString(), obj.GetTypeName());
		}

        public void InvalidTypeCode(ElaValue code)
        {
            Fail(ElaRuntimeError.InvalidTypeCode, code.I4);
        }

        public void InvalidConstructorCode(ElaValue code)
        {
            Fail(ElaRuntimeError.InvalidConstructorCode, code.I4);
        }


		public void InvalidType(string expected, ElaValue given)
		{
			Fail(ElaRuntimeError.InvalidType, expected, given.GetTypeName());
        }

        public void UnknownField(string field, ElaValue given)
        {
            Fail(ElaRuntimeError.UnknownField, field, given.ToString(), given.GetTypeName());
        }


		public void NoOperator(ElaValue value, string op)
		{
			Fail(ElaRuntimeError.InvalidOp, value.GetTypeName(), op);
		}


		public void Fail(ElaRuntimeError error, params object[] args)
		{
			Fail(new ElaError(error, args));
		}


		public void Fail(string message)
		{
			Fail(new ElaError(message));
		}


		internal void Fail(ElaError err)
		{
			if (!Failed)
			{
				Failed = true;
				Error = err;
			}
		}


		internal void Reset()
		{
			Failed = false;
			Error = null;
		}


        public void SetDeffered(ElaFunction fun, int args)
        {
            Failed = true;
            Fun = fun;
            DefferedArgs = args;
        }
		#endregion


		#region Properties
		internal bool Failed { get; set; }

		internal ElaError Error { get; set; }

        internal int DefferedArgs;

		internal ElaLazy Thunk;

        internal ElaFunction Fun;

        internal string Tag;
		#endregion
	}
}

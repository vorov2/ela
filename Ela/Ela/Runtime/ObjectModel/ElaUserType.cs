using System;
using System.Collections.Generic;
using System.Text;
using Ela.Linking;

namespace Ela.Runtime.ObjectModel
{
    public abstract class ElaUserType : ElaObject
    {
        protected readonly string typeName;
        protected readonly int tag;

        protected ElaUserType(string typeName, int typeCode, int tag) : base((ElaTypeCode)typeCode)
        {
            this.typeName = typeName;
            this.tag = tag;
        }

        protected internal override string GetTypeName()
        {
            return typeName;
        }

        internal override int GetTag()
        {
            return tag;
        }
    }

    internal sealed class ElaUserType0 : ElaUserType
    {
        internal ElaUserType0(string typeName, int typeCode, int tag)
            : base(typeName, typeCode, tag)
        {
            
        }

        internal override ElaValue Untag(CodeAssembly asm, ExecutionContext ctx, int index)
        {
            ctx.Fail(new ElaError(ElaRuntimeError.InvalidTypeArgument, asm.Constructors[tag].Name, typeName, index + 1));
            return Default();
        }
    }

    internal sealed class ElaUserType1 : ElaUserType
    {
        internal ElaUserType1(string typeName, int typeCode, int tag, ElaValue value) : base(typeName, typeCode, tag)
        {
            Value = value;
        }

        internal override ElaValue Untag(CodeAssembly asm, ExecutionContext ctx, int index)
        {
            if (index != 0)
            {
                ctx.Fail(new ElaError(ElaRuntimeError.InvalidTypeArgument, asm.Constructors[tag].Name, typeName, index + 1));
                return Default();
            }

            return Value;
        }

        internal ElaValue Value { get; set; }
    }

    internal sealed class ElaUserType2 : ElaUserType
    {
        internal ElaUserType2(string typeName, int typeCode, int tag, ElaValue value1, ElaValue value2) : base(typeName, typeCode, tag)
        {
            Value1 = value1;
            Value2 = value2;
        }

        internal override ElaValue Untag(CodeAssembly asm, ExecutionContext ctx, int index)
        {
            if (index == 0)
                return Value1;

            if (index == 1)
                return Value2;
            
            ctx.Fail(new ElaError(ElaRuntimeError.InvalidTypeArgument, asm.Constructors[tag].Name, typeName, index + 1));
            return Default();
        }

        internal ElaValue Value1 { get; set; }
        internal ElaValue Value2 { get; set; }
    }

    internal sealed class ElaUserTypeVariadic : ElaUserType
    {
        internal ElaUserTypeVariadic(string typeName, int typeCode, int tag, ElaValue[] values)
            : base(typeName, typeCode, tag)
        {
            Values = values;
        }

        internal override ElaValue Untag(CodeAssembly asm, ExecutionContext ctx, int index)
        {
            if (index >= Values.Length)
            {
                ctx.Fail(new ElaError(ElaRuntimeError.InvalidTypeArgument, asm.Constructors[tag].Name, typeName, index + 1));
                return Default();
            }

            return Values[index];
        }

        internal ElaValue[] Values { get; set; }
    }
}

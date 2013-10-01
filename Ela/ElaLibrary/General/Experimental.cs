using System;
using Ela.Linking;
using Ela.Runtime.ObjectModel;
using Ela.Runtime;
using System.Diagnostics;

namespace Ela.Library.General
{
    public sealed class Experimental : ForeignModule
    {
        #region Construction
        public Experimental()
        {

        }

        public class SubFun : ElaFunction
        {
            public SubFun()
                : base(2)
            {
                
            }
        }
        #endregion


        #region Methods
        public override void Initialize()
        {
            Add<ElaValue,String,ElaObject>("newtype", NewType2);
        }

        public ElaObject NewType2(ElaValue val, string name)
        {
            return new CustomType(val, name);
        }
        #endregion
    }


    public sealed class CustomType : ElaObject
    {
        private string name;

        public CustomType(ElaValue val, string name)
        {
            Value = val;
            this.name = name;
        }

        protected override string GetTypeName()
        {
            return name;
        }

        public ElaValue Value { get; private set; }
    }
}

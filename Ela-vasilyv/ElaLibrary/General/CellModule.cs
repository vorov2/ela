using System;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.General
{
    public sealed class CellModule : ForeignModule
    {
        private sealed class ElaCell : ElaObject
        {
            public ElaCell()
            {

            }

            internal ElaValue Value { get; set; }
        }

        public CellModule()
        {

        }
        
        public override void Initialize()
        {
            Add<ElaValue,ElaObject,ElaObject>("mutate", Mutate);
            Add<ElaValue,ElaObject>("newCell", NewCell);
            Add<ElaObject,ElaValue>("valueof", ValueOf);
        }

        public ElaObject Mutate(ElaValue value, ElaObject obj)
        {
            ((ElaCell)obj).Value = value;
            return obj;
        }

        public ElaObject NewCell(ElaValue value)
        {
            return new ElaCell { Value = value };
        }

        public ElaValue ValueOf(ElaObject value)
        {
            return ((ElaCell)value).Value;
        }
    }
}

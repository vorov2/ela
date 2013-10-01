using System;
using System.Collections.Generic;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.Collections
{
    public sealed class QueueModule : ForeignModule
    {
        #region Construction
        public QueueModule()
        {

        }
        #endregion


        #region Methods
        public override void Initialize()
        {
            Add("empty", ElaQueue.Empty);
            Add<IEnumerable<ElaValue>,ElaQueue>("queue", CreateQueue);
            Add<ElaQueue,ElaValue>("peek", Peek);
            Add<ElaQueue,ElaQueue>("dequeue", Dequeue);
            Add<ElaValue,ElaQueue,ElaQueue>("enqueue",Enqueue);
            Add<ElaQueue,ElaList>("toList", ToList);
        }


        public ElaQueue CreateQueue(IEnumerable<ElaValue> seq)
        {
            return new ElaQueue(seq);
        }


        public ElaValue Peek(ElaQueue queue)
        {
            return queue.Peek();
        }


        public ElaQueue Dequeue(ElaQueue queue)
        {
            return queue.Dequeue();
        }


        public ElaQueue Enqueue(ElaValue value, ElaQueue queue)
        {
            return queue.Enqueue(value);
        }


        public ElaList ToList(ElaQueue queue)
        {
            return queue.ToList();
        }
        #endregion
    }
}
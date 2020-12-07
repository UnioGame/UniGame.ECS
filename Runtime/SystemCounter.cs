namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using System.Threading;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    [Serializable]
    public class SystemCounter : IPoolable
    {
        public int id;
        public int counter;

        public SystemCounter Initialize(int counterId)
        {
            id      = counterId;
            counter = 0;
            return this;
        }

        public int Decrease()
        {
            Interlocked.Decrement(ref counter);
            return counter;
        }

        public int Increase()
        {
            Interlocked.Increment(ref counter);
            return counter;
        }

        public void Release()
        {
            id      = 0;
            counter = 0;
        }
        
        public override int GetHashCode() => id;

        public override bool Equals(object obj)
        {
            if (obj is SystemCounter systemCounter)
                return systemCounter.id == id;
            return base.Equals(obj);
        }
    }
}
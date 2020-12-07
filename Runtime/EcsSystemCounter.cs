namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using System.Threading;
    using Abctract;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UnityEngine;

    [Serializable]
    public class EcsSystemCounter : IPoolable
    {
        public int            counter;
        
        [SerializeReference]
        public IUniEcsSystem ecsSystem;
        
        public EcsSystemCounter Initialize(IUniEcsSystem systems)
        {
            counter    = 1;
            ecsSystem = systems;
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
            counter = 0;
            ecsSystem = null;
        }
        
    }
}
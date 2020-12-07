using System;
using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.ECS.Runtime.Abctract;

namespace UniModules.UniGame.ECS.Runtime
{
    [Serializable]
    public abstract class UniEcsWorld<TWorld> : IUniEcsWorld<TWorld>
    {
        private TWorld             _world;
        private LifeTimeDefinition _lifeTime;

        public int Id => World == null ? 0 : World.GetHashCode();

        public TWorld World => GetWorld(); 

        public ILifeTime LifeTime => _lifeTime = _lifeTime ?? new LifeTimeDefinition();

        public void Dispose()
        {
            DestroyWorld();
            _lifeTime?.Terminate();
        }

        private TWorld GetWorld()
        {
            if (_world != null)
                return _world;
            
            _world = CreateConcreteWorld();
            
            //register current world into system
            this.RegisterWorld();
            
            return _world;
        }

        protected abstract void DestroyWorld();

        protected abstract TWorld CreateConcreteWorld();
    }
}
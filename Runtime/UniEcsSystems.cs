using UniModules.UniCore.Runtime.DataFlow;
using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.ECS.Runtime.Abctract;

namespace UniModules.UniGame.ECS.Runtime
{
    using System;
    using UniRx;

    public abstract class UniEcsSystems<TWorld, TSystemGroup, TSystem> :
        IUniEcsSystems<TWorld, TSystemGroup, TSystem>
        where TSystemGroup : class, IUniEcsSystems<TWorld, TSystemGroup, TSystem>
        where TSystem : IUniEcsSystem
    {
        #region private data

        protected IUniEcsWorld<TWorld>             _world;
        protected ReactiveCollection<TSystem>      _ecsSystems      = new ReactiveCollection<TSystem>();
        protected ReactiveCollection<TSystemGroup> _ecsGroupSystems = new ReactiveCollection<TSystemGroup>();

        private LifeTimeDefinition _lifeTime  = new LifeTimeDefinition();
        private bool               _isStarted = false;
        private string             _name      = string.Empty;

        #endregion

        #region constrictor

        public TSystemGroup Initialize(IUniEcsWorld<TWorld> world, string name = null)
        {
            Reset();

            _world = world;
            _name  = string.IsNullOrEmpty(name) ? String.Empty : name;

            OnInitialize();

            return ThisSystem;
        }

        #endregion

        #region public properties

        public IUniEcsWorld<TWorld> World => _world;

        public TSystemGroup ThisSystem => this as TSystemGroup;

        public IReadOnlyReactiveCollection<TSystem> EcsSystems => _ecsSystems;

        public IReadOnlyReactiveCollection<TSystemGroup> EcsGroupSystems => _ecsGroupSystems;

        public ILifeTime LifeTime => _lifeTime;

        public virtual string Name => _name;

        public bool IsStarted => _isStarted;

        #endregion

        public void SetName(string name) => _name = name;

        public TSystemGroup Execute()
        {
            if (_isStarted) return ThisSystem;

            foreach (var ecsSystem in EcsSystems)
            {
                ecsSystem.Execute();
            }

            foreach (var systemGroup in _ecsGroupSystems)
            {
                systemGroup.Execute();
            }

            OnExecute();

            return ThisSystem;
        }

        public TSystemGroup Add(TSystemGroup systems)
        {
            if (_ecsGroupSystems.Contains(systems)) return ThisSystem;
            _ecsGroupSystems.Add(systems);
            return ThisSystem;
        }

        public TSystemGroup Add(TSystem system)
        {
            if (_ecsSystems.Contains(system)) return ThisSystem;
            _ecsSystems.Add(system);
            return ThisSystem;
        }

        public void Dispose() => Reset();

        protected virtual void OnExecute()
        {
        }

        protected virtual void OnInitialize()
        {
            
        }

        private void Reset()
        {
            _world = null;
            _lifeTime.Release();
            
            foreach (var ecsSystem in EcsSystems)
            {
                ecsSystem.Dispose();
            }

            foreach (var systemGroup in EcsGroupSystems)
            {
                systemGroup.Dispose();
            }
            
            _ecsGroupSystems.Clear();
            _ecsSystems.Clear();

        }
    }
}
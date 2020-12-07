using UnityEngine;

namespace UniModules.UniGame.ECS.Runtime.Leo
{
    using System;
    using Abstract;
    using Leopotam.Ecs;

    [Serializable]
    public class LeoEcsSystem : UniEcsSystem, ILeoEcsSystem
    {
        #region inspector

        [SerializeReference] public IEcsSystem ecsSystem;

        [SerializeField] public bool isRunSystem = false;

        [SerializeField] public string systemName;

        [SerializeField] public bool isSharedSystem = true;

        #endregion

        #region private data

        private EcsSystems _ecsSystems;

        #endregion

        public EcsSystems EcsSystems => _ecsSystems;

        public bool IsShared => isSharedSystem;

        public sealed override int Id => EcsSystem?.GetType().GetHashCode() ?? 0;

        public sealed override bool IsActive => _ecsSystems != null;

        public IEcsSystem EcsSystem => ecsSystem;

        public bool IsRunSystem => isRunSystem;

        public string SystemName => systemName;

        public ILeoEcsSystem Activate(EcsSystems ecsSystems)
        {
            if (IsActive)
            {
                Debug.LogError("Try to Active already active LEO ECS System ");
                return this;
            }
            _ecsSystems = ecsSystems;
            
            return this;
        }
        
        public ILeoEcsSystem Inject<TData>(TData value)
        {
            if (IsActive) return this;
            _ecsSystems.Inject(value, typeof(TData));
            return this;
        }

        protected override void OnStart()
        {
            _ecsSystems.ProcessInjects();
            _ecsSystems.Init();
        }

        protected override void OnDestroy()
        {
            _ecsSystems.Destroy();
            _ecsSystems = null;
        }
    }
}
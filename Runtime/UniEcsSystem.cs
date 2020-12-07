using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
using UniModules.UniGame.ECS.Runtime.Abctract;

namespace UniModules.UniGame.ECS.Runtime
{
    using System;
    using global::UniCore.Runtime.ProfilerTools;
    using UniCore.Runtime.DataFlow;

    [Serializable]
    public abstract class UniEcsSystem : IUniEcsSystem
    {
        #region private data

        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();
        private bool               _isActive = false;
        private int                _worldId;

        #endregion

        // Start is called before the first frame update
        public abstract int Id { get; }

        public int WorldId { get; private set; }

        public virtual bool IsActive => _isActive;

        public bool Execute()
        {
            if (_isActive)
                return false;

            _isActive = true;

            OnStart();
            
            return true;
        }

        public ILifeTime LifeTime => _lifeTime = _lifeTime ?? new LifeTimeDefinition();


        public bool Initialize(int worldId)
        {
            WorldId = worldId;
            OnAwake();
            return true;
        }

        public void Dispose()
        {
            if (!EcsWorldsMap.RemoveSystem(WorldId, Id))
            {
                return;
            }

            GameLog.Log($"ECS: Destroy system ID {Id} {GetType().Name}");
            
            _isActive = false;
            _worldId  = 0;
            _lifeTime?.Release();
            
            OnDestroy();
        }

        protected virtual void OnAwake() { }
        
        protected abstract void OnStart();

        protected abstract void OnDestroy();
    }
}
namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.DataFlow.Interfaces;
    using Leo.Abstract;
    using Leopotam.Ecs;
    using UniCore.Runtime.DataFlow;
    using UnityEngine;

    [Serializable]
    public class LeoEcsSystemGroup : ILeoEcsSystemGroup
    {
        #region inspector

        public string groupName;

        [SerializeReference]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.ListDrawerSettings(ShowPaging = false)]
#endif
        public List<ILeoEcsSystem> ecsSystems = new List<ILeoEcsSystem>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.ListDrawerSettings(ShowPaging = false)]
#endif
        public List<LeoEcsSystemGroup> subGroups = new List<LeoEcsSystemGroup>();

        #endregion

        #region private fields

        private EcsSystems         _ecsSystems;
        private ILeoEcsWorld       _world;
        private bool               _isSystemsCreated = false;
        private LifeTimeDefinition _lifeTime;
        private bool               _isNestedGroup = false;

        #endregion

        public int Id => this.GetHashCode();
        
        public string GroupName => groupName;

        public EcsSystems LeoEcsSystems => _ecsSystems;

        public ILeoEcsWorld World => _world;

        public EcsWorld EcsWorld => _world.World;

        public IReadOnlyList<ILeoEcsSystem> EcsSystems => ecsSystems;

        public IReadOnlyList<ILeoEcsSystemGroup> SubGroups => subGroups;

        public ILifeTime LifeTime => (_lifeTime = _lifeTime ?? new LifeTimeDefinition());

        public ILeoEcsSystemGroup RegisterGroup(ILeoEcsWorld world, bool isNestedGroup)
        {
            if (_ecsSystems != null)
                return this;
            
            _isNestedGroup    = isNestedGroup;
            _isSystemsCreated = true;
            _world            = world;
            
            //get current group systems
            _ecsSystems       = world.AddSystems(this);
            //register subgroups
            world.RegisterSystemsGroups(this,subGroups);
            
            return this;
        }

        public ILeoEcsSystemGroup Inject<T>(T value)
        {
            _ecsSystems?.Inject(value);
            return this;
        }

        public void Init()
        {
            _ecsSystems.ProcessInjects();
            _ecsSystems.Init();
        }

        public bool Validate() => true;

        public void Dispose()
        {
            _lifeTime?.Release();
            _ecsSystems?.Destroy();
            _isSystemsCreated = false;
            _ecsSystems       = null;
            _isNestedGroup           = false;
        }
    }
}
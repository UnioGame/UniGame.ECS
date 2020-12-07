using UnityEngine;

namespace UniModules.UniGame.ECS.Runtime.Leo
{
    using System;
    using Abstract;
    using Leopotam.Ecs;

    [Serializable]
    public class LeoEcsSystem :  ILeoEcsSystem
    {
        #region inspector

        [SerializeReference]
        public IEcsSystem ecsSystem;

        [SerializeField]
        public bool isRunSystem = false;

        [SerializeField]
        public string systemName; 
        
        #endregion

        public int Id => EcsSystem.GetType().GetHashCode();

        public IEcsSystem EcsSystem => ecsSystem;

        public bool IsRunSystem => isRunSystem;

        public string SystemName => systemName;
    }
}

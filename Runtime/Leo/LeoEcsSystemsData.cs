namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using System.Collections.Generic;
    using Leopotam.Ecs;
    using UnityEngine;

    [Serializable]
    public class LeoEcsSystemsData : ILeoEcsSystemsData
    {
        #region inspector

        public string groupName;

        [SerializeReference]
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.ListDrawerSettings(ShowPaging = false)]
#endif
        public List<IEcsSystem> ecsSystems = new List<IEcsSystem>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.ListDrawerSettings(ShowPaging = false)]
#endif
        public List<LeoEcsSystemsData> subGroups = new List<LeoEcsSystemsData>();

        #endregion

        public string GroupName => groupName;

        public IReadOnlyList<IEcsSystem> EcsSystems => ecsSystems;

        public IReadOnlyList<ILeoEcsSystemsData> Groups => subGroups;

        public bool Validate() => true;
    }
}
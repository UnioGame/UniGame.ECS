namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using System.Collections.Generic;
    using ECS.Runtime.Leo;
    using Leopotam.Ecs;
    using UnityEngine;

    [Serializable]
    public class EcsSystemsData : IEcsSystemsData
    {
        [SerializeReference]
        public List<IEcsSystem> systems = new List<IEcsSystem>();

        
        public IReadOnlyList<IEcsSystem> Systems => systems;

    }
}

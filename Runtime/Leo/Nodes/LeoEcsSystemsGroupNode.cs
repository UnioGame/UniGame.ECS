namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.Interfaces;
    using Cysharp.Threading.Tasks;
    using Leopotam.Ecs;
    using UniGameFlow.Nodes.Runtime.States;
    using UniRx;
    using UnityEngine;

    [Serializable]
    public class LeoEcsSystemsGroupNode : AsyncStateNode
    {
        #region inspector
        
        [SerializeReference]
        public List<LeoEcsSystemGroup> ecsSystems = new List<LeoEcsSystemGroup>();

        public string systemsName = string.Empty;

        #endregion
        
        #region async state method

        public sealed override async UniTask<AsyncStatus> ExecuteStateAsync(IContext value)
        {
            return AsyncStatus.Succeeded;
            var world   = await value.Receive<EcsWorld>().First();
            var systems = new EcsSystems(world,systemsName);
            systems.Add(systems, systemsName);
            return AsyncStatus.Succeeded;
        }

        #endregion
    }
}

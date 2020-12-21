using UnityEngine;

namespace UniModules.UniGame.ECS.Leo.Runtime.Nodes
{
    using System;
    using Core.Runtime.DataFlow.Interfaces;
    using Core.Runtime.Interfaces;
    using GameFlow.GameFlow.Runtime.Nodes.States;
    using Leopotam.Ecs;
    using UniGameFlow.NodeSystem.Runtime.Core.Attributes;
    using UniRx;

    [CreateNodeMenu(menuName:"Leo/EcsSystems",nodeName = "EcsSystems")]
    public class LeoSystemsAsyncState : RxStateNode
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        public EcsSystemsData systemsData = new EcsSystemsData();

        private EcsWorld _world;
        private bool     _activate = false;
        
        public override IObservable<Unit> ExecuteState(IContext value,ILifeTime lifeTime)
        {
            var observable = value.Receive<EcsWorld>()
                .Do(ActivateSystems)
                .AsUnitObservable();

            return observable;
        }

        public override void ExitState()
        {
            foreach (var system in systemsData.Systems)
            {
                _world?.DisableSystem(system.GetType());
            }

            _world    = null;
            _activate = false;
        }

        private void ActivateSystems(EcsWorld world)
        {
            if (_activate) return;
            _activate = true;
            
            var systems       = systemsData.Systems;
            foreach (var system in systems)
            {
                world.ActivateSystem(system.System,system.Info);
            }

        }
    }
}

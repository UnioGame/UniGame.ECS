using UnityEngine;

namespace UniModules.UniGame.ECS.Leo.Runtime.Nodes
{
    using Core.Runtime.Interfaces;
    using Cysharp.Threading.Tasks;
    using Leopotam.Ecs;
    using UniGameFlow.Nodes.Runtime.States;
    using UniGameFlow.NodeSystem.Runtime.Core.Attributes;
    using UniRx;

    [CreateNodeMenu(menuName:"Leo/EcsSystems",nodeName = "EcsSystems")]
    public class LeoSystemsAsyncState : AsyncStateUniNode
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        public EcsSystemsData systemsData = new EcsSystemsData();

        private EcsWorld _world;
        private bool     _activate = false;
        
        public override async UniTask<AsyncStatus> ExecuteStateAsync(IContext value)
        {
            _world = await value.Receive<EcsWorld>().First();

            ActivateSystems(_world);
            
            var tokenLifeTime = Token.LifeTime;
            await UniTask.WaitWhile(() => IsActive).
                WithCancellation(tokenLifeTime.AsCancellationToken());

            return AsyncStatus.Succeeded;
        }

        public override async UniTask ExitAsync(IContext data)
        {
            var systems = systemsData.Systems;
            foreach (var system in systems)
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
                world.ActivateSystem(system.GetType());
            }

        }
    }
}

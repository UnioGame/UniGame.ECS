namespace UniModules.UniGame.ECS.Leo.Runtime.Nodes
{
    using Core.Runtime.DataFlow.Interfaces;
    using Core.Runtime.Interfaces;
    using Cysharp.Threading.Tasks;
    using global::UniGame.UniNodes.NodeSystem.Runtime.Attributes;
    using global::UniGame.UniNodes.NodeSystem.Runtime.Core;
    using Leopotam.Ecs;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniGameFlow.Nodes.Runtime.States;
    using UniGameFlow.NodeSystem.Runtime.Core.Attributes;

    [CreateNodeMenu(menuName:"Leo/LeoWorldSource",nodeName = "LeoWorldSource")]
    public class LeoWorldAsyncStateSource : AsyncStateUniNode
    {
        #region inspector

        [Port(PortIO.Output)]
        public object output;
        
        public bool destroyWorldOnExit = true;
        
        #endregion
        
        private EcsWorld _world;
        
        public override async UniTask<AsyncStatus> ExecuteStateAsync(IContext value)
        {
            var lifeTime = value.LifeTime;
            _world = ActivateWorld(lifeTime);
            
            value.Publish(_world);
            
            PublishToken(GetPort(nameof(output)));

            await UniTask.WaitWhile(() => IsActive);
            
            return AsyncStatus.Succeeded;
        }

        public override async UniTask ExitAsync(IContext data)
        {
            if (destroyWorldOnExit)
            {
                _world?.DestroyWorld();
                _world = null; 
            }
        }

        private EcsWorld ActivateWorld(ILifeTime lifeTime)
        {
            if (_world != null)
                return _world;
            
            var world = CreateWorld();
            world.InitializeWorld();

            var destroyAction = ClassPool.Spawn<DisposableAction>();
            destroyAction.Initialize(world.Destroy);

            lifeTime.AddDispose(destroyAction);

            return world;
        }

        protected virtual EcsWorld CreateWorld()
        {
            return new EcsWorld();
        }

    }
}

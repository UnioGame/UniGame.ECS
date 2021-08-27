namespace UniModules.UniGame.ECS.Leo.Runtime.Nodes
{
    using System;
    using Core.Runtime.DataFlow.Interfaces;
    using Core.Runtime.Interfaces;
    using Cysharp.Threading.Tasks;
    using GameFlow.GameFlow.Runtime.Nodes.States;
    using global::UniModules.GameFlow.Runtime.Attributes;
    using global::UniModules.GameFlow.Runtime.Core;
    using Leopotam.Ecs;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniGameFlow.NodeSystem.Runtime.Core.Attributes;
    using UniRx;

    [CreateNodeMenu(menuName:"Leo/LeoWorldSource",nodeName = "LeoWorldSource")]
    public class LeoWorldAsyncStateSource : RxStateNode
    {
        #region inspector

        [Port(PortIO.Output)]
        public object output;
        
        public bool destroyWorldOnExit = true;
        
        #endregion
        
        private EcsWorld _world;
        
        public override IObservable<Unit> ExecuteState(IContext value, ILifeTime lifeTime)
        {
            _world = ActivateWorld(lifeTime);
            
            value.Publish(_world);
            
            PublishToken(GetPort(nameof(output)));

            UniTask.WaitWhile(() => IsActive);
            
            return Observable.Return(Unit.Default);
        }

        public override void ExitState()
        {
            if (!destroyWorldOnExit)
            {
                return;
            }

            _world?.DestroyWorld();
            _world = null;
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

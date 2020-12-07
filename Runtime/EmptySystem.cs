namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using Abctract;
    using Core.Runtime.DataFlow.Interfaces;

    public class EmptySystem : IUniEcsSystem
    {
        public int  Id       => 0;
        public int  WorldId  => 0;
        public bool IsActive => false;

        public bool Execute() => true;

        public bool Initialize(int worldId) => true;

        public bool IsShared => true;
        
        public ILifeTime LifeTime { get; }
        
        public void Dispose() { }
    }
}
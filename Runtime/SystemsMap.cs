namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using Core.Runtime.DataStructure;

    [Serializable]
    public class SystemsMap : SerializableDictionary<int, EcsSystemCounter>
    {
        public SystemsMap(int capacity) : base(capacity)
        {
            
        }
    }
}
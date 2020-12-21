namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using Cysharp.Threading.Tasks;

    [Serializable]
    public struct LeoSystemData
    {
        public PlayerLoopTiming updateType;
        public PlayerLoopTiming UpdateType => updateType;
    }
}
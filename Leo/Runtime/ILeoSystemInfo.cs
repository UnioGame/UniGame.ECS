namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using Cysharp.Threading.Tasks;

    public interface ILeoSystemInfo
    {
        bool   IsOneFrame { get; }
        bool   IsRun  { get; }
        string SystemName { get; }

        PlayerLoopTiming UpdateType { get; } 
    }
    
}
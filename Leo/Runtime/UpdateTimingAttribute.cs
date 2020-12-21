using System;

namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using Cysharp.Threading.Tasks;

    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateTimingAttribute : Attribute
    {
        public readonly PlayerLoopTiming updateType = PlayerLoopTiming.Update;

        public UpdateTimingAttribute(PlayerLoopTiming updateType)
        {
            this.updateType = updateType;
        }
    }
}

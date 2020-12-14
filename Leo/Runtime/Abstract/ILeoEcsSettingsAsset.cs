namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using ECS.Runtime.Leo;

    public interface ILeoEcsSettingsAsset : IEcsSystemsData,ILeoSystemsFactory
    {
        ILeoSystemsFactory CreateSystemsFactory();
    }
}
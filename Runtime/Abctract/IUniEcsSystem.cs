namespace UniModules.UniGame.ECS.Runtime.Abctract
{
    public interface IUniEcsSystem<TEcsSystem>
    {
        int Id { get; }

        TEcsSystem EcsSystem { get; }
    }
}

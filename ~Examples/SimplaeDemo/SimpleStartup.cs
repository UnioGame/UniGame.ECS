using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UniModules.UniGame.ECS.SimplaeDemo.Systems;
using UnityEngine;



public class SimpleStartup : MonoBehaviour
{

    public  LaunchType launchType = LaunchType.Single;
    public  GameObject _prefab;
    public  int        _amount = 100;
    public  float      _radius = 10f;
    public  float      _speed  = 5f;
    private EcsWorld   _world  = new EcsWorld();
    private EcsSystems _ecsSystems;
    
    private List<EcsSystems> _systems = new List<EcsSystems>();
    
    // Start is called before the first frame update
    private void Start()
    {

        switch (launchType)
        {
            case LaunchType.Single:
                StartWithSingleSystems();
                break;
            case LaunchType.Multi:
                StartWithMultiSystems();
                break;
            case LaunchType.Distinct:
                StartDistinct();
                break;
            case LaunchType.EcsRemove:
                StartEcs();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private EcsSystems StartWithSingleSystems(EcsWorld world)
    {
        _ecsSystems = new EcsSystems(world)
            .Add(new MotionTargetInitSystem()
            {
                prefab = _prefab,
                amount = _amount,
                radius = _radius
            })
            .Add(new MotionDirectionSystem() {speedLimits =_speed})
            .Add(new MotionTargetSystem());
        
        _ecsSystems.Init();
        
        return _ecsSystems;
    }

    private void StartEcs()
    {
        var system1 = new MotionTargetInitSystem()
        {
            prefab = _prefab,
            amount = _amount,
            radius = _radius
        };
        var system2 = new MotionDirectionSystem();
        var system3 = new MotionTargetSystem();
        
        _world = new EcsWorld();
        _ecsSystems = new EcsSystems(_world)
            .Add(system1)
            .Add(system2)
            .Add(system3);
        
        _ecsSystems.Init();
        _ecsSystems.Destroy();
        
        _systems.Add(new EcsSystems(_world).Add(system2));    
        _systems.Add( new EcsSystems(_world).Add(system3));
        _systems.ForEach(x => x.Init());
    }
    
    private void StartWithMultiSystems()
    {
        _world = new EcsWorld();
        _systems.Add( new EcsSystems(_world)
            .Add(new MotionTargetInitSystem()
            {
                prefab = _prefab,
                amount = _amount,
                radius = _radius
            }));
        _systems.Add(new EcsSystems(_world).Add(new MotionDirectionSystem() {speedLimits =_speed}));    
        _systems.Add( new EcsSystems(_world).Add(new MotionTargetSystem()));    
        
        _systems.ForEach(x => x.ProcessInjects());
        _systems.ForEach(x => x.Init());
    }
    
    private void StartWithMultiInjectSystems()
    {
        _world = new EcsWorld();
        _systems.Add( new EcsSystems(_world)
            .Add(new MotionTargetInitSystem()
            {
                prefab = _prefab,
                amount = _amount,
                radius = _radius
            }));
        _systems.Add(new EcsSystems(_world).Add(new MotionDirectionSystem() {speedLimits =_speed}));    
        _systems.Add( new EcsSystems(_world).Add(new MotionTargetSystem()));    
        ;
        _systems.ForEach(x =>
        {
            x.ProcessInjects();
            x.Init();
        });
    }

    private void StartWithSingleSystems()
    {
        _systems.Add(StartWithSingleSystems(new EcsWorld()));
    }

    private void StartDistinct()
    {
        _world = new EcsWorld();
        var ecsSystems = new EcsSystems(_world)
            .Add(new MotionTargetInitSystem()
            {
                prefab = _prefab,
                amount = _amount,
                radius = _radius
            });
        ecsSystems.ProcessInjects();
        ecsSystems.Init();
        
        _systems.Add(ecsSystems);

        ecsSystems = new EcsSystems(_world)
            .Add(new MotionTargetInitSystem()
            {
                prefab = _prefab,
                amount = _amount,
                radius = _radius
            });
        ecsSystems.ProcessInjects();
        ecsSystems.Init();
        
        _systems.Add(ecsSystems);

        ecsSystems = new EcsSystems(_world)
            .Add(new MotionTargetInitSystem()
            {
                prefab = _prefab,
                amount = _amount,
                radius = _radius
            });
        ecsSystems.ProcessInjects();
        ecsSystems.Init();
        
        _systems.Add(ecsSystems);
        
    }
    
    // Update is called once per frame
    private void Update()
    {
        foreach (var ecsSystem in _systems)
        {
            ecsSystem.Run();
        }
    }

    public enum LaunchType
    {
        Single,
        Multi,
        Distinct,
        EcsRemove,
    }

}

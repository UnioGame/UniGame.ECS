namespace UniModules.UniGame.ECS.Leo.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor;
    using Core.EditorTools.Editor.AssetOperations;
    using Core.EditorTools.Editor.Tools;
    using Cysharp.Threading.Tasks;
    using Leopotam.Ecs;
    using Runtime;
    using UniCore.EditorTools.Editor.Utility;
    using UniCore.Runtime.ReflectionUtils;
    using UnityEditor;

    public class EditorSystemsLeoProcessor
    {
        public static string AssetPath { get; private set; } = EditorPathConstants.GeneratedContentDefaultPath.CombinePath($"GameFlow/LeoEcs/Resources/");
        
        [MenuItem("UniGame/Ecs/Leo/Refresh Leo Settings")]
        public static void UpdateLeoSystems()
        {
            var gameSystemAsset  = AssetEditorTools.LoadOrCreate<LeoEcsSettingsAsset>(AssetPath);
            var systems          = GetAllSystems();
            var data             = gameSystemAsset.systemsData;
            var systemsContainer = data.systems;
            
            foreach (var ecsSystem in systems)
            {
                if(systemsContainer.Any(x => x.system?.GetType() == ecsSystem.GetType()))
                    continue;
                
                var updateType = PlayerLoopTiming.Update;
                var systemType = ecsSystem.GetType();
                var updateTimingAttribute = systemType.GetCustomAttribute<UpdateTimingAttribute>();
                updateType = updateTimingAttribute == null ? updateType : updateTimingAttribute.updateType;
                
                systemsContainer.Add(new EcsSystemData()
                {
                    system = ecsSystem,
                    systemData = new LeoSystemData()
                    {
                        updateType = updateType,
                    }
                });
            }
            
            gameSystemAsset.MarkDirty();
            AssetDatabase.Refresh();
        }

        private static List<IEcsSystem> GetAllSystems()
        {
            var systems      = new List<IEcsSystem>();
            var systemsTypes = typeof(IEcsSystem).GetAssignableTypes();
            systemsTypes = systemsTypes.
                Where(x => x.HasDefaultConstructor()).
                Where(x=> x.IsGenericType == false).
                ToList();

            foreach (var systemsType in systemsTypes)
            {
                var system = Activator.CreateInstance(systemsType) as IEcsSystem;
                systems.Add(system);
            }

            return systems;
        }
        
    }
    
}

namespace UniModules.UniGame.ECS.Leo.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor;
    using Core.EditorTools.Editor.AssetOperations;
    using Core.EditorTools.Editor.Tools;
    using Leopotam.Ecs;
    using Runtime;
    using UniCore.Runtime.ReflectionUtils;
    using UnityEditor;

    public class EditorSystemsLeoProcessor
    {
        public static string AssetPath { get; private set; } =
            EditorPathConstants.GeneratedContentDefaultPath.CombinePath($"GameFlow/LeoEcs/Resources/");
        
        [MenuItem("UniGame/Ecs/Leo/Refresh Leo Settings")]
        public static void UpdateLeoSystems()
        {
            var gameSystemAsset = AssetEditorTools.LoadOrCreate<LeoEcsSettingsAsset>(AssetPath);
            var systems         = GetAllSystems();
            var data = gameSystemAsset.systemsData;
            //update asset systems
            data.systems = systems;
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

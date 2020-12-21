using UnityEngine;

namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Leopotam.Ecs;

    [CreateAssetMenu(menuName = "GameFlow/Ecs/LeoEcs/LeoEcsSettingsAsset", fileName = nameof(LeoEcsSettingsAsset))]
    public class LeoEcsSettingsAsset : ScriptableObject, ILeoEcsSettingsAsset
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        public EcsSystemsData systemsData = new EcsSystemsData();

        public IReadOnlyList<IEcsSystemData> Systems => systemsData.Systems;

        public virtual ILeoSystemsFactory CreateSystemsFactory() => 
            ScriptableObject.Instantiate(this);

        public void Dispose() => Destroy(this);

        public IEcsSystem Create(Type systemType)
        {
            var systems = systemsData.Systems;
            var system  = systems.FirstOrDefault(x => x.System.GetType() == systemType);
            return system.System;
        }

    }
}
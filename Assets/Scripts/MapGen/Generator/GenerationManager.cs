using System;
using UnityEngine;

namespace MapGen.Generator
{
    public class GenerationManager: MonoBehaviour
    {
        public static GenerationManager instance;
        public static GenerationConfig Config => instance.profile.config;
        
        [SerializeField]
        private GenerationProfile profile;

        public GenerationTile[,] _map;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("More than one instance of GenerationManager");
                Destroy(this);
            }
            instance = this;
        }

        public void Generate()
        {
            _map = new GenerationTile[profile.config.size.y, profile.config.size.x];
            foreach (var generationStep in profile.pipeline)
            {
                generationStep.Run(profile.config, _map);
            }
        }
    }
}
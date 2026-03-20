using System;
using UnityEngine;

namespace MapGen.Generator
{
    [Serializable]
    public abstract class GenerationStep : ScriptableObject
    {
        protected GenerationConfig Config;

        public virtual void Setup(GenerationConfig config)
        {
            Config = config;
        }
        
        public virtual void Run(GenerationConfig config, GenerationTile[,] map)
        {
            Setup(config);
            for (int i = 0; i < config.size.y; i++)
            {
                for (int j = 0; j < config.size.x; j++)
                {
                    ProcessTile(ref map[i, j], new Vector2Int(j, i));
                }
            }
        }
        
        protected abstract void ProcessTile(ref GenerationTile tile, Vector2Int position);
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace MapGen.Generator
{
    [CreateAssetMenu(fileName = "Generation", menuName = "GenerationProfile", order = 0)]
    public class GenerationProfile : ScriptableObject
    {
        public GenerationConfig config;
        public List<GenerationStep> pipeline;
        
        public void Generate(out GenerationTile[,] map)
        {
            map = new GenerationTile[config.size.y, config.size.x];
            foreach (var generationStep in pipeline)
            {
                generationStep.Run(config, map);
            }
        }
    }
}
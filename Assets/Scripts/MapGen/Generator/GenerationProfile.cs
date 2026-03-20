using System.Collections.Generic;
using UnityEngine;

namespace MapGen.Generator
{
    [CreateAssetMenu(fileName = "Generation", menuName = "GenerationProfile", order = 0)]
    public class GenerationProfile : ScriptableObject
    {
        public GenerationConfig config;
        public List<GenerationStep> pipeline;
    }
}
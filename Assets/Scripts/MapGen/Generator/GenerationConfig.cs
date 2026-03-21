using UnityEngine;

namespace MapGen.Generator
{
    [CreateAssetMenu(fileName = "GenerationConfig", menuName = "Generation/GenerationConfig", order = 0)]
    public class GenerationConfig : ScriptableObject
    {
        public int seed;
        public Vector2Int size;
        public float spacing;
    }
}
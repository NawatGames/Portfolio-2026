using UnityEngine;
using UnityEngine.Serialization;

namespace MapGen.Generator.Steps
{
    [CreateAssetMenu(fileName = "NoiseGenerationStep", menuName = "GenerationStep/HeightStep", order = 0)]
    public class NoiseStep : GenerationStep
    {
        [SerializeField] private int offsetRange = 10;
        [SerializeField] private float noiseOffsetDivider = 1f;
        
        private System.Random _rand;
        private Vector2 _offset;

        private Vector2 GetOffset()
        {
            float offsetX = _rand.Next(-offsetRange, offsetRange);
            float offsetY = _rand.Next(-offsetRange, offsetRange);
            return new Vector2(offsetX, offsetY);
        }

        public override void Setup(GenerationConfig config)
        {
            base.Setup(config);
            _rand = new System.Random(Config.size.GetHashCode());
            _offset = GetOffset();
        }

        protected override void ProcessTile(ref GenerationTile tile, Vector2Int position)
        {
            Vector2 localOffset = (Vector2)position / noiseOffsetDivider;
            Vector2 noisePosition = _offset + localOffset;
            tile.Height = Mathf.PerlinNoise(noisePosition.x, noisePosition.y);
        }
    }
}
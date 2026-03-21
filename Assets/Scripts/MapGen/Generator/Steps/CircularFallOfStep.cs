using System;
using UnityEngine;

namespace MapGen.Generator.Steps
{
    [CreateAssetMenu(fileName = "CircularFallOfStep", menuName = "GenerationStep/CircularFallOfStep", order = 0)]
    public class CircularFallOfStep : GenerationStep
    {
        [SerializeField] private float radius;
        [SerializeField] private float softness;

        private Vector2 _center;

        public override void Setup(GenerationConfig config)
        {
            base.Setup(config);

            float centerX = Config.size.x / 2f;
            float centerY = Config.size.y / 2f;
            _center = new Vector2(centerX, centerY);
        }

        private static float SmoothStep(float edge0, float edge1, float x)
        {
            float t = (x - edge0) / (edge1 - edge0);
            t = Math.Clamp(t, 0f, 1f);
            return t * t * (3f - 2f * t);
        }

        protected override void ProcessTile(ref GenerationTile tile, Vector2Int position)
        {
            float dist = Vector2.Distance(position, _center);
            float t = SmoothStep(radius, radius + softness, dist);
            float output = 1f - t;

            tile.Height *= output;
        }
    }
}
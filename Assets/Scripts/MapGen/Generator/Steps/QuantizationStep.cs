using UnityEngine;

namespace MapGen.Generator.Steps
{
    [CreateAssetMenu(fileName = "QuantizationStep", menuName = "GenerationStep/QuantizationStep", order = 0)]
    public class QuantizationStep : GenerationStep
    {
        [SerializeField] private float quantizationInterval = .5f;
        protected override void ProcessTile(ref GenerationTile tile, Vector2Int position)
        {
            float originalHeight = tile.Height;
            float finalHeight = Mathf.Round(originalHeight / quantizationInterval) * quantizationInterval;
            tile.Height = finalHeight;
        }
    }
}
using UnityEngine;

namespace MapGen.Generator.Steps
{
    [CreateAssetMenu(fileName = "MultiplierStep", menuName = "GenerationStep/MultiplierStep", order = 0)]
    public class MultiplyStep : GenerationStep
    {
        [SerializeField] private float multiplier = 1f;
        protected override void ProcessTile(ref GenerationTile tile, Vector2Int position)
        {
            tile.Height *= multiplier;
        }
    }
}
using MapGen.Generator;
using UnityEngine;
using UnityEngine.Serialization;

namespace MapGen.Renderer
{
    public class MapRenderer : MonoBehaviour
    {
        [SerializeField] private GenerationProfile profile;
        [SerializeField] private Material material;

        [ContextMenu("Generate")]
        public void Generate()
        {
            profile.Generate(out GenerationTile[,] tiles);
            MapMeshGenerator meshGen = new MapMeshGenerator(profile.config.size, profile.config.spacing);
            Mesh mesh = meshGen.GenerateMesh(tiles);
            GameObject obj = new GameObject("Map");
            
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            
            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            meshRenderer.material = material;
        }
    }
}
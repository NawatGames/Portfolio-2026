using System.Collections.Generic;
using MapGen.Generator;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace MapGen.Renderer
{
    public class MapMeshGenerator
    {
        private readonly Vector2Int _size;
        private readonly float _spacing;

        public MapMeshGenerator(Vector2Int size, float spacing)
        {
            _size = size;
            _spacing = spacing;
        }

        private List<Vector3> GenerateVertices(GenerationTile[,] tiles)
        {
            List<Vector3> vertices = new List<Vector3>();

            for (int i = 0; i < _size.y; i++)
            {
                for (int j = 0; j < _size.x; j++)
                {
                    float height = tiles[i, j].Height * _spacing;
                    Vector3 position = new Vector3(i * _spacing, height, j * _spacing);
                    vertices.Add(position);
                }
            }

            return vertices;
        }

        private List<int> GenerateTriangles()
        {
            List<int> triangles = new List<int>();

            for (int i = 0; i < _size.y - 1; i++)
            {
                for (int j = 0; j < _size.x - 1; j++)
                {
                    int current = i * _size.x + j;
                    int nextRow = (i + 1) * _size.x + j;

                    triangles.Add(current + 1);
                    triangles.Add(nextRow);
                    triangles.Add(current);

                    triangles.Add(nextRow + 1);
                    triangles.Add(nextRow);
                    triangles.Add(current + 1);
                }
            }

            return triangles;
        }

        public Mesh GenerateMesh(GenerationTile[,] tiles)
        {
            List<Vector3> vertices = GenerateVertices(tiles);
            List<int> triangles = GenerateTriangles();

            Mesh m = new Mesh
            {
                name = "MapMesh",
                vertices = vertices.ToArray(),
                triangles = triangles.ToArray()
            };
            m.RecalculateNormals();
            m.RecalculateBounds();

            return m;
        }
    }
}
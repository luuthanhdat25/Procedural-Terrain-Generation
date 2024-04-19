using System;
using TextureGen;
using UnityEngine;

namespace MeshGen
{
    public class TerrainMesh : RepeatMonoBehaviour
    {
        private enum DrawMode {
            HeightNoiseMap,
            ColourMap
        }

        [SerializeField]
        private DrawMode drawMode = DrawMode.HeightNoiseMap;
        
        [Header("Settings Values")]
        [SerializeField]
        private int xSize;

        [SerializeField]
        private int zSize;
        
        [SerializeField]
        private int yScale;
        
        [SerializeField]
        private float noiseScale;
            
        [SerializeField]
        private int octaves;

        [SerializeField]
        private float persistance;

        [SerializeField]
        private float lacunarity;

        [SerializeField]
        private int seed;

        [SerializeField]
        private Vector2 offset;

        [SerializeField]
        private TerrainType[] terrainTypes;
        
        [Header("Component")]
        [SerializeField] 
        private MeshFilter meshFilter;
        
        [SerializeField] 
        private Renderer renderer;
        
        private Mesh _mesh;
        private Vector3[] _vertices;
        private Vector2[] _uv;
        private int[] _triangles;
        private float[,] _noiseMap;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            LoadMeshFilter();
            LoadRenderer();
        }

        private void LoadMeshFilter()
        {
            if (meshFilter != null) return;
            meshFilter = GetComponent<MeshFilter>();
        }

        private void LoadRenderer()
        {
            if (renderer != null) return;
            renderer = GetComponent<Renderer>();
        }
        
        private void Start()
        {
            _mesh = new Mesh();
            meshFilter.mesh = _mesh;
        
            _noiseMap = PerlinNoise.GenerateNoiseMap(xSize + 1, zSize + 1, noiseScale,
                seed,
                octaves, persistance, lacunarity,
                offset);
            
            CreateShapes();
            UpdateMesh();
            GenerateMap();
        }

        private void UpdateMesh()
        {
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.uv = _uv;
            _mesh.RecalculateNormals();
        }

        private void CreateShapes()
        {
            int numVertices = (xSize + 1) * (zSize + 1);
            _vertices = new Vector3[numVertices];
            _uv = new Vector2[numVertices];
            
            for (int z = 0, index = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++, index++)
                {
                    float y = _noiseMap[x, z] * yScale;
                    _vertices[index] = new Vector3(x, y, z);
                    _uv[index] = new Vector2((float)x / xSize, (float)z / zSize);
                }
            }

            _triangles = new int[xSize * zSize * 6];
            
            for (int z = 0, vert = 0, tris = 0; z < zSize; z++, vert++) {
                for (int x = 0; x < xSize; x++) {
                    _triangles[tris + 0] = vert + 0;
                    _triangles[tris + 1] = vert + xSize + 1;
                    _triangles[tris + 2] = vert + 1;

                    _triangles[tris + 3] = vert + xSize + 1;
                    _triangles[tris + 4] = vert + xSize + 2;
                    _triangles[tris + 5] = vert + 1;

                    vert++;
                    tris += 6;
                }
            }
        }
        
        public void GenerateMap()
        {
            if (renderer == null) {
                Debug.LogWarning("Game object " + gameObject.name + " is not have a renderer component!");
                return;
            }
            
            if (drawMode == DrawMode.ColourMap) {
                renderer.DrawTexture(TerrainTextureGenerator.GetColorMapByTerrainTypeTexture(_noiseMap, terrainTypes));
            }else {
                renderer.DrawTexture(TerrainTextureGenerator.GetHeightMapTexture(_noiseMap));
            }
        }
    }
}

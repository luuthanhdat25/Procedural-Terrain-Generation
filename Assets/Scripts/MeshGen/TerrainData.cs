using UnityEngine;

namespace MeshGen
{
    [System.Serializable]
    public struct TerrainData
    {
        public TextureDrawMode drawMode;
        
        [Header("Settings Values")]
        public int xGripSize;
        public int zGripSize;
        public int yScale;
        public float noiseScale;
        public int octaves;
        public float persistance;
        public float lacunarity;
        public int seed;
        public Vector2 offset;
        public TerrainType[] terrainTypes;
    }
}
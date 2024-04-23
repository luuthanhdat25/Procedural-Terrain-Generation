using UnityEngine;
using UnityEngine.Serialization;

namespace MeshGen
{
    [System.Serializable]
    public struct TerrainData
    {
        public TextureDrawMode drawMode;
        
        [Header("Settings Values")]
        [Range(0, 6)] 
        public int levelOfDetail;
        
        public int yHeightMultiplier;
        public AnimationCurve heightCurve;
        public float noiseScale;
        public int octaves;
        public float persistance;
        public float lacunarity;
        public int seed;
        public Vector2 offset;
        public TerrainType[] terrainTypes;
    }
}
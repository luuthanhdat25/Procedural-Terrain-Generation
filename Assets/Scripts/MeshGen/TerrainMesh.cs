using TextureGen;
using UnityEngine;

namespace MeshGen
{
    public class TerrainMesh : RepeatMonoBehaviour
    {
        [SerializeField] 
        private TerrainData terrainData;
        
        [Header("Component")]
        [SerializeField] 
        private MeshFilter meshFilter;
        
        [SerializeField] 
        private Renderer renderer;
        
        [SerializeField]
        private bool autoUpdate;
        public bool IsAutoUpdate() => this.autoUpdate;
        
        private new Mesh _mesh;

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
        
         public void GenerateMap()
         {
             _mesh = new Mesh();
             meshFilter.mesh = _mesh;

             float[,] noiseMap = PerlinNoise.GenerateNoiseMap(
                 MeshDisplay.MAP_CHUNK_SIZE + 1, MeshDisplay.MAP_CHUNK_SIZE + 1, terrainData.noiseScale,
                 terrainData.seed,
                 terrainData.octaves, terrainData.persistance, terrainData.lacunarity,
                 terrainData.offset);
             
             _mesh.GenerateByNoiseMap(terrainData, noiseMap);
             
             if (terrainData.drawMode == TextureDrawMode.ColourMap) {
                 renderer.DrawTexture(TerrainTextureGenerator.GetColorMapByTerrainTypeTexture(noiseMap, terrainData.terrainTypes));
             }else {
                 renderer.DrawTexture(TerrainTextureGenerator.GetHeightMapTexture(noiseMap));
             }
         }
    }
}

using UnityEngine;

namespace TextureGen
{
    public class TerrainTexture : RepeatMonoBehaviour
    {
        [SerializeField]
        private new Renderer renderer;

        private enum DrawMode {
            HeightNoiseMap,
            ColourMap
        }

        [SerializeField]
        private DrawMode drawMode = DrawMode.HeightNoiseMap;
        
        [Header("Settings Values")]
        [SerializeField]
        private int width;

        [SerializeField]
        private int height;
        
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
        
        [SerializeField]
        private bool autoUpdate;
        public bool IsAutoUpdate() => this.autoUpdate;

        #region Load Component
        protected override void LoadComponents()
        {
            base.LoadComponents();
            if (renderer != null) return;
            renderer = GetComponent<Renderer>();
        }
        #endregion
    

        public void GenerateMap()
        {
            if (renderer == null) {
                Debug.LogWarning("Game object " + gameObject.name + " is not have a renderer component!");
                return;
            }
        
            float[,] noiseMap = PerlinNoise.GenerateNoiseMap(width, height, noiseScale,
                seed,
                octaves, persistance, lacunarity,
                offset);
        
            if (drawMode == DrawMode.ColourMap) {
                renderer.DrawTexture(TerrainTextureGenerator.GetColorMapByTerrainTypeTexture(noiseMap, terrainTypes));
            }else {
                renderer.DrawTexture(TerrainTextureGenerator.GetHeightMapTexture(noiseMap));
            }
        }

        private void OnValidate()
        {
            if (width < 1) width = 1;
            if (height < 1) height = 1;
            if (lacunarity < 1) lacunarity = 1;
            if (octaves < 0) octaves = 0;
        }
    }
}
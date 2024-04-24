using UnityEngine;

namespace TextureGen
{
    public static class TerrainTextureGenerator
    {
        private static Texture2D GetTextureFromColorMap(Color[] colorMap, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.SetPixels(colorMap);
            texture.Apply();
            return texture;
        }

        public static Texture2D GetHeightMapTexture(float[,] heightMap)
        {
            int width = heightMap.GetLength(0), height = heightMap.GetLength(1);
            
            Color[] colorMap = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colorMap[y * width + x] = 
                        Color.Lerp(Color.black, Color.white, heightMap[x, y]);
                }
            }

            return GetTextureFromColorMap(colorMap, width, height);
        } 
        
        public static Texture2D GetColorMapByTerrainTypeTexture(float[,] heightMap, TerrainType[] regions)
        {
            int width = heightMap.GetLength(0), height = heightMap.GetLength(1);
            
            Color[] colorMap = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float currentHeight = heightMap[x, y];

                    //Get lowest color terrain
                    foreach (var region in regions)
                    {
                        if (currentHeight <= region.height)
                        {
                            colorMap[y * width + x] = region.colour;
                            break;
                        }
                    }
                }
            }

            return GetTextureFromColorMap(colorMap, width, height);
        } 
    }
}
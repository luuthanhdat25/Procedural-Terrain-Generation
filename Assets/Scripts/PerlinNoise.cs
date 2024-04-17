using UnityEngine;

public static class PerlinNoise
{
    public static float[,] GenerateNoiseMap(
        int width, int height, float scale, 
        int seed,
        int octaves, float persistance, float lacunarity,
        Vector2 offset)
    {
        if (scale <= 0) scale = 0.0001f;
        float[,] noiseMap = new float[width, height];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
            
        System.Random random = new System.Random(seed);

        Vector2[] octaveOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = random.Next(-10000, 10000) + offset.x;
            float offsetY = random.Next(-10000, 10000) + offset.y;
            octaveOffset[i] = new Vector2(offsetX, offsetY);
        }

        float halfWidth = (float)width / 2;
        float halfHeight = (float)height / 2;
            
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                    
                    
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x-halfWidth) / scale * frequency + octaveOffset[i].x;
                    float sampleY = (y-halfHeight) / scale * frequency + octaveOffset[i].y;
                        
                    float perlinNoiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // To make perlinNoiseValue from 1 to -1
                    noiseHeight += perlinNoiseValue * amplitude;
        
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                    
                if(noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                if(noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
                    
                noiseMap[x, y] = noiseHeight;
            }
        }

            
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
            
        return noiseMap;
    }
}
using TextureGen;
using UnityEngine;

namespace MeshGen
{
    public static class MeshDisplay
    {
        public static void GenerateTerrain(
            this Mesh mesh, 
            TerrainData terrainData, ref Renderer renderer)
        {
            float[,] noiseMap = PerlinNoise.GenerateNoiseMap(
                    terrainData.xGripSize + 1, terrainData.zGripSize + 1, terrainData.noiseScale,
                    terrainData.seed,
                    terrainData.octaves, terrainData.persistance, terrainData.lacunarity,
                    terrainData.offset);
            
            int numVertices = (terrainData.xGripSize + 1) * (terrainData.zGripSize + 1);
            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uv = new Vector2[numVertices];
             
            for (int z = 0, index = 0; z <= terrainData.zGripSize; z++)
            {
                for (int x = 0; x <= terrainData.xGripSize; x++, index++)
                {
                    float y = noiseMap[x, z] * terrainData.yScale;
                    vertices[index] = new Vector3(x, y, z);
                    uv[index] = new Vector2((float)x / terrainData.xGripSize, (float)z / terrainData.zGripSize);
                }
            }

            int[] triangles = new int[terrainData.xGripSize * terrainData.zGripSize * 6];
             
            for (int z = 0, vert = 0, tris = 0; z < terrainData.zGripSize; z++, vert++) {
                for (int x = 0; x < terrainData.xGripSize; x++) {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + terrainData.xGripSize + 1;
                    triangles[tris + 2] = vert + 1;

                    triangles[tris + 3] = vert + terrainData.xGripSize + 1;
                    triangles[tris + 4] = vert + terrainData.xGripSize + 2;
                    triangles[tris + 5] = vert + 1;

                    vert++;
                    tris += 6;
                }
            }
            
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
           
             
            if (terrainData.drawMode == TextureDrawMode.ColourMap) {
                renderer.DrawTexture(TerrainTextureGenerator.GetColorMapByTerrainTypeTexture(noiseMap, terrainData.terrainTypes));
            }else {
                renderer.DrawTexture(TerrainTextureGenerator.GetHeightMapTexture(noiseMap));
            }
        }
    }
}
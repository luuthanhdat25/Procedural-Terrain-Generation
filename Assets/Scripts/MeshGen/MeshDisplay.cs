using UnityEngine;

namespace MeshGen
{
    public static class MeshDisplay
    {
        public const int MAP_CHUNK_SIZE = 241;

        public static void GenerateByNoiseMap(
            this Mesh mesh, 
            TerrainData terrainData, float[,] noiseMap)
        {
            int numVertices = (MAP_CHUNK_SIZE + 1) * (MAP_CHUNK_SIZE + 1);
            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uv = new Vector2[numVertices];

            int meshSimplificationIncrement = terrainData.levelOfDetail == 0 ? 1 : terrainData.levelOfDetail * 2;
            
            for (int z = 0, index = 0; z <= MAP_CHUNK_SIZE; z += meshSimplificationIncrement)
            {
                for (int x = 0; x <= MAP_CHUNK_SIZE; x += meshSimplificationIncrement, index++)
                {
                    float y = terrainData.heightCurve.Evaluate(noiseMap[x, z]) * terrainData.yHeightMultiplier;
                    vertices[index] = new Vector3(x, y, z);
                    uv[index] = new Vector2((float)x / MAP_CHUNK_SIZE, (float)z / MAP_CHUNK_SIZE);
                }
            }

            int numTiles = MAP_CHUNK_SIZE / meshSimplificationIncrement;
            int[] triangles = new int[numTiles * numTiles * 6];
             
            for (int z = 0, vert = 0, tris = 0; z < numTiles; z++, vert++) {
                for (int x = 0; x < numTiles; x++) {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + numTiles + 1;
                    triangles[tris + 2] = vert + 1;

                    triangles[tris + 3] = vert + numTiles + 1;
                    triangles[tris + 4] = vert + numTiles + 2;
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
        }
    }
}

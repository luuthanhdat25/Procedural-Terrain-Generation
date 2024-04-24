using TextureGen;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TerrainTexture))]
    public class TerrainTextureEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            TerrainTexture terrainTexture = (TerrainTexture)target;
            if (DrawDefaultInspector()) {
                if(terrainTexture.IsAutoUpdate()) 
                    terrainTexture.GenerateMap();
            }

            if (GUILayout.Button("Generate")) {
                terrainTexture.GenerateMap();
            }
        }
    }
}

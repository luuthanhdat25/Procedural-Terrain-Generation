using MeshGen;
using TextureGen;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TerrainMesh))]
    public class TerrainMeshEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            TerrainMesh terrainMesh = (TerrainMesh)target;
            if (DrawDefaultInspector()) {
                if(terrainMesh.IsAutoUpdate()) 
                    terrainMesh.GenerateMap();
            }

            if (GUILayout.Button("Generate")) {
                terrainMesh.GenerateMap();
            }
        }
    }
}

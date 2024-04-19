using System;
using TextureGen;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        private Mesh _mesh;

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
        
         private void Start()
         {
             _mesh = new Mesh();
             meshFilter.mesh = _mesh;
         
             _mesh.GenerateTerrain(terrainData, ref renderer);
         }
    }
}

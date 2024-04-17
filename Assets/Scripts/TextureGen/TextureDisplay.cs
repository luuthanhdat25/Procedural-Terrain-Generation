using UnityEngine;

namespace TextureGen
{
    public static class TextureDisplay
    {
        public static void DrawTexture(this Renderer textureRenderer, Texture2D texture2D)
        {
            textureRenderer.sharedMaterial.mainTexture = texture2D;
            textureRenderer.transform.localScale = 
                new Vector3(texture2D.width, 1, texture2D.height);
        }
    }
}
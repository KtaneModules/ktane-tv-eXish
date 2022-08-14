using UnityEngine;

namespace SterographicMaze
{
    public static class Stereogram
    {
        private const int size = 300;
        private const int stdDist = 60;
        private const int linkDist = 35;
        public static Texture2D RenderSterogram(Texture2D DepthTexture)
        {
            Texture2D tex = new Texture2D(size, size);

            Color[,] pxls = new Color[size, size];

            for (int x = 0; x < stdDist; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    pxls[x, y] = Random.ColorHSV();
                }
            }

            for (int x = stdDist; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    pxls[x, y] = pxls[x - Mathf.FloorToInt(Mathf.Lerp(stdDist, linkDist, 1f - DepthTexture.GetPixel(x, y).grayscale)), y];
                }
            }

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    tex.SetPixel(x, y, pxls[x, y]);
                }
            }

            tex.Apply();

            Object.Destroy(DepthTexture); // Textures won't be automatically destroyed.

            return tex;
        }
    }
}

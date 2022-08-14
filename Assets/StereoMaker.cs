using UnityEngine;
using SterographicMaze;

public class StereoMaker : MonoBehaviour {
    public RenderTexture tex;
    public Material texOut;
    private Material texOut2;
    public Renderer r;

    public bool DoRender { get; set; }

    void Start()
    {
        texOut2 = new Material(texOut);
        r.material = texOut2;
        texOut2.mainTexture = new Texture();
    }
    
    void Update () {
        if (DoRender)
        {
            Destroy(texOut2.mainTexture); // Textures won't be automatically destroyed.
            texOut2.mainTexture = Stereogram.RenderSterogram(toTexture2D(tex));
        }
	}

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(300, 300, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0, false);
        tex.Apply();
        return tex;
    }
}

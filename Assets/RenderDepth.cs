using UnityEngine;

[ExecuteInEditMode]
public class RenderDepth : MonoBehaviour
{
    [Range(0f, 10f)]
    public float depthLevel = 0.5f;

    public Shader m;

    private Shader _shader;
    private Shader Shader
    {
        get { return _shader ?? (_shader = m); }
    }

    private Material _material;
    private Material Material
    {
        get
        {
            if(_material == null)
            {
                _material = new Material(Shader)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }
            return _material;
        }
    }

    private void Start()
    {
        if(!SystemInfo.supportsImageEffects)
        {
            print("System doesn't support image effects");
            enabled = false;
            return;
        }
        if(Shader == null || !Shader.isSupported)
        {
            enabled = false;
            print("Shader " + Shader.name + " is not supported");
            return;
        }

        // turn on depth rendering for the camera so that the shader can access it via _CameraDepthTexture
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;

        GetComponent<Camera>().fieldOfView *= transform.lossyScale.x;
        GetComponent<Camera>().farClipPlane *= transform.lossyScale.x;
    }

    private void OnDisable()
    {
        if(_material != null)
            DestroyImmediate(_material);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(Shader != null)
        {
            Material.SetFloat("_DepthLevel", depthLevel);
            Graphics.Blit(src, dest, Material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
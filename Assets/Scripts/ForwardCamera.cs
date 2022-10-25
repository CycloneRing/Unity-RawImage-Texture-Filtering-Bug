using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ForwardCamera : MonoBehaviour
{
    float lastScreenWidth = 0;
    float lastScreenHeight = 0;
    RenderTexture offscreenRenderTexture = null;

    // Public Values
    public UnityEngine.UI.RawImage rawImage;
    public UnityEngine.UI.Dropdown dropdown;
    public Camera offscreenCamera;
    public Material sideWallMtl;
    [Range(0.5f, 8.0f)] public float ResolutionMultiplier = 1.0f;
    public FilterMode OffscreenFilter = FilterMode.Point;


    // Mono Functions
    private void Start()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        UpdateOffscreenRender();
        offscreenCamera.enabled = true;
        rawImage.enabled = true;
    }
    private void Update()
    {
        if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            UpdateOffscreenRender();
        }
    }
    private void UpdateOffscreenRender()
    {
        if (offscreenRenderTexture != null)
        {
            offscreenRenderTexture.DiscardContents();
            offscreenRenderTexture.Release();
            offscreenRenderTexture = null;
        }
        offscreenRenderTexture = new RenderTexture((int)(Screen.width * ResolutionMultiplier), (int)(Screen.height * ResolutionMultiplier), 0, RenderTextureFormat.Default);
        offscreenRenderTexture.name = "OffscreenRender-DynamicRT";
        offscreenRenderTexture.filterMode = OffscreenFilter;
        offscreenRenderTexture.anisoLevel = 2;
        offscreenCamera.targetTexture = offscreenRenderTexture;
        rawImage.texture = offscreenRenderTexture;
        rawImage.color = Color.white;
        sideWallMtl.SetTexture("_EmissionMap", offscreenRenderTexture);
    }
    private void LateUpdate() 
    {
        offscreenCamera.Render();
    }

    // Public Functions
    public void UpdateFiltering()
    {
        if (dropdown.value == 0) OffscreenFilter = FilterMode.Point;
        if (dropdown.value == 1) OffscreenFilter = FilterMode.Bilinear;
        if (dropdown.value == 2) OffscreenFilter = FilterMode.Trilinear;
        UpdateOffscreenRender();
    }
}

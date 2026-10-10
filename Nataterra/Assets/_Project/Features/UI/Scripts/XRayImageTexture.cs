using UnityEngine;
using UnityEngine.UI;

public class XRayImageTexture : MonoBehaviour
{
    [SerializeField] Texture texture;

    public Material runtimeMaterial;
    Image sourceImage;

    void Start()
    {
        Material newMat = new Material(runtimeMaterial);
        newMat.SetTexture("_MainTex", texture);

        sourceImage = GetComponent<Image>();
        sourceImage.material = newMat;
    }

    void OnDestroy()
    {
        if (runtimeMaterial != null)
            Destroy(runtimeMaterial);
    }
}

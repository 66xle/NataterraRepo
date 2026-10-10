using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionIcon : MonoBehaviour
{
    public Material XrayMaterial;
    Image sourceImage;

    public void SetMaterial(Texture2D texture)
    {
        Material newMat = new Material(XrayMaterial);
        newMat.SetTexture("_MainTex", texture);

        sourceImage = GetComponent<Image>();
        sourceImage.material = newMat;
    }

    void OnDestroy()
    {
        if (XrayMaterial != null)
            Destroy(XrayMaterial);
    }
}

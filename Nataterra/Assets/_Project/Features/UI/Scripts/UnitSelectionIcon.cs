using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionIcon : MonoBehaviour
{
    public Material XrayMaterial;
    public Image selectedImage;
    public Image textureImage;

    public void SelectIcon(bool value)
    {
        selectedImage.gameObject.SetActive(value);
    }

    public void SetMaterial(Texture2D texture)
    {
        Material newMat = new Material(XrayMaterial);
        newMat.SetTexture("_MainTex", texture);

        textureImage.material = newMat;
    }

    void OnDestroy()
    {
        if (XrayMaterial != null)
            DestroyImmediate(XrayMaterial);
    }
}

using UnityEngine;
using PurrNet;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TMP_Text FoodText;
    public TMP_Text WoodText;
    public TMP_Text MetalText;

    void Awake()
    {
        InstanceHandler.RegisterInstance(this);
    }

    private void OnDestroy()
    {
        InstanceHandler.UnregisterInstance<ResourceUI>();
    }

    public void SetResource(int food, int wood, int metal)
    {
        FoodText.text = food.ToString();
        WoodText.text = wood.ToString();
        MetalText.text = metal.ToString();
    }
}

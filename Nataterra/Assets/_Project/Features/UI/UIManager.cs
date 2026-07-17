using PurrNet;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text PhaseTitle;

    public List<UnitUI> _listOfUnitUI;

    private void Awake()
    {
        InstanceHandler.RegisterInstance(this);
    }

    private void OnDestroy()
    {
        InstanceHandler.UnregisterInstance<UIManager>();
    }

    public async void ShowPhaseUI(string text)
    {
        PhaseTitle.text = text;
        PhaseTitle.transform.parent.gameObject.SetActive(true);

        await Task.Delay(1000);

        PhaseTitle.transform.parent.gameObject.SetActive(false);
    }

    public async void ShowFactionTurn(string text)
    {
        PhaseTitle.text = text;
        PhaseTitle.transform.parent.gameObject.SetActive(true);

        await Task.Delay(1000);

        PhaseTitle.transform.parent.gameObject.SetActive(false);
    }


    public void UpdateUnitAvaliable(UnitType type, int amount)
    {
        int index = ((int)type / 10) % 10;

        _listOfUnitUI[index].AvaliableText.text = $"Avaliable: <color=green>{amount}</color>";
    }
}

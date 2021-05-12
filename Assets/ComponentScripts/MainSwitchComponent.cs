using TMPro;
using UnityEngine;

public class MainSwitchComponent : MonoBehaviour
{
    public GameObject m_onSwtich;
    public GameObject m_ofSwtich;
    public Canvas m_readingCanvas;
    public TextMeshProUGUI m_Readings;

    public Material m_glowingRedLight;
    public Material m_glowingGreenLight;

    bool CanSelectLever= false;

    public void AtStart()
    {
        SwitchOnMachine();
    }

    public void AtEnd()
    {
        GameEvents.instance.Step3Complete();
    }

    void Start()
    {
        m_Readings.text = "";
        m_readingCanvas.worldCamera = Camera.main.GetComponent<Camera>();
        m_glowingGreenLight.DisableKeyword("_EMISSION");
        m_glowingRedLight.DisableKeyword("_EMISSION");
    }

    void SwitchOnMachine()
    {
        m_ofSwtich.SetActive(false);
        m_onSwtich.SetActive(true);
        m_glowingGreenLight.EnableKeyword("_EMISSION");
        m_glowingRedLight.EnableKeyword("_EMISSION");
        m_Readings.text = "0.0";
        AtEnd();
    }

}

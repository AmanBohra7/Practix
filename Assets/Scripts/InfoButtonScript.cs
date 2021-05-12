using UnityEngine;
using UnityEngine.UI;

public class InfoButtonScript : MonoBehaviour
{
    public GameObject infoPanel;

    Button infoBtn;

    static bool isButtonPressed = false;

    void Start(){
        infoPanel.SetActive(false);
        infoBtn = gameObject.GetComponent<Button>();
        infoBtn.onClick.AddListener(ButtonPressed)  ;
    }

    void ButtonPressed(){
        if(!isButtonPressed) ShowInfoPanel();
        else HideInfoPanel();
        isButtonPressed = !isButtonPressed;
    }

    public void ShowInfoPanel(){
        Time.timeScale = 0f;
        infoPanel.SetActive(true);
    }

    public void HideInfoPanel(){
        Time.timeScale = 1f;
        infoPanel.SetActive(false);
    }

}

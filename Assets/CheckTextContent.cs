using TMPro;
using UnityEngine;

public class CheckTextContent : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    bool isCanvasActive = true;

    // Update is called once per frame
    void Update()
    {
        if(infoText.text == "" && isCanvasActive){
            isCanvasActive = false;
            gameObject.SetActive(false);
        }
    }
}

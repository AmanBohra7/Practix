using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
///
///</summary>
public class MessageScript : MonoBehaviour{
    
    private float animTime = 0.2f;

    void Start(){
        StartCoroutine(MessagePanelAnimation());
    }

    IEnumerator MessagePanelAnimation(){
        yield return new WaitForSeconds(0);

        LeanTween.moveY(
            gameObject.GetComponent<RectTransform>(),
            -60,
            0.2f
        ).setEaseInQuad();


        LeanTween.alphaCanvas(
            gameObject.GetComponent<CanvasGroup>(),
            0,
            0.8f
        ).setDelay(0.8f);

        yield return new WaitForSeconds(1.6f);
        Destroy(gameObject);
    }

    public void ChangeText(string text){
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

}

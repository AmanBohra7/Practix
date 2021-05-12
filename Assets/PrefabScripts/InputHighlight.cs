using UnityEngine.UI;
using UnityEngine;


public class InputHighlight : MonoBehaviour
{
    public Image bgImage;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.name != "DiameterValue")
            bgImage.enabled = false;
        LeanTween.scale(bgImage.GetComponent<RectTransform>(),
            new Vector3(1.4f,1.4f,1.4f),1.2f)
            .setEaseInBack()
            .setLoopPingPong();
    }

    public void StartHighlighter(){
        bgImage.enabled = true;
    }

    public void StopHighlighter(){bgImage.enabled = false;}
}

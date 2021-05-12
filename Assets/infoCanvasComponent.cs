using TMPro;
using UnityEngine;

public class infoCanvasComponent : MonoBehaviour
{
    bool isPopupedUp = false;
    public TextMeshProUGUI m_infoText;
    float anim_timer = 1.5f;

    void Start()
    {
        gameObject.SetActive(false);
        // Invoke("Popup",2.5f);
    }

    public void Popup()
    {
        Invoke("HelperPopup", 0.5f);
    }

    void HelperPopup()
    {
        gameObject.SetActive(true);
        LeanTween.moveLocalY(gameObject, .45f, anim_timer).setEaseOutBack();
        LeanTween.scale(gameObject, new Vector3(.6f, .6f, .6f), anim_timer).setEaseOutBack();
        isPopupedUp = true;
    }

    public void Dropdown()
    {
        LeanTween.moveLocalY(gameObject, -0.036f, anim_timer).setEaseInBack();
        LeanTween.scale(gameObject, new Vector3(.02f, .02f, .02f), anim_timer).setEaseInBack();
        Invoke("SetActiveFalse", anim_timer);
    }

    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public void SetInfoText(string infoText)
    {
        m_infoText.text = infoText;
    }

    void FixedUpdate()
    {
        if (isPopupedUp)
            transform.LookAt(new Vector3(
               Camera.main.transform.position.x,
               this.transform.position.y,
               Camera.main.transform.position.z
           ));
    }

    //  0.86

}

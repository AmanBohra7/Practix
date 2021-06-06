using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
public class RodValueAnimation : MonoBehaviour
{
    public GameObject lengthSprite;
    public GameObject diameterSprite;
    public GameObject lengthValue;
    public GameObject diameterValue;

    List<GameObject> imageList = new List<GameObject>();


    void Start()
    {
        imageList.Add(lengthSprite);
        imageList.Add(diameterSprite);
        imageList.Add(lengthValue);
        imageList.Add(diameterValue);

        foreach (GameObject obj in imageList)
        {
            if (obj.GetComponent<Image>() != null)
            {
                obj.GetComponent<Image>().enabled = false;
            }
            else
            {
                obj.GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }

        // AnimateForValues();

    }


    public void AnimateForValues()
    {
        gameObject.GetComponent<Outline>().enabled = true;

        // LeanTween.move(lengthSprite.GetComponent<RectTransform>(),new Vector3(251.2752f,-297.3809f,0f),1f).setDelay(2f);
        // Invoke("EnableLengthSprite",2f);
        float waitTime = 2.4f;
        // float offSet = 0.2f;
        foreach (GameObject obj in imageList)
        {
            if (obj.GetComponent<Image>() != null)
            {
                StartCoroutine(EnableSprite(obj.GetComponent<Image>(), waitTime));
            }
            else
            {
                StartCoroutine(EnableText(obj.GetComponent<TextMeshProUGUI>(), waitTime));
            }
            // waitTime += offSet;
        }


        LeanTween.moveLocal(gameObject, new Vector3(0.09f, 0.18f, 0.22f), 2f).setEaseOutBack();
        LeanTween.scale(gameObject, new Vector3(.18f, .18f, .18f), 2f).setDelay(.9f).setEaseOutBack();
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f,25f), 1.5f).setDelay(.8f).setEaseOutBack();
    }

    public void AnimateBack()
    {
        // lengthSprite.enabled = false; . . . .....
         foreach (GameObject obj in imageList)
        {
            if (obj.GetComponent<Image>() != null)
            {
                obj.GetComponent<Image>().enabled = false;
            }
            else
            {
                obj.GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
        LeanTween.scale(gameObject, new Vector3(.1f, .1f, .1f), 2f).setEaseInBack();
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0, 0), 1.5f).setDelay(.6f).setEaseInBack();
        LeanTween.moveLocal(gameObject, new Vector3(0.135f, 0.02f, 0.001f), 2f).setDelay(.4f).setEaseInBack();
    }


    IEnumerator EnableSprite(Image img, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        img.enabled = true;
    }

    IEnumerator EnableText(TextMeshProUGUI txt, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        txt.enabled = true;
    }

  
}

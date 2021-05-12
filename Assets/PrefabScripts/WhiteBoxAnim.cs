using UnityEngine.UI;
using UnityEngine;

public class WhiteBoxAnim : MonoBehaviour
{
    public Image innerBox;
    public Image blueLoader;

    float loaderPoppigTime;
    float whiteBoxFillingTime;
    float blueBarFillingTime;

    float offsetForWhiteBoxLoading;
    float offsetForBlueLoading;
    float offsetTransparentAnim;

    float totalAnimTime;

    void Start()
    {

        // animation timers
        loaderPoppigTime = 1f;
        whiteBoxFillingTime = 2f;
        blueBarFillingTime = 4f;


        // offset timers depend uppon animation timers
        offsetForWhiteBoxLoading = loaderPoppigTime ;
        offsetForBlueLoading = loaderPoppigTime + whiteBoxFillingTime;
        offsetTransparentAnim = offsetForWhiteBoxLoading + whiteBoxFillingTime;

        gameObject.transform.localScale = Vector3.zero;
        Vector3 scaleSize = new Vector3(0.5173747f,0.60902f,0.60902f);
        LeanTween.scale(gameObject.GetComponent<RectTransform>(),scaleSize,loaderPoppigTime).setEaseOutBack();

        // white box animation filling
        LeanTween.value(gameObject,UpdateBoxRadialValue,0,1,whiteBoxFillingTime).setDelay(offsetForWhiteBoxLoading);

        //  blue bar filling animation
        LeanTween.value(blueLoader.gameObject,UpdateLoadingRadialValue,0,1,blueBarFillingTime).setDelay(offsetForBlueLoading);

        // white box transparent animation 
        Vector3 tempScale = new Vector3(1.141167f,1.228403f,1.1556f);
        LeanTween.scale(innerBox.GetComponent<RectTransform>(),tempScale,1.5f).setLoopCount(50).setDelay(offsetTransparentAnim);  
        LeanTween.alpha(innerBox.GetComponent<RectTransform>(),0.1f,1.5f).setLoopCount(50).setDelay(offsetTransparentAnim);      


        // Invoke("RollBack",10f);

    }

    public float GetAnimationTime(){
        return 7f;
    }

    void UpdateBoxRadialValue(float val, float ratio){
        gameObject.GetComponent<Image>().fillAmount = val ;
        if(val >= 1) innerBox.gameObject.SetActive(true);
    }

    void UpdateLoadingRadialValue(float val,float rotio){
        blueLoader.fillAmount = val;
    }
    

    public void RollBack(float waitTime){
        Invoke("HellperStopper",waitTime);
    }

    void HellperStopper(){
        gameObject.SetActive(false);
    }

}

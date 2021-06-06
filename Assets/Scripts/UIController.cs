using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [HideInInspector]
    public bool m_ScanningNoticeRunning = true;
    public GameObject m_ScanningNotice;

    [HideInInspector]
    public bool m_RotationNoticeRunning = false;
    public Image m_RotationNotice;

    [HideInInspector]
    public bool m_ScalingNoticeRunning = false;
    public Image m_ScalingNotice;

    [HideInInspector]
    public bool m_TouchNoticeRunning = false;
    public Image m_TouchNotice;

    [HideInInspector]
    public bool m_SizeButtonRunning = false;
    public GameObject m_SizeButton;

    [HideInInspector]
    public bool m_SubmitButtonRunning = false;
    public GameObject m_SubmitButton;

    public TextMeshProUGUI informationText;


    Button m_increment;
    Button m_decrement;

    private void Awake() {
        if(instance != null){
            Destroy(gameObject);
        }else{
            instance = this;
        }
    }

    public void SetInfoText(string text){
        // Debug.Log("SET INFO TEXT function called!");
        informationText.text  = text;
    }

   
    public void TurnoverScanningNotice(){ 
        if(m_ScanningNoticeRunning){
            m_ScanningNotice.SetActive(false);
            m_ScanningNoticeRunning = false; 
        }else{
            m_ScanningNotice.SetActive(true);
            m_ScanningNoticeRunning = true; 
        }
        
    }

    public void TurnoverRotationNotice(){
        if(m_RotationNoticeRunning){
            m_RotationNotice.gameObject.SetActive(false);
            m_RotationNoticeRunning = false;
        }else{
            m_RotationNotice.gameObject.SetActive(true);
            m_RotationNoticeRunning = true;
        }
        
    }

    public void TurnoverScalingNotice(){
        if(m_ScalingNoticeRunning){
            m_ScalingNotice.gameObject.SetActive(false);
            m_ScalingNoticeRunning = false;
        }else{
             m_ScalingNotice.gameObject.SetActive(true);
            m_ScalingNoticeRunning = true;
        }
        
    }

    public void TurnoverTouchNotice(){
        if(m_TouchNoticeRunning){
            m_TouchNoticeRunning = false;
            m_TouchNotice.gameObject.SetActive(false);
        }else{
            m_TouchNoticeRunning = true;
            Invoke("HelperWaitFunctionForTouch",5f);
        }
    }

    public void TurnoverSizeButton(){
        if(m_SizeButtonRunning){
            m_SizeButton.SetActive(false);
            m_SizeButtonRunning = false;
        }else{
            m_SizeButton.SetActive(true);
            m_SizeButtonRunning = true;
        }
    }

    public void TurnoverSubmitButton(){
        if(m_SubmitButtonRunning){
            m_SubmitButton.SetActive(false);
            m_SubmitButtonRunning = false;
        }else{
            m_SubmitButton.SetActive(true);
            m_SubmitButtonRunning = true;
        }
    }

    public void SetSubmitButtonText(string text){
        m_SubmitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Submit";
    }

    void HelperWaitFunctionForTouch(){
        m_TouchNotice.gameObject.SetActive(true);
    }

    public void SetSubmitButtonFunc(string setFor){
        Button submitButtonInstance = m_SubmitButton.GetComponent<Button>();
        if(setFor == "rotation"){
            submitButtonInstance.onClick.AddListener(delegate{SetFunctionHelper(setFor);});
        }
        if(setFor == "scaling"){
            submitButtonInstance.onClick.AddListener(delegate{SetFunctionHelper(setFor);});
        }
    }


    void SetFunctionHelper(string setFor){
        if(setFor == "rotation"){
            GameObject.FindGameObjectWithTag("Machine").GetComponent<RotatorScript>().RotationSet();
            AddSizeButtonListeners();
        }
        if(setFor == "scaling"){
            GameObject.FindGameObjectWithTag("Machine").GetComponent<ScalingScript>().ScalingSet();  
            gameObject.SetActive(false);
        }
    }
    
    void AddSizeButtonListeners(){
        m_increment = m_SizeButton.transform.Find("increment").gameObject.GetComponent<Button>();
        Debug.Log("Increment Button! "+m_increment.gameObject.name);
        m_increment.onClick.AddListener(CallIncrementFunction);
        m_decrement = m_SizeButton.transform.Find("decrement").gameObject.GetComponent<Button>();
        m_decrement.onClick.AddListener(CallDecrementFunction);
    }

    void CallIncrementFunction(){
        GameObject.FindGameObjectWithTag("Machine").GetComponent<ScalingScript>().IncreaseSize();
    }

    void CallDecrementFunction(){
        GameObject.FindGameObjectWithTag("Machine").GetComponent<ScalingScript>().DecreaseSize();
    }
    

}

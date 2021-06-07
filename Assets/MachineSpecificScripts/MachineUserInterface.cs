using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class MachineUserInterface : MonoBehaviour
{
    public Camera m_mainCamera;

    public static MachineUserInterface instance;

    public GameObject m_InfoPointerPrefab;
    // public GameObject m_DoublePointerPrefab;

    GameObject currentArrowPrefab;
    GameObject currentDoubleArrowInstance;

    public GameObject m_VideoPlayer;
    public VideoClip video02;

    public TextMeshProUGUI subtitleText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        // m_VideoPlayer.SetActive(true);
        subtitleText = GameObject.FindGameObjectWithTag("subtitleText").GetComponent<TextMeshProUGUI>();
        UpdateSubtitleText("");
    }

     public void UpdateSubtitleText(string str){
        if(str == ""){
            subtitleText.gameObject.GetComponentInParent<Image>().enabled = false;
        }else{
            subtitleText.gameObject.GetComponentInParent<Image>().enabled = true;
        }
        subtitleText.text = str;
    }

    // public void EnableSubtitleBg(){
    //     subtitleText.gameObject.GetComponentInParent<Image>().enabled = true;
    // }


    public void SetActiveVideoObject(float timeDelay = 0,int num=1)
    {
        StartCoroutine(HellperAbove(timeDelay,num));
    }

    IEnumerator HellperAbove(float timeDelay,int num)
    {
        yield return new WaitForSeconds(timeDelay);
        m_VideoPlayer.GetComponent<VideoProblem>().PlayVideo(num);
    }

    /// <summary>
    /// Instantiate a info pointer at the provided transform
    /// </summary>
    /// <param name="instantiateOverThisObejct">Over this gameobject the pointer is instantiated</param>
    public void InstantiateArrowPrefab(GameObject instantiateOverThisObject, string information, string name = "null")
    {
        Debug.Log("Creating new Instance of Pointer!!!");
        Vector3 instantiatePosition = instantiateOverThisObject.transform.position;
        BoxCollider collider = instantiateOverThisObject.GetComponent<BoxCollider>();
        float upFromCenter;
        if (name == "null")
            upFromCenter = collider.size.y / 2;
        else if(name == "rod"){
            upFromCenter = 0.01f;
        }
        else{
            upFromCenter = 0.07f;
        }

        instantiatePosition.y += upFromCenter;

        GameObject m_InfoPointerPrefabInstance = Instantiate(
            m_InfoPointerPrefab,
            instantiatePosition,
            m_InfoPointerPrefab.transform.rotation
        );

        currentArrowPrefab = m_InfoPointerPrefabInstance;

        // setting camera to the canvas
        m_InfoPointerPrefabInstance.GetComponentInChildren<Canvas>().worldCamera = m_mainCamera;

        // updating text of the info tag
        m_InfoPointerPrefabInstance.GetComponent<InfoPointer>().SetInstruction(information);
    }


    //  currently passing DoubleArrow object as a parameter to the function 
    public void CreateDoubleArrowPoint(GameObject onThisObject)
    {
        currentDoubleArrowInstance = onThisObject;
        currentDoubleArrowInstance.SetActive(true);
    }

    public void DestroyDoubleArrow()
    {
        currentDoubleArrowInstance.SetActive(false);
    }

    public void DestroyArrowPrefab()
    {
        if (currentArrowPrefab)
            Destroy(currentArrowPrefab);
    }


    public void CloseAllUiTabs()
    {
        if (currentArrowPrefab)
            Destroy(currentArrowPrefab);
        DestroyDoubleArrow();
    }



}


//  :: Model related problems 
/* 
- Motor pivot centerize 
- Lever normal recalculate 
- Wire
- Normal map
*/
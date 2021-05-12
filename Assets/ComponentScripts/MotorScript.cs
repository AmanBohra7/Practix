using UnityEngine.UI;
using UnityEngine;

public class MotorScript : MonoBehaviour
{

    MachineUserInterface m_machineUserInterfaceInstace;

    public GameObject directionCanvas;

    Image leftDirection;
    Image rightDirection;

    int DirectionFlowCount = 2;

    bool IsDirectionLoopPlaying = false;

    bool dragging = false;
    Transform toDrag;
    float dist;
    Vector3 offset;

    private Vector3 screenPoint;
    private Vector3 newPose;
    //  TEST -------
    private Vector3 currMousePose;

    // Steps for the starting section of the component
    public void AtStart(){
        TriggerDirection();
        dragging = true;
    }

    // Steps for the end section of teh componenet
    public void AtEnd(){
        dragging  = false;
        GameEvents.instance.Step2Complete();
    }

    void Start()
    {
        m_machineUserInterfaceInstace = MachineUserInterface.instance;
        directionCanvas.SetActive(false);
        leftDirection = directionCanvas.transform.Find("left").GetComponent<Image>();
        rightDirection = directionCanvas.transform.Find("right").GetComponent<Image>();
        leftDirection.fillAmount = 0;
        rightDirection.fillAmount = 0;


        // // TEST CODE - -- - -
        // TriggerDirection();
        // dragging = true;
    }

    void TriggerDirection(){
        IsDirectionLoopPlaying = true;
        directionCanvas.SetActive(true);
        LeanTween.value( gameObject, updateValueExampleCallback, 1f, 100f, 1f).setEase(LeanTweenType.easeOutElastic);
    }

    void updateValueExampleCallback( float val, float ratio ){
        leftDirection.fillAmount += .05f;
        rightDirection.fillAmount += .05f;
    }

    
    
    void OnMouseDown()
    {
        if(!dragging) return;

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.localPosition);
        offset = gameObject.transform.localPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Debug.Log(offset);

        //  TEST -- -
        currMousePose = Input.mousePosition;
    }
      
    // This logic consist for all stopping scene
    void OnMouseDrag()
    {
        if(!dragging) return;

        if(Input.mousePosition.x > currMousePose.x) {
            // Debug.Log("RIGHT TURN");


            // offset = localpose of motor - screen to world pose of current mouse pose
            // Vector3 test  = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)) ;
            // Debug.Log("RIGHT TURN : "+Mathf.Abs(offset.x));
            newPose = new Vector3(
                gameObject.transform.localPosition.x - 0.001f,
                gameObject.transform.localPosition.y,
                gameObject.transform.localPosition.z
            );
            gameObject.transform.localPosition = newPose;
        }
        else if(Input.mousePosition.x < currMousePose.x) {
            // Debug.Log("LEFT TURN");

            Vector3 test  = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)) ;
            // Debug.Log("LEFT TURN : "+Mathf.Abs(offset.x));
            newPose = new Vector3(
                gameObject.transform.localPosition.x + 0.001f,
                gameObject.transform.localPosition.y,
                gameObject.transform.localPosition.z
            );
            gameObject.transform.localPosition = newPose;
        }
        else {
            // Debug.Log("CENTER!");
            return ;
        }


        // Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        // Vector3 curPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)) + offset;
        // curPosition.y = transform.localPosition.y;
        // curPosition.z = transform.localPosition.z;
        // transform.localPosition = curPosition;

        if(transform.localPosition.x >= -0.094f) {
            transform.localPosition = new Vector3(-0.0822f,transform.localPosition.y,transform.localPosition.z);
            m_machineUserInterfaceInstace.DestroyArrowPrefab();
            gameObject.GetComponent<Outline>().enabled = false;
            directionCanvas.SetActive(false);
            AtEnd();
        } 

        // -0.0822 correct || -0.094
        // max: -.19
    }

    void FixedUpdate(){
        if(dragging){
             if(transform.localPosition.x < -.19)
                transform.localPosition = new Vector3(-.19f, transform.localPosition.y, transform.localPosition.z);
        }
    }
    

}

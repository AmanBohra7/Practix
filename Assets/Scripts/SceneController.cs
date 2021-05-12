 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

#if UNITY_EDITOR
     using Input = GoogleARCore.InstantPreviewInput;
#endif

public class SceneController : MonoBehaviour
{

    private int screenHeight;
    private int screenWidth;

    private bool m_PrefabObjectCreated = false;
    private bool m_MachinePlaced = false;

    public GameObject m_PrefabObject;
    private GameObject m_TrackerObjectInstance;

    public GameObject m_MachinePrefab;
    private GameObject m_MachineObject;

    Anchor m_MachineAnchor = null;

    public Camera FirstPersonCamera;
    // Start is called before the first frame update

    UIController uiControllerInstance ;

    GameObject m_MachineObjectInstance;

    void Start(){
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        QuitOnConnectionErrors();
        uiControllerInstance = UIController.instance;
        uiControllerInstance.SetInfoText("Scan for flat surface!");
    }

    
    public void CallForNextState(int nextStateNumber){
        if(nextStateNumber == 3){
            m_MachineObjectInstance = GameObject.FindGameObjectWithTag("Machine");
            m_MachineObjectInstance.GetComponent<RotatorScript>().enabled = false;
            uiControllerInstance.TurnoverRotationNotice();
            uiControllerInstance.TurnoverScalingNotice();
        }
        else if(nextStateNumber == 4){
            // stopping the scalling state 
            uiControllerInstance.TurnoverSubmitButton();
            uiControllerInstance.TurnoverSizeButton();
            m_MachineObjectInstance = GameObject.FindGameObjectWithTag("Machine");
            m_MachineObjectInstance.GetComponent<ScalingScript>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Session.Status != SessionStatus.Tracking){
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }else{
            if(uiControllerInstance.m_ScanningNoticeRunning && m_PrefabObjectCreated) 
                uiControllerInstance.TurnoverScanningNotice();
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Performing STEP1 

        Vector2 centerPose = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f);
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;
            if(!m_MachinePlaced)
                if(Frame.Raycast(centerPose.x, centerPose.y, raycastFilter, out hit)){
                    if(hit.Trackable is DetectedPlane){

                        if(!m_PrefabObjectCreated){
                            m_TrackerObjectInstance = Instantiate(m_PrefabObject,
                                        Vector3.zero,
                                        m_PrefabObject.transform.rotation);
                            m_TrackerObjectInstance.transform.position =
                                    hit.Pose.position;
                            m_PrefabObjectCreated = true;
                            uiControllerInstance.SetInfoText("Tap on screen to place machine 3D model");
                        }else{
                            if(!m_MachinePlaced){
                                m_TrackerObjectInstance.transform.position =
                                        hit.Pose.position;   
                            }   
                        }
                        // if(!uiControllerInstance.m_TouchNoticeRunning && !m_MachinePlaced)
                        //         uiControllerInstance.TurnoverTouchNotice();
                       


                        if(Input.touchCount > 0 && m_PrefabObjectCreated && !m_MachinePlaced){
                            
                            m_MachinePlaced = true;
                            // if(uiControllerInstance.m_TouchNoticeRunning)
                            //     uiControllerInstance.TurnoverTouchNotice();
                            uiControllerInstance.SetInfoText("Swipe and adjust rotation of Machine");

                             Vector3 updateTransform = new Vector3(
                                hit.Pose.position.x,
                                hit.Pose.position.y + 0.3f,
                                hit.Pose.position.z
                            );
                            m_MachineObject = Instantiate(m_MachinePrefab,
                                    updateTransform,
                                    m_MachinePrefab.transform.rotation);
                           
                            m_MachineAnchor = hit.Trackable.CreateAnchor(hit.Pose);
                            m_MachineAnchor.tag = "MachineAnchor";
                            m_MachineObject.transform.parent = m_MachineAnchor.transform;
                            Destroy(m_TrackerObjectInstance);
                        }
                    }
                }
    }

    void QuitOnConnectionErrors()
    {
        if (Session.Status ==  SessionStatus.ErrorPermissionNotGranted)
        {
            Debug.Log(
                "Camera permission is needed to run this application.");
        }
        else if (Session.Status.IsError())
        {

            Debug.Log(
                "ARCore encountered a problem connecting. Please restart the app.");
        }
    }



}

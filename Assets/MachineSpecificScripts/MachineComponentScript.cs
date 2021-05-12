using UnityEngine;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif


public class MachineComponentScript : MonoBehaviour
{
    
    MachineUserInterface m_machineUserInterfaceInstace;

    // All the interactive Machine Model parts are declared here!
    public GameObject m_RodModel;
    public GameObject m_Motor;
    public GameObject m_Switch;
    public GameObject m_Lever;

    public GameObject m_DoubleArrowRef;

    public GameObject m_WhiteBoardObject;
    public GameObject m_BoadPointer;

    UIController uiInstance;

    // Set this variable as currnt machine model
    GameObject m_userSelectionObject;

    // Bool used for update logic
    bool m_IsUserSelectingObject = false;
    bool m_IsUserDraggingObject = false;


    // Some different variable for dragging logic [Testing]
    bool dragging = false;
    float dist;
    Vector3 newPose, offset;


    void Start()
    {
        m_machineUserInterfaceInstace = MachineUserInterface.instance;
        uiInstance = UIController.instance;

        // Disabling White Board object in starting 
        m_WhiteBoardObject.SetActive(false);

        m_DoubleArrowRef = GameObject.FindGameObjectWithTag("DoubleArrowPointer");
        // Debug.Log( m_DoubleArrowRef.transform.gameObject.name);
        m_DoubleArrowRef.SetActive(false);
    }

    
    public void EndOfMachineSteps(){
        // Debug.Log("We have reached the end of the machien walkthrough!");
    }

    public void GetRodInput(){
        // Enabling White board object in first STEP of walkthrough
        m_WhiteBoardObject.SetActive(true);
        m_BoadPointer.SetActive(true);
        m_RodModel.GetComponent<RodValueAnimation>().AnimateForValues();
    }


    public void OnRodInputComplete(){
        m_RodModel.GetComponent<RodValueAnimation>().AnimateBack();
    }


    // STEP1:: Pointing towards rod and starting tap and select logic
    public void InitializeStep1()
    {

        // Setting up current working model
        m_userSelectionObject = m_RodModel;

        // Calling for its look up guide
        string info = "Tap and select the rod";
        m_machineUserInterfaceInstace.InstantiateArrowPrefab(m_userSelectionObject,info,"rod");

        // Setting up corresponding boolean
        m_IsUserSelectingObject = true;

        // Enabling outline
        m_userSelectionObject.GetComponent<Outline>().enabled = true;

        // remove pointer from this sections [specfic for 0 step setup]
        m_BoadPointer.SetActive(false);

    }

    // STEP2:: Pointing towards motor object and adding directional instructiosn 
    public void InitializeStep2()
    {
        // Setting up cuurent working model
        m_userSelectionObject = m_Motor;

        // Calling for its look up guide
        string info = "Drag motor and adjust accordingly";
        m_machineUserInterfaceInstace.InstantiateArrowPrefab(m_userSelectionObject,info);

        // Setting up corresponding vairable
        // m_IsUserDraggingObject = true;

        // enabling outline
        m_userSelectionObject.GetComponent<Outline>().enabled = true;

        // Calling for start function
        m_userSelectionObject.GetComponent<MotorScript>().AtStart();
    }

    // STEP3:: Switching on the machine 
    public void InitializeStep3()
    {
        // Setting up cuurent working model
        m_userSelectionObject = m_Switch;

        // Calling for its look up guide
        string info = "Switch on the machine";
        m_machineUserInterfaceInstace.InstantiateArrowPrefab(m_userSelectionObject,info);

        // Setting up corresponding boolean
        m_IsUserSelectingObject = true;

        // enabling outline
        m_userSelectionObject.GetComponent<Outline>().enabled = true;

    }   

    // STEP4:: Dragging lever to right and holder animation
    public void InitializeStep4()
    {
        // Setting up cuurent working model
        m_userSelectionObject = m_Lever;

        // Setting up corresponding vairable
        // m_IsUserDraggingObject = true;

        // enabling outline
        m_userSelectionObject.GetComponent<Outline>().enabled = true;

        // Calling for start function
        m_userSelectionObject.GetComponent<LeverScript02>().AtStart(m_DoubleArrowRef);

    }

    //  STEP5:: Rod Braking effect
    public void InitializeStep5()
    {
        // Debug.Log("STEP 5 CALLED SUCCESSFULLYYY!");
        m_userSelectionObject = m_RodModel;
        m_userSelectionObject.GetComponent<MainRodComponent>().BreakRod(1.2f);
    }


    // Custom script to call according to the componentName
    void callForRequiredScriptEnds(string componentName)
    {
        if (componentName == "broken_rod_with_parent"){
            m_userSelectionObject.GetComponent<MainRodComponent>().AtEnd();
            // m_InfoCanvasObject.GetComponent<infoCanvasComponent>().Dropdown();
        }
            
        if (componentName == "MainSwitch")
        {
            m_userSelectionObject.GetComponent<Outline>().enabled = false;
            m_userSelectionObject.GetComponent<MainSwitchComponent>().AtStart();
        }
    }

    void Update()
    {

        // This is for selecting object 
        if (m_IsUserSelectingObject)
        {
            if (Input.GetMouseButton(0))
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(raycast, out raycastHit))
                {
                    if (raycastHit.collider.name == m_userSelectionObject.name)
                    {
                        // Debug.Log("Tapped on : " + m_userSelectionObject.name);
                        m_IsUserSelectingObject = false;
                        m_machineUserInterfaceInstace.DestroyArrowPrefab();
                        callForRequiredScriptEnds(m_userSelectionObject.name);
                    }
                }

            }
        } // end if

    }

}

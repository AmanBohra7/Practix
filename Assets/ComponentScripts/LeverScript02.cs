using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class LeverScript02 : MonoBehaviour
{
    // machine user interface ref
    MachineUserInterface ui_interface;

    public GameObject m_LeverObj;
    public GameObject m_HolderObj;

    GameObject m_BoardPointer;
    public GameObject m_WhiteBox;
    public GameObject m_WhiteBox2;

    public GameObject m_EndDisplay;

    //  starting from 0 steps [Example: 3 -> means total 4 steps]
    int totalSteps = 1;
    int currentStep = 0;

    bool m_StepInitialized = false;
    bool m_CanTapOnLever = true;

    // Some variable for future removal
    Animator leverAnimator;
    GameObject doubleArrowRef;

    public TextMeshProUGUI m_Reading;

    void Start()
    {
        m_BoardPointer = GameObject.FindGameObjectWithTag("BoardPointer");
        m_BoardPointer.SetActive(false);

        m_WhiteBox.SetActive(false);
        m_WhiteBox2.SetActive(false);

        ui_interface = MachineUserInterface.instance;
        // testing video section 

    }

    public void AtStart(GameObject test)
    {

        m_StepInitialized = true;

        string info = "Drag lever to forward direction!";
        ui_interface = MachineUserInterface.instance;
        ui_interface.InstantiateArrowPrefab(m_LeverObj, info, "lever");

        // assign lever animator
        leverAnimator = m_LeverObj.GetComponent<Animator>();

        doubleArrowRef = test;
    }

    public void AtEnd()
    {
        m_CanTapOnLever = false;
        gameObject.GetComponent<Outline>().enabled = false;
        GameEvents.instance.Step4Complete();
        m_StepInitialized = false;
    }

    void OnMouseDown()
    {

        if (!m_StepInitialized) return;

        if (!m_CanTapOnLever) return;

        m_CanTapOnLever = false;

        switch (currentStep)
        {
            case 0:
                StartCoroutine(LeverStep1());
                break;
            case 1:
                StartCoroutine(LeverStep2());
                break;
            default:
                Debug.Log("You reached default in LeverScript02 OnMouseDown()!");
                break;
        }

    }

    IEnumerator PlayCutSceneVideo(int num = 1)
    {
        if (num == 1)
            m_WhiteBox.SetActive(true);
        else
            m_WhiteBox2.SetActive(true);
        // float waitTime = m_WhiteBox.GetComponentInChildren<WhiteBoxAnim>().GetAnimationTime();
        float waitTime = 7f;
        ui_interface.SetActiveVideoObject(waitTime, num);

        if (num == 1)
            m_WhiteBox.GetComponent<WhiteBoxAnim>().RollBack(waitTime + 0.5f);
        else
        {
            m_WhiteBox2.GetComponent<WhiteBoxAnim>().RollBack(waitTime + 0.5f);
            // waitTime += 2f;
        }



        yield return new WaitForSeconds(waitTime + 9f);
    }


    // called when case 0
    IEnumerator LeverStep1()
    {
        currentStep += 1;
        ui_interface.DestroyArrowPrefab();
        yield return StartCoroutine(PlayCutSceneVideo());
        //  Animation part of the step
        LeverMovement("forward", 1.5f);
        HolderRotation(90.0f, 1.5f, 1.5f);
        LeverMovement("backword", 1.5f, 3f);

        StartCoroutine(HelperCallDoubleArrow(3f));
        m_Reading.text = "403"+" kg.cm";


        // Change summary text after view is played
        string sub = "Take readings of torque and angle of twist. Note down those values on the adjacent board.";
        ui_interface.UpdateSubtitleText(sub);
    }


    // called when case  1 || only called when t_01 and t_02 are entered
    IEnumerator LeverStep2()
    {
        yield return new WaitForSeconds(0);
        currentStep += 1;

        ui_interface.DestroyArrowPrefab();

        yield return StartCoroutine(PlayCutSceneVideo(2));

        LeverMovement("forward", 1.5f);
        HolderRotation(45f, 1.5f, 1.5f);
        LeverMovement("backword", 1.5f, 3f);

        // m_BoardPointer.SetActive(false);

        StartCoroutine(HelperCallDoubleArrow(3f));
        m_Reading.text = "980"+" kg.cm";

    }



    bool test01 = false;
    bool test02 = false;
    bool test03 = false;
    void FixedUpdate()
    {
        if (InputManager.inputOneCompleted && !test01)
        {
            test01 = true;
            m_CanTapOnLever = true;

            m_BoardPointer.SetActive(false);

            ui_interface.DestroyDoubleArrow();
            string info = "Drag lever to forward direction!";
            // ui_interface = MachineUserInterface.instance;
            ui_interface.InstantiateArrowPrefab(m_LeverObj, info, "lever");

             // Change summary text after view is played
            string sub = "Now continue applying torque till the specimen fractures and take readings at a certain interval.";
            ui_interface.UpdateSubtitleText(sub);

        }

        if (InputManager.inputTwoCompleted && !test02)
        {
            test02 = true;
            m_CanTapOnLever = true;

            m_BoardPointer.SetActive(false);

            ui_interface.DestroyDoubleArrow();
            // string info = "Drag lever to forward direction!";
            // ui_interface = MachineUserInterface.instance;
            // ui_interface.InstantiateArrowPrefab(m_LeverObj, info);

        }

        if (InputManager.reachedMachineEnd && !test03)
        {
            Debug.Log("End of lever scrip 02");
            test03 = true;
            ui_interface.CloseAllUiTabs();
            
            m_BoardPointer.SetActive(false);


            // call for the end information panel !!
            // string txt = "You have reached to the END! Thank you!";
            // m_EndDisplay.GetComponent<infoCanvasComponent>().SetInfoText(txt);
            m_EndDisplay.GetComponent<infoCanvasComponent>().Popup();
        }

    }


    /// <summary>
    /// Calling function for Lever Animation
    /// </summary>
    void LeverMovement(string direction, float animTime, float delay = 0)
    {
        StartCoroutine(CoLeverMovement(direction, animTime, delay));
    }

    void HolderRotation(float angle, float animTime, float delay = 0)
    {
        m_HolderObj.GetComponent<Outline>().enabled = true;
        StartCoroutine(CoHoldeRotation(angle, animTime, delay));
    }


    IEnumerator CoHoldeRotation(float angle, float animTime, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        LeanTween.rotateAroundLocal(m_HolderObj, Vector3.left, angle, animTime);
        StartCoroutine(HelperOutlineDisable(animTime));
        if (currentStep > totalSteps) AtEnd();
    }


    // has to update code with LeanTween for ease animTime delay use
    IEnumerator CoLeverMovement(string direction, float animTime, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if (direction == "forward")
        {
            leverAnimator.Play("LeverForwordAnimation");
        }
        else
        {
            leverAnimator.Play("LeverBackwordAnimation");
        }
    }

    IEnumerator HelperOutlineDisable(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_HolderObj.GetComponent<Outline>().enabled = false;
    }

    IEnumerator HelperCallDoubleArrow(float delay)
    {
        yield return new WaitForSeconds(delay);
        ui_interface.CreateDoubleArrowPoint(doubleArrowRef);

        // new lines
        if (currentStep == 1)
        {
            InputManager.instance.HighlightInputField("t_01");

        }

        if (currentStep == 2)
            InputManager.instance.HighlightInputField("t_02");

        m_BoardPointer.SetActive(true);
    }

}

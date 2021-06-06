using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepWiseGuideController : MonoBehaviour
{

    public static StepWiseGuideController instance;

    public static int StepState = 0;

    GameObject MachineObject;
    MachineComponentScript m_machineComponentScriptInstance;

    MachineUserInterface m_MachineInstanceUI;


    private void Awake() {
        if(instance != null){
            Destroy(gameObject);
        }else{
            instance = this;
        }
    }

    void Start(){

        // subsrcibing events

        GameEvents.instance.onRodInputsComplete += HelperCallforStep1;
        GameEvents.instance.onStep1Complete += CallforStep2;
        GameEvents.instance.onStep2Complete += CallforStep3;
        GameEvents.instance.onStep3Complete += CallforStep4;
        GameEvents.instance.onStep4Complete += CallforStep5;

        GameEvents.instance.onMachineStepsCompleted += CallEnd;

        MachineObject = GameObject.FindGameObjectWithTag("Machine");
        // CallforStep1();

        m_machineComponentScriptInstance = MachineObject.GetComponent<MachineComponentScript>();
        m_MachineInstanceUI = MachineUserInterface.instance;


        StartCoroutine(callForRodInputStep());

        // enable parent image for subtitle text bg
        m_MachineInstanceUI.EnableSubtitleBg();

        string sub = "Objective of this experiment is to conduct torsion test on a specimen to determine modulus of rigidity.";
        m_MachineInstanceUI.UpdateSubtitleText(sub);
    }


    void CallEnd(){
        m_machineComponentScriptInstance.EndOfMachineSteps();
    }

    IEnumerator callForRodInputStep(){
        yield return new WaitForSeconds(5f);
        string sub = " The specimen is a circular rod made up of aluminium alloy material. \n Its gauge length is 110mm and its radius is 7.5mm. \n Note down the gauge length and diameter of the specimen on the board.";
        m_MachineInstanceUI.UpdateSubtitleText(sub);
        m_machineComponentScriptInstance.GetRodInput();
    }


    // current Adding function for break point to give time for rod to goBack to pose
    void HelperCallforStep1(){
        m_machineComponentScriptInstance.OnRodInputComplete();
        Invoke("CallforStep1",2.7f);
    }


    // pick and animate rod to position
    public void CallforStep1(){
        StepState = 1;  
        m_machineComponentScriptInstance.InitializeStep1();
    }

    // drag and place the motor
    public void CallforStep2(){
        StepState = 2;
        m_machineComponentScriptInstance.InitializeStep2();
    }

    // switch on the button and lights glows
    public void CallforStep3(){
        StepState = 3;
        m_machineComponentScriptInstance.InitializeStep3();
    }
    
    public void CallforStep4(){
        StepState = 4;
        m_machineComponentScriptInstance.InitializeStep4();
    }
    
    public void CallforStep5(){
        StepState = 5;
        m_machineComponentScriptInstance.InitializeStep5();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepWiseGuideController : MonoBehaviour
{

    public static StepWiseGuideController instance;

    public static int StepState = 0;

    GameObject MachineObject;
    MachineComponentScript m_machineComponentScriptInstance;


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
        callForRodInputStep();
        
    }


    void CallEnd(){
        m_machineComponentScriptInstance.EndOfMachineSteps();
    }

    void callForRodInputStep(){
        m_machineComponentScriptInstance = MachineObject.GetComponent<MachineComponentScript>();
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

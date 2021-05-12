using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScalingScript : MonoBehaviour
{
    GameObject anchorInstance;

    UIController m_UIControllerInstance;

    Vector3 sizeOffSet = new Vector3(0.25f,0.25f,0.25f);

    GameObject machineGuideObject;

    void Start(){

        m_UIControllerInstance = UIController.instance;
        m_UIControllerInstance.TurnoverSizeButton();
        m_UIControllerInstance.SetSubmitButtonText("Confirm");
        m_UIControllerInstance.SetSubmitButtonFunc("scaling");
        m_UIControllerInstance.SetInfoText("Incrase and adjust machine size");

        anchorInstance = GameObject.FindGameObjectWithTag("MachineAnchor");
        
    
        GameObject arrowObject = GameObject.FindGameObjectWithTag("Rotation_arrow");
        arrowObject.SetActive(false);
        

        gameObject.GetComponent<RotatorScript>().enabled = false;

        machineGuideObject = GameObject.FindGameObjectWithTag("MachineGuide");

    }

    public void IncreaseSize(){
        anchorInstance.transform.localScale += sizeOffSet;
    }

    public void DecreaseSize(){
        anchorInstance.transform.localScale -= sizeOffSet;
    }

    public void ScalingSet(){ 
        m_UIControllerInstance.TurnoverSizeButton();
        m_UIControllerInstance.TurnoverSubmitButton();
        m_UIControllerInstance.SetInfoText("");
        machineGuideObject.GetComponent<StepWiseGuideController>().enabled = true;
        gameObject.GetComponent<ScalingScript>().enabled = false;
    }

}

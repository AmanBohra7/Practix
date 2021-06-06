 using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{

    public static GameEvents instance;

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

    public event Action onRodInputsComplete;

    public event Action onStep1Complete;
    public event Action onStep2Complete;
    public event Action onStep3Complete;
    public event Action onStep4Complete;


    public event Action onMachineStepsCompleted;

    public void MachineStepsCompleted(){
        if(onMachineStepsCompleted != null){
            onMachineStepsCompleted();
        }
    }

    public void RodInputCompleted(){
        if(onRodInputsComplete != null){
            onRodInputsComplete();
        }
    }

    public void Step1Complete()
    {
        if (onStep1Complete != null)
        {
            onStep1Complete();
        }
    }

    public void Step2Complete()
    {
        if (onStep2Complete != null)
        {
            onStep2Complete();
        }
    }

    public void Step3Complete()
    {
        if (onStep3Complete != null)
        {
            onStep3Complete();
        }
    }

    public void Step4Complete()
    {
        if (onStep4Complete != null)
        {
            onStep4Complete();
        }
    }

}

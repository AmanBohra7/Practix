using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;

public class InputManager : MonoBehaviour
{

    class BoardInput
    {
        public TMP_InputField inputField;
        public bool valueStored;
        public BoardInput(TMP_InputField inf, bool val)
        {
            this.inputField = inf;
            this.valueStored = val;
        }
        public void setValueStored(bool val)
        {
            this.valueStored = val;
            // Debug.Log("TESTING: " + this.valueStored);
        }
    };


    public static InputManager instance;
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

    Dictionary<string, BoardInput> inputFieldHolder = new Dictionary<string, BoardInput>();

    public TMP_InputField t_01;
    public TMP_InputField t_02;

    public TMP_InputField angle_01;
    public TMP_InputField angle_02;

    public TMP_InputField g_01;
    public TMP_InputField g_02;

    public TMP_InputField diameter;
    public TMP_InputField length;

    public static bool inputOneCompleted = false;
    public static bool inputTwoCompleted = false;
    public static bool diameterValueStored = false;
    public static bool lengthValueStored = false;

    public static bool reachedMachineEnd = false;


    double m_Diameter = 0.5;
    double m_Length = 0.1;

    void Start()
    {

        inputFieldHolder.Add("t_01", new BoardInput(t_01, false));
        inputFieldHolder.Add("angle_01", new BoardInput(angle_01, false));

        inputFieldHolder.Add("t_02", new BoardInput(t_02, false));
        inputFieldHolder.Add("angle_02", new BoardInput(angle_02, false));

        inputFieldHolder.Add("diameter", new BoardInput(diameter, false));
        inputFieldHolder.Add("length", new BoardInput(length, false));


        if(UserConnection.Instance.isUserDataRecieved){
            AddPreviousUserExp(UserConnection.Instance.userData);
        }
    
    }

    private void AddPreviousUserExp(JSONNode userData)
    {
        inputFieldHolder["t_01"].inputField.text = userData["values"]["expValues"]["row01"]["torque"];
        inputFieldHolder["t_02"].inputField.text = userData["values"]["expValues"]["row02"]["torque"];

        inputFieldHolder["angle_01"].inputField.text = userData["values"]["expValues"]["row01"]["angleOfTwist"];
        inputFieldHolder["angle_02"].inputField.text = userData["values"]["expValues"]["row02"]["angleOfTwist"];

        length.text = userData["values"]["rodInfo"]["length"];
        diameter.text = userData["values"]["rodInfo"]["diameter"];


        g_01.text = userData["values"]["expValues"]["row01"]["modulusG"];
        g_02.text = userData["values"]["expValues"]["row02"]["modulusG"];
    }



    public void StoreDiameterValue()
    {
        m_Diameter = double.Parse(diameter.text);
        HighlightInputField("length");
        diameter.GetComponent<InputHighlight>().StopHighlighter();
    }

    public void StoreLengthValue()
    {
        m_Length = double.Parse(length.text);
        length.GetComponent<InputHighlight>().StopHighlighter();
        GameEvents.instance.RodInputCompleted();
        // Debug.Log("Both Values Recieved!");
    }

    public void InputTaken(string inputName)
    {
        inputFieldHolder[inputName].setValueStored(true);
        // Debug.Log("SET: " + inputFieldHolder[inputName].valueStored);
        inputFieldHolder[inputName].inputField.GetComponent<InputHighlight>().StopHighlighter();
        if (inputName == "t_01")
            inputFieldHolder["angle_01"].inputField.GetComponent<InputHighlight>().StartHighlighter();
        if (inputName == "t_02")
            inputFieldHolder["angle_02"].inputField.GetComponent<InputHighlight>().StartHighlighter();

        // placing end logic currently in InputTaken function 
        if(inputName == "angle_02"){
            if(inputFieldHolder["angle_01"].inputField.text == "") return;
            reachedMachineEnd = true;

            UpdatedUserData();
            
            GameEvents.instance.MachineStepsCompleted();
            
        }
    
    }

    private void UpdatedUserData()
    {
        UserExp userexp = new UserExp();
        userexp.SetValues(
            float.Parse(length.text),
            float.Parse(diameter.text),
            float.Parse(inputFieldHolder["t_01"].inputField.text),
            float.Parse(inputFieldHolder["angle_01"].inputField.text),
            float.Parse(g_01.text),
            float.Parse(inputFieldHolder["t_02"].inputField.text),
            float.Parse(inputFieldHolder["angle_02"].inputField.text),
            float.Parse(g_02.text)
        );

        UserConnection.Instance.UpdateUserExp(userexp);
    }

    public void HighlightInputField(string name)
    {
        // Debug.Log("Highlighter called for : "+name);
        inputFieldHolder[name].inputField.GetComponent<InputHighlight>().StartHighlighter();
    }


    float CalcuateModulus(double t, double a)
    {
        double j = (3.14 / 32) * (m_Diameter * m_Diameter * m_Diameter * m_Diameter);
        double ret = (t * m_Length) / (j * a);
        float ans = (float)Math.Round(ret, 2);
        return ans;
    }

    void FixedUpdate()
    {
        if (inputFieldHolder["t_01"].valueStored && inputFieldHolder["angle_01"].valueStored && !inputOneCompleted)
        {
            inputOneCompleted = true;
            g_01.text = CalcuateModulus(double.Parse(t_01.text), double.Parse(angle_01.text)).ToString();
            // Debug.Log("Input one completed!" + g_01.text);
        }
        if (inputFieldHolder["t_02"].valueStored && inputFieldHolder["angle_02"].valueStored && !inputTwoCompleted)
        {
            inputTwoCompleted = true;
            // Debug.Log("Input one completed!");
            g_02.text = CalcuateModulus(double.Parse(t_02.text), double.Parse(angle_02.text)).ToString();
        }
    }

}

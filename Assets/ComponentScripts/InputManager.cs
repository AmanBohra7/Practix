using UnityEngine;
using TMPro;
using System.Collections.Generic;
using SimpleJSON;

public class InputManager : MonoBehaviour
{

    // machine user interface ref
    MachineUserInterface ui_interface;

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

    public TMP_InputField modulusG;

    public TMP_InputField diameter;
    public TMP_InputField length;

    public static bool inputOneCompleted = false;
    public static bool inputTwoCompleted = false;
    public static bool diameterValueStored = false;
    public static bool lengthValueStored = false;

    public static bool reachedMachineEnd = false;


    float m_Diameter = 0.5f;
    float m_Length = 0.1f;

    void Start()
    {

        inputFieldHolder.Add("t_01", new BoardInput(t_01, false));
        inputFieldHolder.Add("angle_01", new BoardInput(angle_01, false));

        inputFieldHolder.Add("t_02", new BoardInput(t_02, false));
        inputFieldHolder.Add("angle_02", new BoardInput(angle_02, false));

        inputFieldHolder.Add("diameter", new BoardInput(diameter, false));
        inputFieldHolder.Add("length", new BoardInput(length, false));

        ui_interface = MachineUserInterface.instance;
        Debug.Log("Testing : "+ui_interface.gameObject.name);

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


        modulusG.text = System.Math.Round(float.Parse(userData["values"]["expValues"]["modulusG"]),2 ).ToString();
        // g_02.text = userData["values"]["expValues"]["row02"]["modulusG"];
    }



    public void StoreDiameterValue()
    {
        m_Diameter = float.Parse(diameter.text);
        HighlightInputField("length");
        diameter.GetComponent<InputHighlight>().StopHighlighter();
    }

    public void StoreLengthValue()
    {
        m_Length = float.Parse(length.text);
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

        modulusG.text = CalcuateModulus(float.Parse(t_01.text), 
                float.Parse(t_02.text),
                float.Parse(angle_01.text),
                float.Parse(angle_02.text)
                ).ToString();

        userexp.SetValues(
            float.Parse(length.text),
            float.Parse(diameter.text),
            float.Parse(inputFieldHolder["t_01"].inputField.text),
            float.Parse(inputFieldHolder["angle_01"].inputField.text),
            float.Parse(inputFieldHolder["t_02"].inputField.text),
            float.Parse(inputFieldHolder["angle_02"].inputField.text),
            float.Parse(modulusG.text)
        );

        Debug.Log("UserExp value in inputmanager : " + float.Parse(modulusG.text) + " - " + userexp.values.expValues.modulusG.ToString());
        UserConnection.Instance.UpdateUserExp(userexp);
    }

    public void HighlightInputField(string name)
    {
        // Debug.Log("Highlighter called for : "+name);
        inputFieldHolder[name].inputField.GetComponent<InputHighlight>().StartHighlighter();
    }


    float CalcuateModulus(float t1,float t2, float q1,float q2)
    {
        // q1 = 270 , q2 = 180
        q2 += 360;
        // q2 = 540
        // t1 = 403 , t2 = 980
        float num = 5731 * (t2-t1) * m_Length;
        float din  = Mathf.Pow(m_Diameter,4) * (q2-q1);
        float cal_g = num / din;

        float exp_g = 27;

        if( Mathf.Abs(exp_g - cal_g) > 1 ){
            AudioManager.Instance.PlayAudio(10);
            Debug.LogWarning("Calculated value is very different!");
            string sub = "You have found the wrong value of modulus of rigidity. Please try to perform the experiment again.";
            ui_interface.UpdateSubtitleText(sub);
        }else{
            AudioManager.Instance.PlayAudio(9);
            string sub = "You have found the correct value of modulus of rigidity. You have successfully completed the experiment.";
            ui_interface.UpdateSubtitleText(sub);
        }

        Debug.Log("Calculated G : "+cal_g.ToString());
        Debug.Log("Actual G : "+exp_g.ToString());
        return (float)System.Math.Round(cal_g,2);
    }

    void FixedUpdate()
    {
        if (inputFieldHolder["t_01"].valueStored && inputFieldHolder["angle_01"].valueStored && !inputOneCompleted){
            inputOneCompleted = true;
            // modulusG.text = CalcuateModulus(float.Parse(t_01.text), float.Parse(angle_01.text)).ToString();
            // Debug.Log("Input one completed!" + modulusG.text);
        }
        if (inputFieldHolder["t_02"].valueStored && inputFieldHolder["angle_02"].valueStored && !inputTwoCompleted){
            inputTwoCompleted = true;
            // Debug.Log("Input one completed!");
            modulusG.text = CalcuateModulus(float.Parse(t_01.text), 
                float.Parse(t_02.text),
                float.Parse(angle_01.text),
                float.Parse(angle_02.text)
                ).ToString();
        }
    }

}

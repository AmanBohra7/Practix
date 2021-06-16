using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
///
///</summary>
public class UserConnection : MonoBehaviour{
    
    public static UserConnection Instance;

    private string LOGIN_URL = "https://sanmatilabs.herokuapp.com/userlogin";
    private string SIGNUP_URL = "https://sanmatilabs.herokuapp.com/createuser";
    private string ADD_USEREXP_URL = "https://sanmatilabs.herokuapp.com/userdata";
    private string GET_USEREXP_URL = "https://sanmatilabs.herokuapp.com/userdata";

    [HideInInspector]
    public string USERID;

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;


    [HideInInspector]
    public bool isUserDataRecieved;
    public JSONNode userData;

    public GameObject loadingSign;
    public GameObject loginBtn;


    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
    }


    void Start(){
        // StartCoroutine(SendUserExp());

        // StartCoroutine(GetUserExp());
        loadingSign.SetActive(false);
        loginBtn.SetActive(true);
    }

    IEnumerator CreateUser(){
        using(UnityWebRequest www = UnityWebRequest.Get(SIGNUP_URL) ){
            
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError){
                Debug.Log(www.error);
                yield break;
            }

            JSONNode jsonData = JSON.Parse(www.downloadHandler.text);

            Debug.Log("DATA : "+jsonData["UserData"]);
            // Debug.Log(www.downloadHandler.text);

        }
    }


    public void LoginBtnPressed(){

        // check if input fields are empty or not
        if(emailInput.text.Length == 0
            || passwordInput.text.Length == 0) {
                Debug.LogWarning("Empty Field!");
                return;}
        
        loadingSign.SetActive(true);
        loginBtn.SetActive(false);

        string email = emailInput.text;
        string password = passwordInput.text;

        StartCoroutine(Login(email,password));

    }

    class User{
        public string email;
        public string password;
    }


    IEnumerator Login(string email,string pass){
        
        User user = new User();
        user.email = email;
        user.password = pass;
        string sendData = JsonUtility.ToJson(user);
        // print(sendData);

        using (UnityWebRequest www = UnityWebRequest.Put(LOGIN_URL, sendData))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type","application/json");
            www.SetRequestHeader("Accept","application/json");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                // Debug.Log(www.error);
                if(www.responseCode == 401){
                    Debug.Log("No user with this email!");
                    StartCoroutine(instantiateMessage("Incorrect Email!"));
                    loadingSign.SetActive(false);
                    loginBtn.SetActive(true);
                    emailInput.text = "";
                    passwordInput.text = "";
                }
                    
                if(www.responseCode == 403){
                    Debug.Log("Wrong password!");
                    StartCoroutine(instantiateMessage("Incorrect Password!"));
                    loadingSign.SetActive(false);
                    loginBtn.SetActive(true);
                    emailInput.text = "";
                    passwordInput.text = "";
                }
                    
            }
            else
            {
                JSONNode jsonData = JSON.Parse(www.downloadHandler.text);
                
                StartCoroutine(instantiateMessage("Logged in successfully!"));

        
                Debug.Log("Logged In with ID: "+jsonData["userid"]);
                USERID = jsonData["userid"];
                StartCoroutine(GetUserExp());
                // if we have logged in getting user data with the userid

                

                // SceneManager.LoadSceneAsync(2);
                StartCoroutine(LoadScene(2));

            }
        }

    }

     IEnumerator LoadScene(int num)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(num);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                // m_Text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                // if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }



    IEnumerator GetUserExp(){
        using(UnityWebRequest www = UnityWebRequest.Get(GET_USEREXP_URL + "/" + USERID)){
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                JSONNode jsonData = JSON.Parse(www.downloadHandler.text);
                // print(www.responseCode);
                isUserDataRecieved = true;
                userData = jsonData;
                Debug.Log("Got user data of id : "+jsonData["userid"]);
            }
        }
    }



    public void UpdateUserExp(UserExp userexp){
        userexp.userid = USERID;
        StartCoroutine(
            SendUserExp(userexp)
        );
    }

    IEnumerator SendUserExp(UserExp userexp= null){

        
        if(userexp == null){
            userexp = new UserExp();
            userexp.SetValues(
                10.0f,
                20.0f,
                20.0f,
                20.0f,
                20.0f,
                20.0f,
                20.0f
            );
        }
    

        string sendData = JsonUtility.ToJson(userexp);

        Debug.Log(sendData);

        using(UnityWebRequest www = UnityWebRequest.Put(ADD_USEREXP_URL,sendData)){

            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type","application/json");
            www.SetRequestHeader("Accept","application/json");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                JSONNode jsonData = JSON.Parse(www.downloadHandler.text);
                
                Debug.Log(jsonData["message"]);


            }

        }

    }


    public GameObject messagePrefab;
    public GameObject canvasParent;
    IEnumerator instantiateMessage(string text){
        yield return new WaitForSeconds(0);

        GameObject messageObj = Instantiate(
            messagePrefab,
            Vector3.zero,
            messagePrefab.transform.rotation,
            canvasParent.gameObject.transform
        );
        messageObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 50,0);
        messageObj.AddComponent<MessageScript>();
        messageObj.GetComponent<MessageScript>().ChangeText(text);
    }

}

  
/*
    starting from 0 :: Q
    1. 270
    2. 270 + 270 -> 180

    starting from 0.0 :: t 
    1. 403
    2. 980

    G = 5731 X (t2-t1) X length 
                / 
        ( D^4 ) X (Q2 - Q1)

    G : 27 GPa
*/



// confirming the calculation part
// testing the application in android 
// Restart button logic 
// completing login page UI 


// seperate the git repo for backend
// hosting 
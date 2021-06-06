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

    private string LOGIN_URL = "http://localhost:5001/userlogin";
    private string SIGNUP_URL = "http://localhost:5001/createuser";
    private string ADD_USEREXP_URL = "http://localhost:5001/userdata";
    private string GET_USEREXP_URL = "http://localhost:5001/userdata";

    public string USERID;

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;


    public bool isUserDataRecieved;
    public JSONNode userData;

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
                if(www.responseCode == 401)
                    Debug.Log("No user with this email!");
                if(www.responseCode == 403)
                    Debug.Log("Wrong password!");
            }
            else
            {
                JSONNode jsonData = JSON.Parse(www.downloadHandler.text);
    

        
                Debug.Log("Logged In with ID: "+jsonData["userid"]);
                USERID = jsonData["userid"];
                StartCoroutine(GetUserExp());
                // if we have logged in getting user data with the userid



                SceneManager.LoadScene(2);

            }
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

}


/*
1. Loading panel after login [UI]
2. Showing warning | wrong pass, no user [UI]
4. Login page UI [UI]
7. Chaning green board [UI]

8. Hosting the backend

*/


 
// 5. Updating real values 
    /*
    REAL VALUES

    kg.cm

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
// 6. Below subtitles with voice | Yotube style 
// 9. Validation 
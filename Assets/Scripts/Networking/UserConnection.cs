using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;

/// <summary>
///
///</summary>
public class UserConnection : MonoBehaviour{
    
    private string LOGIN_URL = "http://localhost:5001/userlogin";
    private string SIGNUP_URL = "http://localhost:5001/createuser";
    private string ADD_USEREXP_URL = "http://localhost:5001/userdata";

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    void Start(){
        StartCoroutine(SendUserExp());
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
                Debug.Log(www.error);
            }
            else
            {
                JSONNode jsonData = JSON.Parse(www.downloadHandler.text);
                
                if(!jsonData["userid"]){
                    Debug.Log("Something went wrong!");
                }else{
                    Debug.Log("Logged In with ID: "+jsonData["userid"]);
                }

            }
        }

    }



    IEnumerator SendUserExp(){

        UserExp userexp = new UserExp();

        // userexp.name = "aman";

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

        yield break;
    }

}

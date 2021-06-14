using UnityEngine;

/// <summary>
///
///</summary>
public class SingltonTest : MonoBehaviour{

    public static SingltonTest instance;

    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else{
            Destroy(gameObject);
        }
    }

    

}

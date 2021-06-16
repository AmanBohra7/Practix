using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleARCore;
using GoogleARCoreInternal;

/// <summary>
/// This class main logic is now changed, it aim is to go back to first scene! 
///</summary>
public class RestartAR : MonoBehaviour{
    
    Button btn;

    public GameObject test;

    void Start(){
        btn = gameObject.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate {RestartExperience();});
    }

    // go back to menu 
    void RestartExperience(){
        // LifecycleManager.Instance.ResetSession();
        SceneManager.LoadScene(0);
    }

}

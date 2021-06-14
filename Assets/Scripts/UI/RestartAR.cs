using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleARCore;
using GoogleARCoreInternal;

/// <summary>
///
///</summary>
public class RestartAR : MonoBehaviour{
    
    Button btn;

    public GameObject test;

    void Start(){
        btn = gameObject.GetComponentInChildren<Button>();
        btn.onClick.AddListener(delegate {RestartExperience();});
    }

    void RestartExperience(){
        LifecycleManager.Instance.ResetSession();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

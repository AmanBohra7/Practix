using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
///
///</summary>
public class DemoScene : MonoBehaviour{
    public void BackToMainScene(){
        SceneManager.LoadScene(0);
    }
}

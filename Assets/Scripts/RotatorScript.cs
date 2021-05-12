using UnityEngine;

// #if UNITY_EDITOR
// using Input = GoogleARCore.InstantPreviewInput;
// #endif

public class RotatorScript : MonoBehaviour
{

    UIController m_UIcontrollerInstance;

    Vector3 cameraPreviousPose = Vector3.zero;
    Vector3 cameraPoseoffset;

    public float rotatespeed = 10000f;
    private Vector3 _startingPosition;

    float rotationAngle;

    void Start()
    {
        m_UIcontrollerInstance = UIController.instance;
        ShowRotationNotice();
    }


    void ShowRotationNotice()
    {
        // m_UIcontrollerInstance.TurnoverRotationNotice();
        m_UIcontrollerInstance.TurnoverSubmitButton();
        m_UIcontrollerInstance.SetSubmitButtonText("Confirm");
        m_UIcontrollerInstance.SetSubmitButtonFunc("rotation");
    }

    public void RotationSet()
    {
        gameObject.GetComponent<ScalingScript>().enabled = true;
    }

    // void OnMouseDrag()
    // {
    //     float rotX = Input.GetAxis("Mouse X") * 10f * Mathf.Deg2Rad;
    //     transform.Rotate(Vector3.up, -rotX);
    // }


    void Update()
    {


        if (Input.GetMouseButton(0))
        {
            cameraPoseoffset = Input.mousePosition - cameraPreviousPose;
            cameraPoseoffset = new Vector3(
                cameraPoseoffset.x,
                cameraPoseoffset.y,
                cameraPoseoffset.z);
            

            if (Input.mousePosition.x > cameraPreviousPose.x){

                gameObject.transform.Rotate(
                new Vector3(0, - rotationAngle * .5f, 0),
                Space.World);

                gameObject.transform.parent.transform.Rotate(
                new Vector3(0, - rotationAngle * .5f, 0),
                Space.World);

            }else if (Input.mousePosition.x < cameraPreviousPose.x){
                
                gameObject.transform.Rotate(
                new Vector3(0, + rotationAngle * .5f, 0),
                Space.World);

                gameObject.transform.parent.transform.Rotate(
                new Vector3(0, - rotationAngle * .5f, 0),
                Space.World);

            }
            else{
                return;
            }

            rotationAngle = Mathf.Abs(Vector3.Dot(cameraPoseoffset, Camera.main.transform.right));

            // transform.Rotate(transform.up, -Vector3.Dot(cameraPoseoffset, Camera.main.transform.right), Space.World);
            // gameObject.transform.parent.transform.Rotate(transform.up, -Vector3.Dot(cameraPoseoffset, Camera.main.transform.right), Space.World);

        }
        cameraPreviousPose = Input.mousePosition;


    }



}

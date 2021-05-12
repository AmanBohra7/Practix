using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{

    Vector3 cameraPoseoffset;
    Vector3 cameraPreviousPose;
    float rotationAngle;



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
            }else if (Input.mousePosition.x < cameraPreviousPose.x){
                gameObject.transform.Rotate(
                new Vector3(0, + rotationAngle * .5f, 0),
                Space.World);
            }
            else{
                return;
            }

            rotationAngle = Mathf.Abs(Vector3.Dot(cameraPoseoffset, Camera.main.transform.right));
            
            // transform.Rotate(transform.up, -rotationAngle, Space.World);
            // gameObject.transform.parent.transform.Rotate(transform.up, -rotationAngle, Space.World);

        }
        cameraPreviousPose = Input.mousePosition;

    }

}

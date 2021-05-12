using TMPro;
using UnityEngine;
public class InfoPointer : MonoBehaviour
{
    public GameObject m_InfoPanel;
    public GameObject m_DottedLine;
    public bool isDoubleArrow = false;

    Vector3 initalRotation;

    void Start()
    {
        initalRotation = gameObject.transform.rotation.eulerAngles;
    }

    public void SetInstruction(string instruction)
    {
        m_InfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = instruction;
    }

    void FixedUpdate()
    {
        if (!isDoubleArrow)
        {

            transform.LookAt(new Vector3(
                Camera.main.transform.position.x,
                this.transform.position.y,
                Camera.main.transform.position.z
            ));

        }

        else
        {
            m_InfoPanel.transform.LookAt(new Vector3(
                Camera.main.transform.position.x,
                this.transform.position.y,
                Camera.main.transform.position.z
            ));
            m_DottedLine.transform.LookAt(new Vector3(
                Camera.main.transform.position.x,
                this.transform.position.y,
                Camera.main.transform.position.z
            ));
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            Debug.Log("Camera triggered IN !");
            gameObject.GetComponentInChildren<Canvas>().enabled = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            Debug.Log("Camera triggered OUT !");
            gameObject.GetComponentInChildren<Canvas>().enabled = true;
        }
    }
}

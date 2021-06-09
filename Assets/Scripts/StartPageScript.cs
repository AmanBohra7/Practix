﻿using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class StartPageScript : MonoBehaviour
{ 
    public void PracticeBtnPressed(){
       SceneManager.LoadScene(
           SceneManager.GetActiveScene().buildIndex + 1
       );
    }


    public void TestBtnPressed(){
        StartCoroutine(instantiateMessage());
    }

    public void DemoBtnPressed(){

    }

    public GameObject messagePrefab;
    IEnumerator instantiateMessage(){
        yield return new WaitForSeconds(0);

        GameObject messageObj = Instantiate(
            messagePrefab,
            Vector3.zero,
            messagePrefab.transform.rotation,
            gameObject.transform
        );
        messageObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 50,0);
        messageObj.AddComponent<MessageScript>();
    }

}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
///</summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour{
    

    public static AudioManager Instance;

    public List<AudioClip> subtitleAudioList;

    private AudioSource audioSource;

    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
    }

    void Start(){
        // subtitleAudioList = new List<AudioClip>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayAudio(int index){
        print("list count : "+subtitleAudioList.Count);
        if(index >= subtitleAudioList.Count) return;
        audioSource.clip = subtitleAudioList[index];
        audioSource.Play();
    }

}

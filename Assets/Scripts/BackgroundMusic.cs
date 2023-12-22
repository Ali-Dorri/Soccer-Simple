using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
    static BackgroundMusic Instance=null;

    public AudioSource audioSource;
    public AudioClip BgMusic;

    public Scene ThisScene;
  
	void Start ()
    {
        BgMusic = (AudioClip)Resources.Load("Sounds/BackGround Music");
        ThisScene = SceneManager.GetActiveScene();
        

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BgMusic;
        audioSource.Play();
        
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                GameObject.DontDestroyOnLoad(gameObject);
            }
      
        
	}
	
}

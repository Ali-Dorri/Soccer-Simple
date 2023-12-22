using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour {

    public AudioClip Shoot;
    public AudioClip[] GoalScream;
    public AudioSource AudioSource;
    public int RandomNum;
	void Start ()
    {

        AudioSource = GetComponent<AudioSource>();
        GoalScream = new AudioClip[3];
        Shoot = (AudioClip)Resources.Load("Sounds/Shoot");
        GoalScream[0] = (AudioClip)Resources.Load("Sounds/GoalNoise1");
        GoalScream[1] = (AudioClip)Resources.Load("Sounds/GoalNoise2");
        GoalScream[2] = (AudioClip)Resources.Load("Sounds/GoalNoise3");

	}
    public void PlayShootAudio()
    {
        AudioSource.clip = Shoot;
        AudioSource.Play();
    }

    public void PlayGoalScreamAudio()
    {
        RandomNum = Random.Range(0,2);
        AudioSource.clip = GoalScream[RandomNum]; 
        AudioSource.Play();
    }
}

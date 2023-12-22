using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WinMessage : MonoBehaviour {

    public OptionContainer option;
    public GameObject RedMessage, BlueMessage,DrawMessage;

    public BackgroundMusic bgMusic;
	void Start () 
    {

        bgMusic = FindObjectOfType<BackgroundMusic>();
        option = FindObjectOfType<OptionContainer>();
        RedMessage = GameObject.FindGameObjectWithTag("RedMessage");
        BlueMessage = GameObject.FindGameObjectWithTag("BlueMessage");
        DrawMessage = GameObject.FindGameObjectWithTag("Draw");

        bgMusic.audioSource.Play();

        if (option.BlueIsWinner == true && option.Draw == false)
        {
            BlueMessage.GetComponent<Text>().enabled = true;
        }
        else if(option.BlueIsWinner == false && option.Draw == false)
        {
            RedMessage.GetComponent<Text>().enabled = true;
        }
        else if(option.Draw == true)
        {
            DrawMessage.GetComponent<Text>().enabled = true;
        }

	}
	
}

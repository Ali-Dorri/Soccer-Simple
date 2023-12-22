using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

    OptionContainer Option;
    public Text TimerText;
    public float CountDownTime;
	void Start () {
        Option = GameObject.FindGameObjectWithTag("OptionContainer").GetComponent<OptionContainer>();
        TimerText = GetComponent<Text>();
        CountDownTime = Option.TimeLimit * 60;
	}
	
	void Update () {
        CountDownTime -= Time.deltaTime;
        if (CountDownTime <= 0)
        {
            CountDownTime = 0;
        }
        int Minutes = Mathf.FloorToInt(CountDownTime / 60f);
        int Seconds = Mathf.FloorToInt(CountDownTime - Minutes * 60);
        string NiceTime = string.Format("{0:0}:{1:00}",Minutes,Seconds);
        TimerText.text = NiceTime;
     
	}
}

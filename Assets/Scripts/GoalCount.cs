using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalCount : MonoBehaviour {
    public Text ScoreText;
    public int Score;
    public GameObject S_Ball;
    public Rigidbody2D Ballrigidbody;
    public Ball ball;
    public SoccerEnableHandler enableHandler;
    public SoundHandler soundHandler;
    public GameSetup gameSetup;

    int isCreated = 0;
    public bool isGoal = false;
  //  OptionContainer Option;
	void Start () 
    {
//        Option = GameObject.FindGameObjectWithTag("OptionContainer").GetComponent<OptionContainer>();
        S_Ball = GameObject.FindGameObjectWithTag("Ball");
        Ballrigidbody = S_Ball.GetComponent<Rigidbody2D>();
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
        gameSetup = FindObjectOfType<GameSetup>();
        enableHandler = FindObjectOfType<SoccerEnableHandler>();
        soundHandler = FindObjectOfType<SoundHandler>();
        
        if (this.tag == "RedTeam")
        {
            ScoreText = GameObject.FindGameObjectWithTag("RedScore").GetComponent<Text>();
        }
        if(this.tag == "BlueTeam")
        {
            ScoreText = GameObject.FindGameObjectWithTag("BlueScore").GetComponent<Text>();
        }
        Counttext();
	}
    void OnTriggerEnter2D(Collider2D collid)
    {
        if(collid.gameObject.tag == "Ball")
        {
            FindObjectOfType<AIControll>().enabled = false;
            if (!isGoal)
            {
                Score++;             
                gameSetup.ScoreCheck = true;
                soundHandler.PlayGoalScreamAudio();
                if (this.tag == "Redteam")
                {
                    gameSetup.RedScore++;
                }
                else
                {
                    gameSetup.BlueScore++;
                }  
                isGoal = true;
            }
            Counttext();
            ball.Friction = 30f;
            Invoke("RePos",2);
        }
    }
    public void Counttext()
    {
        ScoreText.text = Score.ToString();
    }
    public void RePos()
    {
        //////////////////// Reset Ball
        S_Ball.transform.position = new Vector3(0f,0f,-1f);
        ball.Friction = 0f;
        Ballrigidbody.angularVelocity = 0f;
        ///////////////////////////////
        isGoal = false;
        ///////////////////////////////
        enableHandler.TeamReposition();
    }
}

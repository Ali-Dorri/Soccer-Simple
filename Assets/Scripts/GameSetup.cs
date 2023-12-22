using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour {
    
    static GameSetup instance = null;

    public bool IsCreated = false,ScoreCheck = false;
    public int TeamCount,BlueScore = 0,RedScore = 0;
    public float Timer;
    public Ball ball;

    public OptionContainer Option;
    public SoccerEnableHandler soccerEnableHandler;
    public TimerController timeController;
    public AIControll AIcontroll;
    public BackgroundMusic bgMusic;

    public GameObject RedPrefab, RedParent;
    public GameObject BluePrefab, BlueParent;
    public GameObject GameTimer;
    public GameObject FirstBluePlayer;

    public enum GameLimit { Score, Time }
	void Start () {
        TeamCount = 1;

        Option = GameObject.FindGameObjectWithTag("OptionContainer").GetComponent<OptionContainer>();
        RedParent = GameObject.FindGameObjectWithTag("RedParent");
        BlueParent = GameObject.FindGameObjectWithTag("BlueParent");
        ball = FindObjectOfType<Ball>();
        AIcontroll = FindObjectOfType<AIControll>();
        bgMusic = FindObjectOfType<BackgroundMusic>();
        soccerEnableHandler = GetComponent<SoccerEnableHandler>();
        GameTimer = GameObject.Find("Timer");
        timeController = GameTimer.GetComponent<TimerController>();

        CreatePlayers();
        AIcontroll.FindPlayers();
        PrimaryInitializer.PrimaryInitialize(FirstBluePlayer);
        bgMusic.audioSource.Stop();
	}

    void Update()
    {
        if (Option.TypeOfGameLimit == Time_Score.GameLimitType.Score)
        {         
            ByScore();
        }
        else { ByTime(); }
    }

    void ByScore()
    {
        GameTimer.SetActive(false);
        if (ScoreCheck)
        {
            /////////////////// wirte thie code with seitch case!!
            if(BlueScore == Option.ScoreLimit)
            {
                Option.BlueIsWinner = true;
            }
            if(RedScore == Option.ScoreLimit)
            {
                Option.BlueIsWinner = false;
            }

            ///////////////////
            if (BlueScore == Option.ScoreLimit || RedScore == Option.ScoreLimit)
            {
                Invoke("LoadMatchStatus", 1);
            }
            ScoreCheck = false;
        }
    }

    void ByTime()
    {
        GameTimer.SetActive(true);
        if (timeController.CountDownTime <= 0)
        {
            if (BlueScore > RedScore)
            {
                Option.BlueIsWinner = true;
            }
            if (RedScore > BlueScore)
            {
                Option.BlueIsWinner = false;
            }
            if (BlueScore == RedScore)
            {
                Option.Draw = true;
            }
            Invoke("LoadMatchStatus", 1);
        }
    }
    void CreatePlayers()
    {
        // Create RedPlayer
        foreach (Transform child in RedParent.transform)
        {
            if (TeamCount <= Option.TeamNumber)
            {
                GameObject player = Instantiate(RedPrefab, child.transform.position, Quaternion.identity) as GameObject;
                player.transform.parent = child;
                TeamCount++;
            }
        }

        TeamCount = 1;
        // Create BluePlayer
        foreach (Transform child in BlueParent.transform)
        {
            if(TeamCount <= Option.TeamNumber)
            {
                GameObject player = Instantiate(BluePrefab, child.transform.position, Quaternion.identity) as GameObject;
                player.transform.parent = child;
                TeamCount++;
            }
        }
        IsCreated = true;
        ball.Initialize();
    }

    void LoadMatchStatus()
    {
        Application.LoadLevel("End");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControll : MonoBehaviour {
    public GameObject RedParent,BlueParent;
    public GameObject[] RedTeamPlayers;
    public GameObject[] BlueTeamPlayers;
    public GameObject NearestBlue,NearestRed;
    public GameObject NearBlue, NearRed;
    public GameObject Ball;

    public enum BallStatus { Free, Friend, Enemy, Me }
    public AI ai;
    public GameSetup gameSetup;

    int j;
    float Distance_BluePlayer_From_Ball,Distance_Redplayer_from_Ball;
    float Distance_Another_BluePlayer;
    float Temp,Trash,xdistance,ydistance;

	void Start () 
    {

        gameSetup = FindObjectOfType<GameSetup>(); 


        RedParent = GameObject.FindGameObjectWithTag("RedParent");
        BlueParent = GameObject.FindGameObjectWithTag("BlueParent");
        Ball = GameObject.FindGameObjectWithTag("Ball");
       
        RedTeamPlayers = new GameObject[4];
        BlueTeamPlayers = new GameObject[4];
	}
	
	
	void Update () 
    {
        FindNearPlayer();
	}

    public void FindPlayers()
    {
        int i = 0;
        foreach (Transform child in RedParent.transform)
        {
            if (child.transform.childCount > 0)
            {
                RedTeamPlayers[i] = child.transform.Find("red_player (1)(Clone)").gameObject;
                i++;
            }
        }
        i = 0;
        foreach (Transform child in BlueParent.transform)
        {
            if (child.transform.childCount > 0)
            {
                BlueTeamPlayers[i] = child.transform.Find("blue_player (1)(Clone)").gameObject;
                i++;
            }
        }
        gameSetup.FirstBluePlayer = BlueTeamPlayers[0].gameObject;
    }
    public void FindNearPlayer()
    {
        // for red team 
        for (int i = 0; i < 4; i++)
        {
            if (RedTeamPlayers[i] != null)
            {
                Distance_Redplayer_from_Ball = Vector2.Distance(RedTeamPlayers[i].transform.position, Ball.transform.position);
                if (i == 0 || Distance_Redplayer_from_Ball < Temp)
                {
                    Temp = Distance_Redplayer_from_Ball;
                    j = i;
                }
            }
        }
        NearestRed = RedTeamPlayers[j];

        // for blue team
        for (int i = 0; i < 4; i++)
        {
            if (BlueTeamPlayers[i] != null)
            {
                Distance_BluePlayer_From_Ball = Vector2.Distance(BlueTeamPlayers[i].transform.position, Ball.transform.position);
                if (i == 0 || Distance_BluePlayer_From_Ball < Temp)
                {
                    Temp = Distance_BluePlayer_From_Ball;
                    j = i;
                }
            }
        }
        NearestBlue = BlueTeamPlayers[j];
        PlayerMovement();
    }

    void PlayerMovement()
    {
        ///////////////////////////////////////////////
        ///////////////////////////////////////////////
        AI BlueAI = NearestBlue.GetComponent<AI>();
        AI RedAI = NearestRed.GetComponent<AI>();
        SoccerPlayer Blue_soccerPlayer = NearestBlue.GetComponent<SoccerPlayer>();
        SoccerPlayer Red_soccerPlayer = NearestRed.GetComponent<SoccerPlayer>();

        Vector2 BlueDistance = new Vector2(Ball.transform.position.x - NearestBlue.transform.position.x, Ball.transform.position.y - NearestBlue.transform.position.y);
        Vector2 RedDistance = new Vector2(Ball.transform.position.x - NearestRed.transform.position.x, Ball.transform.position.y - NearestRed.transform.position.y);
        ///////////////////////////////////////////////
        ///////////////////////////////////////////////
        if ((BallStatus)BlueAI.bstat == BallStatus.Free)
        {
            //Blue_soccerPlayer.PlayerOrAIType != 0
            if (BlueAI.enabled == true)
            {
                Blue_soccerPlayer.MoveWithRotation(BlueDistance, 5f);
            }
            for (int i = 0; i < 4; i++)
            {
                if (BlueTeamPlayers[i] != null)
                {
                    if (BlueTeamPlayers[i].GetComponent<AI>().enabled == true && BlueTeamPlayers[i] != NearestBlue)
                    {
                        BlueTeamPlayers[i].GetComponent<AI>().GetBack();
                    }
                }
            }
        }
        //////////////////////////////////////////////
        //////////////////////////////////////////////
        if((BallStatus)RedAI.bstat == BallStatus.Free)
        {
            Red_soccerPlayer.MoveWithRotation(RedDistance, 5f);
            for (int i = 0; i < 4;i++ ) 
            {
              if(RedTeamPlayers[i] != null && RedTeamPlayers[i] != NearestRed)
               {
                   RedTeamPlayers[i].GetComponent<AI>().GetBack();
               }
            }
        }
        //////////////////////////////////////////////
        //////////////////////////////////////////////
        if((BallStatus)BlueAI.bstat == BallStatus.Me)
        {
            for (int i = 0; i < 4;i++ ) 
            {
                if (BlueTeamPlayers[i] != null && BlueTeamPlayers[i] != NearestBlue)
                {
                    float PlayersDistance = Vector2.Distance(BlueTeamPlayers[i].transform.position, Ball.transform.position);

                    if (PlayersDistance <= 2.5)
                    {
                        Vector2 TakeDistance = new Vector2(Ball.transform.position.x - BlueTeamPlayers[i].transform.position.x, Ball.transform.position.y - BlueTeamPlayers[i].transform.position.y);
                        BlueTeamPlayers[i].GetComponent<SoccerPlayer>().MoveWithRotation(-TakeDistance, 2f);
                    }
                    else { BlueTeamPlayers[i].GetComponent<AI>().GetBack(); }
                }
            }
        }
        //////////////////////////////////////////////
        //////////////////////////////////////////////
        if ((BallStatus)RedAI.bstat == BallStatus.Me)
        {
            RedAI.Goal();
            for (int i = 0; i < 4; i++)
            {
                if (RedTeamPlayers[i] != null && RedTeamPlayers[i] != NearestRed)
                {
                    float PlayersDistance = Vector2.Distance(RedTeamPlayers[i].transform.position, Ball.transform.position);

                    if (PlayersDistance <= 2.5)
                    {
                        Vector2 TakeDistance = new Vector2(Ball.transform.position.x - RedTeamPlayers[i].transform.position.x, Ball.transform.position.y - RedTeamPlayers[i].transform.position.y);
                        RedTeamPlayers[i].GetComponent<SoccerPlayer>().MoveWithRotation(-TakeDistance, 2f);
                    }
                    else { RedTeamPlayers[i].GetComponent<AI>().GetBack(); }
                }
            }

        }
        //////////////////////////////////////////////
        //////////////////////////////////////////////
        if ((BallStatus)BlueAI.bstat == BallStatus.Enemy)
        {
            if (BlueAI.enabled == true)
            {
                Blue_soccerPlayer.MoveWithRotation(BlueDistance,5f);
            }

            for (int i = 0; i < 4; i++)
            {
                if(BlueTeamPlayers[i] != null && BlueTeamPlayers[i].GetComponent<AI>().enabled == true && BlueTeamPlayers[i] != NearestBlue)
                {
                    BlueTeamPlayers[i].GetComponent<AI>().GetBack();
                }
            }
            
        }
        //////////////////////////////////////////////
        //////////////////////////////////////////////
        if((BallStatus)RedAI.bstat == BallStatus.Enemy)
        {
            Red_soccerPlayer.MoveWithRotation(RedDistance, 5f);
            
            for (int i = 0; i < 4;i++ ) 
            {
                if (RedTeamPlayers[i] != NearestRed && RedTeamPlayers[i] != null)
                {
                    RedTeamPlayers[i].GetComponent<AI>().GetBack();
                }
            }

        }
        //////////////////////////////////////////////
        //////////////////////////////////////////////
    }
}

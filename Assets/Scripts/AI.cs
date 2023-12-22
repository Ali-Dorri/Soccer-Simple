using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI : MonoBehaviour , IPlayEnable 


{
    GameObject ball;
    SoccerPlayer soccerPlayer;
    Rigidbody2D Rig;

    float xdistance, ydistance;

    public GameObject Main;
    public Ball Ball;

    public bool InTrigger, EnemyInTrigger;

    public enum BallStatus { Free, Friend, Enemy, Me }

    public BallStatus bstat;
    void Start()
    {
        Main = this.transform.parent.gameObject;
        soccerPlayer = GetComponent<SoccerPlayer>();
        ball = GameObject.FindGameObjectWithTag("Ball");
        Rig = this.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        xdistance = ball.transform.position.x - this.transform.position.x;
        ydistance = ball.transform.position.y - this.transform.position.y;
    }
    public void GetBack()
    {
        float XDistance = Main.transform.position.x - this.transform.position.x;
        float YDistance = Main.transform.position.y - this.transform.position.y;
       if(Vector3.Distance(this.transform.position , Main.transform.position) > 0.2f)
       {
            soccerPlayer.MoveWithRotation(new Vector2(XDistance, YDistance), 2f);
       }
       if (Vector3.Distance(this.transform.position, Main.transform.position) <= 0.2f)
       {
           Rig.velocity = new Vector2(0f,0f);     
       }
    }

    public void SetBallSt(BallStatus bs)
    {
        bstat = bs;
    }

    /* private void CheckCondition()
    {
        if (bstat == BallStatus.Enemy && InTrigger == true)
        {
            soccerPlayer.Move(new Vector2(xdistance, ydistance), 5f);
        }

        if (bstat == BallStatus.Free && InTrigger == true)
        {
            soccerPlayer.Move(new Vector2(xdistance, ydistance), 5f);
        //    AI_Move();
        }

        if (bstat == BallStatus.Friend && InTrigger == true)
        {
            soccerPlayer.Move(new Vector2(-xdistance, -ydistance * 2), 2f);
        }

        if (bstat == BallStatus.Me && this.tag == "team1")
        {

            Goal();

        }

    } */
    public void Goal()
    {
        if (this.transform.position.x > -1.5f)
        {
            soccerPlayer.MoveWithRotation(new Vector2(-1, 0), 5f);        
        }

        if (this.transform.position.x <= -2.5)
        {
            float Direction;

            if (this.transform.position.y > 1.25)
            {
                if (this.transform.position.x <= -6.5)
                {
                    soccerPlayer.MoveWithRotation(new Vector2(1f, 0f), 5f);
                }
                if (this.transform.position.x > -6.5 && this.transform.position.x < -2.5)
                {
                    Direction = Random.Range(-0.3f, -0.8f);
                    soccerPlayer.Shoot(new Vector2(-1f, Direction));
                }
            }
            else if (this.transform.position.y < -1.25)
            {
                if (this.transform.position.x <= -6.5)
                {
                    soccerPlayer.MoveWithRotation(new Vector2(1f, 0f), 5f);
                }
                if (this.transform.position.x > -6.5 && this.transform.position.x < -2.5)
                {
                    Direction = Random.Range(0.3f, 0.8f);
                    soccerPlayer.Shoot(new Vector2(-1f, Direction));
                }
            }
            else
            {
                Direction = Random.Range(-0.2f, 0.2f);
                soccerPlayer.Shoot(new Vector2(-1f, Direction));
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collid)
    {
        
    }
    void OnTriggerEnter2D(Collider2D collid)
    {
       
    }
    void OnTriggerExit2D(Collider2D collid)
    {
        //if(collid.transform.parent.tag == "Ball" )
        // InTrigger = false;
        EnemyInTrigger = false;
    }
    void OnTriggerStay2D(Collider2D collid)
    {
        if (collid.gameObject.tag == "Ball" && collid.gameObject.transform.parent != null)
        {
            InTrigger = true;
            collid.GetComponent<Rigidbody2D>().angularVelocity = 45f;
        }
    }

    public bool IsPlayEnabled
    {
        set
        {
            this.enabled = value;
            FindObjectOfType<AIControll>().enabled = value;
        }
        get
        {
            return true;
        }
    }

}

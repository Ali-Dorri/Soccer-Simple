using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //
    //Fields
    //

    [SerializeField] private float friction;
    private SoccerPlayer holderSoccerPlayer = null;
    private Rigidbody2D ballRigidBody;
    private new Collider2D collider;
    private AI[] team1AIs;
    private AI[] team2AIs;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public float Friction
    {
        get
        {
            return friction;
        }
        set
        {
            if (value >= 0)
            {
                friction = value;
            }
            else
            {
                friction = 0;
            }
        }
    }

    public SoccerPlayer HolderSoccerPlayer
    {
        get
        {
            return holderSoccerPlayer;
        }
        set
        {
            holderSoccerPlayer = value;
            SetBotsBallStatus();
        }
    }

    public Rigidbody2D RigidBody
    {
        get
        {
            return ballRigidBody;
        }
    }

    public Collider2D Collider
    {
        get
        {
            return collider;
        }
    }

    public AI[] Team1AIs
    {
        get
        {
            return team1AIs;
        }
    }

    public AI[] Team2AIs
    {
        get
        {
            return team2AIs;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        if (friction < 0)
        {
            friction = 1;
        }

        ballRigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        //FindAITeams(FindObjectsOfType<AI>());
    }

    public void Initialize()
    {
        FindAITeams(FindObjectsOfType<AI>());
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    private void Update()
    {
        AddFriction(ballRigidBody.velocity);
    }

    private void AddFriction(Vector2 velocity)
    {
        if (velocity.magnitude != 0)
        {
            //find friction
            float xRatio = -velocity.x / velocity.magnitude;
            float yRatio = -velocity.y / velocity.magnitude;
            float frameFriction = friction * Time.deltaTime;
            Vector2 frictionVector = new Vector2(frameFriction * xRatio, frameFriction * yRatio);

            //
            //affect friction
            //

            float xVelocity = velocity.x + frictionVector.x;
            float yVelocity = velocity.y + frictionVector.y;

            //if friction can velocity.x sign
            if (xVelocity * velocity.x < 0)
            {
                xVelocity = 0;
            }

            //if friction can velocity.y sign
            if (yVelocity * velocity.y < 0)
            {
                yVelocity = 0;
            }

            //
            //set rigidBody.velocity
            //
            ballRigidBody.velocity = new Vector2(xVelocity, yVelocity);
        }
        else
        {
            //stop
            ballRigidBody.velocity = new Vector2(0, 0);
        }
    }

    private void FindAITeams(AI[] allAIs)
    {
        int team1Count = 0;
        int team2Count = 0;

        //find team arrays' lengths
        foreach(AI ai in allAIs)
        {
            if (ai.tag == "team1")
            {
                team1Count++;
            }
            else
            {
                team2Count++;
            }
        }

        //
        //fill team arrays
        //

        team1AIs = new AI[team1Count];
        team2AIs = new AI[team2Count];
        team1Count = 0;
        team2Count = 0;
        
        foreach(AI ai in allAIs)
        {
            if (ai.tag == "team1")
            {
                team1AIs[team1Count] = ai;
                team1Count++;
            }
            else
            {
                team2AIs[team2Count] = ai;
                team2Count++;
            }
        }
    }

    private void SetBotsBallStatus()
    {
        if (holderSoccerPlayer != null)
        {
            //set the holderAI's ball status
            AI holderAI = holderSoccerPlayer.GetComponent<AI>();
            holderAI.SetBallSt(AI.BallStatus.Me);

            //set other AI's ball status
            if (holderSoccerPlayer.tag == "team1")
            {
                foreach(AI ai in team1AIs)
                {
                    if (ai != holderAI)
                    {
                        ai.SetBallSt(AI.BallStatus.Friend);
                    }
                }
                foreach (AI ai in team2AIs)
                {
                    ai.SetBallSt(AI.BallStatus.Enemy);
                }
            }
            else
            {
                foreach (AI ai in team2AIs)
                {
                    if (ai != holderAI)
                    {
                        ai.SetBallSt(AI.BallStatus.Friend);
                    }
                }
                foreach (AI ai in team1AIs)
                {
                    ai.SetBallSt(AI.BallStatus.Enemy);
                }
            }
        }
        else
        {
            //the ball has no holder
            foreach(AI ai in team1AIs)
            {
                ai.SetBallSt(AI.BallStatus.Free);
            }
            foreach (AI ai in team2AIs)
            {
                ai.SetBallSt(AI.BallStatus.Free);
            }
        }
    }
}

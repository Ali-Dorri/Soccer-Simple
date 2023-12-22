using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerPlayer : MonoBehaviour, IPlayResetable
{
    //
    //Concept Definition
    //

    public enum PlayerOrAI { Player, AI }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    //variables
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite withoutShadow;
    [SerializeField] Sprite withShadow;
    public SoundHandler soundHandler;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float accelerateAmount;
    [SerializeField] protected float friction;
    [SerializeField] private int teamID;
    protected float bodyRadius;
    [SerializeField] protected float holderSize;
    [SerializeField] protected float shoootSpeed;
    protected Ball holdedBall = null;
    protected Rigidbody2D soccerRigidBody;
    private bool isCatching = false;
    [SerializeField] private PlayerOrAI playerOrAI = PlayerOrAI.AI;
    [SerializeField] private bool isPlayerable = false;
    IPlayEnable playerEnablable;
    IPlayEnable botEnablable;
    /// <summary>
    /// Determine whether the SoccerPlayer is Player at match start or not
    /// </summary>
    private bool isDefaultPlayer = false;
    private bool isRobbed = false;    
    private Coroutine attractCoroutine;
    /// <summary>
    /// Don't set this directly. Use it's property.
    /// </summary>
    private Quaternion desiredRotation = Quaternion.identity;
    [Tooltip("Degree per Second")]
    [SerializeField] private float rotationSpeed = 180;
    /// <summary>
    /// Don't set this directly. Use it's property instead.
    /// </summary>
    private Vector3 desiredDirection = new Vector3(0, 1, 0);
    private Vector2 previousDirection;
    private Vector2 currentDirection;
    private bool canChangeDesiredDirection = true;
    private Coroutine checkShootCoroutine;

    //soccer enable handler requierments
    bool isPlayEnabled = true;

    //constants
    private const int ROBBED_TIME = 1;
    private const float ATTRACT_SPEED = 6;

    //events
    public event Action<Ball> CatchedBall;
    public event Action ReleasedBall;
    public event Action DesiredRotationChanged;
    public event Action<bool> AfterDirectionalShoot;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //
    private void Start()
    {
        if (maxSpeed <= 0)
        {
            maxSpeed = 3;
        }
        if (accelerateAmount <= 0)
        {
            accelerateAmount = 1;
        }
        if (friction <= 0)
        {
            friction = 0.5f;
        }
        if (holderSize <= 0)
        {
            holderSize = GameObject.FindGameObjectWithTag("Ball").GetComponent<CircleCollider2D>().radius;
        }
        if (shoootSpeed <= 0)
        {
            shoootSpeed = 7f;
        }
        if (teamID == 1)
        {
            tag = "team1";
        }
        else if (teamID == 2)
        {
            tag = "team2";
        }
        if (tag == "team1")
        {
            // rigth side
            DesiredDirection = new Vector3(-1, 0, 0);
        }
        else if (tag == "team2")
        {
            // left side
            DesiredDirection = new Vector3(1, 0, 0);
        }

        soundHandler = FindObjectOfType<SoundHandler>();
        //physics
        soccerRigidBody = GetComponent<Rigidbody2D>();
        bodyRadius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
        previousDirection = transform.up;
        currentDirection = transform.up;

        //default player or AI
        if(playerOrAI == PlayerOrAI.Player)
        {
            isDefaultPlayer = true;
        }

        //add this to SoccerEnableHandler
        SoccerEnableHandler.AddResetable(this);
    }


    public void Initialize(PlayerOrAI playerOrAI)
    {
        playerEnablable = GetComponent<Player>();
        botEnablable = GetComponent<AI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        PlayerOrAIType = playerOrAI;
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public float AccelerateAmount
    {
        get
        {
            return accelerateAmount;
        }
    }

    protected Vector2 HolderPosition
    {
        get
        {
            //consider that the return value(Vector3) will cast to Vector2
            return (transform.position + ((bodyRadius + holderSize) * transform.up.normalized));
        }
    }

    protected Vector2 BallVelocityByMove
    {
        get
        {
            float deltaRadianAngle = Vector2.SignedAngle(previousDirection, currentDirection) * Mathf.PI / 180;
            float tangentSpeed = 2 * bodyRadius * Mathf.Sin(deltaRadianAngle / 2) / Time.deltaTime;
            Vector2 tangentVelocity = new Vector2(-transform.up.y, transform.up.x).normalized * tangentSpeed;
            return tangentVelocity; //we dont affect linear velocity of soccer player
        }
    }

    public PlayerOrAI PlayerOrAIType
    {
        get
        {
            return playerOrAI;
        }
        set
        {
            if (value == PlayerOrAI.Player)
            {
                SetTeammatePlayerToAI();

                if (playerEnablable != null)
                {
                    playerEnablable.IsPlayEnabled = true;
                }
                if (botEnablable != null)
                {
                    ((AI)botEnablable).enabled = false;
                }     
           
                //sprite
                spriteRenderer.sprite = withShadow;
            }
            else
            {
                if (playerEnablable != null)
                {
                    playerEnablable.IsPlayEnabled = false;
                }
                if (botEnablable != null)
                {
                    ((AI)botEnablable).enabled = true;
                }

                //sprite
                spriteRenderer.sprite = withoutShadow;
            }

            playerOrAI = value;
        }
    }

    public Vector3 DesiredDirection
    {
        get
        {
            return desiredDirection;
        }
        set
        {
            if (value != desiredDirection && (Vector2)value != new Vector2(0, 0))
            {
                if (canChangeDesiredDirection)
                {
                    desiredDirection = value;
                    DesiredRotation = Quaternion.LookRotation(new Vector3(0, 0, 1), desiredDirection);
                }               
            }
        }
    }

    protected Quaternion DesiredRotation
    {
        get
        {
            return desiredRotation;
        }
        set
        {
            if (value != desiredRotation)
            {
                desiredRotation = value;
                OnDesiredRotationChanged();
            }
        }
    }

    public Ball HoldedBall
    {
        get
        {
            return holdedBall;
        }
    } 

    public bool IsPlayEnabled
    {
        get
        {
            return isPlayEnabled;
        }
        set
        {
            //determine the enbale condition
            isPlayerable = value;

            //update method
            enabled = value;

            //physics
            GetComponent<Collider2D>().enabled = value;

            //SoccerPlayer functions
            if (value == false)
            {
                //it handles the runnig coroutines, too(stop them if needed).
                ReleaseBall();
                //stop physics
                soccerRigidBody.velocity = new Vector2(0, 0);
                if (playerEnablable != null)
                {
                    playerEnablable.IsPlayEnabled = false;
                }
                if (botEnablable != null)
                {
                    botEnablable.IsPlayEnabled = false;
                }
            } 
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    private void Update()
    {
        Rotate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject ifBall = collision.gameObject;
        CheckCatchBall(ifBall);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ifBall = collision.gameObject;
        CheckCatchBall(ifBall);
    }

    private void CheckCatchBall(GameObject ifBallGameObject)
    {
        if (!isRobbed)
        {
            if (!isCatching)
            {
                if (ifBallGameObject.tag == "Ball")
                {
                    Ball ball = ifBallGameObject.GetComponent<Ball>();
                    SoccerPlayer holderSoccerPlayer = ball.HolderSoccerPlayer;

                    if (holderSoccerPlayer != null)
                    {
                        if (tag == "team1")
                        {
                            if (holderSoccerPlayer.tag == "team2")
                            {
                                //to avoid transferring ball between two soccerPlayer multiple times in a moment
                                //we use ReleaseBall() and CatchBall() in the Rob() function
                                Rob(holderSoccerPlayer, ball);
                            }
                        }
                        else
                        {
                            if (holderSoccerPlayer.tag == "team1")
                            {
                                //to avoid transferring ball between two soccerPlayer multiple times in a moment
                                //we use ReleaseBall() and CatchBall() in the Rob() function
                                Rob(holderSoccerPlayer, ball);
                            }
                        }
                    }
                    else
                    {
                        CatchBall(ball);
                    }
                }
            }
        }
    }

    protected virtual void CatchBall(Ball ball)
    {
        if (isPlayerable && playerOrAI == PlayerOrAI.AI)
        {
            PlayerOrAIType = PlayerOrAI.Player;
        }

        ball.RigidBody.velocity = new Vector2(0, 0);
        ball.RigidBody.bodyType = RigidbodyType2D.Kinematic;
        ball.Collider.isTrigger = true;
        ball.HolderSoccerPlayer = this;
        holdedBall = ball;
        ball.transform.SetParent(transform, true); 
        attractCoroutine = StartCoroutine(AttractBall(ball));
        //ball.transform.position = HolderPosition;
        OnCatchBall(ball);

        isCatching = true;
    }

    IEnumerator AttractBall(Ball ball)
    {
        while(Vector2.Distance(ball.transform.position, HolderPosition) >= ATTRACT_SPEED * Time.deltaTime)
        {
            yield return null;

            Vector2 direction = HolderPosition - (Vector2)(ball.transform.position);
            Vector2 deltaPos = direction.normalized * (ATTRACT_SPEED * Time.deltaTime);
            ball.transform.position += (Vector3)deltaPos;
        }

        attractCoroutine = null;
    }

    protected virtual void ReleaseBall()
    {
        if(holdedBall != null)
        {
            if (attractCoroutine != null)
            {
                StopCoroutine(attractCoroutine);
                attractCoroutine = null;
            }

            holdedBall.HolderSoccerPlayer = null;
            holdedBall.transform.SetParent(null, true);
            holdedBall.RigidBody.bodyType = RigidbodyType2D.Dynamic;
            holdedBall.Collider.isTrigger = false;
            holdedBall = null;           
            OnReleaseBall();

            isCatching = false;
        }
    }

    /// <summary>
    /// Move the soccer player in the direction by it's accelerate, rotate it in the direction smoothly
    /// and affect the physics. 
    /// </summary>
    public void MoveWithRotation(Vector2 direction)
    {
        MoveWithRotation(direction, accelerateAmount);
    }

    public void MoveWithRotation(Vector2 direction, float acceleration)
    {
        if (isPlayEnabled)
        {
            //rotation requirements
            DesiredDirection = direction;

            //move
            MoveOnly(direction, acceleration);
        }
    }

    /// <summary>
    /// Move the soccer player in the direction by it's accelerate and affect the physics.
    /// </summary>
    public void MoveOnly(Vector2 direction)
    {
        MoveOnlyPhysics(direction, accelerateAmount);
    }

    /// <summary>
    /// Move the soccer player in the direction by the accelerate and affect the physics.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="accelerate">Size of accelerate that pushes the soccer player.</param>
    public void MoveOnly(Vector2 direction, float accelerate)
    {
        MoveOnlyPhysics(direction, accelerate);
    }

    private void MoveOnlyPhysics(Vector2 direction, float accelerate)
    {
        if (isPlayEnabled)
        {
            if(direction != new Vector2(0, 0))
            {
                //find result velocity
                float frameAccelerate = accelerate * Time.deltaTime;
                Vector2 deltaVelocity = direction.normalized * frameAccelerate;
                Vector2 velocity = soccerRigidBody.velocity + deltaVelocity;

                //move body by affecting friction
                AddFriction(velocity);
            }
            else
            {
                AddFriction(soccerRigidBody.velocity);
            }
        }
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

            //if friction can change velocity.x sign
            if (xVelocity * velocity.x < 0)
            {
                xVelocity = 0;
            }

            //if friction can change velocity.y sign
            if (yVelocity * velocity.y < 0)
            {
                yVelocity = 0;
            }

            //
            //set rigidBody.velocity
            //
            SetVelocity(new Vector2(xVelocity, yVelocity));
        }
        else
        {
            //stop
            soccerRigidBody.velocity = new Vector2(0, 0);
        }
    }

    private void SetVelocity(Vector2 velocity)
    {
        //check max speed
        if (velocity.magnitude > maxSpeed)
        {
            float xRatio = velocity.x / velocity.magnitude;
            float yRatio = velocity.y / velocity.magnitude;

            soccerRigidBody.velocity = new Vector2(maxSpeed * xRatio, maxSpeed * yRatio);
        }
        else
        {
            soccerRigidBody.velocity = velocity;
        }
    }

    /// <summary>
    /// SoccerPlayer Rotates to the direction and then shoot. SoccerPlayer persists on shooting unless the ball
    /// be released by another effect. If you want do something after Shoot(Vector2), you can use 
    /// AfterDirectionalShoot(bool) event.
    /// </summary>
    /// <param name="direction">The direction which the soccerPlayer will shoot at.</param>
    public virtual void Shoot(Vector2 direction)
    {
        if (isPlayEnabled)
        {
            DesiredDirection = direction;
            canChangeDesiredDirection = false;

            checkShootCoroutine = StartCoroutine(CheckShoot());
            ReleasedBall += StopUnFinishedShoot;
        } 
    }

    IEnumerator CheckShoot()
    {
        yield return null;  //to make "checkShootCoroutine != null"

        while (checkShootCoroutine != null)
        {
            yield return null;

            if (Quaternion.Angle(transform.rotation, desiredRotation) < rotationSpeed * Time.deltaTime)
            { 
                StopDirectionalShoot(true);
                Shoot();
            }
        }
    }

    private void StopUnFinishedShoot()
    {
        StopDirectionalShoot(false);
    }

    private void StopDirectionalShoot(bool isSucceededShoot)
    {
        if (checkShootCoroutine != null)
        {
            StopCoroutine(checkShootCoroutine);
            canChangeDesiredDirection = true;
            checkShootCoroutine = null;
            ReleasedBall -= StopUnFinishedShoot;

            OnAfterDirectionalShoot(isSucceededShoot);
        }
    }

    /// <summary>
    /// Shoot at the SoccerPlayer's direction instantly.
    /// </summary>
    public void Shoot()
    {
        if (isPlayEnabled)
        {
            if (holdedBall != null)
            {
                if (attractCoroutine != null)
                {
                    holdedBall.transform.position = HolderPosition;
                }

                //shoot the ball
                Vector2 position2D = transform.up;
                float xRatio = position2D.x / position2D.magnitude;
                float yRatio = position2D.y / position2D.magnitude;
                soundHandler.PlayShootAudio();
                holdedBall.RigidBody.velocity = BallVelocityByMove + new Vector2(xRatio * shoootSpeed, yRatio * shoootSpeed);

                //release the ball(set the required variables)
                ReleaseBall();
            }
        }   
    }

    private void OnCatchBall(Ball ball)
    {
        if (CatchedBall != null)
        {
            CatchedBall(ball);
        }
    }

    private void OnReleaseBall()
    {
        if(ReleasedBall != null)
        {
            ReleasedBall();
        }
    }

    private void SetTeammatePlayerToAI()
    {
        //if a bot catches the ball it's teammate player(if exists) becomes AI and itself becomes Player.
        if (tag == "team1")
        {
            GameObject[] teammates = GameObject.FindGameObjectsWithTag("team1");

            foreach (GameObject teammate in teammates)
            {
                if (teammate.GetComponent<SoccerPlayer>().PlayerOrAIType == PlayerOrAI.Player)
                {
                    teammate.GetComponent<SoccerPlayer>().PlayerOrAIType = PlayerOrAI.AI;
                }
            }
        }
        else
        {
            GameObject[] teammates = GameObject.FindGameObjectsWithTag("team2");

            foreach (GameObject teammate in teammates)
            {
                if (teammate.GetComponent<SoccerPlayer>().PlayerOrAIType == PlayerOrAI.Player)
                {
                    teammate.GetComponent<SoccerPlayer>().PlayerOrAIType = PlayerOrAI.AI;
                }
            }
        }
    }

    private void Rob(SoccerPlayer robbedPlayer, Ball ball)
    {
        robbedPlayer.ReleaseBall();

        // let the robbed player be able to catch the ball only after a certain seconds.
        StartCoroutine(AgainCanCatch(robbedPlayer));

        CatchBall(ball);
    }

    IEnumerator AgainCanCatch(SoccerPlayer robbedPlayer)
    {
        robbedPlayer.isRobbed = true;
        yield return new WaitForSeconds(ROBBED_TIME);
        robbedPlayer.isRobbed = false;
    }

    private void Rotate()
    {
        previousDirection = currentDirection;
        float rotateAngle = Quaternion.Angle(transform.rotation, desiredRotation);
        float relativeInterpolate = (rotationSpeed * Time.deltaTime) / rotateAngle;
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, relativeInterpolate);
        currentDirection = transform.up;
    }

    private void OnDesiredRotationChanged()
    {
        if (DesiredRotationChanged != null)
        {
            DesiredRotationChanged();
        }
    }

    private void OnAfterDirectionalShoot(bool isSucceededShoot)
    {
        if (AfterDirectionalShoot != null)
        {
            AfterDirectionalShoot(isSucceededShoot);
        }
    }

    public void PlayReset()
    {
        //enable the soccer player
        IsPlayEnabled = true;

        //set required variables to default
        previousDirection = transform.up;
        currentDirection = transform.up;
        if (isDefaultPlayer)
        {
            PlayerOrAIType = PlayerOrAI.Player;
        }

        FindObjectOfType<AIControll>().enabled = true;
    }
}

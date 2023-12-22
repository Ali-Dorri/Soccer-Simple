using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//Delegate definition
//

public delegate void TwoVector2Assigner(ref Vector2 v1, ref Vector2 v2);
public delegate void BooleanAssigner(ref bool boolVar);

public class Player : MonoBehaviour, IPlayEnable
{
    //
    //Concept Definition
    //

    public enum InputType { MouseAndKeyboard, JoyStick, MouseAndKeyboard_JoyStick }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    SoccerPlayer soccerPlayer;
    PlayerInput[] playerInputs;
    InputType inputType;

    //events
    public event TwoVector2Assigner DetermineMove;
    public event BooleanAssigner CheckShoot;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public bool IsPlayEnabled
    {
        get
        {
            return playerInputs[0].IsPlayEnabled;
        }
        set
        {
            this.enabled = value;

            foreach(PlayerInput playerInput in playerInputs)
            {
                playerInput.IsPlayEnabled = value;
            }
        }
    }

    public InputType TypeOfInput
    {
        get
        {
            return inputType;
        }
        set
        {
            inputType = value;

            //empty events' function references
            DetermineMove = null;
            CheckShoot = null;

            //add or find player inputs according to inputType
            SetPlayerInputs(value);

            //set events
            foreach(PlayerInput playerInput in playerInputs)
            {
                DetermineMove += playerInput.DetermineMove;
                CheckShoot += playerInput.CheckShoot;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    public void Initialize(InputType inputType)
    {
        soccerPlayer = GetComponent<SoccerPlayer>();
        TypeOfInput = inputType;
    }

    /////////////////////////////////////////////////////////////////////////////

    void Update ()
    {
        //find moveDirection, faceDirection and hasShooted
        Vector2 moveDirection;
        Vector2 faceDirection;
        bool hasShooted;
        DetermineInput(out moveDirection, out faceDirection, out hasShooted);

        //move
        soccerPlayer.MoveOnly(moveDirection,5f);
        //rotation
        soccerPlayer.DesiredDirection = faceDirection;
        //check shoot
        if (hasShooted)
        {
            soccerPlayer.Shoot();
        }
	}

    private void DetermineInput(out Vector2 moveDirection, out Vector2 faceDirection, out bool hasShooted)
    {
        moveDirection = new Vector2(0, 0);
        faceDirection = soccerPlayer.DesiredDirection;
        hasShooted = false;

        //determin input if any input is occured
        OnDetermineMove(ref moveDirection, ref faceDirection);
        OnCheckShoot(ref hasShooted);
    }

    private void OnDetermineMove(ref Vector2 moveDirection, ref Vector2 faceDirection)
    {
        if (DetermineMove != null)
        {
            DetermineMove(ref moveDirection, ref faceDirection);
        }
    }

    private void OnCheckShoot(ref bool hasShooted)
    {
        if (CheckShoot != null)
        {
            CheckShoot(ref hasShooted);
        }
    }

    private void SetPlayerInputs(InputType inputType)
    {
        if (inputType == InputType.MouseAndKeyboard)
        {
            playerInputs = new PlayerInput[1];

            //add or find keyboard input component
            PlayerInput keyboardInput = GetComponent<KeyboardAndMouseInput>();
            if (keyboardInput != null)
            {
                playerInputs[0] = keyboardInput;
            }
            else
            {
                playerInputs[0] = gameObject.AddComponent<KeyboardAndMouseInput>();
            }

            //delete joystick input component
            PlayerInput joyStickInput = GetComponent<JoyStickInput>();
            if (joyStickInput != null)
            {
                Destroy(joyStickInput);
            }
        }
        else if (inputType == InputType.JoyStick)
        {
            playerInputs = new PlayerInput[1];

            //add or find joystick input component
            PlayerInput joyStickInput = GetComponent<JoyStickInput>();
            if (joyStickInput != null)
            {
                playerInputs[0] = joyStickInput;
            }
            else
            {
                playerInputs[0] = gameObject.AddComponent<JoyStickInput>();
            }

            //delete keyboard input component
            PlayerInput keyboardInput = GetComponent<KeyboardAndMouseInput>();
            if (keyboardInput != null)
            {
                Destroy(keyboardInput);
            }
        }
        else
        {
            playerInputs = new PlayerInput[2];

            PlayerInput keyboardInput = GetComponent<KeyboardAndMouseInput>();
            PlayerInput joyStickInput = GetComponent<JoyStickInput>();
            if (keyboardInput != null)
            {
                playerInputs[0] = keyboardInput;
            }
            else
            {
                playerInputs[0] = gameObject.AddComponent<KeyboardAndMouseInput>();
            }
            if (joyStickInput != null)
            {
                playerInputs[1] = joyStickInput;
            }
            else
            {
                playerInputs[1] = gameObject.AddComponent<JoyStickInput>();
            }
        }
    }
}
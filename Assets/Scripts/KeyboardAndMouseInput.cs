using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardAndMouseInput : PlayerInput
{
    //
    //Concept Definition
    //

    protected enum XDirection { Left, Right, Stop }
    protected enum YDirection { Up, Down, Stop }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    //direction variables
    XDirection xDirection = XDirection.Stop;
    YDirection yDirection = YDirection.Stop;
    bool isRightPressed = false;
    bool isLeftPressed = false;
    bool isUpPressed = false;
    bool isDownPressed = false;

    //key codes
    KeyCode upKey = KeyCode.W;
    KeyCode downKey = KeyCode.S;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;
    KeyCode shootKey = KeyCode.Mouse0;  //left click

    //other variables
    Vector2 sceneCenter;
    float mainCameraHeight;
    Transform soccerPlayerTransform;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        sceneCenter = Camera.main.transform.position;
        mainCameraHeight = Camera.main.orthographicSize * 2;
        soccerPlayerTransform = transform;
    }

    public void Initialize(KeyCode upKey, KeyCode downKey, KeyCode leftKey, KeyCode rightKey, KeyCode shootKey)
    {
        this.upKey = upKey;
        this.downKey = downKey;
        this.leftKey = leftKey;
        this.rightKey = rightKey;
        this.shootKey = shootKey;
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    private void CheckButtons()
    {
        //left and right check
        if (Input.GetKey(leftKey))
        {
            if (!isLeftPressed)
            {
                xDirection = XDirection.Left;
                isLeftPressed = true;
            }
        }
        else
        {
            if (isLeftPressed)
            {
                isLeftPressed = false;

                if (isRightPressed)
                {
                    xDirection = XDirection.Right;
                }
                else
                {
                    xDirection = XDirection.Stop;
                }
            }
        }

        if (Input.GetKey(rightKey))
        {
            if (!isRightPressed)
            {
                xDirection = XDirection.Right;
                isRightPressed = true;
            }
        }
        else
        {
            if (isRightPressed)
            {
                isRightPressed = false;

                if (isLeftPressed)
                {
                    xDirection = XDirection.Left;
                }
                else
                {
                    xDirection = XDirection.Stop;
                }
            }
        }

        //up and down check
        if (Input.GetKey(upKey))
        {
            if (!isUpPressed)
            {
                yDirection = YDirection.Up;
                isUpPressed = true;
            }
        }
        else
        {
            if (isUpPressed)
            {
                isUpPressed = false;

                if (isDownPressed)
                {
                    yDirection = YDirection.Down;
                }
                else
                {
                    yDirection = YDirection.Stop;
                }
            }
        }

        if (Input.GetKey(downKey))
        {
            if (!isDownPressed)
            {
                yDirection = YDirection.Down;
                isDownPressed = true;
            }
        }
        else
        {
            if (isDownPressed)
            {
                isDownPressed = false;

                if (isUpPressed)
                {
                    yDirection = YDirection.Up;
                }
                else
                {
                    yDirection = YDirection.Stop;
                }
            }
        }
    }

    protected override void DetermineMoveByButtonStatus(ref Vector2 moveDirection, ref Vector2 faceDirection)
    {
        //find the xDirection and yDirection
        CheckButtons();

        //move direction
        DetermineMoveDirection(ref moveDirection);
        //face direction
        DetermineFaceDirection(ref faceDirection);
    }

    private void DetermineMoveDirection(ref Vector2 moveDirection)
    {
        //stop check
        if (xDirection == XDirection.Stop && yDirection == YDirection.Stop)
        {
            moveDirection = new Vector2(0, 0);
        }
        else
        {
            //xdirection check
            if (xDirection == XDirection.Left)
            {
                moveDirection.x = -1;
            }
            else if (xDirection == XDirection.Right)
            {
                moveDirection.x = 1;
            }
            else
            {
                moveDirection.x = 0;
            }

            //y direction check
            if (yDirection == YDirection.Up)
            {
                moveDirection.y = 1;
            }
            else if (yDirection == YDirection.Down)
            {
                moveDirection.y = -1;
            }
            else
            {
                moveDirection.y = 0;
            } 
        }
    }

    private void DetermineFaceDirection(ref Vector2 faceDirection)
    {
        float mouseXPixel = Input.mousePosition.x - (Screen.width / 2);
        float mouseYPixel = Input.mousePosition.y - (Screen.height / 2);
        float screenToWorldRatio = mainCameraHeight / Screen.height;
        Vector2 mouseVectorFromCenter = new Vector2(mouseXPixel * screenToWorldRatio, mouseYPixel * screenToWorldRatio);
        faceDirection = (sceneCenter + mouseVectorFromCenter) - (Vector2)soccerPlayerTransform.position;
    }

    public override void CheckShoot(ref bool hasShooted)
    {
        if (IsPlayEnabled)
        {
            if (Input.GetKeyDown(shootKey))
            {
                hasShooted = true;
            }
        }
    }
}

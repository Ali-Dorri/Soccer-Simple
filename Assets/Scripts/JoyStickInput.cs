using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickInput : PlayerInput
{
    //
    //Concept Definition
    //

    delegate float FloatReturner(int a);

    private enum Index { MoveX, MoveY, FaceX, FaceY }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    KeyCode shootKey = KeyCode.JoystickButton7;

    //movement and rotation axis inputs(keycode or axis)
    IAxisInput[] axisInputs = new IAxisInput[4];

    //movement and rotation booleans
    FloatReturner determineMoveX;
    FloatReturner determineMoveY;
    FloatReturner determineFaceX;
    FloatReturner determineFaceY;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    /// <summary>
    /// Default initialization.
    /// </summary>
    public void Initialize()
    {
        shootKey = KeyCode.JoystickButton7;

        axisInputs[(int)Index.MoveX] = new AxisName("LeftJoyStickX");
        axisInputs[(int)Index.MoveY] = new AxisName("LeftJoyStickY");
        axisInputs[(int)Index.FaceX] = new AxisName("RightJoyStickX");
        axisInputs[(int)Index.FaceY] = new AxisName("RightJoyStickY");
        determineMoveX = DetermineByAxis;
        determineMoveY = DetermineByAxis;
        determineFaceX = DetermineByAxis;
        determineFaceY = DetermineByAxis;
    }

    public void Initialize(KeyCode shootKey, IAxisInput moveX, IAxisInput moveY, IAxisInput faceX, IAxisInput faceY)
    {
        this.shootKey = shootKey;

        //fill the axis input array
        axisInputs[(int)Index.MoveX] = moveX;
        axisInputs[(int)Index.MoveY] = moveY;
        axisInputs[(int)Index.FaceX] = faceX;
        axisInputs[(int)Index.FaceY] = faceY;

        //set delegate functions (by axis or keycode)
        if(moveX is AxisName)
        {
            determineMoveX = DetermineByAxis;
        }
        else
        {
            determineMoveX = DetermineByKey;
        }
        if (moveY is AxisName)
        {
            determineMoveY = DetermineByAxis;
        }
        else
        {
            determineMoveY = DetermineByKey;
        }
        if (faceX is AxisName)
        {
            determineFaceX = DetermineByAxis;
        }
        else
        {
            determineFaceX = DetermineByKey;
        }
        if (faceY is AxisName)
        {
            determineFaceY = DetermineByAxis;
        }
        else
        {
            determineFaceY = DetermineByKey;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    protected override void DetermineMoveByButtonStatus(ref Vector2 moveDirection, ref Vector2 faceDirection)
    {
        //determine movement and rotation directions
        float xMoveDirection = determineMoveX((int)Index.MoveX);
        float yMoveDirection = determineMoveY((int)Index.MoveY);
        float xFaceDirection = determineFaceX((int)Index.FaceX);
        float yFaceDirection = determineFaceY((int)Index.FaceY);

        //set the directions
        if (xMoveDirection != 0)
        {
            if (yMoveDirection != 0)
            {
                moveDirection = new Vector2(xMoveDirection, yMoveDirection);
            }
            else
            {
                moveDirection = new Vector2(xMoveDirection, 0);
            }
        }
        else
        {
            if (yMoveDirection != 0)
            {
                moveDirection = new Vector2(0, yMoveDirection);
            }
        }
        if (xFaceDirection != 0)
        {
            if (yFaceDirection != 0)
            {
                faceDirection = new Vector2(xFaceDirection, yFaceDirection);
            }
            else
            {
                faceDirection = new Vector2(xFaceDirection, 0);
            }
        }
        else
        {
            if (yFaceDirection != 0)
            {
                faceDirection = new Vector2(0, yFaceDirection);
            }
        }
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

    private float DetermineByKey(int index)
    {
        AxisKey axisKey = (AxisKey)axisInputs[index];


        //find axiKeyStatus
        if (Input.GetKey(axisKey.negativeKey))
        {
            if (!axisKey.isNegative)
            {
                axisKey.axisKeyStatus = AxisKeyStatus.Negative;
                axisKey.isNegative = true;
            }
        }
        else
        {
            if (axisKey.isNegative)
            {
                axisKey.isNegative = false;

                if (axisKey.isPositive)
                {
                    axisKey.axisKeyStatus = AxisKeyStatus.Positive;
                }
                else
                {
                    axisKey.axisKeyStatus = AxisKeyStatus.Neutral;
                }
            }
        }

        if (Input.GetKey(axisKey.positiveKey))
        {
            if (!axisKey.isPositive)
            {
                axisKey.axisKeyStatus = AxisKeyStatus.Positive;
                axisKey.isPositive = true;
            }
        }
        else
        {
            if (axisKey.isPositive)
            {
                axisKey.isPositive = false;

                if (axisKey.isNegative)
                {
                    axisKey.axisKeyStatus = AxisKeyStatus.Negative;
                }
                else
                {
                    axisKey.axisKeyStatus = AxisKeyStatus.Neutral;
                }
            }
        }




        ////find axiKeyStatus
        //if (Input.GetKeyDown(axisKey.positiveKey))
        //{
        //    axisKey.isPositive = true;
        //    axisKey.axisKeyStatus = AxisKeyStatus.Positive;
        //}
        //if (Input.GetKeyDown(axisKey.negativeKey))
        //{
        //    axisKey.isNegative = true;
        //    axisKey.axisKeyStatus = AxisKeyStatus.Negative;
        //}
        //if (Input.GetKeyUp(axisKey.positiveKey))
        //{
        //    axisKey.isPositive = false;

        //    if (axisKey.isNegative)
        //    {
        //        axisKey.axisKeyStatus = AxisKeyStatus.Negative;
        //    }
        //    else
        //    {
        //        axisKey.axisKeyStatus = AxisKeyStatus.Neutral;
        //    }
        //}
        //if (Input.GetKeyUp(axisKey.negativeKey))
        //{
        //    axisKey.isNegative = false;
        //    if (axisKey.isPositive)
        //    {
        //        axisKey.axisKeyStatus = AxisKeyStatus.Positive;
        //    }
        //    else
        //    {
        //        axisKey.axisKeyStatus = AxisKeyStatus.Neutral;
        //    }
        //}

        //find float representation of axis
        if (axisKey.axisKeyStatus == AxisKeyStatus.Positive)
        {
            return 1;
        }
        else if (axisKey.axisKeyStatus == AxisKeyStatus.Negative)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    private float DetermineByAxis(int index)
    {
        return ((AxisName)axisInputs[index]).GetAxis();
    }
}

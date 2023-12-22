using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface to let programmer store AxisKey and AxisName in an array or list.
/// </summary>
public interface IAxisInput
{
    //nothing 
}

public enum AxisKeyStatus { Positive, Negative, Neutral }

/// <summary>
/// Each two keys can perform like an axis.
/// </summary>
public class AxisKey : IAxisInput
{
    //key
    public KeyCode positiveKey;
    public KeyCode negativeKey;

    //direction status
    public bool isPositive;
    public bool isNegative;
    public AxisKeyStatus axisKeyStatus;

    //constructor
    public AxisKey(KeyCode positiveKey, KeyCode negativeKey)
    {
        this.positiveKey = positiveKey;
        this.negativeKey = negativeKey;

        isPositive = false;
        isNegative = false;
        axisKeyStatus = AxisKeyStatus.Neutral;
    }
}

public struct AxisName : IAxisInput
{
    //
    //Fields
    //

    public string name;
    private bool isInverted;
    private int sign;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public bool IsInverted
    {
        get
        {
            return isInverted;
        }
        set
        {
            isInverted = value;

            if (value)
            {
                sign = -1;
            }
            else
            {
                sign = 1;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Constructors
    //

    public AxisName(string name)
    {
        this.name = name;
        isInverted = false;
        sign = 1;
    }

    public AxisName(string name, bool isInverted)
    {
        this.name = name;
        this.isInverted = isInverted;

        if (isInverted)
        {
            sign = -1;
        }
        else
        {
            sign = 1;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public float GetAxis()
    {
        return sign * (Input.GetAxis(name));
    }
}
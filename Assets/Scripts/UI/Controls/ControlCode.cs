using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlCode
{
    protected readonly int code;

    public int Code
    {
        get
        {
            return code;
        }
    }

    public ControlCode(int code)
    {
        this.code = code;
    }
}

public class AxisCode : ControlCode
{
    IAxisInput axisInput;

    public IAxisInput AxisInput
    {
        get
        {
            return axisInput;
        }
        set
        {
            axisInput = value;
        }
    }

    public AxisCode(int code, IAxisInput axisInput) : base(code)
    {
        this.axisInput = axisInput;
    }
}

public class KeyCodeCode : ControlCode
{
    KeyCode keyCode;

    public KeyCode ControlKeyCode
    {
        get
        {
            return keyCode;
        }
        set
        {
            keyCode = value;
        }
    }

    public KeyCodeCode(int code, KeyCode keyCode) : base(code)
    {
        this.keyCode = keyCode;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections.Generic;

public class ControlsInputManager : MonoBehaviour
{
    //
    //Concept Definition
    //

    private enum InputControlCode
    {
        KeyboardMoveUp, KeyboardMoveDown, KeyboardMoveRight, KeyboardMoveLeft, KeyboardFaceUp,
        KeyboardFaceDown, KeyboardFaceRight, KeyboardFaceLeft, KeyboardShoot, JoyStickMoveUp,
        JoyStickMoveDown, JoyStickMoveRight, JoyStickMoveLeft, JoyStickFaceUp, JoyStickFaceDown,
        JoyStickFaceRight, JoyStickFaceLeft, JoyStickShoot
    }

    enum SavedAxisCodeType { AxisKey, AxisName }

    private delegate bool Condition(int a, string b);

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    OptionContainer optionContainer;

    //controls
    ControlCode[] controls;
    Text[] controlTexts;
    Button[] controlButtons;

    //keyboard and joystick enabling
    bool isKeyboardAndMouseEanbled = true;
    bool isJoyStickEnabled = true;

    //constants
    const string FILE_PATH_Without_NAME = "Option Variables";
    const string FILE_NAME = "ControlsData.controls";
    const string FULL_FILE_PATH = FILE_PATH_Without_NAME + "\\" + FILE_NAME;
    const string LOCAL_OPTION_FOLDER = "Option Variables";
    const string LOCAL_CONTROLS_FILE = "ControlsData.controls";

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    void Start ()
    {
        controlTexts = new Text[18];
        controlButtons = new Button[18];
        optionContainer = FindObjectOfType<OptionContainer>();
        GameObject.Find("Apply").GetComponent<Button>().onClick.AddListener(ApplyButton);
        GameObject.Find("Help").GetComponent<Button>().onClick.AddListener(HelpButton);
        GameObject.Find("Default").GetComponent<Button>().onClick.AddListener(DefaultButton);

        //the order is necessary from here
        InitializeControlsOnRows();
        FillControlCodesArrayAndTexts();
        InitializeKeyboardAndJoyStickEnabling();
	}

    /// <summary>
    /// Initialize controls on rows and fill controlTexts and controlButtons arrays.
    /// </summary>
    private void InitializeControlsOnRows()
    {
        ControlOnRow[] controls = FindObjectsOfType<ControlOnRow>();
        foreach(ControlOnRow control in controls)
        {
            control.Initialize(controlTexts, controlButtons);
        }
    }

    private void FillControlCodesArrayAndTexts()
    {
        //required variables
        KeyCode[] keyboardKeyCodes = optionContainer.KeyboardKeyCodes;
        IAxisInput[] joystickAxes = optionContainer.JoyStickAxes;
        KeyCode joystickShootKey = optionContainer.JoyStickShootKey;
        int[] codes = optionContainer.CodesOfControls;
        int loopLength = codes.Length;
        int keycodeCounter = 0;
        int iaxisCounter = 0;
        int codeCounter = 0;

        //initialize controls
        controls = new ControlCode[loopLength];
        for(codeCounter = 0; codeCounter < loopLength; codeCounter++)
        {
            int code = codes[codeCounter];

            if ((code >= 0 && code < 4) || code == 8)
            {
                controls[codeCounter] = new KeyCodeCode(code, keyboardKeyCodes[keycodeCounter]);
                FillTextKeyboard(code, keyboardKeyCodes[keycodeCounter]);
                keycodeCounter++;
            }
            else if(code > 8 && code < 17 && code % 2 == 1)
            {
                controls[codeCounter] = new AxisCode(code, joystickAxes[iaxisCounter]);
                FillTextJoystick(code, joystickAxes[iaxisCounter]);
                iaxisCounter++;
            }
            else if(code == 17)
            {
                controls[codeCounter] = new KeyCodeCode(code, joystickShootKey);
                FillTextJoystick(code, joystickShootKey);
            }
            else
            {
                MessageBox.ShowMessage("Some controls were not saved correctly so they will be shown here incorrectly." +
                                       "You can reload controls menu again or set all controls here again and apply them.");
            }
        }
    }

    private void FillTextKeyboard(int code, KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.Q:
                SetControlText(code, "Q");
                break;
            case KeyCode.W:
                SetControlText(code, "W");
                break;
            case KeyCode.E:
                SetControlText(code, "E");
                break;
            case KeyCode.R:
                SetControlText(code, "R");
                break;
            case KeyCode.T:
                SetControlText(code, "T");
                break;
            case KeyCode.Y:
                SetControlText(code, "Y");
                break;
            case KeyCode.U:
                SetControlText(code, "U");
                break;
            case KeyCode.I:
                SetControlText(code, "I");
                break;
            case KeyCode.O:
                SetControlText(code, "O");
                break;
            case KeyCode.P:
                SetControlText(code, "P");
                break;
            case KeyCode.LeftBracket:
                SetControlText(code, "[");
                break;
            case KeyCode.RightBracket:
                SetControlText(code, "]");
                break;
            case KeyCode.A:
                SetControlText(code, "A");
                break;
            case KeyCode.S:
                SetControlText(code, "S");
                break;
            case KeyCode.D:
                SetControlText(code, "D");
                break;
            case KeyCode.F:
                SetControlText(code, "F");
                break;
            case KeyCode.G:
                SetControlText(code, "G");
                break;
            case KeyCode.H:
                SetControlText(code, "H");
                break;
            case KeyCode.J:
                SetControlText(code, "J");
                break;
            case KeyCode.K:
                SetControlText(code, "K");
                break;
            case KeyCode.L:
                SetControlText(code, "L");
                break;
            case KeyCode.Semicolon:
                SetControlText(code, ";");
                break;
            case KeyCode.Quote:
                SetControlText(code, "'");
                break;
            case KeyCode.Backslash:
                SetControlText(code, "\\");
                break;
            case KeyCode.Z:
                SetControlText(code, "Z");
                break;
            case KeyCode.X:
                SetControlText(code, "X");
                break;
            case KeyCode.V:
                SetControlText(code, "V");
                break;
            case KeyCode.B:
                SetControlText(code, "B");
                break;
            case KeyCode.N:
                SetControlText(code, "N");
                break;
            case KeyCode.M:
                SetControlText(code, "M");
                break;
            case KeyCode.Comma:
                SetControlText(code, ",");
                break;
            case KeyCode.Period:
                SetControlText(code, ".");
                break;
            case KeyCode.Slash:
                SetControlText(code, "/");
                break;
            case KeyCode.LeftShift:
                SetControlText(code, "LeftShift");
                break;
            case KeyCode.RightShift:
                SetControlText(code, "RightShift");
                break;
            case KeyCode.LeftControl:
                SetControlText(code, "LeftControl");
                break;
            case KeyCode.RightControl:
                SetControlText(code, "RightControl");
                break;
            case KeyCode.LeftAlt:
                SetControlText(code, "LeftAlt");
                break;
            case KeyCode.RightAlt:
                SetControlText(code, "RightAlt");
                break;
            case KeyCode.Space:
                SetControlText(code, "Space");
                break;
            case KeyCode.UpArrow:
                SetControlText(code, "UpArrow");
                break;
            case KeyCode.DownArrow:
                SetControlText(code, "DownArrow");
                break;
            case KeyCode.RightArrow:
                SetControlText(code, "RightArrow");
                break;
            case KeyCode.LeftArrow:
                SetControlText(code, "LeftArrow");
                break;
            case KeyCode.Tab:
                SetControlText(code, "Tab");
                break;
            case KeyCode.Alpha0:
                SetControlText(code, "0");
                break;
            case KeyCode.Alpha1:
                SetControlText(code, "1");
                break;
            case KeyCode.Alpha2:
                SetControlText(code, "2");
                break;
            case KeyCode.Alpha3:
                SetControlText(code, "3");
                break;
            case KeyCode.Alpha4:
                SetControlText(code, "4");
                break;
            case KeyCode.Alpha5:
                SetControlText(code, "5");
                break;
            case KeyCode.Alpha6:
                SetControlText(code, "6");
                break;
            case KeyCode.Alpha7:
                SetControlText(code, "7");
                break;
            case KeyCode.Alpha8:
                SetControlText(code, "8");
                break;
            case KeyCode.Alpha9:
                SetControlText(code, "9");
                break;
            case KeyCode.Equals:
                SetControlText(code, "=");
                break;
            case KeyCode.Minus:
                SetControlText(code, "-");
                break;
            case KeyCode.Mouse0:
                SetControlText(code, "Left Click");
                break;
            case KeyCode.Mouse1:
                SetControlText(code, "Right Click");
                break;
            case KeyCode.Mouse2:
                SetControlText(code, "Middle Mouse");
                break;
        }
    }

    private void FillTextJoystick(int oddCode, IAxisInput axis)
    {
        if(axis is AxisName)
        {
            FillTextJoystick(oddCode, (AxisName)axis, true);        //positive text
            FillTextJoystick(oddCode + 1, (AxisName)axis, false);   //negative text
        }
        else
        {
            FillTextJoystick(oddCode, (AxisKey)axis);
        }
    }

    private void FillTextJoystick(int oddCode, AxisKey axis)
    {
        FillTextJoystick(oddCode, axis.positiveKey);
        FillTextJoystick(oddCode + 1, axis.negativeKey);
    }

    private void FillTextJoystick(int code, KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.JoystickButton0:
                SetControlText(code, "JoystickButton 0");
                break;
            case KeyCode.JoystickButton1:
                SetControlText(code, "JoystickButton 1");
                break;
            case KeyCode.JoystickButton2:
                SetControlText(code, "JoystickButton 2");
                break;
            case KeyCode.JoystickButton3:
                SetControlText(code, "JoystickButton 3");
                break;
            case KeyCode.JoystickButton4:
                SetControlText(code, "JoystickButton 4");
                break;
            case KeyCode.JoystickButton5:
                SetControlText(code, "JoystickButton 5");
                break;
            case KeyCode.JoystickButton6:
                SetControlText(code, "JoystickButton 6");
                break;                      
            case KeyCode.JoystickButton7:
                SetControlText(code, "JoystickButton 7");
                break;                      
            case KeyCode.JoystickButton8:
                SetControlText(code, "JoystickButton 8");
                break;                      
            case KeyCode.JoystickButton9:
                SetControlText(code, "JoystickButton 9");
                break;                      
            case KeyCode.JoystickButton10:
                SetControlText(code, "JoystickButton 10");
                break;                      
            case KeyCode.JoystickButton11:
                SetControlText(code, "JoystickButton 11");
                break;                      
            case KeyCode.JoystickButton12:
                SetControlText(code, "JoystickButton 12");
                break;                      
            case KeyCode.JoystickButton13:
                SetControlText(code, "JoystickButton 13");
                break;                      
            case KeyCode.JoystickButton14:
                SetControlText(code, "JoystickButton 14");
                break;                      
            case KeyCode.JoystickButton15:
                SetControlText(code, "JoystickButton 15");
                break;                      
            case KeyCode.JoystickButton16:
                SetControlText(code, "JoystickButton 16");
                break;                      
            case KeyCode.JoystickButton17:
                SetControlText(code, "JoystickButton 17");
                break;
            case KeyCode.JoystickButton18:
                SetControlText(code, "JoystickButton 18");
                break;
            case KeyCode.JoystickButton19:
                SetControlText(code, "JoystickButton 19");
                break;
        }
    }

    private void FillTextJoystick(int code, AxisName axis, bool isPositiveText)
    {
        string axisName = axis.name;

        if(axisName == "LeftJoyStickX")
        {
            if (isPositiveText)
            {
                if (axis.IsInverted)
                {
                    SetControlText(code, "Left Stick Left");
                }
                else
                {
                    SetControlText(code, "Left Stick Right");
                }
            }
            else
            {
                if (axis.IsInverted)
                {
                    SetControlText(code, "Left Stick Right");
                }
                else
                {
                    SetControlText(code, "Left Stick Left");
                }
            }
        }
        else if (axisName == "LeftJoyStickY")
        {
            if (isPositiveText)
            {
                if (axis.IsInverted)
                {
                    SetControlText(code, "Left Stick Down");
                }
                else
                {
                    SetControlText(code, "Left Stick Up");
                }
            }
            else
            {
                if (axis.IsInverted)
                {
                    SetControlText(code, "Left Stick Up");
                }
                else
                {
                    SetControlText(code, "Left Stick Down");
                }
            }
        }
        else if (axisName == "RightJoyStickX")
        {
            if (isPositiveText)
            {
                if (axis.IsInverted)
                {
                    SetControlText(code, "Right Stick Left");
                }
                else
                {
                    SetControlText(code, "Right Stick Right");
                }
            }
            else
            {
                if (axis.IsInverted)
                {
                    SetControlText(code, "Right Stick Right");
                }
                else
                {
                    SetControlText(code, "Right Stick Left");
                }
            }
        }                     
        else if (axisName == "RightJoyStickY")
        {
            if (isPositiveText)
            {
                if (axis.IsInverted)
                {
                    SetControlText(code, "Right Stick Down");
                }
                else
                {
                    SetControlText(code, "Right Stick Up");
                }
            }
            else
            {
                if (axis.IsInverted)
                {
                    SetControlText(code, "Right Stick Up");
                }
                else
                {
                    SetControlText(code, "Right Stick Down");
                }
            }
        }
    }

    /// <summary>
    /// Determine enable status of keyboard and mouse and joystick cotrols.
    /// </summary>
    private void InitializeKeyboardAndJoyStickEnabling()
    {
        if (optionContainer.InputType == Player.InputType.MouseAndKeyboard)
        {
            EnableKeyboardAndMouseInput(true);
            EnableJoyStickInput(false);
        }
        else if (optionContainer.InputType == Player.InputType.JoyStick)
        {
            EnableKeyboardAndMouseInput(false);
            EnableJoyStickInput(true);
        }
        else
        {
            EnableKeyboardAndMouseInput(true);
            EnableJoyStickInput(true);
        }

        //add listener to "Keyboard And Mouse" and "JoyStick" buttons
        GameObject.Find("Keyboard And Mouse Button").GetComponent<Button>().onClick.AddListener(EnableKeyboardAndMouseInput);
        GameObject.Find("JoyStick Button").GetComponent<Button>().onClick.AddListener(EnableJoyStickInput);
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void EnableCheckControlInput(int codeOfControl)
    {
        if(0 <= codeOfControl && codeOfControl <= 8)    //keyboard and mouse
        {
            StartCoroutine(CheckInput(codeOfControl, KeyboardCheckInput));
        }
        else if(8 < codeOfControl && codeOfControl < 18)    //joystick
        {
            StartCoroutine(CheckInput(codeOfControl, JoyStickCheckInput));
        }
        else
        {
            throw new ArgumentOutOfRangeException("There is no control code out of 0 to 17 code in ControlsInputManager's controls!");
        }
    }

    private IEnumerator CheckInput(int code, Condition checker)
    {
        //required variables
        bool isInputSelected = false;
        Text selectedText = controlTexts[code];
        string previousString = selectedText.text;

        //clear the button
        selectedText.text = string.Empty;
        controlButtons[code].enabled = false;

        while (!isInputSelected)
        {
            yield return null;

            isInputSelected = checker(code, previousString);
        }

        //enable button again in a few next frames to not allow click heppen on the button just after the input is
        //selected(in situation we press left click for input)
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        controlButtons[code].enabled = true;
    }

    #region Keyboard functions
    private bool KeyboardCheckInput(int code, string previousString)
    {
        bool returnValue = false;

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                controlTexts[code].text = previousString;
                returnValue = true;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                RemoveControlCode(code);
                controlTexts[code].text = "Empty";
                returnValue = true;
            }
            else
            {
                returnValue = KeyboardIfStatements(code);
            } 
        }

        return returnValue;
    }

    private bool KeyboardIfStatements(int code)
    {
        bool returnValue = true;

        if (Input.GetKeyDown(KeyCode.Q))
            SetKeyboardControl(code, KeyCode.Q, "Q");
        else if (Input.GetKeyDown(KeyCode.W))
            SetKeyboardControl(code, KeyCode.W, "W");
        else if (Input.GetKeyDown(KeyCode.E))
            SetKeyboardControl(code, KeyCode.E, "E");
        else if (Input.GetKeyDown(KeyCode.R))
            SetKeyboardControl(code, KeyCode.R, "R");
        else if (Input.GetKeyDown(KeyCode.T))
            SetKeyboardControl(code, KeyCode.T, "T");
        else if (Input.GetKeyDown(KeyCode.Y))
            SetKeyboardControl(code, KeyCode.Y, "Y");
        else if (Input.GetKeyDown(KeyCode.U))
            SetKeyboardControl(code, KeyCode.U, "U");
        else if (Input.GetKeyDown(KeyCode.I))
            SetKeyboardControl(code, KeyCode.I, "I");
        else if (Input.GetKeyDown(KeyCode.O))
            SetKeyboardControl(code, KeyCode.O, "O");
        else if (Input.GetKeyDown(KeyCode.P))
            SetKeyboardControl(code, KeyCode.P, "P");
        else if (Input.GetKeyDown(KeyCode.LeftBracket))
            SetKeyboardControl(code, KeyCode.LeftBracket, "[");
        else if (Input.GetKeyDown(KeyCode.RightBracket))
            SetKeyboardControl(code, KeyCode.RightBracket, "]");
        else if (Input.GetKeyDown(KeyCode.A))
            SetKeyboardControl(code, KeyCode.A, "A");
        else if (Input.GetKeyDown(KeyCode.S))
            SetKeyboardControl(code, KeyCode.S, "S");
        else if (Input.GetKeyDown(KeyCode.D))
            SetKeyboardControl(code, KeyCode.D, "D");
        else if (Input.GetKeyDown(KeyCode.F))
            SetKeyboardControl(code, KeyCode.F, "F");
        else if (Input.GetKeyDown(KeyCode.G))
            SetKeyboardControl(code, KeyCode.G, "G");
        else if (Input.GetKeyDown(KeyCode.H))
            SetKeyboardControl(code, KeyCode.H, "H");
        else if (Input.GetKeyDown(KeyCode.J))
            SetKeyboardControl(code, KeyCode.J, "J");
        else if (Input.GetKeyDown(KeyCode.K))
            SetKeyboardControl(code, KeyCode.K, "K");
        else if (Input.GetKeyDown(KeyCode.L))
            SetKeyboardControl(code, KeyCode.L, "L");
        else if (Input.GetKeyDown(KeyCode.Semicolon))
            SetKeyboardControl(code, KeyCode.Semicolon, ";");
        else if (Input.GetKeyDown(KeyCode.Quote))
            SetKeyboardControl(code, KeyCode.Quote, "'");
        else if (Input.GetKeyDown(KeyCode.Backslash))
            SetKeyboardControl(code, KeyCode.Backslash, "\\");
        else if (Input.GetKeyDown(KeyCode.Z))
            SetKeyboardControl(code, KeyCode.Z, "Z");
        else if (Input.GetKeyDown(KeyCode.X))
            SetKeyboardControl(code, KeyCode.X, "X");
        else if (Input.GetKeyDown(KeyCode.C))
            SetKeyboardControl(code, KeyCode.C, "C");
        else if (Input.GetKeyDown(KeyCode.V))
            SetKeyboardControl(code, KeyCode.V, "V");
        else if (Input.GetKeyDown(KeyCode.B))
            SetKeyboardControl(code, KeyCode.B, "B");
        else if (Input.GetKeyDown(KeyCode.N))
            SetKeyboardControl(code, KeyCode.N, "N");
        else if (Input.GetKeyDown(KeyCode.M))
            SetKeyboardControl(code, KeyCode.M, "M");
        else if (Input.GetKeyDown(KeyCode.Comma))
            SetKeyboardControl(code, KeyCode.Comma, ",");
        else if (Input.GetKeyDown(KeyCode.Period))
            SetKeyboardControl(code, KeyCode.Period, ".");
        else if (Input.GetKeyDown(KeyCode.Slash))
            SetKeyboardControl(code, KeyCode.Slash, "/");
        else if (Input.GetKeyDown(KeyCode.LeftShift))
            SetKeyboardControl(code, KeyCode.LeftShift, "LeftShift");
        else if (Input.GetKeyDown(KeyCode.RightShift))
            SetKeyboardControl(code, KeyCode.RightShift, "RightShift");
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            SetKeyboardControl(code, KeyCode.LeftControl, "LeftControl");
        else if (Input.GetKeyDown(KeyCode.RightControl))
            SetKeyboardControl(code, KeyCode.RightControl, "RightControl");
        else if (Input.GetKeyDown(KeyCode.LeftAlt))
            SetKeyboardControl(code, KeyCode.LeftAlt, "LeftAlt");
        else if (Input.GetKeyDown(KeyCode.RightAlt))
            SetKeyboardControl(code, KeyCode.RightAlt, "RightAlt");
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            SetKeyboardControl(code, KeyCode.UpArrow, "UpArrow");
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            SetKeyboardControl(code, KeyCode.DownArrow, "DownArrow");
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SetKeyboardControl(code, KeyCode.RightArrow, "RightArrow");
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            SetKeyboardControl(code, KeyCode.LeftArrow, "LeftArrow");
        else if (Input.GetKeyDown(KeyCode.Tab))
            SetKeyboardControl(code, KeyCode.Tab, "Tab");
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            SetKeyboardControl(code, KeyCode.Alpha0, "0");
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            SetKeyboardControl(code, KeyCode.Alpha1, "1");
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetKeyboardControl(code, KeyCode.Alpha2, "2");
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetKeyboardControl(code, KeyCode.Alpha3, "3");
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SetKeyboardControl(code, KeyCode.Alpha4, "4");
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SetKeyboardControl(code, KeyCode.Alpha5, "5");
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SetKeyboardControl(code, KeyCode.Alpha6, "6");
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            SetKeyboardControl(code, KeyCode.Alpha7, "7");
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            SetKeyboardControl(code, KeyCode.Alpha8, "8");
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SetKeyboardControl(code, KeyCode.Alpha9, "9");
        else if (Input.GetKeyDown(KeyCode.Minus))
            SetKeyboardControl(code, KeyCode.Minus, "-");
        else if (Input.GetKeyDown(KeyCode.Equals))
            SetKeyboardControl(code, KeyCode.Equals, "=");
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            SetKeyboardControl(code, KeyCode.Mouse0, "Left Click");
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            SetKeyboardControl(code, KeyCode.Mouse1, "Right Click");
        else if (Input.GetKeyDown(KeyCode.Mouse2))
            SetKeyboardControl(code, KeyCode.Mouse2, "Middle Mouse");
        else if (Input.GetKeyDown(KeyCode.Space))
            SetKeyboardControl(code, KeyCode.Space, "Space");
        else
            returnValue = false;

        return returnValue;
    }

    private void SetKeyboardControl(int code, KeyCode keyCode, string inputText)
    {
        SetControlCode(new KeyCodeCode(code, keyCode));
        controlTexts[code].text = inputText;
    }

    #endregion Keyboard functions

    #region Joystick functions
    private bool JoyStickCheckInput(int code, string previousString)
    {
        bool returnValue = false;

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                controlTexts[code].text = previousString;
                returnValue = true;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                RemoveJoystickControl(code);
                returnValue = true;
            }
            else
            {
                returnValue = JoyStickIfStatements(code);
            }         
        }
        else
        {
            returnValue = JoystickAxisCheck(code);
        }

        return returnValue;    
    }

    private bool JoyStickIfStatements(int code)
    {
        bool returnValue = true;

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
            SetJoystickControlCode(code, KeyCode.JoystickButton0, "JoystickButton 0");
        else if (Input.GetKeyDown(KeyCode.JoystickButton1))
            SetJoystickControlCode(code, KeyCode.JoystickButton1, "JoystickButton 1");
        else if (Input.GetKeyDown(KeyCode.JoystickButton2))
            SetJoystickControlCode(code, KeyCode.JoystickButton2, "JoystickButton 2");
        else if (Input.GetKeyDown(KeyCode.JoystickButton3))
            SetJoystickControlCode(code, KeyCode.JoystickButton3, "JoystickButton 3");
        else if (Input.GetKeyDown(KeyCode.JoystickButton4))
            SetJoystickControlCode(code, KeyCode.JoystickButton4, "JoystickButton 4");
        else if (Input.GetKeyDown(KeyCode.JoystickButton5))
            SetJoystickControlCode(code, KeyCode.JoystickButton5, "JoystickButton 5");
        else if (Input.GetKeyDown(KeyCode.JoystickButton6))
            SetJoystickControlCode(code, KeyCode.JoystickButton6, "JoystickButton 6");
        else if (Input.GetKeyDown(KeyCode.JoystickButton7))
            SetJoystickControlCode(code, KeyCode.JoystickButton7, "JoystickButton 7");
        else if (Input.GetKeyDown(KeyCode.JoystickButton8))
            SetJoystickControlCode(code, KeyCode.JoystickButton8, "JoystickButton 8");
        else if (Input.GetKeyDown(KeyCode.JoystickButton9))
            SetJoystickControlCode(code, KeyCode.JoystickButton9, "JoystickButton 9");
        else if (Input.GetKeyDown(KeyCode.JoystickButton10))
            SetJoystickControlCode(code, KeyCode.JoystickButton10, "JoystickButton 10");
        else if (Input.GetKeyDown(KeyCode.JoystickButton11))
            SetJoystickControlCode(code, KeyCode.JoystickButton11, "JoystickButton 11");
        else if (Input.GetKeyDown(KeyCode.JoystickButton12))
            SetJoystickControlCode(code, KeyCode.JoystickButton12, "JoystickButton 12");
        else if (Input.GetKeyDown(KeyCode.JoystickButton13))
            SetJoystickControlCode(code, KeyCode.JoystickButton13, "JoystickButton 13");
        else if (Input.GetKeyDown(KeyCode.JoystickButton14))
            SetJoystickControlCode(code, KeyCode.JoystickButton14, "JoystickButton 14");
        else if (Input.GetKeyDown(KeyCode.JoystickButton15))
            SetJoystickControlCode(code, KeyCode.JoystickButton15, "JoystickButton 15");
        else if (Input.GetKeyDown(KeyCode.JoystickButton16))
            SetJoystickControlCode(code, KeyCode.JoystickButton16, "JoystickButton 16");
        else if (Input.GetKeyDown(KeyCode.JoystickButton17))
            SetJoystickControlCode(code, KeyCode.JoystickButton17, "JoystickButton 17");
        else if (Input.GetKeyDown(KeyCode.JoystickButton18))
            SetJoystickControlCode(code, KeyCode.JoystickButton18, "JoystickButton 18");
        else if (Input.GetKeyDown(KeyCode.JoystickButton19))
            SetJoystickControlCode(code, KeyCode.JoystickButton19, "JoystickButton 19");
        else
            returnValue = false;

        return returnValue;
    }

    private bool JoystickAxisCheck(int code)
    {
        if(code > 8 && code < 17)
        {
            int correspondingCodeOfControl;
            int textSign = GetJoystickTextSign(code, out correspondingCodeOfControl);
            bool isInputSelected = false;

            //check axes inputs and set them if they were inputted
            JoystickSpecificAxisCheck(textSign, Input.GetAxis("LeftJoyStickY"), "LeftJoyStickY", "Left Stick Up",
                                      "Left Stick Down", code, correspondingCodeOfControl, ref isInputSelected);
            JoystickSpecificAxisCheck(textSign, Input.GetAxis("LeftJoyStickX"), "LeftJoyStickX", "Left Stick Right",
                                      "Left Stick Left", code, correspondingCodeOfControl, ref isInputSelected);
            JoystickSpecificAxisCheck(textSign, Input.GetAxis("RightJoyStickY"), "RightJoyStickY", "Right Stick Up",
                                      "Right Stick Down", code, correspondingCodeOfControl, ref isInputSelected);
            JoystickSpecificAxisCheck(textSign, Input.GetAxis("RightJoyStickX"), "RightJoyStickX", "Right Stick Right",
                                      "Right Stick Left", code, correspondingCodeOfControl, ref isInputSelected);

            return isInputSelected;
        }

        return false;
    }

    private void JoystickSpecificAxisCheck(int textSign, float axisSign, string axisName, string positiveAxisName,
                                           string negativeAxisName, int code, int correspondingCodeOfControl,
                                           ref bool isAxisSelected)
    {
        if (axisSign != 0)
        {
            isAxisSelected = true;

            //axis selected is not inverted
            if (textSign * axisSign > 0)
            {
                SetControlCode(new AxisCode(correspondingCodeOfControl, new AxisName(axisName, false)));

                //set texts
                if (textSign > 0)
                {
                    SetControlText(code, positiveAxisName);
                    SetControlText(code + 1, negativeAxisName);
                }
                else
                {
                    SetControlText(code, negativeAxisName);
                    SetControlText(code - 1, positiveAxisName);
                }
            }

            //axis selected is inverted
            else
            {
                SetControlCode(new AxisCode(correspondingCodeOfControl, new AxisName(axisName, true)));

                //set texts
                if (textSign > 0)
                {
                    SetControlText(code, negativeAxisName);
                    SetControlText(code + 1, positiveAxisName);
                }
                else
                {
                    SetControlText(code, positiveAxisName);
                    SetControlText(code - 1, negativeAxisName);
                }
            }
        }
    }

    /// <summary>
    /// Remove joystick shootKey or axis or a keyCode of an axis that matches with the code and update the texts.
    /// </summary>
    /// <param name="code">The code that matches with the controlTexts[code].</param>
    private void RemoveJoystickControl(int code)
    {
        if(code == 17)
        {
            RemoveControlCode(code);
            MakeControlTextEmpty(code);
        }
        else if(8 < code && code < 17)
        {
            //find the corresponding code that is in controls array
            int correspondingCodeOfControl;
            if(code % 2 != 0)
                correspondingCodeOfControl = code;
            else
                correspondingCodeOfControl = code - 1;

            // remove controlCode in controls array or some part of it
            RemoveJoystickControlOrAPart(code, correspondingCodeOfControl);
        }
        else
        {
            throw new ArgumentOutOfRangeException("Joystick control code are not out of 9 to 17 range!");
        }
    }

    private void RemoveJoystickControlOrAPart(int code, int correspondingCodeOfControl)
    {
        ControlCode control = FindControlCode(correspondingCodeOfControl);

        if (control != null)
        {
            if (((AxisCode)control).AxisInput is AxisName)
            {
                RemoveControlCode(correspondingCodeOfControl);

                //make texts "Empty"
                if (correspondingCodeOfControl != code)
                {
                    MakeControlTextEmpty(correspondingCodeOfControl);
                    MakeControlTextEmpty(code);
                }
                else
                {
                    MakeControlTextEmpty(code);
                    MakeControlTextEmpty(code + 1);
                }
            }
            else //((AxisCode)control).AxisInput is AxisKey
            {
                if (code == correspondingCodeOfControl)
                {
                    ((AxisKey)((AxisCode)control).AxisInput).positiveKey = KeyCode.None;
                }
                else
                {
                    ((AxisKey)((AxisCode)control).AxisInput).negativeKey = KeyCode.None;
                }

                MakeControlTextEmpty(code);
            }
        }
        else
        {
            MakeControlTextEmpty(code);
        }
    }

    private void SetJoystickControlCode(int code, KeyCode keyCode, string inputText)
    {
        if (code == 17)
        {
            SetControlCode(new KeyCodeCode(code, keyCode));
            SetControlText(code, inputText);
        }
        else if (8 < code && code < 17)
        {
            //find the corresponding code that is in controls array
            int correspondingCodeOfControl;
            if (code % 2 != 0)
                correspondingCodeOfControl = code;
            else
                correspondingCodeOfControl = code - 1;

            // remove controlCode in controls array or some part of it
            SetJoystickAxisKey(code, correspondingCodeOfControl, keyCode, inputText);
        }
        else
        {
            throw new ArgumentOutOfRangeException("Joystick control code are not out of 9 to 17 range!");
        }
    }

    private void SetJoystickAxisKey(int code, int correspondingCodeOfControl, KeyCode keyCode, string inputText)
    {
        ControlCode control = FindControlCode(correspondingCodeOfControl);

        if(control != null)
        {
            if(((AxisCode)control).AxisInput is AxisName)
            {
                SetJoystickNewAxisKey(code, correspondingCodeOfControl, keyCode, inputText);
            }
            else    //((AxisCode)control).AxisInput is AxisKey
            {
                if(code == correspondingCodeOfControl)
                {
                    ((AxisKey)((AxisCode)control).AxisInput).positiveKey = keyCode;
                }
                else
                {
                    ((AxisKey)((AxisCode)control).AxisInput).negativeKey = keyCode;
                }

                SetControlText(code, inputText);
            }
        }
        else
        {
            SetJoystickNewAxisKey(code, correspondingCodeOfControl, keyCode, inputText);
        }
    }

    private void SetJoystickNewAxisKey(int code, int correspondingCodeOfControl, KeyCode keyCode, string inputText)
    {
        if (code == correspondingCodeOfControl)
        {
            SetControlCode(new AxisCode(code, new AxisKey(keyCode, KeyCode.None)));
            SetControlText(code, inputText);
            MakeControlTextEmpty(code + 1);
        }
        else
        {
            SetControlCode(new AxisCode(code, new AxisKey(KeyCode.None, keyCode)));
            SetControlText(code, inputText);
            MakeControlTextEmpty(correspondingCodeOfControl);
        }
    }

    private int GetJoystickTextSign(int code, out int correspondingCodeOfControl)
    {
        if (code % 2 != 0)  // positive direction
        {
            correspondingCodeOfControl = code;
            return 1;
        }

        // negative position
        correspondingCodeOfControl = code - 1;
        return -1;
    }

    #endregion Joystick functions

    /// <summary>
    /// Add or replace the controlCode with the code in controls array and update the array with new size.
    /// </summary>
    /// <param name="newControl">The controlCode to be added or replaced.</param>
    private void SetControlCode(ControlCode newControl)
    {
        if(controls != null)
        {
            int code = newControl.Code;
            int controlIndex = -1;

            //find new control index
            int length = controls.Length;
            for (int i = 0; i < length; i++)
            {
                if (code == controls[i].Code)
                {
                    //replace previous control with new control if a control with the code exists
                    controls[i] = newControl;
                    return; 
                }
                else if (code < controls[i].Code)
                {
                    controlIndex = i;
                    break;
                }
            }
            if(controlIndex == -1)
            {
                //the code is greatest between the controls' codes array
                controlIndex = length;
            }

            //make new controls array
            ControlCode[] permanentControls = controls;
            controls = new ControlCode[++length];
            int permanentControlsCounter = 0;
            for(int controlsCounter = 0; controlsCounter < length; controlsCounter++)
            {
                if (controlsCounter != controlIndex)
                {
                    controls[controlsCounter] = permanentControls[permanentControlsCounter];
                    permanentControlsCounter++;
                }
                else
                {
                    controls[controlsCounter] = newControl;
                }
            }
        }
        else
        {
            controls = new ControlCode[1];
            controls[0] = newControl;
        }
    }

    /// <summary>
    /// Remove the controlCode with the code from controls array and update the array with new size.
    /// </summary>
    /// <param name="code">Code of the controlCode to be removed.</param>
    private void RemoveControlCode(int code)
    {
        if(controls != null)
        {
            int controlIndex = FindControlCodeIndex(code);

            if(controlIndex != -1)
            {
                ControlCode[] permanentControls = controls;
                int length = controls.Length;
                controls = new ControlCode[length - 1];

                int controlCounter = 0;
                for(int permanentControlsCounter = 0; permanentControlsCounter < length; permanentControlsCounter++)
                {
                    if(permanentControlsCounter != controlIndex)
                    {
                        controls[controlCounter] = permanentControls[permanentControlsCounter];
                        controlCounter++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Find the controlCode with the code in controls array.
    /// </summary>
    private ControlCode FindControlCode(int code)
    {
        if(controls != null)
        {
            foreach(ControlCode control in controls)
            {
                if(control != null)
                {
                    if(control.Code == code)
                    {
                        return control;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Find the controlCode with the code and return it's index in controls array. Returns -1 if there isn't any
    /// matched control.
    /// </summary>
    private int FindControlCodeIndex(int code)
    {
        if (controls != null)
        {
            int length = controls.Length;

            for (int i = 0; i < length; i++)
            {
                if (controls[i] != null)
                {
                    if (controls[i].Code == code)
                    {
                        return i;
                    }
                }
            }
        }

        return -1;
    }

    private void MakeControlTextEmpty(int code)
    {
        controlTexts[code].text = "Empty";
    }

    private void SetControlText(int code, string text)
    {
        controlTexts[code].text = text;
    }

    /// <summary>
    /// Change enabling of keyboard and mouse controls. It is called by "Keyboard And Mouse" button. 
    /// </summary>
    private void EnableKeyboardAndMouseInput()
    {
        bool enabled;

        if (isKeyboardAndMouseEanbled == true)
        {
            enabled = false;
        }
        else
        {
            enabled = true;
        }

        EnableKeyboardAndMouseInput(enabled);
    }

    /// <summary>
    /// Change enabling of joystick controls. It is called by "JoyStick" button. 
    /// </summary>
    private void EnableJoyStickInput()
    {
        bool enabled;

        if (isJoyStickEnabled == true)
        {
            enabled = false;
        }
        else
        {
            enabled = true;
        }

        EnableJoyStickInput(enabled);
    }

    /// <summary>
    /// Enable or disable keyboard and mouse controls. It is called in the InitializeKeyboardAndJoyStickEnabling method.
    /// </summary>
    /// <param name="enabled"></param>
    private void EnableKeyboardAndMouseInput(bool enabled)
    {
        isKeyboardAndMouseEanbled = enabled;
        EnableControls(enabled, (int)InputControlCode.KeyboardMoveUp, (int)InputControlCode.KeyboardShoot);
    }

    /// <summary>
    /// Enable or disable joystick controls. It is called in the InitializeKeyboardAndJoyStickEnabling method.
    /// </summary>
    /// <param name="enabled"></param>
    private void EnableJoyStickInput(bool enabled)
    {
        isJoyStickEnabled = enabled;
        EnableControls(enabled, (int)InputControlCode.JoyStickMoveUp, (int)InputControlCode.JoyStickShoot);
    }

    /// <summary>
    /// Enable controls of keyboard and mouse or joystick.
    /// </summary>
    /// <param name="enabled"></param>
    /// <param name="startIndex">The start index to enabling in controlTexts and controlButtons arrays.</param>
    /// <param name="endIndex">The end index to enabling in controlTexts and controlButtons arrays.</param>
    private void EnableControls(bool enabled, int startIndex, int endIndex)
    {
        if (enabled == true)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                controlTexts[i].color = Color.white;
                controlButtons[i].enabled = true;
            }
        }
        else
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                controlTexts[i].color = Color.gray;
                controlButtons[i].enabled = false;
            }
        }
    }

    private void DefaultButton()
    {
        //enabling
        EnableKeyboardAndMouseInput(true);
        EnableJoyStickInput(false);

        //controls array
        controls = new ControlCode[10];
        controls[0] = new KeyCodeCode(0, KeyCode.W);
        controls[1] = new KeyCodeCode(1, KeyCode.S);
        controls[2] = new KeyCodeCode(2, KeyCode.D);
        controls[3] = new KeyCodeCode(3, KeyCode.A);
        controls[4] = new KeyCodeCode(8, KeyCode.Mouse0);
        controls[5] = new AxisCode(9, new AxisName("LeftJoyStickY"));
        controls[6] = new AxisCode(11, new AxisName("LeftJoyStickX"));
        controls[7] = new AxisCode(13, new AxisName("RightJoyStickY"));
        controls[8] = new AxisCode(15, new AxisName("RightJoyStickX"));
        controls[9] = new KeyCodeCode(17, KeyCode.JoystickButton7);

        //control texts
        SetControlText(0, "W");
        SetControlText(1, "S");
        SetControlText(2, "D");
        SetControlText(3, "A");
        SetControlText(8, "Left Click");
        SetControlText(9, "Left Stick Up");
        SetControlText(10, "Left Stick Down");
        SetControlText(11, "Left Stick Right");
        SetControlText(12, "Left Stick Left");
        SetControlText(13, "Right Stick Up");
        SetControlText(14, "Right Stick Down");
        SetControlText(15, "Right Stick Right");
        SetControlText(16, "Right Stick Left");
        SetControlText(17, "JoystickButton 7");
    }

    private void ApplyButton()
    {
        bool isApplyAllowed = false;

        try
        {
            if (isKeyboardAndMouseEanbled)
            {
                if (isJoyStickEnabled)
                {
                    //keyboard check
                    KeyCodeCodeApplyCheck(new int[] { 0, 1, 2, 3, 8 }, 0, 4);
                    //joystick axes check
                    JoystickAxesApplyCheck(new int[] { 9, 11, 13, 15 }, 5, 8);
                    //joystick shoot key check
                    KeyCodeCodeApplyCheck(new int[] { 17 }, 9, 9);

                    //if no exception was thrown save it to optionContainer
                    KeyCode[] keyCodes = new KeyCode[]
                    {
                        ((KeyCodeCode)controls[0]).ControlKeyCode, ((KeyCodeCode)controls[1]).ControlKeyCode,
                        ((KeyCodeCode)controls[2]).ControlKeyCode, ((KeyCodeCode)controls[3]).ControlKeyCode,
                        ((KeyCodeCode)controls[4]).ControlKeyCode
                    };
                    IAxisInput[] axes = new IAxisInput[]
                    {
                        ((AxisCode)controls[5]).AxisInput, ((AxisCode)controls[6]).AxisInput,
                        ((AxisCode)controls[7]).AxisInput, ((AxisCode)controls[8]).AxisInput
                    };
                    optionContainer.SetControlsVariables(Player.InputType.MouseAndKeyboard_JoyStick, keyCodes, axes,
                                                         ((KeyCodeCode)controls[9]).ControlKeyCode);

                }
                else
                {
                    //keyboard check
                    KeyCodeCodeApplyCheck(new int[] { 0, 1, 2, 3, 8 }, 0, 4);

                    //if no exception was thrown save it to optionContainer
                    KeyCode[] keyCodes = new KeyCode[]
                    {
                        ((KeyCodeCode)controls[0]).ControlKeyCode, ((KeyCodeCode)controls[1]).ControlKeyCode,
                        ((KeyCodeCode)controls[2]).ControlKeyCode, ((KeyCodeCode)controls[3]).ControlKeyCode,
                        ((KeyCodeCode)controls[4]).ControlKeyCode
                    };
                    optionContainer.SetControlsVariables(Player.InputType.MouseAndKeyboard, keyCodes, null,
                                                         KeyCode.None);
                }
            }
            else
            {
                if (isJoyStickEnabled)
                {
                    //find start index of jpystick axes check
                    int startIndex = -1;
                    int length = controls.Length;
                    for(int i = 0; i < length; i++)
                    {
                        if(controls[i].Code == 9)
                        {
                            startIndex = i;
                        }
                    }
                    if(startIndex == -1)
                    {
                        throw new InvalidCastException();
                    }

                    //joystick axes check
                    JoystickAxesApplyCheck(new int[] { 9, 11, 13, 15 }, startIndex, startIndex + 3);
                    //joystick shoot key check
                    KeyCodeCodeApplyCheck(new int[] { 17 }, startIndex + 4, startIndex + 4);

                    //if no exception was thrown save it to optionContainer
                    IAxisInput[] axes = new IAxisInput[]
                    {
                        ((AxisCode)controls[startIndex]).AxisInput, ((AxisCode)controls[startIndex + 1]).AxisInput,
                        ((AxisCode)controls[startIndex + 2]).AxisInput, ((AxisCode)controls[startIndex + 3]).AxisInput
                    };
                    optionContainer.SetControlsVariables(Player.InputType.JoyStick, null, axes,
                                                         ((KeyCodeCode)controls[startIndex + 4]).ControlKeyCode);
                }
                else
                {
                    MessageBox.ShowMessage("Input type is not selected. Please select type of control you want to use.");
                }
            }

            isApplyAllowed = true;
        }
        catch (InvalidCastException)
        {
            ShowInputNotCompleteError();
        }
        catch (IndexOutOfRangeException)
        {
            ShowInputNotCompleteError();
        }
        catch (NullReferenceException)
        {
            ShowInputNotCompleteError();
        }
        finally
        {
            //save controls in file
            if (isApplyAllowed)
            {
                Save();
            }
        }
    }

    private void KeyCodeCodeApplyCheck(int[] codes, int startIndex, int endIndex)
    {
        int codeCounter = 0;

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (controls[i].Code != codes[codeCounter])
            {
                throw new InvalidCastException();
            }
            else
            {
                KeyCodeCode a = (KeyCodeCode)controls[i];
            }

            codeCounter++;
        }
    }

    private void JoystickAxesApplyCheck(int[] codes, int startIndex, int endIndex)
    {
        int codeCounter = 0;

        for (int i = startIndex; i <= endIndex; i++)
        {
            if (controls[i].Code != codes[codeCounter])
            {
                throw new InvalidCastException();
            }
            else
            {
                if (((AxisCode)controls[i]).AxisInput is AxisKey)
                {
                    if (((AxisKey)((AxisCode)controls[i]).AxisInput).positiveKey == KeyCode.None)
                    {
                        throw new InvalidCastException();
                    }
                    if (((AxisKey)((AxisCode)controls[i]).AxisInput).negativeKey == KeyCode.None)
                    {
                        throw new InvalidCastException();
                    }
                }
            }

            codeCounter++;
        }
    }

    private static void ShowInputNotCompleteError()
    {
        MessageBox.ShowMessage("Not all controls selected. Please select all and then apply.");
    }

    private void HelpButton()
    {
        MessageBox.ShowMessage("Click on any control and then press any key to set it. When you click on a control you " +
                               "can press escape to set the control back or press backspace to remove the control.");
    }

    public static void Load(out Player.InputType inputType, out KeyCode[] keyCodes, out IAxisInput[] axes,
                            out KeyCode joystickShootKey, out int[] codesOfControls)
    {
        FileStream fileStream = null;
        bool isLoadingSucceeded = false;

        //just ensure the compiler that all out variables ar assigned
        inputType = Player.InputType.MouseAndKeyboard;
        keyCodes = null;
        axes = null;
        joystickShootKey = KeyCode.None;
        codesOfControls = null;

        //paths
        string folderPath = Path.Combine(Application.persistentDataPath, LOCAL_OPTION_FOLDER);
        string filePath = Path.Combine(folderPath, LOCAL_CONTROLS_FILE);

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fileStream);

            //read the controls from file
            ReadFromFile(out inputType, out keyCodes, out axes, out joystickShootKey, out codesOfControls, fileStream,
                         reader);

            isLoadingSucceeded = true;
        }
        catch (WrongCodeException)
        {
            MessageBox.ShowMessage("Controls saved wrongly. Controls will be set to default." +
                                    " You may need to save the controls again.");
        }
        catch (ControlNotAssigned)
        {
            MessageBox.ShowMessage("Game was unable to load controls completely. They may be saved wrongly. Controls will" +
                                   " be set to default. You may need to save the controls again.");
        }
        catch (FileNotFoundException)
        {
            MessageBox.ShowMessage("Controls were not found. Controls will be set to default." +
                                    " You may need to save the controls again.");
        }
        catch (IOException)
        {
            MessageBox.ShowMessage("Some stream error was occured while loading controls. Controls will be set to default." +
                                    " You may need to save the controls again.");
        }
        catch
        {
            MessageBox.ShowMessage("Some error was occured while loading controls. Controls will be set to default." +
                                    " You may need to save the controls again.");
        }
        finally
        {
            if (fileStream != null)
            {
                fileStream.Close();
            }
            if (!isLoadingSucceeded)
            {
                SetDefaultControlsForOptionContainer(out inputType, out keyCodes, out axes, out joystickShootKey,
                                                     out codesOfControls);
                SaveDefaultControls();
            }
        }
    }

    private static void ReadFromFile(out Player.InputType inputType, out KeyCode[] keyCodes, out IAxisInput[] axes,
                                     out KeyCode joystickShootKey, out int[] codesOfControls, FileStream fileStream,
                                     BinaryReader reader)
    {
        KeyCode[] permanentKeyboardKeyCodes = new KeyCode[5];
        IAxisInput[] permanentJoystickAxes = new IAxisInput[4];
        int[] permanentCodes = new int[10];
        long fileLength = fileStream.Length;

        //read iputType
        inputType = (Player.InputType)reader.ReadInt32();

        //read controls
        int keyCodeCounter = 0;
        int iAxisCounter = 0;
        int codeCounter = 0;
        joystickShootKey = KeyCode.None;
        while(fileStream.Position < fileLength)
        {
            int code = reader.ReadInt32();
            permanentCodes[codeCounter] = code;
            codeCounter++;

            if((0 <= code && code < 4) || code == 8)    //keyboard keycodes
            {
                permanentKeyboardKeyCodes[keyCodeCounter] = (KeyCode)reader.ReadInt32();
                keyCodeCounter++;
            }
            else if(8 < code && code < 17 && code % 2 == 1) //joystick axes
            {
                SavedAxisCodeType axisType = (SavedAxisCodeType)reader.ReadInt32();

                if(axisType == SavedAxisCodeType.AxisKey)
                {
                    KeyCode positiveKey = (KeyCode)reader.ReadInt32();
                    KeyCode negativeKey = (KeyCode)reader.ReadInt32();
                    permanentJoystickAxes[iAxisCounter] = new AxisKey(positiveKey, negativeKey);                   
                }
                else
                {
                    string axisName = reader.ReadString();
                    bool isInverted = reader.ReadBoolean();
                    permanentJoystickAxes[iAxisCounter] = new AxisName(axisName, isInverted);
                }

                iAxisCounter++;
            }
            else if(code == 17) //joystick shoot key
            {
                joystickShootKey= (KeyCode)reader.ReadInt32();
            }
            else
            {
                throw new WrongCodeException();
            }
        }

        //set controls
        if (joystickShootKey == KeyCode.None || (keyCodeCounter == 0 && iAxisCounter == 0 && codeCounter == 0))
        {
            throw new ControlNotAssigned();
        }
        keyCodes = new KeyCode[keyCodeCounter];
        axes = new IAxisInput[iAxisCounter];
        codesOfControls = new int[codeCounter];
        for(int i = 0; i < keyCodeCounter; i++)
        {
            keyCodes[i] = permanentKeyboardKeyCodes[i];
        }
        for (int i = 0; i < iAxisCounter; i++)
        {
            axes[i] = permanentJoystickAxes[i];
        }
        for(int i = 0; i < codeCounter; i++)
        {
            codesOfControls[i] = permanentCodes[i];
        }
    }

    private static void SetDefaultControlsForOptionContainer(out Player.InputType inputType, out KeyCode[] keyCodes, out IAxisInput[] axes,
                                           out KeyCode joystickShootKey, out int[] codesOfControls)
    {
        inputType = Player.InputType.MouseAndKeyboard;

        //keyboard keycodes
        keyCodes = new KeyCode[5];
        keyCodes[0] = KeyCode.W;    // move up
        keyCodes[1] = KeyCode.S;    // move down
        keyCodes[2] = KeyCode.D;    // move right
        keyCodes[3] = KeyCode.A;    // move left
        keyCodes[4] = KeyCode.Mouse0;

        //joystick controls
        axes = new IAxisInput[4];
        axes[0] = new AxisName("LeftJoyStickY");   // move y
        axes[1] = new AxisName("LeftJoyStickX");   // move x
        axes[2] = new AxisName("RightJoyStickY");  // face y
        axes[3] = new AxisName("RightJoyStickX");  // face x
        joystickShootKey = KeyCode.JoystickButton7;

        //codes of controls
        codesOfControls = new int[10];
        codesOfControls[0] = 0;
        codesOfControls[1] = 1;
        codesOfControls[2] = 2;
        codesOfControls[3] = 3;
        codesOfControls[4] = 8;
        codesOfControls[5] = 9;
        codesOfControls[6] = 11;
        codesOfControls[7] = 13;
        codesOfControls[8] = 15;
        codesOfControls[9] = 17;
    }

    private void Save()
    {
        FileStream fileStream = null;

        //paths
        string folderPath = Path.Combine(Application.persistentDataPath, LOCAL_OPTION_FOLDER);
        string filePath = Path.Combine(folderPath, LOCAL_CONTROLS_FILE);

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            fileStream = File.Open(filePath, FileMode.Create);
            BinaryWriter writter = new BinaryWriter(fileStream);

            //write input type
            SaveInputType(writter);

            //write controls
            SaveControls(writter);
        }
        catch(IOException)
        {
            MessageBox.ShowMessage("Stream error in saving controls. Please try again.");
        }
        catch
        {
            MessageBox.ShowMessage("Unknown error in saving controls. Please try again.");
        }
        finally
        {
            if(fileStream != null)
            {
                fileStream.Close();
            }
        }
    }

    private void SaveInputType(BinaryWriter writter)
    {
        if (isKeyboardAndMouseEanbled)
        {
            if (isJoyStickEnabled)
            {
                writter.Write((int)Player.InputType.MouseAndKeyboard_JoyStick);
            }
            else
            {
                writter.Write((int)Player.InputType.MouseAndKeyboard);
            }
        }
        else
        {
            writter.Write((int)Player.InputType.JoyStick);
        }
    }

    private void SaveControls(BinaryWriter writter)
    {
        foreach (ControlCode control in controls)
        {
            int code = control.Code;
            writter.Write(code);    //write code

            if ((code >= 0 && code < 4) || code == 8 || code == 17)
            {
                writter.Write((int)((KeyCodeCode)control).ControlKeyCode);  //write key code
            }
            else if (code > 8 && code < 17 && code % 2 == 1)
            {
                AxisCode axisCode = (AxisCode)control;

                if (axisCode.AxisInput is AxisKey)
                {
                    AxisKey axisKey = (AxisKey)axisCode.AxisInput;
                    writter.Write((int)SavedAxisCodeType.AxisKey);  //write axis type code
                    writter.Write((int)axisKey.positiveKey);        //write positive key code
                    writter.Write((int)axisKey.negativeKey);        //write negative key code
                }
                else
                {
                    AxisName axisName = (AxisName)axisCode.AxisInput;
                    writter.Write((int)SavedAxisCodeType.AxisName);     //write axis type code
                    writter.Write(axisName.name);                       //write axis name
                    writter.Write(axisName.IsInverted);                 //write axis's isInverted(boolean)
                }
            }
            else
            {
                throw new WrongCodeException();
            }
        }
    }

    private static void SaveDefaultControls()
    {
        FileStream fileStream = null;

        //paths
        string folderPath = Path.Combine(Application.persistentDataPath, LOCAL_OPTION_FOLDER);
        string filePath = Path.Combine(folderPath, LOCAL_CONTROLS_FILE);

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            fileStream = File.Create(filePath);
            BinaryWriter writer = new BinaryWriter(fileStream);

            //write inputType
            writer.Write((int)Player.InputType.MouseAndKeyboard);

            //write keyboard keycodes
            writer.Write(0);                //code
            writer.Write((int)KeyCode.W);   //keyCode
            writer.Write(1);
            writer.Write((int)KeyCode.S);
            writer.Write(2);
            writer.Write((int)KeyCode.D);
            writer.Write(3);
            writer.Write((int)KeyCode.A);
            writer.Write(8);
            writer.Write((int)KeyCode.Mouse0);

            //write joystick axes
            writer.Write(9);                                //code
            writer.Write((int)SavedAxisCodeType.AxisName);  //axis code
            writer.Write("LeftJoyStickY");                  //axis name(string)
            writer.Write(false);                            //isInverted(boolean)
            writer.Write(11);               
            writer.Write((int)SavedAxisCodeType.AxisName);
            writer.Write("LeftJoyStickX");
            writer.Write(false);
            writer.Write(13);
            writer.Write((int)SavedAxisCodeType.AxisName);
            writer.Write("RightJoyStickY");
            writer.Write(false);
            writer.Write(15);
            writer.Write((int)SavedAxisCodeType.AxisName);
            writer.Write("RightJoyStickX");
            writer.Write(false);

            //write joystick shoot ket
            writer.Write(17);                           //code
            writer.Write((int)KeyCode.JoystickButton7); //keyCode
        }
        catch
        {
            MessageBox.ShowMessage("An error was occured while saving controls but it doesn't affect your current game." +
                                   " Try again if you want to save controls.");
        }
        finally
        {
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
    }
}

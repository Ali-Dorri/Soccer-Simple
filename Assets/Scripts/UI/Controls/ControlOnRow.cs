using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlOnRow : MonoBehaviour, IPointerClickHandler
{
    //
    //Fields
    //

    ControlsInputManager controlManager;
    UISoundHandler soundHandler;
    Button button;

    //code number
    bool isCodeSet = false;
    /// <summary>
    /// The code that is used in ControlCode for distinguishing IAxis and KeyCode.
    /// Don't set this directly, use it's property instead.
    /// </summary>
    int codeNumber;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    /// <summary>
    /// The code that is used in ControlCode for distinguishing IAxis and KeyCode
    /// </summary>
    public int CodeNumber
    {
        get
        {
            return codeNumber;
        }
        private set
        {
            if (!isCodeSet)
            {
                codeNumber = value;
                isCodeSet = true;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    /// <summary>
    /// Set the code number and then put the corresponding text and button in the ControlInputManager's arrays respectively.
    /// And initialize it.
    /// </summary>
    /// <param name="controlTexts">ControlInputManager's controlTexts array.</param>
    /// <param name="controlButtons">ControlInputManager's controlButtons array.</param>
    public void Initialize(Text[] controlTexts, Button[] controlButtons)
    {
        //set code
        SetCodeNumber();

        //set text and button to ControlInputManager's arrays
        controlTexts[codeNumber] = GetComponent<Text>();
        controlButtons[codeNumber] = GetComponent<Button>();

        //find Component fields
        controlManager = FindObjectOfType<ControlsInputManager>();
        soundHandler = FindObjectOfType<UISoundHandler>();
        button = GetComponent<Button>();

        //add button listener (add SetControlInput function or ShowError function)
        AddListener();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    private void SetCodeNumber()
    {
        if(transform.parent.name == "Keyboard And Mouse Container")
        {
            switch (name)
            {
                case "Keyboard MoveUp":
                    CodeNumber = 0;
                    break;
                case "Keyboard MoveDown":
                    CodeNumber = 1;
                    break;
                case "Keyboard MoveRight":
                    CodeNumber = 2;
                    break;
                case "Keyboard MoveLeft":
                    CodeNumber = 3;
                    break;
                case "Keyboard FaceUp":
                    CodeNumber = 4;
                    break;
                case "Keyboard FaceDown":
                    CodeNumber = 5;
                    break;
                case "Keyboard FaceRight":
                    CodeNumber = 6;
                    break;
                case "Keyboard FaceLeft":
                    CodeNumber = 7;
                    break;
                case "Keyboard Shoot":
                    CodeNumber = 8;
                    break;
            }
        }
        else if (transform.parent.name == "JoyStick Container")
        {
            switch (name)
            {
                case "JoyStick MoveUp":
                    CodeNumber = 9;
                    break;
                case "JoyStick MoveDown":
                    CodeNumber = 10;
                    break;
                case "JoyStick MoveRight":
                    CodeNumber = 11;
                    break;
                case "JoyStick MoveLeft":
                    CodeNumber = 12;
                    break;
                case "JoyStick FaceUp":
                    CodeNumber = 13;
                    break;
                case "JoyStick FaceDown":
                    CodeNumber = 14;
                    break;
                case "JoyStick FaceRight":
                    CodeNumber = 15;
                    break;
                case "JoyStick FaceLeft":
                    CodeNumber = 16;
                    break;
                case "JoyStick Shoot":
                    CodeNumber = 17;
                    break;
            }
        }
    }

    private void AddListener()
    {
        if(codeNumber == 4 || codeNumber == 5 || codeNumber == 6 || codeNumber == 7)
        {
            GetComponent<Button>().onClick.AddListener(ShowErroMessage);
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(SetControlInput);
        }
    }

    private void SetControlInput()
    {
        controlManager.EnableCheckControlInput(codeNumber);
    }

    private void ShowErroMessage()
    {
        MessageBox.ShowMessage("The rotation of soccer player in Keyboard and Mouse mode is controled by Mouse position." +
                               "You can not modify it.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button.enabled)
        {
            soundHandler.PlayMouseClick();
        }
    }
}

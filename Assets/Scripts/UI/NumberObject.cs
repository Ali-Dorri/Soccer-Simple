using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberObject : MonoBehaviour, ISoundConstraint
{
    //
    //Fields
    //

    MemberNumberHandler memberHandler;
    [SerializeField] private int number;
    private Text text;
    private Button button;

    private Vector3 localFirstPosition;
    private Vector3 localChoosePosition;

    //events
    public event VoidAction MouseOveredSound;
    public event VoidAction MouseClickedSound;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public int Number
    {
        get
        {
            return number;
        }
    }

    public Text TheText
    {
        get
        {
            return text;
        }
    }

    public Button TheButton
    {
        get
        {
            return button;
        }
    }

    public Vector3 LocalFirstPosition
    {
        get
        {
            return localFirstPosition;
        }
    }

    public Vector3 LocalChoosePosition
    {
        get
        {
            return localChoosePosition;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //
    private void Start()
    {
        memberHandler = transform.parent.GetComponent<MemberNumberHandler>();
    }

    public void Initialize()
    {
        text = GetComponent<Text>();
        button = GetComponent<Button>();
    }

    public void SetLocalPositions(NumberObject firstNumber)
    {
        localChoosePosition = transform.localPosition;
        localFirstPosition = firstNumber.localChoosePosition;
    }


    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void OnClick()
    {
        memberHandler.SelectNumberButton(number);
    }

    public void ConditionalMouseOverSound()
    {
        if (button.enabled)
        {
            OnMouseOveredSound();
        }
    }

    public void ConditionalMouseClickSound()
    {
        //no condition yet
        OnMouseClickedSound();
    }

    private void OnMouseOveredSound()
    {
        if (MouseOveredSound != null)
        {
            MouseOveredSound();
        }
    }

    private void OnMouseClickedSound()
    {
        if (MouseClickedSound != null)
        {
            MouseClickedSound();
        }
    }  
}

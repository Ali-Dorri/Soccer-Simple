using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ConstraintField : MonoBehaviour, IUIMouseSoundWarner, IPointerClickHandler, IPointerEnterHandler
{
    //
    //Fields
    //

    private InputField inputField;
    private Text limitationText;
    [SerializeField] protected int amount;
    [SerializeField] protected int minAmount;
    [SerializeField] protected int maxAmount;
    protected string addingText = string.Empty;
    bool isEditting = false;
    bool isEnabled = true;  //used to enable or disable features in this class 

    //static
    static bool isStaticInitialized = false;
    static Color enabledLimitationTextColor;
    static Color enabledFieldTextColor;

    //events
    public event VoidAction MouseOveredSound;
    public event VoidAction MouseClickedSound;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public bool FieldEnabled
    {
        get
        {
            return isEnabled;
        }
        set
        {
            //logic
            isEnabled = value;
            inputField.enabled = value;

            //graphic
            if (value == true)
            {
                limitationText.color = enabledLimitationTextColor;
                inputField.textComponent.color = enabledFieldTextColor;
            }
            else
            {
                Color grayColor = new Color(80f / 255, 80f / 255, 80f / 255, 178f / 255);
                limitationText.color = grayColor;
                inputField.textComponent.color = grayColor;
            }
        }
    }

    public int Amount
    {
        get
        {
            return amount;
        }
    }

    public int MinAmount
    {
        get
        {
            return minAmount;
        }
    }

    public int MaxAmount
    {
        get
        {
            return maxAmount;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Awake()
    {
        inputField = GetComponent<InputField>();
    }

    /// <summary>
    /// It will be called by OptionHandler on load scene in an order with other OptionUIs' Initialize methods.
    /// </summary>
    public static void StaticInitialize()
    {
        if (!isStaticInitialized)
        {
            //find static default colors
            Text limitText = GameObject.Find("ScoreLimit").GetComponent<Text>();
            InputField limitInputField = GameObject.Find("Score Amount").GetComponent<InputField>();
            enabledLimitationTextColor = limitText.color;
            enabledFieldTextColor = limitInputField.textComponent.color;

            isStaticInitialized = true;
        }   
    }

    public void Initialize(int amount)
    {
        SetAmountLimitations();
        SetAddingText();
        ((Text)inputField.placeholder).text = "Between " + minAmount + " and " + maxAmount + "...";
        limitationText = transform.parent.GetComponent<Text>();
        SetAmountAndText(amount);

        //set sound requirements
        FindObjectOfType<UISoundHandler>().SetMouseEvent(this);
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //  

    private void OnEnable()
    {
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnDisable()
    {
        inputField.onEndEdit.RemoveListener(OnEndEdit);
    }

    private void OnEndEdit(string _string)
    {   
        inputField.characterValidation = InputField.CharacterValidation.None;

        if (!string.IsNullOrEmpty(inputField.text))
        {
            int permanentAmount = int.Parse(inputField.text);
            SetAmountAndText(permanentAmount);
        }
        else
        {
            SetText();
        }

        isEditting = false;
    } 

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isEditting && isEnabled)
        {
            inputField.characterValidation = InputField.CharacterValidation.Integer;
            inputField.text = string.Empty;
            isEditting = true;

            //click sound play
            OnMouseClickedSound();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isEditting && isEnabled)
        {
            //mouse over sound play
            OnMouseOveredSound();
        }
    }

    private void SetAmount(int _amount)
    {
        if (minAmount <= _amount)
        {
            if (_amount <= maxAmount)
            {
                amount = _amount;
            }
            else
            {
                amount = maxAmount;
            }
        }
        else
        {
            amount = minAmount;
        }
    }

    public void SetAmountAndText(int amount)
    {
        SetAmount(amount);
        SetText();
    }

    private void SetText()
    {
        inputField.text = amount.ToString() + addingText;
    }
    protected abstract void SetAmountLimitations();
    protected abstract void SetAddingText();

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

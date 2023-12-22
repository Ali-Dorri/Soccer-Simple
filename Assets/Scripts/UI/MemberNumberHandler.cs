using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberNumberHandler : MonoBehaviour
{
    //
    //Concept Definition
    //

    private enum FadeType { ToTransparent, ToOpaque }
    

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    private int teamNumber;
    bool isChosen = true;
    bool areNumbersMoving = false;
    NumberObject[] numbers;
    [SerializeField] private float moveDuration = 1; //seconds
    [SerializeField] private int maxTeamNumber = 4;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public int TeamNumber
    {
        get
        {
            return teamNumber;
        }
        private set
        {
            if(value >= 1)
            {
                if (value <= maxTeamNumber)
                {
                    teamNumber = value;
                }
                else
                {
                    teamNumber = maxTeamNumber;
                }
            }
            else
            {
                teamNumber = 1;
            }
            
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        NumberObject[] permanentNumbers = FindObjectsOfType<NumberObject>();

        //find first numberObjects
        NumberObject firstNumber = permanentNumbers[0]; //this assignment is for ensuring the compiler that
                                                        //firstnumber is not null
        foreach(NumberObject numberObject in permanentNumbers)
        {
            if (numberObject.Number == 1)
            {
                firstNumber = numberObject;
                firstNumber.SetLocalPositions(firstNumber); //this line has been checked for correctness
            }
        }
              
        foreach(NumberObject numberObject in permanentNumbers)
        {
            //set numberobjects' first and choose local positions
            numberObject.SetLocalPositions(firstNumber);
            //set numberObjects' loacl positions
            numberObject.transform.localPosition = numberObject.LocalFirstPosition;
        }
    }

    public void Initialize(int number)
    {
        NumberObject[] permanentNumbers = FindObjectsOfType<NumberObject>();

        //fill numbers array in order
        numbers = new NumberObject[permanentNumbers.Length];
        int i = 0;
        foreach (NumberObject numberObject in permanentNumbers)
        {
            i = numberObject.Number - 1;
            numbers[i] = numberObject;

            //also initialize it befor using SetNumber(number)
            numberObject.Initialize();
        }

        SetNumber(number);
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void SelectNumberButton(int membersNumber)
    {
        if (isChosen)
        {
            StartMoveNumbers();
            isChosen = false;
        }
        else
        {
            StartMoveNumbersBack(membersNumber);
            isChosen = true;
        }
    }

    public void ChooseNumber(int number)
    {
        if (!areNumbersMoving)
        {
            if (isChosen)
            {
                SetNumber(number);
            }
            else
            {
                StartMoveNumbersBack(number);
                isChosen = true;
            }
        }  
    }

    private void StartMoveNumbers()
    {
        foreach(NumberObject numberObject in numbers)
        {
            numberObject.TheButton.enabled = false;
            numberObject.TheText.enabled = true;
            SetButtonTransparency(numberObject.TheButton, 0);
        }

        //the number "1" doesn't need to move. We just set it's transparency
        SetButtonTransparency(numbers[0].TheButton, 1);

        //move others(thier transparency will be changed in moving, too)
        int length = numbers.Length - 1;
        for(int i = 1; i < length; i++)
        {
            StartCoroutine(MoveNumbers(numbers[i], numbers[i].LocalFirstPosition, numbers[i].LocalChoosePosition,
                        FadeType.ToOpaque, SetAllEnabled, false));
        }
        //move last one with canEnd == true
        StartCoroutine(MoveNumbers(numbers[length], numbers[length].LocalFirstPosition,
                                   numbers[length].LocalChoosePosition, FadeType.ToOpaque, SetAllEnabled, true));
    }

    private void StartMoveNumbersBack(int chosenNumber)
    {
        foreach (NumberObject numberObject in numbers)
        {
            numberObject.TheButton.enabled = false;
            numberObject.TheText.enabled = true;
        }

        //the number "1" doesn't need to move. We just set it's transparency
        SetButtonTransparency(numbers[0].TheButton, 0);

        //move others(thier transparency will be changed in moving, too)
        int length = numbers.Length - 1;
        for (int i = 1; i < length; i++)
        {
            StartCoroutine(MoveNumbers(numbers[i], numbers[i].LocalChoosePosition, numbers[i].LocalFirstPosition,
                        FadeType.ToTransparent, () => SetNumber(chosenNumber), false));
        }
        //move last one with canEnd == true
        StartCoroutine(MoveNumbers(numbers[length], numbers[length].LocalChoosePosition,
                                   numbers[length].LocalFirstPosition, FadeType.ToTransparent,
                                   () => SetNumber(chosenNumber), true));
    }

    private IEnumerator MoveNumbers(NumberObject numberObject, Vector3 startPos, Vector3 endPos, FadeType fadeTo,
                                    VoidAction endAction, bool canEnd)
    {
        //start moving
        areNumbersMoving = true;

        //move variables
        Vector3 trackLine = endPos - startPos;
        float numberSpeed = trackLine.magnitude / moveDuration;
        Vector3 deltaMotion = trackLine.normalized * numberSpeed;
        Transform numberTransform = numberObject.transform;

        //fade variables
        Button numberButton = numberObject.TheButton;
        float trackLength = trackLine.magnitude;
        float fadeRatio; //related to position
        int sign;
        if (fadeTo == FadeType.ToOpaque)
        {
            sign = 1;
        }
        else
        {
            sign = -1;
        }

        //start moving and fading
        while (Vector2.Distance(numberTransform.localPosition, endPos) > numberSpeed * Time.deltaTime)
        {
            yield return null;

            //move
            numberTransform.Translate(deltaMotion * Time.deltaTime);
            //fade
            fadeRatio = numberSpeed * Time.deltaTime / trackLength;
            FadeButton(numberButton, sign * fadeRatio);
        }

        yield return null;

        //set the button position exactly
        numberTransform.localPosition = endPos;

        //number graphic and mouse event
        if (canEnd)
        {
            endAction();
        }

        //finish moving
        areNumbersMoving = false;
    }

    void SetNumber(int number)
    {
        //logic
        TeamNumber = number;

        //graphic and click event
        foreach (NumberObject numberObject in numbers)
        {
            numberObject.TheButton.enabled = false;
            numberObject.TheText.enabled = false;
            SetButtonTransparency(numberObject.TheButton, 0);
        }
        numbers[TeamNumber - 1].TheButton.enabled = true;
        numbers[TeamNumber - 1].TheText.enabled = true;
        SetButtonTransparency(numbers[TeamNumber - 1].TheButton, 1);
    }

    /// <summary>
    /// Set the alpah component of normal color of the button.
    /// </summary>
    /// <param name="button">The button to set it's transparncy.</param>
    /// <param name="alpha">The alpha is clamped between 0 and 1.</param>
    private void SetButtonTransparency(Button button, float alpha)
    {
        ColorBlock colors = button.colors;
        Color normalColor = colors.normalColor;
        normalColor.a = Mathf.Clamp(alpha, 0, 1);
        colors.normalColor = normalColor;
        button.colors = colors;
    }

    /// <summary>
    /// Fade the normal color of the button.
    /// </summary>
    /// <param name="button">The button to change it's transparncy.</param>
    /// <param name="deltaAlpha">It is signed number.</param>
    private void FadeButton(Button button, float deltaAlpha)
    {
        float alpha = button.colors.normalColor.a + deltaAlpha;
        SetButtonTransparency(button, alpha);
    }

    private void SetAllEnabled()
    {
        foreach(NumberObject numberObject in numbers)
        {
            numberObject.TheButton.enabled = true;
            numberObject.TheText.enabled = true;
            SetButtonTransparency(numberObject.TheButton, 1);
        }
    }
}

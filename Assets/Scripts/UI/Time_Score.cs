using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Time_Score : MonoBehaviour
{
    //
    //Concept Definition
    //

    public enum GameLimitType { Score, Time }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    bool isLimitationChosen = true;
    Transform scoreTransform;
    Transform timeTransform;
    Button scoreButton;
    Button timeButton;
    Text scoreText;
    Text timeText;
    Text conclusionText;
    [SerializeField] float buttonMoveSpeed = 0;
    Vector3 scoreChoosePosition;
    Vector3 timeChoosePosition;
    ConstraintField scoreField;
    ConstraintField timeField;

    //constants
    const float BUTTON_DEFAULT_SPEED = 100; //100 units per second

    //option related
    private GameLimitType gameLimit = GameLimitType.Score;

    //events
    public event VoidAction MouseClickedSound;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public GameLimitType GameConstraint
    {
        get
        {
            return gameLimit;
        }
        set
        {
            gameLimit = value;

            if (value == GameLimitType.Score)
            {
                scoreField.FieldEnabled = true;
                timeField.FieldEnabled = false;
                conclusionText.text = "Score";
            }
            else
            {
                scoreField.FieldEnabled = false;
                timeField.FieldEnabled = true;
                conclusionText.text = "Time";
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        //buttons movement
        if (buttonMoveSpeed <= 0)
        {
            buttonMoveSpeed = BUTTON_DEFAULT_SPEED;
        }

        //buttons mouse click disable
        scoreButton = transform.Find("Score").GetComponent<Button>();
        timeButton = transform.Find("Time").GetComponent<Button>();
        scoreButton.enabled = false;
        timeButton.enabled = false;
        scoreText = transform.Find("Score").GetComponent<Text>();
        timeText = transform.Find("Time").GetComponent<Text>();

        //hide buttons
        scoreText.enabled = false;
        timeText.enabled = false;
        ColorBlock colors = scoreButton.colors;
        Color normalColor = colors.normalColor;
        normalColor.a = 0;
        colors.normalColor = normalColor;
        scoreButton.colors = colors;
        timeButton.colors = colors;

        //click sound requirements
        MouseClickedSound += FindObjectOfType<UISoundHandler>().PlayMouseClick;
    }

    public void Initialize(GameLimitType gameLimit)
    {
        conclusionText = transform.Find("Conclusion").GetComponent<Text>();
        scoreField = GameObject.Find("Score Amount").GetComponent<ConstraintField>();
        timeField = GameObject.Find("Time Amount").GetComponent<ConstraintField>();
        GameConstraint = gameLimit;

        //buttons movement
        scoreTransform = transform.Find("Score");
        timeTransform = transform.Find("Time");
        scoreChoosePosition = scoreTransform.localPosition;
        timeChoosePosition = timeTransform.localPosition;
        scoreTransform.localPosition = conclusionText.transform.localPosition;
        timeTransform.localPosition = conclusionText.transform.localPosition;
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void ScoreTimeChoose()
    {
        if (isLimitationChosen)
        {
            //let Score_Time not to be pressed
            isLimitationChosen = false;

            conclusionText.gameObject.SetActive(false);
            ReadyToChoose();
        }
    }

    public void ScoreSelect()
    {
        GameConstraint = GameLimitType.Score;
        EndChoose();
    }

    public void TimeSelect()
    {
        GameConstraint = GameLimitType.Time;
        EndChoose();
    }

    private void ReadyToChoose()
    {
        scoreText.enabled = true;
        timeText.enabled = true;

        //move the buttons to the proper position and enable them
        StartCoroutine(MoveButtons(scoreTransform, scoreChoosePosition, scoreButton));
        StartCoroutine(MoveButtons(timeTransform, timeChoosePosition, timeButton));
    }

    private IEnumerator MoveButtons(Transform buttonTransform, Vector3 endLocalPosition, Button button)
    {
        bool buttonPlaced = false;
        Vector3 deltaMotion = (endLocalPosition - buttonTransform.localPosition).normalized * buttonMoveSpeed;
        ColorBlock colors = button.colors;
        Color normalColor = colors.normalColor;
        float trackLength = (endLocalPosition - buttonTransform.localPosition).magnitude;
        float moveLength = deltaMotion.magnitude;
        float fadeRatio; //related to position

        while (!buttonPlaced)
        {
            yield return null;

            //move button
            buttonTransform.Translate(deltaMotion * Time.deltaTime);
            fadeRatio = moveLength * Time.deltaTime / trackLength;
            normalColor.a += fadeRatio;
            colors.normalColor = normalColor;
            button.colors = colors;

            //check conditions
            float distance = Vector2.Distance(buttonTransform.localPosition, endLocalPosition);
            buttonPlaced = distance <= buttonMoveSpeed * Time.deltaTime;
        }

        yield return null;

        //set the button position exactly
        buttonTransform.localPosition = endLocalPosition;

        //set the button alpha exactly
        normalColor.a = 1;
        colors.normalColor = normalColor;
        button.colors = colors;

        //let player choose limitation
        button.enabled = true;
    }

    private void EndChoose()
    {
        scoreButton.enabled = false;
        timeButton.enabled = false;
        StartCoroutine(MoveButtonsBack(scoreTransform, scoreButton, false));
        StartCoroutine(MoveButtonsBack(timeTransform, timeButton, true));

        //click sound
        OnMouseClickedSound();
    }

    private IEnumerator MoveButtonsBack(Transform buttonTransform, Button button, bool canEnd)
    {
        Vector3 endLocalPosition = conclusionText.transform.localPosition;
        Vector3 deltaMotion = (endLocalPosition - buttonTransform.localPosition).normalized * buttonMoveSpeed;
        ColorBlock colors = button.colors;
        Color normalColor = colors.normalColor;
        float trackLength = (endLocalPosition - buttonTransform.localPosition).magnitude;
        float moveLength = deltaMotion.magnitude;
        float fadeRatio; //related to position

        while (Vector2.Distance(buttonTransform.localPosition,endLocalPosition) > buttonMoveSpeed * Time.deltaTime)
        {
            yield return null;

            //move button
            buttonTransform.Translate(deltaMotion * Time.deltaTime);
            fadeRatio = moveLength * Time.deltaTime / trackLength;
            normalColor.a -= fadeRatio;
            colors.normalColor = normalColor;
            button.colors = colors;
        }

        yield return null;

        //set the button position exactly
        buttonTransform.localPosition = endLocalPosition;

        //set the button alpha exactly
        normalColor.a = 0;
        colors.normalColor = normalColor;
        button.colors = colors;

        if (canEnd)
        {
            isLimitationChosen = true;
            conclusionText.gameObject.SetActive(true);
            scoreText.enabled = false;
            timeText.enabled = false;
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

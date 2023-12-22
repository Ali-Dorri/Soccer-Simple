using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScoreSoundConstraint : MonoBehaviour, ISoundConstraint
{
    //
    //Fields
    //

    public event VoidAction MouseOveredSound;
    public event VoidAction MouseClickedSound;

    private Button button;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        button = GetComponent<Button>();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void ConditionalMouseClickSound()
    {
        //nothing happen ---> the click sound is handled in Time_Score class because we can not use
        //if(button.enabled) because it may be changed before in Time_Score.EndChoose() method
    }

    public void ConditionalMouseOverSound()
    {
        if (button.enabled)
        {
            OnMouseOveredSound();
        }  
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

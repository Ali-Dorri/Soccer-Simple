using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Warn the UISoundHandler to play sound when mouse over or click happened on gameObject it is attached to.
/// </summary>
public class UIMouseSoundWarner : MonoBehaviour, IUIMouseSoundWarner, IPointerEnterHandler, IPointerClickHandler
{
    //
    //Fields
    //

    /// <summary>
    /// Event which is called when the mouse overring UI sound should be played.
    /// </summary>
    public event VoidAction MouseOveredSound;
    /// <summary>
    /// Event which is called when the mouse clicking UI sound should be played.
    /// </summary>
    public event VoidAction MouseClickedSound;

    private ISoundConstraint soundConstraint;
    private bool hasSoundConstraint = false;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        //set UISoundHandler sounds to play on mouse events
        FindObjectOfType<UISoundHandler>().SetMouseEvent(this);
        
        //activate the constraint if there is any ISoundConstraint
        soundConstraint = GetComponent<ISoundConstraint>();
        if (soundConstraint != null)
        {
            hasSoundConstraint = true;
            soundConstraint.MouseOveredSound += MouseOveredSound;
            soundConstraint.MouseClickedSound += MouseClickedSound;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasSoundConstraint)
        {
            soundConstraint.ConditionalMouseOverSound();
        }
        else
        {
            OnMouseOveredSound();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasSoundConstraint)
        {
            soundConstraint.ConditionalMouseClickSound();
        }
        else
        {
            OnMouseClickedSound();
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

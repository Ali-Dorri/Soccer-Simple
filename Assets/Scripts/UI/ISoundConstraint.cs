using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidAction();

/// <summary>
/// Set conditions when mouse over and click sounds play. It is also needed that you attach UIMouseSoundWarner to your
/// gameObject.
/// </summary>
public interface ISoundConstraint
{
    /// <summary>
    /// Event which is called when the mouse overring UI sound should be played.
    /// </summary>
    event VoidAction MouseOveredSound;
    /// <summary>
    /// Event which is called when the mouse clicking UI sound should be played.
    /// </summary>
    event VoidAction MouseClickedSound;

    /// <summary>
    /// Play mouse over sound on certain conditions. It is called when user over the 
    /// gameObject this is attached to. 
    /// </summary>
    void ConditionalMouseOverSound();

    /// <summary>
    /// Play mouse click sound on certain conditions. It is called when user click the 
    /// gameObject this is attached to.
    /// </summary>
    void ConditionalMouseClickSound();
}

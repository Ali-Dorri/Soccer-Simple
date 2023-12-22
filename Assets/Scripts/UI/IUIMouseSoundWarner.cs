using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIMouseSoundWarner
{
    /// <summary>
    /// Event which is called when the mouse overring UI sound should be played.
    /// </summary>
    event VoidAction MouseOveredSound;
    /// <summary>
    /// Event which is called when the mouse clicking UI sound should be played.
    /// </summary>
    event VoidAction MouseClickedSound;
}

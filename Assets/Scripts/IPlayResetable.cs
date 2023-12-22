using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Call the SoccerEnableHandler.AddResetable(IPlayResetable) in the Start method of the implementer class.
/// </summary>
public interface IPlayResetable : IPlayEnable
{
    void PlayReset();
}

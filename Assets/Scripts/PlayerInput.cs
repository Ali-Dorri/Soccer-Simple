using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInput : MonoBehaviour, IPlayEnable
{
    //
    //Fields
    //

    private bool isEnablesd = true;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public bool IsPlayEnabled
    {
        get
        {
            return isEnablesd;
        }
        set
        {
            isEnablesd = value;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void DetermineMove(ref Vector2 moveDirection, ref Vector2 faceDirection)
    {
        if (isEnablesd)
        {
            DetermineMoveByButtonStatus(ref moveDirection, ref faceDirection);
        }
        else
        {
            moveDirection = new Vector2(0, 0);
        }
    }

    protected abstract void DetermineMoveByButtonStatus(ref Vector2 moveDirection, ref Vector2 faceDirection);

    public abstract void CheckShoot(ref bool hasShooted);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeField : ConstraintField
{
    //
    //Fields
    //

    //constants
    private const int DEFAULT_MIN_Time = 5;
    private const int DEFAULT_MAX_Time = 20;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    protected override void SetAmountLimitations()
    {
        if (minAmount <= maxAmount)
        {
            if (minAmount <= 0)
            {
                minAmount = DEFAULT_MIN_Time;
            }
            if (maxAmount <= 0)
            {
                maxAmount = DEFAULT_MAX_Time;
            }
        }
        else
        {
            minAmount = DEFAULT_MIN_Time;
            maxAmount = DEFAULT_MAX_Time;
        }
    }

    protected override void SetAddingText()
    {
        addingText = " Minutes";
    }

}

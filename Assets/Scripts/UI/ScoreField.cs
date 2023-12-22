using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreField : ConstraintField
{
    //
    //Fields
    //

    //constants
    private const int DEFAULT_MIN_Score = 5;
    private const int DEFAULT_MAX_Score = 15;

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
                minAmount = DEFAULT_MIN_Score;
            }
            if (maxAmount <= 0)
            {
                maxAmount = DEFAULT_MAX_Score;
            }
        }
        else
        {
            minAmount = DEFAULT_MIN_Score;
            maxAmount = DEFAULT_MAX_Score;
        }
    }

    protected override void SetAddingText()
    {
        addingText = " Points";
    }
}

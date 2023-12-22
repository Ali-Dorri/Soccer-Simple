using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GraphicOnRow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //
    //Fields
    //

    private bool isMouseOvered = false;
    private ControlRow rowPanel;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public bool IsMouseOvered
    {
        get
        {
            return isMouseOvered;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    public virtual void Initialize(ControlRow rowPanel)
    {
        this.rowPanel = rowPanel;
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void OnPointerEnter(PointerEventData eventData)
    {
        rowPanel.Highlight();
        isMouseOvered = true;
        rowPanel.MouseOveredSound();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //
        //the order is necessary
        //

        isMouseOvered = false;

        bool endCondition = rowPanel.IsMouseOvered || rowPanel.IsAnyGraphicOnRowMouseOvered();
        if (!endCondition)
        {
            rowPanel.SetColorNormal();
        }

        rowPanel.HasMouseExitedArea();
    }
}

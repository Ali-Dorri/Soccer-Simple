using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlRow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //
    //Fields
    //

    //highlighting variables
    private bool isMouseOvered = false;
    [SerializeField] private GraphicOnRow[] graphicsOnThis;
    Image panel;
    [SerializeField] private Color highlightedColor;
    private Color normalColor;

    //sound variables
    /// <summary>
    /// Is mouse in the panel area?
    /// </summary>
    bool isMouseInArea = false;
    UISoundHandler soundHandler;
    Canvas canvas;
    RectTransform rectTransform;

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

    public bool IsMouseInArea
    {
        get
        {
            return isMouseInArea;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        foreach(GraphicOnRow graphic in graphicsOnThis)
        {
            graphic.Initialize(this);
        }

        panel = GetComponent<Image>();
        normalColor = panel.color;
        soundHandler = FindObjectOfType<UISoundHandler>();
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight();
        isMouseOvered = true;
        MouseOveredSound();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //set color back if the mouse exits the panel area
        if (!IsAnyGraphicOnRowMouseOvered())
        {
            SetColorNormal();
        }

        isMouseOvered = false;
        HasMouseExitedArea();
    }

    public bool IsAnyGraphicOnRowMouseOvered()
    {
        //find conditions
        bool[] conditions = new bool[graphicsOnThis.Length];
        for (int i = 0; i < conditions.Length; i++)
        {
            conditions[i] = graphicsOnThis[i].IsMouseOvered;
        }

        //combine conditions by "or"(||)
        bool endCondition = false;
        foreach (bool condition in conditions)
        {
            endCondition = endCondition || condition;
        }

        return endCondition;
    }

    public void Highlight()
    {
        panel.color = highlightedColor;
    }

    public void SetColorNormal()
    {
        panel.color = normalColor;
    }

    public void MouseOveredSound()
    {
        if (!isMouseInArea)
        {
            soundHandler.PlayMouseOver();
            isMouseInArea = true;
        }
    }

    /// <summary>
    /// Set isMouseInArea false if the mouse has exited the panel area.
    /// </summary>
    public void HasMouseExitedArea()
    {
        //find mouse position
        float mouseXPosPixel = Input.mousePosition.x - Screen.width / 2;
        float mouseYPosPixel = Input.mousePosition.y - Screen.height / 2;
        float mouseXPos = mouseXPosPixel * canvas.GetComponent<RectTransform>().rect.width / Screen.width;
        float mouseYPos = mouseYPosPixel * canvas.GetComponent<RectTransform>().rect.height / Screen.height;

        //outside the panel
        float panelLeft = rectTransform.localPosition.x + rectTransform.rect.xMin;
        float panelRight= rectTransform.localPosition.x + rectTransform.rect.xMax;
        float panelBottom = rectTransform.localPosition.y + rectTransform.rect.yMin;
        float panelTop = rectTransform.localPosition.y + rectTransform.rect.yMax;
        if (mouseXPos < panelLeft + 3 || mouseXPos > panelRight - 10 || mouseYPos < panelBottom + 3
                || mouseYPos > panelTop - 3)    //the numbers adding are errors in calculating
        {
            isMouseInArea = false;
            return;
        }

        MessageBox messageBox = FindObjectOfType<MessageBox>();
        if (messageBox)
        {
            RectTransform messageRect = messageBox.GetComponent<RectTransform>();

            //inside the message box
            if (mouseXPos >= messageRect.rect.xMin && mouseXPos <= messageRect.rect.xMax &&
                mouseYPos >= messageRect.rect.yMin && mouseYPos <= messageRect.rect.yMax)
            {
                isMouseInArea = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    //
    //Fields
    //

    private static GameObject prefab;
    private static bool isPrefabReferenced = false;
    static Queue<GameObject> messageBoxes = new Queue<GameObject>();
    [SerializeField] float fadeDuration = 0.5f;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public static void ShowMessage(string message)
    {
        if (!isPrefabReferenced)
        {
            //find MessageBox at @"Assets\Prefabs\Resources\UI\MessageBox.prefab"
            prefab = Resources.Load<GameObject>("UI/MessageBox");
            isPrefabReferenced = true;
        }

        //create message box
        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject messageBox = Instantiate<GameObject>(prefab, canvas.transform);
        messageBoxes.Enqueue(messageBox);
        messageBox.transform.Find("Message Text").GetComponent<Text>().text = message;

        if (messageBoxes.Count != 1)
        {
            //there is already another active message box and maybe some inactive in the queue
            messageBox.SetActive(false);
        }
        
    }

    public void OK()
    {
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        Image boxImage = GetComponent<Image>();
        Text messageText = transform.Find("Message Text").GetComponent<Text>();
        Button OKButton= transform.Find("OK Button").GetComponent<Button>();
        Text buttonText = transform.Find("OK Button").Find("OK Text").GetComponent<Text>();
        float time = 0;

        while (time < fadeDuration)
        {
            yield return null;

            time += Time.deltaTime;
            float alpha = 1 - (time / fadeDuration);
            SetTransparency(boxImage, alpha);
            SetTransparency(messageText, alpha);
            SetTransparency(OKButton, alpha);
            SetTransparency(buttonText, alpha);
        }

        //remove the used message box
        messageBoxes.Dequeue();
        Destroy(gameObject);
        //enable the next message box
        if(messageBoxes.Count > 0)
        {
            messageBoxes.Peek().SetActive(true);
        }
    }

    private void SetTransparency(Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }

    private void SetTransparency(Button button, float alpha)
    {
        ColorBlock colors = button.colors;
        Color highlightedColor = colors.highlightedColor;
        highlightedColor.a = alpha;
        colors.highlightedColor = highlightedColor;
        button.colors = colors;
    }  
}

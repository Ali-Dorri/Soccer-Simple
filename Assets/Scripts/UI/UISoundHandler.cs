using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundHandler : MonoBehaviour
{
    //
    //Fields
    //

    AudioSource audioSource;
    [SerializeField] AudioClip mouseOverClip;
    [SerializeField] AudioClip mouseClickClip;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void PlayMouseOver()
    {
        audioSource.clip = mouseOverClip;
        audioSource.Play();
    }

    public void PlayMouseClick()
    {
        audioSource.clip = mouseClickClip;
        audioSource.Play();
    }

    /// <summary>
    /// Set the proper methods of soundHandler to the MouseOveredSound and MouseClickedSound events of the warner.
    /// </summary>
    /// <param name="warner"></param>
    public void SetMouseEvent(IUIMouseSoundWarner warner)
    {
        warner.MouseOveredSound += PlayMouseOver;
        warner.MouseClickedSound += PlayMouseClick;
    }

}

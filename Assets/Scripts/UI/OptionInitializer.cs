using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It is used to inittilize the OptionContainer in it's Start or Awake method whenever it should be. Because the
/// OptionContainer call the DontDestroyOnLoad() so it's Start and Awake methods will be called only once(not everytime
/// we go to the option
/// menu).
/// </summary>
public class OptionInitializer : MonoBehaviour
{
    void Start ()
    {
        //
        //find first option handler
        //

        OptionContainer[] optionContainers = FindObjectsOfType<OptionContainer>();
        OptionContainer firstOptionContainer = null;

        if (OptionContainer.IsSingletonCreated)
        {
            foreach(OptionContainer loopOptionContainer in optionContainers)
            {
                if (loopOptionContainer.IsSingleton)
                {
                    firstOptionContainer = loopOptionContainer;
                }
            }
        }
        else
        {
            //there is just one option container (this happens when this Start() is called befor OptionHandler's Start())
            firstOptionContainer = optionContainers[0];
        }

        //initialize it
        firstOptionContainer.Initialize();
        Destroy(gameObject);
	}

}

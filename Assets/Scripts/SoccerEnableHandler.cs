using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerEnableHandler : MonoBehaviour
{
    //
    //Fields
    //

    /// <summary>
    /// It will be assigned by AddResetable() that is called by any IPlayResetable.
    /// </summary>
    List<IPlayResetable> resetables = new List<IPlayResetable>();
    Transform[] soccerPlayerTransforms;
    Vector2[] defaultPositions;
    [SerializeField] float backSpeed;

    //singleton requirements
    bool isSingleton = false;
    static bool isSingletonCreated = false;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public bool IsSingleton
    {
        get
        {
            return isSingleton;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        MakeSingleton();
    }

    /// <summary>
    /// This may be called befor in the Start() method
    /// </summary>
    public void MakeSingleton()
    {
        if (isSingletonCreated)
        {
            if (!isSingleton)
            {
                Destroy(this);

                //destroy game object if this component is the only one which is attached to
                if (GetComponent<MonoBehaviour>() == null)
                {
                    Destroy(gameObject);
                }
            } 
        }
        else
        {
            isSingletonCreated = true;
            isSingleton = true;
        }
    }

    /// <summary>
    /// It should be called by PrimaryInitializer.
    /// </summary>
    public void Initialize()
    {
        SoccerPlayer[] soccerPlayers = FindObjectsOfType<SoccerPlayer>();
        int soccerCount = soccerPlayers.Length;
        soccerPlayerTransforms = new Transform[soccerCount];
        defaultPositions = new Vector2[soccerCount];

        for (int i = 0; i < soccerCount; i++)
        {
            soccerPlayerTransforms[i] = soccerPlayers[i].transform;
            defaultPositions[i] = soccerPlayerTransforms[i].position;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    /// <summary>
    /// Add the resetable to the SoccerEnableHandler singleton's IPlayResetable list.
    /// </summary>
    /// <param name="resetable"></param>
    public static void AddResetable(IPlayResetable resetable)
    {
        SoccerEnableHandler[] enableHandlers = FindObjectsOfType<SoccerEnableHandler>();

        if (isSingletonCreated)
        {
            foreach (SoccerEnableHandler enableHandler in enableHandlers)
            {
                if (enableHandler.isSingleton)
                {
                    enableHandler.resetables.Add(resetable);
                }
            }
        }
        else
        {
            //initialize the first one make it the singleton
            enableHandlers[0].MakeSingleton();

            enableHandlers[0].resetables.Add(resetable);
        }
    }

    public void TeamReposition()
    {
        //set all the active elements(including soccerPlayers) disabled
        foreach(IPlayResetable resetable in resetables)
        {
            resetable.IsPlayEnabled = false;
        }

        StartCoroutine(SoccerPlayersReposition());
    }

    IEnumerator SoccerPlayersReposition()
    {
        int recievedNumber = 0;
        int maxRecieved = soccerPlayerTransforms.Length;

        bool[] isRecieveds = new bool[maxRecieved];
        for(int i = 0; i < maxRecieved; i++)
        {
            isRecieveds[i] = false;
        }

        //move soccer players
        while (recievedNumber < maxRecieved)
        {
            yield return null;

            for(int i = 0; i < maxRecieved; i++)
            {
                if (!isRecieveds[i])
                {
                    Vector2 motion = (defaultPositions[i] - (Vector2)soccerPlayerTransforms[i].position).normalized * backSpeed;
                    soccerPlayerTransforms[i].position += (Vector3)(motion * Time.deltaTime);

                    if (Vector2.Distance(soccerPlayerTransforms[i].position, defaultPositions[i]) < backSpeed * Time.deltaTime)
                    {
                        isRecieveds[i] = true;
                        recievedNumber++;
                    }
                }
            }
        }

        //set all the active elements(including soccerPlayers) back to default
        foreach (IPlayResetable resetable in resetables)
        {
            resetable.PlayReset();
        }
    }

    private void OnDestroy()
    {
        if (isSingleton)
        {
            isSingletonCreated = false;
        }
    }
}

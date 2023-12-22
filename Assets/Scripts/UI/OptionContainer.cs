#define DEFAULT_GAME_LIMIT_TYPE_SCORE

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class OptionContainer : MonoBehaviour
{
    //Order of modifying OptionData.option file:
    //teamNumber(int)
    //gameLimitType(bool)
    //timeLimit(int)
    //scoreLimit(int)


    //
    //Concept Definition
    //

    private enum SaveType { Default, UserDefined }
    public enum KeyboardKeyCodeIndex { MoveUp, MoveDown, MoveRight, MoveLeft, Shoot }
    public enum JoyStickIAxisIndex { MoveY, MoveX, FaceY, FaceX }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    //option objects
    Time_Score timeScore;
    ConstraintField scoreAmountField;
    ConstraintField timeAmountField;
    MemberNumberHandler memberNumberHandler;

    //input variables(controls)
    //
    private Player.InputType inputType;
    //keyboard
    private KeyCode[] keyboardKeyCodes; //moveUp,moveDown,moveRight,moveLeft,shootKey
    //joystick
    private IAxisInput[] joystickAxes;  //moveX,moneY,faceX,faceY
    private KeyCode joystickShootKey;
    //codes of controls
    int[] codesOfControls;

    //game variables
    [SerializeField]int teamNumber = DEFAULT_TEAM_NUMBER;
    Time_Score.GameLimitType gameLimitType = DEFAULT_GAME_LIMIT_TYPE;
    int scoreLimit = DEFAULT_SCORE_LIMIT;
    int timeLimit = DEFAULT_TIME_LIMIT;

    //default game variables
    const int DEFAULT_TEAM_NUMBER = 4;
#if DEFAULT_GAME_LIMIT_TYPE_SCORE
    const Time_Score.GameLimitType DEFAULT_GAME_LIMIT_TYPE = Time_Score.GameLimitType.Score;
#else
    const Time_Score.GameLimitType DEFAULT_GAME_LIMIT_TYPE = Time_Score.GameLimitType.Time;
#endif
    const int DEFAULT_SCORE_LIMIT = 5;  //5 points
    const int DEFAULT_TIME_LIMIT = 5;   //5 minutes

    //other variables
    bool isLoaded = false;
    bool isSingleton = false;
    static bool isSingletonCreated = false;
    public bool BlueIsWinner;
    public bool Draw;

    //other constants
    /// <summary>
    /// It doesn't have the file name. If you want the file name too, use FilePathWithName property.
    /// </summary>
    const string FILE_PATH_WITHOUT_NAME = "Option Variables";
    const string FILE_NAME = "OptionData.option";
    const string FULL_FILE_PATH = FILE_PATH_WITHOUT_NAME + "\\" + FILE_NAME;
    const string LOCAL_OPTION_FOLDER = "Option Variables";
    const string LOCAL_OPTION_FILE = "OptionData.option";

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public int TeamNumber
    {
        get
        {
            return teamNumber;
        }
    }

    public Time_Score.GameLimitType TypeOfGameLimit
    {
        get
        {
            return gameLimitType;
        }
    }

    public int ScoreLimit
    {
        get
        {
            return scoreLimit;
        }
    }

    public int TimeLimit
    {
        get
        {
            return timeLimit;
        }
    }

    public bool IsSingleton
    {
        get
        {
            return isSingleton;
        }
    }

    public static bool IsSingletonCreated
    {
        get
        {
            return isSingletonCreated;
        }
    }

    public Player.InputType InputType
    {
        get
        {
            return inputType;
        }
    }

    public KeyCode[] KeyboardKeyCodes
    {
        get
        {
            return keyboardKeyCodes;
        }
    }

    public IAxisInput[] JoyStickAxes
    {
        get
        {
            return joystickAxes;
        }
    }

    public KeyCode JoyStickShootKey
    {
        get
        {
            return joystickShootKey;
        }
    }

    public int[] CodesOfControls
    {
        get
        {
            return codesOfControls;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        if (isSingletonCreated)
        {
            Destroy(gameObject);
        }
        else
        {
            StayAlive();
        }
    }

    /// <summary>
    /// Initialize option objects' variables that should be initialized in their Start method, by game variables.
    /// It is called by OptionInitializer's Start method. 
    /// </summary>
    public void Initialize()
    {
        //Load game variables from file if any
        if (!isLoaded)
        {
            Load();
        }

        //option objects initialize
        if(SceneManager.GetActiveScene().name == "Option")
        {
            InitializeOptionObjects();
        }  
    }

    private void InitializeOptionObjects()
    {
        scoreAmountField = GameObject.Find("Score Amount").GetComponent<ConstraintField>();
        timeAmountField = GameObject.Find("Time Amount").GetComponent<ConstraintField>();
        ConstraintField.StaticInitialize();
        timeScore = FindObjectOfType<Time_Score>();
        memberNumberHandler = FindObjectOfType<MemberNumberHandler>();
        memberNumberHandler.Initialize(teamNumber);
        scoreAmountField.Initialize(scoreLimit);
        timeAmountField.Initialize(timeLimit);
        timeScore.Initialize(gameLimitType);

        //set click events to Apply and Default buttons
        GameObject.Find("Apply").GetComponent<Button>().onClick.AddListener(Apply);
        GameObject.Find("Default").GetComponent<Button>().onClick.AddListener(SetDefault);
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    /// <summary>
    /// Set Option as default.
    /// </summary>
    public void SetDefault()
    {
        //set option objects to default
        memberNumberHandler.ChooseNumber(DEFAULT_TEAM_NUMBER);
        timeScore.GameConstraint = DEFAULT_GAME_LIMIT_TYPE;
        scoreAmountField.SetAmountAndText(DEFAULT_SCORE_LIMIT);
        timeAmountField.SetAmountAndText(DEFAULT_TIME_LIMIT);
    }

    /// <summary>
    /// Make game variables as default.
    /// </summary>
    private void DefaultGameVariables()
    {
        teamNumber = DEFAULT_TEAM_NUMBER;
        gameLimitType = DEFAULT_GAME_LIMIT_TYPE;
        scoreLimit = DEFAULT_SCORE_LIMIT;
        timeLimit = DEFAULT_TIME_LIMIT;
    }

    public void Apply()
    {
        teamNumber = memberNumberHandler.TeamNumber;
        gameLimitType = timeScore.GameConstraint;
        scoreLimit = scoreAmountField.Amount;
        timeLimit = timeAmountField.Amount;

        Save(SaveType.UserDefined);
    }

    /// <summary>
    /// Load the game variables from file.
    /// </summary>
    private void Load()
    {
        FileStream fileStream = null;
        string folderPath = Path.Combine(Application.persistentDataPath, LOCAL_OPTION_FOLDER);
        string filePath = Path.Combine(folderPath, LOCAL_OPTION_FILE);

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fileStream);

            //read game variables
            teamNumber = reader.ReadInt32();
            if (reader.ReadBoolean())   //is gameLimitType == GameLimitType.Score 
            {
                gameLimitType = Time_Score.GameLimitType.Score;
            }
            else
            {
                gameLimitType = Time_Score.GameLimitType.Time;
            }
            timeLimit = reader.ReadInt32();
            scoreLimit = reader.ReadInt32();
        }
        catch(FileNotFoundException)
        {
            MessageBox.ShowMessage("Option was not found. Option will be set to default." +
                                    " You may need to save the option again.");

            //create the file in the proper directory
            fileStream = File.Create(filePath);
            fileStream.Close();
            fileStream = null;

            //write as default
            Save(SaveType.Default);

            //make game variable default
            DefaultGameVariables();
        }
        catch (EndOfStreamException)
        {
            //close it to let the Save() to use it's own fileStream
            fileStream.Close();
            fileStream = null;

            //make game variable default
            DefaultGameVariables();

            //write as default
            Save(SaveType.Default);          
        }
        catch (DirectoryNotFoundException)
        {
            //The OptionData.option may not be created
            MessageBox.ShowMessage("Directory not found. Please check the directory. Option will be set to default. " +
                                    "Reloading the game may correct it.");
            //make game variable default
            DefaultGameVariables();

            //write as default
            Save(SaveType.Default);
        }
        catch(IOException)
        {
            //The OptionData.option may not be created
            MessageBox.ShowMessage("Something went wrong during loading.Option will be set to default. " +
                                    "Reloading the game may correct it.");
            //make game variable default
            DefaultGameVariables();

            //write as default
            Save(SaveType.Default);
        }
        catch
        {
            MessageBox.ShowMessage("Some error was occured while loading option. Option will be set to default." +
                                    " You may need to save the option again.");

            //make game variable default
            DefaultGameVariables();

            //write as default
            Save(SaveType.Default);
        }
        finally
        {
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }

        LoadControls();
        isLoaded = true;
    }

    private void LoadControls()
    {
        ControlsInputManager.Load(out inputType, out keyboardKeyCodes, out joystickAxes, out joystickShootKey,
                                  out codesOfControls);
    }

    /// <summary>
    /// Update the game variables from option objects and then save tham to file.
    /// </summary>
    private void Save(SaveType saveType)
    {
        FileStream fileStream = null;
        string folderPath = Path.Combine(Application.persistentDataPath, LOCAL_OPTION_FOLDER);
        string filePath = Path.Combine(folderPath, LOCAL_OPTION_FILE);

        try
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(fileStream);

            //write game variables to file
            if (saveType == SaveType.Default)
            {
                writer.Write(DEFAULT_TEAM_NUMBER);
#if DEFAULT_GAME_LIMIT_TYPE_SCORE
                writer.Write(true);
#else
                writer.Write(false);
#endif
                writer.Write(DEFAULT_TIME_LIMIT);
                writer.Write(DEFAULT_SCORE_LIMIT);
            }
            else
            {
                writer.Write(teamNumber);
                if(gameLimitType == Time_Score.GameLimitType.Score)
                {
                    writer.Write(true);
                }
                else
                {
                    writer.Write(false);
                }
                writer.Write(timeLimit);
                writer.Write(scoreLimit);
            }
        }
        catch (DirectoryNotFoundException exc)
        {
            MessageBox.ShowMessage("Directory not found. Please check the directory. You can Play without anxiety" +
                                    " but your defined setting wasn't saved. Reloading the game may correct it.");
        }
        catch (IOException exc)
        {
            MessageBox.ShowMessage("Something went wrong during saving to file. You can Play without anxiety but" +
                                    " your defined setting wasn't saved. Reloading the game may correct it.");
        }
        finally
        {
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
    }

    private void StayAlive()
    {
        //the gameObject should have no parent to DontDestroyOnLoad(gameObject) work
        transform.parent = null;

        DontDestroyOnLoad(gameObject);
        isSingletonCreated = true;
        isSingleton = true;
    }

    public void SetControlsVariables(Player.InputType inputType, KeyCode[] keyboardKeyCodes, IAxisInput[] joystickAxes,
                                     KeyCode joystickShootKey)
    {
        this.inputType = inputType;

        if (inputType == Player.InputType.MouseAndKeyboard)
        {
            this.keyboardKeyCodes = keyboardKeyCodes;
        }
        else if (inputType == Player.InputType.JoyStick)
        {
            this.joystickAxes = joystickAxes;
            this.joystickShootKey = joystickShootKey;
        }
        else
        {
            this.keyboardKeyCodes = keyboardKeyCodes;
            this.joystickAxes = joystickAxes;
            this.joystickShootKey = joystickShootKey;
        }
    }
}

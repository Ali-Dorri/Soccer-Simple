using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryInitializer : MonoBehaviour
{
    public static void PrimaryInitialize(GameObject firstPlayer)
    {
        //soccer objects finding
        FindObjectOfType<Ball>().Initialize();
        Player[] players = FindObjectsOfType<Player>();
        SoccerPlayer[] soccerPlayers = FindObjectsOfType<SoccerPlayer>();
        SoccerPlayer firstPlayer_SoccerPlayer = firstPlayer.GetComponent<SoccerPlayer>();
        if(firstPlayer_SoccerPlayer == null)
        {
            Debug.LogError("Wrong gameobject for soccer player that  should be the first player!");
            Debug.LogWarning(string.Format("The {0} for first player doesn't have the SoccePlayer component!", firstPlayer));
        }

        //input variables finding
        OptionContainer optionContainer = FindObjectOfType<OptionContainer>();

        //soccerEnable initialization
        FindObjectOfType<SoccerEnableHandler>().Initialize();

        //
        //the order is neccessary from here
        //

        //soccer objects initialization
        SoccerObjectsInitialization(optionContainer, players, soccerPlayers, firstPlayer_SoccerPlayer);

        //input variables initialization
        InputInitialize(optionContainer);
    }

    private static void SoccerObjectsInitialization(OptionContainer optionContainer,Player[] players
                                , SoccerPlayer[] soccerPlayers, SoccerPlayer firstPlayer_SoccerPlayer)
    {
        Player.InputType inputType = optionContainer.InputType;

        foreach (Player player in players)
        {
            player.Initialize(inputType);
        }
        foreach (SoccerPlayer soccerPlayer in soccerPlayers)
        {
            if (soccerPlayer != firstPlayer_SoccerPlayer)
            {
                soccerPlayer.Initialize(SoccerPlayer.PlayerOrAI.AI);
            }
            else
            {
                soccerPlayer.Initialize(SoccerPlayer.PlayerOrAI.Player);
            }
        }
    }

    private static void InputInitialize(OptionContainer optionContainer)
    {

        KeyboardAndMouseInput[] keyboardInputs = FindObjectsOfType<KeyboardAndMouseInput>();
        JoyStickInput[] joyStickInputs = FindObjectsOfType<JoyStickInput>();
        Player.InputType inputType = optionContainer.InputType;

        if (inputType == Player.InputType.MouseAndKeyboard)
        {
            InitializeKeyboard(optionContainer, keyboardInputs);
        }
        else if (inputType == Player.InputType.JoyStick)
        {
            InitializeJoyStick(optionContainer, joyStickInputs);
        }
        else
        {
            InitializeKeyboard(optionContainer, keyboardInputs);
            InitializeJoyStick(optionContainer, joyStickInputs);
        }
    }

    private static void InitializeKeyboard(OptionContainer optionContainer, KeyboardAndMouseInput[] keyboardInputs)
    {
        KeyCode moveUp = optionContainer.KeyboardKeyCodes[(int)OptionContainer.KeyboardKeyCodeIndex.MoveUp];
        KeyCode moveDown = optionContainer.KeyboardKeyCodes[(int)OptionContainer.KeyboardKeyCodeIndex.MoveDown];
        KeyCode moveRight= optionContainer.KeyboardKeyCodes[(int)OptionContainer.KeyboardKeyCodeIndex.MoveRight];
        KeyCode moveLeft = optionContainer.KeyboardKeyCodes[(int)OptionContainer.KeyboardKeyCodeIndex.MoveLeft];
        KeyCode shootKey = optionContainer.KeyboardKeyCodes[(int)OptionContainer.KeyboardKeyCodeIndex.Shoot];

        foreach (KeyboardAndMouseInput keyInput in keyboardInputs)
        {
            keyInput.Initialize(moveUp, moveDown, moveLeft, moveRight, shootKey);
        }
    }

    private static void InitializeJoyStick(OptionContainer optionContainer,JoyStickInput[] joyStickInputs)
    {
        IAxisInput moveX = optionContainer.JoyStickAxes[(int)OptionContainer.JoyStickIAxisIndex.MoveX];
        IAxisInput moveY = optionContainer.JoyStickAxes[(int)OptionContainer.JoyStickIAxisIndex.MoveY];
        IAxisInput faceX = optionContainer.JoyStickAxes[(int)OptionContainer.JoyStickIAxisIndex.FaceX];
        IAxisInput faceY = optionContainer.JoyStickAxes[(int)OptionContainer.JoyStickIAxisIndex.FaceY];
        KeyCode shootKey = optionContainer.JoyStickShootKey;

        foreach (JoyStickInput joyStickInput in joyStickInputs)
        {
            joyStickInput.Initialize(shootKey, moveX, moveY, faceX, faceY);
        }
    }
}

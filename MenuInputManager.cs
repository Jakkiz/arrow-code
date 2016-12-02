using UnityEngine;
using XInputDotNetPure; // Required in C#
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuInputManager : MonoBehaviour
{
    public GameObject respawnPoint;
    public GameObject playerPrefab;
    public Sprite emptySprite;
    public Sprite padSprite;
    bool playerIndexSet = false;
    int ColorMax = 7;
    GamePadState state;
    GamePadState prevState;
    public GamePadState[] previousStates;
    public List<PlayerIndex> indexList = new List<PlayerIndex>();
    public Image[] sliderList;
    public Color32[] playerColorList;
    public bool[] isPlaying;
    private Color32[] colorPickerList;
    private int[] colorIndexSelected;

    void Start()
    {
        previousStates = new GamePadState[] { GamePad.GetState(0), GamePad.GetState(0), GamePad.GetState(0), GamePad.GetState(0), GamePad.GetState(0), GamePad.GetState(0) };
        colorPickerList = new Color32[] { new Color32(125, 239, 31, 255), new Color32(255, 171, 53, 255), new Color32(55, 232, 158, 255), new Color32(45, 237, 255, 255), new Color32(211, 117, 255, 255), new Color32(255, 117, 236, 255), new Color32(255, 30, 64, 255), new Color32(255, 255, 0, 255) };
        isPlaying = new bool[] { false, false, false, false, false, false };
        playerColorList = new Color32[] { Color.white, Color.white, Color.white, Color.white, Color.white, Color.white };
        colorIndexSelected = new int[] { 0, 0, 0, 0, 0, 0 };
        
        for (int i = 0; i < 4; i++)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                previousStates[(int)testPlayerIndex] = GamePad.GetState(testPlayerIndex);
                indexList.Add(testPlayerIndex);
            }
        }
    }

    void Update()
    {
        //Handles all the player Inputs in this loop
        foreach (PlayerIndex playerNumb in indexList)
        {
            prevState = previousStates[(int)playerNumb];
            state = GamePad.GetState(playerNumb);
            int i = (int)playerNumb;

            if(prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
            {
                isPlaying[i] = true;
                sliderList[i].sprite = padSprite;
                sliderList[i].color = playerColorList[i];
            }
            if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed)
            {
                isPlaying[i] = false;
                sliderList[i].sprite = emptySprite;
            }
            if (isPlaying[i])
            {
                // Detect if a button was pressed this frame
                if (prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed)
                {
                    Debug.Log("leftshoulder");
                    if (colorIndexSelected[i] < 6)
                    {
                        colorIndexSelected[i] = colorIndexSelected[i] + 1;
                        playerColorList[i] = colorPickerList[colorIndexSelected[i]];
                        sliderList[i].color = playerColorList[i];
                        Debug.Log("color:" + playerColorList[i]);
                    }
                }
                // Detect if a button was pressed this frame
                if (prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    Debug.Log("rightshoulder");
                    if (colorIndexSelected[i] > 0)
                    {
                        colorIndexSelected[i] = colorIndexSelected[i] - 1;
                        playerColorList[i] = colorPickerList[colorIndexSelected[i]];
                        sliderList[i].color = playerColorList[i];
                    }
                }
            }

            if(!isPlaying[i])
            {
                sliderList[i].color = Color.white;
            }

            previousStates[i] = state; // Update the previous state to the current state at the end
        }

        //F1 TO PLAY ONE PLAYERS MODE
        if(Input.GetKeyDown(KeyCode.F1))
        {
            isPlaying[0] = true;
            playerColorList[0] = colorPickerList[2];
            SetPlayersActive();
            SceneManager.LoadScene(1);
        }

        //F2 TO PLAYER TWO PLAYER MODE
        if (Input.GetKeyDown(KeyCode.F2))
        {
            isPlaying[0] = true;
            isPlaying[1] = true;
            playerColorList[0] = colorPickerList[2];
            playerColorList[1] = colorPickerList[4];
            SetPlayersActive();
            SceneManager.LoadScene(1);
        }

        //F3 TO PLAY THREE PLAYERS MODE
        if (Input.GetKeyDown(KeyCode.F3))
        {
            isPlaying[0] = true;
            isPlaying[1] = true;
            isPlaying[2] = true;
            playerColorList[0] = colorPickerList[2];
            playerColorList[1] = colorPickerList[4];
            playerColorList[2] = colorPickerList[1];
            SetPlayersActive();
            SceneManager.LoadScene(1);
        }

        //F4 TO PLAY FOUR PLAYERS
        if (Input.GetKeyDown(KeyCode.F4))
        {
            isPlaying[0] = true;
            isPlaying[1] = true;
            isPlaying[2] = true;
            isPlaying[3] = true;
            playerColorList[0] = colorPickerList[2];
            playerColorList[1] = colorPickerList[4];
            playerColorList[2] = colorPickerList[1];
            playerColorList[3] = colorPickerList[3];
            SetPlayersActive();
            SceneManager.LoadScene(1);
        }

    }
    
 /*

    // Detect if a button was pressed this frame
    if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
    {
        GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
    }
    // Detect if a button was released this frame
    if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
    {
        GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    // Set vibration according to triggers
    GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);

    // Make the current object turn
    transform.localRotation *= Quaternion.Euler(0.0f, state.ThumbSticks.Left.X * 25.0f * Time.deltaTime, 0.0f);
}
*/
    public void SetPlayersActive()
    {
        List<PlayerIndex> playerPlaying = new List<PlayerIndex>();
        List<Color32> playerPlayingColor = new List<Color32>();
        foreach (PlayerIndex playerNumb in indexList)
        {
            int i = (int)playerNumb;
            if (isPlaying[i])
            {
                playerPlaying.Add(indexList[i]);
                playerPlayingColor.Add(playerColorList[i]);
            }
            Debug.Log("playerNumb:" + i);
        }
        GlobalScript.playerPlaying = playerPlaying;
        GlobalScript.playerPlayingColor = playerPlayingColor;
    }

    //void OnGUI()
    //{
    //    string text = "Use left stick to turn the cube, hold A to change color\n";
    //    text += string.Format("IsConnected {0} Packet #{1}\n", state.IsConnected, state.PacketNumber);
    //    text += string.Format("\tTriggers {0} {1}\n", state.Triggers.Left, state.Triggers.Right);
    //    text += string.Format("\tD-Pad {0} {1} {2} {3}\n", state.DPad.Up, state.DPad.Right, state.DPad.Down, state.DPad.Left);
    //    text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", state.Buttons.Start, state.Buttons.Back, state.Buttons.Guide);
    //    text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", state.Buttons.LeftStick, state.Buttons.RightStick, state.Buttons.LeftShoulder, state.Buttons.RightShoulder);
    //    text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", state.Buttons.A, state.Buttons.B, state.Buttons.X, state.Buttons.Y);
    //    text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
    //    GUI.Label(new Rect(0, 0, Screen.width, Screen.height), text);
    //}
    
    
}

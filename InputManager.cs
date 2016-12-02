using UnityEngine;
using XInputDotNetPure; // Required in C#
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{

    public GameObject playerPrefab;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    public GamePadState[] stateList;
    public GamePadState prevState;
    //PlayerIndex[] playersList;
    //GamePadState[] statesList;
    List<PlayerIndex> indexList = new List<PlayerIndex>();
    public List<GameObject> playersList = new List<GameObject>();
    List<RespawnPoint> respawnPointsList = new List<RespawnPoint>();

    // Use this for initialization
    void Start()
    {
        for (int i = 1; i <= 5; i++)
        {
            RespawnPoint rPoint = new RespawnPoint();
            rPoint.number = i;
            rPoint.taken = false;
            respawnPointsList.Add(rPoint);
        }


        indexList = GlobalScript.playerPlaying;
        foreach (PlayerIndex index in indexList)
        {
            // prevState = state;
            state = GamePad.GetState(index);
            // statesList.Add(state);
            GameObject newPlayer = (GameObject)Instantiate(playerPrefab, GameObject.Find("SpawnPoint" + GetRespawn()).transform.position, Quaternion.Euler(0, 0, 0));
            newPlayer.GetComponent<PlayerController>().state = state;
            newPlayer.GetComponent<PlayerController>().ControllerNumber = (int)index;
            newPlayer.GetComponent<PlayerController>().color = GlobalScript.playerPlayingColor[(int)index];
            playersList.Add(newPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject player in playersList)
        {
            PlayerIndex newIndex = (PlayerIndex)player.GetComponent<PlayerController>().ControllerNumber;
            prevState = state;
            state = GamePad.GetState(newIndex);
            player.GetComponent<PlayerController>().prevState = prevState;
            player.GetComponent<PlayerController>().state = state;
        }
    }

    int GetRespawn()
    {
        bool found = false;
        int n = Random.Range(1, 5);
        while (!found)
        {
            if (!respawnPointsList[n].taken)
            {
                respawnPointsList[n].taken = true;
                return respawnPointsList[n].number;
            }
            else
            {
                n = Random.Range(1, 5);
            }
        }
        return 1;
    }

    public class RespawnPoint
    {
        public int number;
        public bool taken;
    }

}

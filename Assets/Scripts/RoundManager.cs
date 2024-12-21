using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    private static readonly List<PlayerObject> playerObjects = new List<PlayerObject>();
    private static int activePlayerIndex;
    private static bool roundOver;

    private void Awake()
    {
        instance = this;
        MakeNewPlayer("Bob");
        MakeNewPlayer("Jill");
        // Transform playerTransform = playerObjects[0].GetComponent<Transform>();
        // MeshRenderer arrowMeshRenderer = playerObjects[0].
        // // meshRendererList = playerObjects[0].GetComponentsInChildren<MeshRenderer>();
        
    }

    private void MakeNewPlayer(string playerName)
    {
        PlayerObject newPlayer = gameObject.AddComponent<PlayerObject>();
        newPlayer.SetName(playerName);
        playerObjects.Add(newPlayer);
    }

    public static void TakeStroke()
    {
        playerObjects[activePlayerIndex].TakeStroke();
    }

    public void AddScore(int score)
    {
        playerObjects[activePlayerIndex].AddScore(score);
    }

    private static void CyclePlayer()
    {
        int playerCount = playerObjects.Count;
        int checkedPlayers = 0;
        activePlayerIndex = (activePlayerIndex + 1) % (playerObjects.Count - 1);
        if (playerObjects[activePlayerIndex]._inHole)
        {
            checkedPlayers++;
            if (checkedPlayers >= playerCount)
            {
                // do end of match logic
                return;
            }
            CyclePlayer();
        }
    }
    
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class HoleDetection : MonoBehaviour
{
    public static HoleDetection instance;
    public static float _chipShotTime;
    private float chipShotDelay = 2.5f;
    public static string _chipShotID;
    private float lastEnter;
    private float delay = 0.5f;
    private List<string> playerIDs = new List<string>();

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastEnter < delay) return;
        if (!other.CompareTag("Player")) return;
        PlayerObject newPlayer = other.GetComponentInParent<PlayerObject>();
        string playerID = newPlayer._playerID;
        if (playerIDs.Contains(playerID)) return;
        lastEnter = Time.time;
        playerIDs.Add(playerID);
        newPlayer.playerState = PlayerState.InHole;
        if (_chipShotID == playerID && Time.time - chipShotDelay < _chipShotTime)
        {
            newPlayer._currentScore += 5000;
            print("CHIP SHOT!!!");
        }
        GameManager._playersInHole++;
        print(newPlayer._playerID + " entered the hole in " + newPlayer._strokeCount + " strokes and a score of " + newPlayer._currentScore);
    }
    
}


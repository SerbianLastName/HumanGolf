using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerObject : MonoBehaviour
{
     private string _name;
     public int _strokeCount;
     public int _currentScore;
     public bool _inHole;
     public bool _inGreen;
     public bool _showHole;
     public MeshRenderer _arrowRenderer;
     public Transform _arrowTransform;
     public Rigidbody _pelvisRigidbody;
     public Rigidbody[] _rbList;
     public PlayerState playerState;
     public string _playerID;

     private void Awake()
     {
          _playerID = System.Guid.NewGuid().ToString();
     }
     
     public void UpdatePlayerState(PlayerState newState)
     {
          playerState = newState;
     }
     public void SetName(string newName)
     {
          _name = newName;
     }
     public string GetName() => _name;

     public void TakeStroke()
     {
          _strokeCount++;
     }

     public void AddScore(int score)
     {
          _currentScore += score;
     }
     

     // public bool InHole() => _inHole;


}

public enum PlayerState
{
     Aiming,
     MidAir,
     OnGround,
     InHole,
     AfterStroke,
}
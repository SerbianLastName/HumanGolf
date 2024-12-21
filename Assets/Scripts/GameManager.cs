using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameState gameState;
    public static event Action<GameState> OnGameStateChanged;
    public static ActivePlayerState activePlayerState;
    public static event Action<ActivePlayerState> OnActivePlayerStateChanged;
    public static float shotTaken;
    public static int currentCourse;
    private static int _currentStroke;
    [SerializeField] GameObject[] playerPrefabList;
    private static List<GameObject> _playerPrefabList = new List<GameObject>();
    private static List<GameObject> _playerList = new List<GameObject>();
    private static List<PlayerObject> _playerObjectList = new List<PlayerObject>();
    [SerializeField] Transform spawnPoint;
    private static Transform _spawnPoint;
    private static GameObject _playerPrefab;
    private static int _currentPlayerIndex;
    public static int _playersInHole;
    private void Awake()
    {
        instance = this;
        foreach (GameObject obj in playerPrefabList)
        {
            _playerPrefabList.Add(obj);
        }
        _spawnPoint = spawnPoint;
        GameObject currentPlayer = Instantiate(playerPrefabList[0], _spawnPoint.position, _spawnPoint.rotation);
        _playerPrefab = currentPlayer;
        _playerList.Add(currentPlayer);
        InitPlayer(_playerPrefab);
    }

  

    private void Start()
    {
        UpdateGameState(GameState.Playing);
        UpdateActivePlayerState(ActivePlayerState.Aiming);
    }

    private void FixedUpdate()
    {
        
    }
    
    private static void InitPlayer(GameObject playerPrefab)
    {
        if (_currentPlayerIndex >= _playerObjectList.Count)
        {
            PlayerObject playerObject = playerPrefab.GetComponent<PlayerObject>();
            _playerObjectList.Add(playerObject);
        }
        ShotController.InitShotController(_playerObjectList[_currentPlayerIndex]);
        PlayerPhysicsController.InitController(_playerObjectList[_currentPlayerIndex]._rbList);
        SetHoleLocation(_playerObjectList[_currentPlayerIndex]._showHole);
        UpdateActivePlayerState(ActivePlayerState.Aiming);
    }

    public static void HandleStrokeOver() // LOLOLOLOLOLOLOLOLOL
    {
        if (_playersInHole >= _playerPrefabList.Count)
        {
            print("everyone is here.");
            return;
        }
        _currentPlayerIndex++;
        if (_playerList.Count < _playerPrefabList.Count)
        {
            GameObject newPlayer = Instantiate(_playerPrefabList[_playerList.Count], _spawnPoint.position, _spawnPoint.rotation);
            _playerList.Add(newPlayer);
            _playerPrefab = _playerList[_currentPlayerIndex];
            InitPlayer(_playerPrefab);
            return;
        }
        if (_currentPlayerIndex >= _playerList.Count) _currentPlayerIndex = 0;
        if (_playerObjectList[_currentPlayerIndex].playerState == PlayerState.InHole)
        {
            HandleStrokeOver();
        };
        _playerPrefab = _playerList[_currentPlayerIndex];
        InitPlayer(_playerPrefab);

    }
    public static void UpdateGameState(GameState newState)
    {
        gameState = newState;
        switch (newState)
        {case GameState.Playing:
            break;
        case GameState.Paused:
            break;
        case GameState.CourseComplete:
            break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    public static void UpdateActivePlayerState(ActivePlayerState newState)
    {
        activePlayerState = newState;
        switch (newState)
        {
            case ActivePlayerState.Aiming:
                if (_playerObjectList[_currentPlayerIndex].playerState == PlayerState.InHole) return;
                _playerObjectList[_currentPlayerIndex].UpdatePlayerState(PlayerState.Aiming);
                ShotController.ResetArrow();
                break;
            case ActivePlayerState.MidAir:
                shotTaken = Time.time;
                if (_playerObjectList[_currentPlayerIndex].playerState == PlayerState.InHole) return;
                _playerObjectList[_currentPlayerIndex].UpdatePlayerState(PlayerState.MidAir);
                break;
            case ActivePlayerState.OnGround:
                if (_playerObjectList[_currentPlayerIndex].playerState == PlayerState.InHole) return;
                _playerObjectList[_currentPlayerIndex].UpdatePlayerState(PlayerState.OnGround);
                break;
            case ActivePlayerState.InHole:
                _playerObjectList[_currentPlayerIndex].UpdatePlayerState(PlayerState.InHole);
                HandleInHole();
                break;
            case ActivePlayerState.AfterStroke:
                break;
        }
        OnActivePlayerStateChanged?.Invoke(newState);
    }

    public static int TakeStroke()
    {
        _playerObjectList[_currentPlayerIndex]._strokeCount++;
        return _currentStroke;
    }

    private static void HandleInHole()
    {
        _playersInHole++;
        if (_playersInHole >= _playerPrefabList.Count)
        {
            print("everyone is here.");
            return;
        }
        HandleStrokeOver();
    }

    public static void ToggleHoleLocation()
    {
        _playerObjectList[_currentPlayerIndex]._showHole = !_playerObjectList[_currentPlayerIndex]._showHole;
        HoleLocation.ToggleLocation(_playerObjectList[_currentPlayerIndex]._showHole);
    }

    private static void SetHoleLocation(bool show)
    {
        HoleLocation.ToggleLocation(show);
    }
    
    private static void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}

public enum GameState
{
    Playing,
    CourseComplete,
    Paused
}

public enum ActivePlayerState
{
    Aiming,
    MidAir,
    OnGround,
    InHole,
    AfterStroke,
}
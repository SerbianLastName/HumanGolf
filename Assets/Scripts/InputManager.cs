using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    private static InputSystemHumanGolf _inputSystemHumanGolf;
    private static bool _isPaused;

    private void Awake()
    {
        instance = this;
        _inputSystemHumanGolf = new InputSystemHumanGolf();
        _inputSystemHumanGolf.Player.Action.started += ctx => HandleActionButtonDown();
        _inputSystemHumanGolf.Player.Action.canceled += ctx => HandleActionButtonUp();
        _inputSystemHumanGolf.Player.Start.started += ctx => HandleStartButtonDown();
        _inputSystemHumanGolf.Player.Select.started += ctx => HandleSelectButtonDown();
        _inputSystemHumanGolf.Player.ShowHole.started += ctx => HandleShowButtonDown();
        _inputSystemHumanGolf.Player.Focus.started += ctx => HandleFocusButtonDown();
        _inputSystemHumanGolf.Player.Focus.canceled += ctx => HandleFocusButtonUp();

    }
    private void OnEnable()
    {
        _inputSystemHumanGolf.Enable();
    }

    private void OnDisable()
    {
        _inputSystemHumanGolf.Disable();
    }

    private void FixedUpdate()
    {
        HandleDirectionInput();
    }

    private static void HandleDirectionInput()
    {
        Vector2 input = _inputSystemHumanGolf.Player.Move.ReadValue<Vector2>();
        switch (GameManager.activePlayerState)
        {
            case ActivePlayerState.Aiming:
                ShotController.HandleDirectionInput(input);
                break;
            case ActivePlayerState.MidAir:
                PlayerPhysicsController.HandleMotion(input);
                break;
        }
    }

    private static void HandleActionButtonDown()
    {
        if (GameManager.activePlayerState == ActivePlayerState.Aiming) ShotController.StartCharging();
    }

    private static void HandleActionButtonUp()

    {
        if (GameManager.activePlayerState == ActivePlayerState.Aiming) ShotController.ReleaseCharge();
    }

    private static void HandleStartButtonDown()
    {
        if (GameManager.gameState != GameState.Paused)
        {
            Time.timeScale = 0;
            GameManager.UpdateGameState(GameState.Paused);
            return;
        }
        Time.timeScale = 1;
        GameManager.UpdateGameState(GameState.Playing);
    }

    private static void HandleSelectButtonDown()
    {
        if (GameManager.gameState != GameState.Paused) return;
        {
            GameManager.UpdateGameState(GameState.Playing);
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    private static void HandleShowButtonDown()
    {
        if (GameManager.gameState == GameState.Paused) return;
        GameManager.ToggleHoleLocation();
    }

    private static void HandleFocusButtonDown()
    {
        ShotController.inFocus = true;
    }
    private static void HandleFocusButtonUp()
    {
        ShotController.inFocus = false;
    }
}

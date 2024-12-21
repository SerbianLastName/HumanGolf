using UnityEngine;
using Unity.Cinemachine;

public class ShotController : MonoBehaviour
{
    // INSTANCE
    public static ShotController instance;
    // SERIALIZED // FOR NOW!!!!
    private static Transform arrow;
    private static MeshRenderer arrowRenderer;
    private static Rigidbody followTarget;
    private static PlayerObject _playerObject;
    [SerializeField] CinemachineCamera followCamera;
    // GAME OBJECTS
    private static Transform _arrow;
    private static MeshRenderer _arrowRenderer;
    private static Rigidbody _followTarget;
    private static CinemachineCamera _followCamera;
    private InputSystemHumanGolf _inputSystemHumanGolf;
    // GAME LOGIC
    private static float currentPower;
    private static bool isCharging;
    private static bool reverseCharge;
    private static float verticalAngle;
    public static float shotTakenAt;
    public static bool inFocus;
    // public static bool _inGreen;
    // CONSTANTS
    private const float MAX_POWER = 4000f;
    private const float CHARGE_FACTOR = 1.5f;
    private const float GREEN_CHARGE_FACTOR = 0.25f;
    private const float VERTICAL_AIM_LIMIT = 75f;
    private const float ARROW_RESET_HEIGHT = 0.5f;
    private static readonly int FILL_AMOUNT = Shader.PropertyToID("_fillAmount");

    private void Awake()
    {
        instance = this;
        _followCamera = followCamera;
    }

    private void Start()
    {
        _followCamera.Follow = _arrow.transform;
    }
    private void Update()
    {
        if (GameManager.activePlayerState == ActivePlayerState.Aiming)
        {
            HandlePowerCharge();
        }
    }

    public static void InitShotController(
        // Transform _arrowTransform,
        // MeshRenderer _arrowMeshRenderer,
        // Rigidbody _rbFollowTarget,
        PlayerObject _player)
    {
        _arrow = _player._arrowTransform;
        _arrowRenderer = _player._arrowRenderer;
        _followTarget = _player._pelvisRigidbody;
        _playerObject = _player;
        inFocus = false;
    }
    public static void HandleDirectionInput(Vector2 input)
    {
        _arrow.Rotate(Vector3.up, input.x * 100f * Time.deltaTime);
        verticalAngle -= input.y * 100f * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle, -VERTICAL_AIM_LIMIT, 0);
        _arrow.localRotation = Quaternion.Euler(verticalAngle, _arrow.localRotation.eulerAngles.y, 0f);
    }
    private void HandlePowerCharge()
    {
        float powerChargeSpeed = ChargeSpeed();
        if (isCharging && !reverseCharge && currentPower >= MAX_POWER)
        {
            reverseCharge = true;
        }

        if (isCharging && reverseCharge && currentPower <= 0)
        {
            reverseCharge = false;
        }

        
        switch (isCharging)
        {
            case true when !reverseCharge:
                currentPower += powerChargeSpeed * Time.deltaTime;
                currentPower = Mathf.Clamp(currentPower, 0f, MAX_POWER);
                UpdatePowerMeterUI(currentPower / MAX_POWER);
                break;
            case true when reverseCharge:
                currentPower -= powerChargeSpeed * Time.deltaTime;
                currentPower = Mathf.Clamp(currentPower, 0f, MAX_POWER);
                UpdatePowerMeterUI(currentPower / MAX_POWER);
                break;
        }
    }

    public static void StartCharging()
    {
        isCharging = true;
        currentPower = 0f;
        UpdatePowerMeterUI(0f);
    }
    public static void ReleaseCharge()
    {
        if (!isCharging) return;
        {
            GameManager.TakeStroke();
            ApplyForce(currentPower);
            GameManager.UpdateActivePlayerState(ActivePlayerState.MidAir);
            UpdatePowerMeterUI(0f);
            _followCamera.Follow = _followTarget.transform;
            _arrowRenderer.enabled = false;
            isCharging = false;
            currentPower = 0f;
        }
    }
    private static void ApplyForce(float power)
    {
        shotTakenAt = Time.time;
        Vector3 forceDirection = _arrow.forward;
        Vector3 force = forceDirection * power;
        PlayerPhysicsController.ApplyForceToPlayer(force);
    }
    public static void ResetArrow()
    {
        Vector3 humanPosition = _followTarget.position;
        Vector3 newPosition = new Vector3(humanPosition.x, humanPosition.y + ARROW_RESET_HEIGHT, humanPosition.z);
        _arrow.position = newPosition;
        _arrow.rotation = Quaternion.Euler(0, _followCamera.transform.rotation.eulerAngles.y, 0);
        _arrowRenderer.enabled = true;
        _followCamera.Follow = _arrow.transform;
        currentPower = 0f;
        reverseCharge = false;
        UpdatePowerMeterUI(0f);
    }
    private static void UpdatePowerMeterUI(float normalizedPower)
    {
        _arrowRenderer.material.SetFloat(FILL_AMOUNT, normalizedPower);
    }

    private static float ChargeSpeed()
    {
        float newSpeed = _playerObject._inGreen && inFocus ? MAX_POWER * GREEN_CHARGE_FACTOR : MAX_POWER * CHARGE_FACTOR;
        return newSpeed;
    }

    
}


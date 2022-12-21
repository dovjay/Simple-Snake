using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveRange = .3f;

    [SerializeField]
    private float moveDelay = .5f;
    [SerializeField]
    private float minMoveDelay = .05f;
    [SerializeField]
    private float speedBoost = 1.5f;
    [SerializeField]
    private float moveDelayReduction = 0.05f;

    [SerializeField]
    private Canvas JoystickCanvas;
    [SerializeField]
    private Canvas DPadCanvas;

    private float defaultMoveDelay;
    private float delayLeft = 0f;
    private Vector2 moveDirection = new Vector2(0f, 1f);
    private bool boostNormal = false;

    // Powerups
    private bool boostPowerUp;
    private bool inverseControl;

    private InputManager inputManager;
    private PlayerBody head;
    private GameManager gameManager;


    private void Awake() {
        inputManager = InputManager.Instance;
        head = transform.GetChild(0).GetComponent<PlayerBody>();
        gameManager = FindObjectOfType<GameManager>();

        defaultMoveDelay = moveDelay;
    }

    private void OnEnable() {
        SubscribeToEvent(true);
        GameManager.OnGameOver += StopMovement;
        GameManager.OnTimerThreshold += UpdateMoveDelay;
    }

    private void OnDisable() {
        SubscribeToEvent(false);
        GameManager.OnGameOver -= StopMovement;
        GameManager.OnTimerThreshold -= UpdateMoveDelay;
    }

    private void SubscribeToEvent(bool isEnable) {
        if (!isEnable) {
            inputManager.OnBoost -= BoostMovement;

            SwipeDetection.OnSwipe -= ChangeMoveDirection;
            
            JoystickController.OnMove -= ChangeMoveDirection;
            JoystickCanvas.gameObject.SetActive(false);

            DPadController.OnMove -= ChangeMoveDirection;
            DPadCanvas.gameObject.SetActive(false);
            return;
        }

        inputManager.OnBoost += BoostMovement;

        switch (inputManager.controlOption)
        {
            case ControlOption.Swipe:
                SwipeDetection.OnSwipe += ChangeMoveDirection;
                break;
            case ControlOption.Joystick:
                JoystickCanvas.gameObject.SetActive(true);
                JoystickController.OnMove += ChangeMoveDirection;
                break;
            case ControlOption.DPad:
                DPadCanvas.gameObject.SetActive(true);
                DPadController.OnMove += ChangeMoveDirection;
                break;
            default:
                Debug.LogError("You should choose one control option!");
                break;
        }
    }

    private void Update() {
        if (gameManager.isPaused) return;
        if (gameManager.isGameOver) return;
        
        if (delayLeft <= 0f) {
            MovePlayer();
            delayLeft = moveDelay;
        } else {
            delayLeft -= Time.deltaTime;
        }
    }

    public void BoostPowerUp(bool active) {
        boostPowerUp = active;
        moveDelay = active ? defaultMoveDelay / 3f : defaultMoveDelay;
    }

    private void BoostMovement(bool isOnHold) {
        if (boostPowerUp) return;

        boostNormal = isOnHold;

        if (isOnHold) moveDelay = defaultMoveDelay / speedBoost;
        else moveDelay = defaultMoveDelay;
    }

    public void InverseControlPowerUp(bool active) => inverseControl = active;

    private void UpdateMoveDelay() {
        defaultMoveDelay -= moveDelayReduction;

        if (boostPowerUp || boostNormal)
            moveDelay = Mathf.Max(defaultMoveDelay, minMoveDelay) / speedBoost;
        else
            moveDelay = Mathf.Max(defaultMoveDelay, minMoveDelay);
    }

    private void ChangeMoveDirection(Vector2 v) {
        if (inverseControl) v *= -1;
        if (v == -head.GetDirection() || v == Vector2.zero) return;
        moveDirection = v;
    }

    private void StopMovement() => enabled = false;

    private void MovePlayer() {
        MoveHead();
        MoveBody();

        if (DeadlyBoundary()) {
            ChangeBodySprite();
            return;
        }

        MoveBodyOutsideBoundary();

        ChangeBodySprite();
    }

    private void MoveBodyOutsideBoundary() {
        for (int i = 0; i < transform.childCount; i++) {
            PlayerBody body = transform.GetChild(i).GetComponent<PlayerBody>();
            body.MoveBodyOutsideBoundary();
        }
    }

    private bool DeadlyBoundary() {
        Head head = transform.GetChild(0).GetComponent<Head>();

        if (head.DeadlyBoundaryChecker()) {
            gameManager.TriggerGameOver();
            return true;
        }

        return false;
    }

    private void MoveHead() {
        PlayerBody firstBody = transform.GetChild(0).GetComponent<PlayerBody>();
        firstBody.SetDirection(moveDirection);
        firstBody.Move(moveRange);
    }

    private void MoveBody() {
        PlayerBody newBody = transform.GetChild(1).GetComponent<PlayerBody>();
        newBody.Move(moveRange);
        newBody.SetDirection(moveDirection);

        PlayerBody lastBody = newBody;

        for (int i = 2; i < transform.childCount; i++) {
            newBody = transform.GetChild(i).GetComponent<PlayerBody>();
            newBody.Move(moveRange);

            if (newBody.GetDirection() != lastBody.GetPrevDirection()) {
                newBody.SetDirection(lastBody.GetPrevDirection());
            }

            lastBody = newBody;
        }
    }

    private void ChangeBodySprite() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            PlayerBody body = transform.GetChild(i).GetComponent<PlayerBody>();
            body.ChangeBodySprite();
            body.FoodProcess();
        }
    }
}

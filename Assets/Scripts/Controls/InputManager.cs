using UnityEngine;
using UnityEngine.InputSystem;
using System;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    #region Events
    public event Action<Vector2, float> OnStartTouch;
    public event Action<Vector2, float> OnEndTouch;
    public event Action<bool> OnBoost;
    public event Action<Vector2> OnMoveJoystick;

    #endregion

    public TouchControl touchControl;
    public ControlOption controlOption;

    private Camera mainCamera;

    private void Awake() {
        touchControl = new TouchControl();
        mainCamera = Camera.main;
        controlOption = (ControlOption)PlayerPrefs.GetInt(OptionKey.CONTROL_OPTION, 0);
    }

    private void OnEnable() {
        touchControl.Enable();
    }

    private void OnDisable() {
        touchControl.Disable();
    }

    void Start()
    {
        touchControl.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        touchControl.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

        touchControl.Joystick.Move.performed += ctx => MoveJoystick(ctx);

        // BOOST FUNCTIONALITY
        if (controlOption == ControlOption.Swipe) {
            touchControl.Touch.PrimaryHold.performed += ctx => OnBoost?.Invoke(true);
            touchControl.Touch.PrimaryHold.canceled += ctx => OnBoost?.Invoke(false);
        } 
        else if (
            controlOption == ControlOption.Joystick ||
            controlOption == ControlOption.DPad
        ) {
            touchControl.Joystick.Boost.performed += ctx => OnBoost?.Invoke(true);
            touchControl.Joystick.Boost.canceled += ctx => OnBoost?.Invoke(false);
        }
    }

    // SWIPE FUNCTIONALITY
    private void StartTouchPrimary(InputAction.CallbackContext context) {
        OnStartTouch?.Invoke(Utils.ScreenToWorld(mainCamera, touchControl.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext context) {
        OnEndTouch?.Invoke(Utils.ScreenToWorld(mainCamera, touchControl.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);
    }

    private void MoveJoystick(InputAction.CallbackContext context) {
        OnMoveJoystick?.Invoke(context.ReadValue<Vector2>());
    }

    public Vector2 PrimaryPosition() {
        return Utils.ScreenToWorld(mainCamera, touchControl.Touch.PrimaryPosition.ReadValue<Vector2>());
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static CameraControlActions;

[CreateAssetMenu(menuName = "CameraInputReader")]
public class CameraInputReader : ScriptableObject, ICameraActions {

    private CameraControlActions camActions;
    private bool rotationStarted; //Whether user started rotating
    private bool clickedOnLeft => Mouse.current.position.ReadValue().x < Screen.width / 2; //Which side did he click

    void OnEnable() {
        if(camActions == null) {
            camActions = new CameraControlActions();

            camActions.Camera.SetCallbacks(this);
            SetCamera();
        }
    }

    private void OnDisable() {
        camActions.Disable();
    }

    private void SetCamera() {
        camActions.Camera.Enable();
    }

    //Two separate events for reading mouse movement on the left side and right side of screen
    public event Action<Vector2> MouseMoveOnLeftEvent;
    public event Action<Vector2> MouseMoveOnRightEvent;

    public event Action<float> ZoomLeftEvent;
    public event Action<float> ZoomRightEvent;

    public void OnRotationStarted(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed)
            rotationStarted = true;
        else if (context.phase == InputActionPhase.Canceled)
            rotationStarted = false;
    }

    public void OnRotation(InputAction.CallbackContext context) {
        if (!rotationStarted)
            return;
        if (clickedOnLeft)
            MouseMoveOnLeftEvent.Invoke(context.ReadValue<Vector2>());
        else
            MouseMoveOnRightEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnZoom(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            if (clickedOnLeft)
                ZoomLeftEvent.Invoke(context.ReadValue<Vector2>().y);
            else
                ZoomRightEvent.Invoke(context.ReadValue<Vector2>().y);
        }
    }
}

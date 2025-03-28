using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static CameraControlActions;

[CreateAssetMenu(menuName = "CameraInputReader")]
public class CameraInputReader : ScriptableObject, ICameraActions {

    private CameraControlActions camActions;
    private bool rotationStarted; //Whether user started rotating
    private CameraSide camSide => Input.mousePosition.x < Screen.width / 2 ? CameraSide.Left : CameraSide.Right; //Which side did he click

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

    public event Action<CameraSide> MousePressedEvent;
    public event Action MouseReleasedEvent;

    public void OnRotationStarted(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed)
            rotationStarted = true;
        else if (context.phase == InputActionPhase.Canceled)
            rotationStarted = false;
    }

    public void OnRotation(InputAction.CallbackContext context) {
        if (!rotationStarted)
            return;
        if (camSide == CameraSide.Left)
            MouseMoveOnLeftEvent.Invoke(context.ReadValue<Vector2>());
        else
            MouseMoveOnRightEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnZoom(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Performed) {
            if (camSide == CameraSide.Left)
                ZoomLeftEvent.Invoke(context.ReadValue<Vector2>().y);
            else
                ZoomRightEvent.Invoke(context.ReadValue<Vector2>().y);
        }
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            MousePressedEvent.Invoke(camSide);
        else if (context.phase == InputActionPhase.Canceled)
            MouseReleasedEvent.Invoke();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static ControlActions;

[CreateAssetMenu(menuName = "InputReader")]
public class InputReader : ScriptableObject, ICameraActions {

    private ControlActions controlActions;
    private bool rotationStarted; //Whether user started rotating
    private CameraSide camSide => Input.mousePosition.x < Screen.width / 2 ? CameraSide.Left : CameraSide.Right; //Which side did he click

    void OnEnable() {
        if(controlActions == null) {
            controlActions = new ControlActions();

            controlActions.Camera.SetCallbacks(this);
            SetActions();
        }
    }

    private void OnDisable() {
        controlActions.Disable();
    }

    private void SetActions() {
        controlActions.Camera.Enable();
    }

    //Two separate events for reading mouse movement on the left side and right side of screen
    public event Action<Vector2> MouseMoveOnLeftEvent;
    public event Action<Vector2> MouseMoveOnRightEvent;

    public event Action<float> ZoomLeftEvent;
    public event Action<float> ZoomRightEvent;

    public event Action<CameraSide> MousePressedEvent;
    public event Action MouseReleasedEvent;

    //UI events
    public event Action<Semaphore> UILeftMouseClick;

    //Camera inputs
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

    //UI inputs
    public void OnClickLeft(Semaphore sem) {
        UILeftMouseClick?.Invoke(sem);
    }

    public void OnClickRight() {
        
    }
}

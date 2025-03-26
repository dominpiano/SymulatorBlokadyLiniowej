using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamerAcontroller : MonoBehaviour {

    private CameraControlActions camActions;
    private Camera cam;

    [SerializeField]
    private int MaxZoom = 4;
    private int currentZoom = 0;
    private float zoomFactor; //This is used for constraining rotation in axes X and Y
    private readonly (float, float) terminalXs = (-7f, 7f); //(-6f, 8f)
    private readonly (float, float) terminalYs = (-8f, 8f); //(-3f, 13f)

    private void Awake() {
        camActions = new CameraControlActions();
        cam = GetComponent<Camera>();
    }

    private void OnEnable() {
        camActions.Camera.Look.performed += LookAround;
        camActions.Camera.Zoom.performed += Zoom;
        camActions.Camera.Enable();
    }

    private void OnDisable() {
        camActions.Camera.Look.performed -= LookAround;
        camActions.Camera.Zoom.performed -= Zoom;
        camActions.Disable();
    }

    private void LookAround(InputAction.CallbackContext inputValue) {
        if (!Mouse.current.rightButton.isPressed)
            return;
        Vector2 value = inputValue.ReadValue<Vector2>() * 0.02f; 
        value.y *= -1f; //Invering axis
        //X axis of mouse is Y axis of rotation of the camera and vice-versa
        float currRotX = transform.localRotation.eulerAngles.y - (transform.localRotation.eulerAngles.y > 180f ? 360f : 0f);
        float currRotY = transform.localRotation.eulerAngles.x - (transform.localRotation.eulerAngles.x > 180f ? 360f : 0f);

        //That +1 is from centering in order to properly create boundaries
        //zoomFactor = 0 - zero movement
        //zoomFactor = 0.33 - from -7*0.33+1 to 7*0.33+1 etc.
        if (zoomFactor == 0)
            return;
        if((currRotX > terminalXs.Item1 * zoomFactor + 1f || value.x > 0) && (currRotX < terminalXs.Item2 * zoomFactor + 1f || value.x < 0))
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, value.x + transform.localRotation.eulerAngles.y, 0f);

        if ((currRotY > terminalYs.Item1 * zoomFactor + 5f || value.y > 0) && (currRotY < terminalYs.Item2 * zoomFactor + 5f || value.y < 0))
            transform.localRotation = Quaternion.Euler(value.y + transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 0f);
    }

    private void Zoom(InputAction.CallbackContext inputValue) {
        int direction = inputValue.ReadValue<Vector2>().y > 0 ? 1 : -1;
        if(direction == 1 && currentZoom < MaxZoom) {
            cam.fieldOfView -= 5;
            currentZoom++;
        }
        else if(direction == -1 && currentZoom != 0) {
            cam.fieldOfView += 5;
            currentZoom--;
            //Basic view
            if(currentZoom == 0) {
                transform.localRotation = Quaternion.Euler(5f, 0f, 0f);
            }
        }
        zoomFactor = 1f / MaxZoom * currentZoom;
    }
}

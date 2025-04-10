using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamerAcontroller : MonoBehaviour {

    [SerializeField]
    private InputReader inputReader;
    private Camera cam;

    [SerializeField]
    private int CameraNumber; //1 - left camera, 2 - right camera

    [SerializeField]
    private int MaxZoom = 3;
    private int currentZoom = 0;
    private float zoomFactor; //This is used for constraining rotation in axes X and Y
    private readonly (float, float) terminalXs = (-7f, 7f); //(-6f, 8f)
    private readonly (float, float) terminalYs = (-8f, 8f); //(-3f, 13f)

    private void Awake() {
        cam = GetComponent<Camera>();
    }

    //We have to cameras, so if it's camera one we assign OnLookAround only to react to MouseMoveLeft and ZoomLeft events
    private void OnEnable() {
        if (CameraNumber == 1) {
            inputReader.MouseMoveOnLeftEvent += OnLookAround;
            inputReader.ZoomLeftEvent += OnZoom;
        }
        else if (CameraNumber == 2) {
            inputReader.MouseMoveOnRightEvent += OnLookAround;
            inputReader.ZoomRightEvent += OnZoom;
        }
        
    }

    private void OnDisable() {
        if (CameraNumber == 1) {
            inputReader.MouseMoveOnLeftEvent -= OnLookAround;
            inputReader.ZoomLeftEvent -= OnZoom;
        }
        else if (CameraNumber == 2) {
            inputReader.MouseMoveOnRightEvent -= OnLookAround;
            inputReader.ZoomRightEvent -= OnZoom;
        }
        
    }

    private void OnLookAround(Vector2 inputValue) {
        
        Vector2 value = inputValue * 0.02f; 
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

    private void OnZoom(float inputValue) {
        int direction = inputValue > 0 ? 1 : -1;
        if(direction == 1 && currentZoom < MaxZoom)
            currentZoom++;
        else if(direction == -1 && currentZoom != 0) {
            currentZoom--;
            //Basic view
            if(currentZoom == 0) {
                transform.DOLocalRotate(new Vector3(5f, 0f, 0f), 0.1f);
            }
        }
        cam.DOFieldOfView(20 - currentZoom * 5, 0.1f);
        zoomFactor = 1f / MaxZoom * currentZoom;
    }
}

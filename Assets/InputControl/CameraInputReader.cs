using UnityEngine;
using UnityEngine.InputSystem;
using static CameraControlActions;

[CreateAssetMenu(menuName = "CameraInputReader")]
public class CameraInputReader : ScriptableObject, ICameraActions {

    private CameraControlActions camActions;

    void OnEnable() {
        
        if(camActions == null) {
            camActions = new CameraControlActions();

            camActions.Camera.SetCallbacks(this);
        }

    }

    public void OnLook(InputAction.CallbackContext context) {
        
    }

    public void OnZoom(InputAction.CallbackContext context) {
        
    }
}

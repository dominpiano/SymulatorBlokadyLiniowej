using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private CameraInputReader inputReader;

    //Global signalbox
    [SerializeField]
    private SignalboxController signalBox;

    //Klawisz things
    private bool klawiszPressed;
    private Animator TempDevicesAnimator;

    LayerMask layerMask;

    private void Awake() {
        layerMask = LayerMask.GetMask("SignalboxDevices");
    }

    private void OnEnable() {
        inputReader.MousePressedEvent += OnMousePressed;
        inputReader.MouseReleasedEvent += OnMouseReleased;
    }

    private void OnDisable() {
        inputReader.MousePressedEvent -= OnMousePressed;
        inputReader.MouseReleasedEvent -= OnMouseReleased;
    }

    private void Update() {
        
    }

    private void OnMousePressed(CameraSide camSide) {

        //Get cameras
        Camera leftCam = Camera.allCameras.Where(cam => cam.name == "Main Camera").First();
        Camera rightCam = Camera.allCameras.Where(cam => cam.name == "Second Camera").First();

        //Clicked on the left side
        if (camSide == CameraSide.Left) {
            if (Physics.Raycast(leftCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, layerMask)) {
                if (hit.collider.gameObject.name.ContainsInsensitive("klawisz")) {
                    TempDevicesAnimator = hit.collider.gameObject.GetComponentInParent<Animator>();
                    TempDevicesAnimator.SetBool("KlawiszDown", true);
                    klawiszPressed = true;

                    //Tarczka
                    
                }
            }
        }
        else if(camSide == CameraSide.Right) {

        }
        
    }

    private void OnMouseReleased() {
        //Klawisz blokowy
        if (klawiszPressed) {
            TempDevicesAnimator.SetBool("KlawiszDown", false);
            klawiszPressed = true;
        }
    }
}

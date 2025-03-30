using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private CameraInputReader inputReader;

    //Signalbox segments
    private SignalboxSegment signalboxSegment;
    
    LayerMask layerMask;

    private void Start() {
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
                    //Getting segment
                    signalboxSegment = hit.collider.gameObject.GetComponentInParent<SignalboxSegment>();

                    signalboxSegment.SegmentAnimator.SetBool("KlawiszDown", true);
                    signalboxSegment.KlawiszDown = true;

                    //Tarczka
                    signalboxSegment.SegmentAnimator.SetFloat("TarczkaSpeed", 1);
                    if (!signalboxSegment.Tarczka.TarczkaState)
                        signalboxSegment.SegmentAnimator.SetBool("TarczkaUp", true);
                    else if (signalboxSegment.Tarczka.TarczkaState) 
                        signalboxSegment.SegmentAnimator.SetBool("TarczkaUp", false);
                    
                }
            }
        }
        else if(camSide == CameraSide.Right) {

        }
        
    }

    private void OnMouseReleased() {
        //Klawisz blokowy
        if (signalboxSegment) {
            if (signalboxSegment.KlawiszDown) {
                signalboxSegment.SegmentAnimator.SetBool("KlawiszDown", false);
                signalboxSegment.KlawiszDown = true;
            }

            signalboxSegment.SegmentAnimator.SetFloat("TarczkaSpeed", 0);
        }
    }
}

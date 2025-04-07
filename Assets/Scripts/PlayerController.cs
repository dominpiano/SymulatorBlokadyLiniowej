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

        if (camSide == CameraSide.Left) {
            SignalboxAction(1, leftCam);
        }
        else if(camSide == CameraSide.Right) {
            SignalboxAction(2, rightCam);
        }
    }

    //Main logic
    private void SignalboxAction(int sbNum, Camera cam) {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, layerMask)) {
            //Klawisz
            if (hit.collider.gameObject.name.ContainsInsensitive("klawisz")) {
                //Getting segment
                signalboxSegment = hit.collider.gameObject.GetComponentInParent<SignalboxSegment>();

                SignalboxController controller = signalboxSegment.GetComponentInParent<SignalboxController>();
                switch (signalboxSegment.Type) {
                    case BlockType.Poz:
                        if (controller.Poz.BlockState && controller.Po.BlockState && controller.Ko.BlockState) {
                            signalboxSegment.SegmentAnimator.SetBool("KlawiszDown", true);
                            signalboxSegment.KlawiszDown = true;
                            StationConnection.Instance.AnimatePozChange(sbNum, signalboxSegment);
                        }
                        else {
                            signalboxSegment.SegmentAnimator.SetBool("KlawiszLockedDown", true);
                            signalboxSegment.KlawiszLockedDown = true;
                        }
                        break;
                    case BlockType.Po:
                        //We can block Po only if there is NO train on the line and we set the signal for the train and Po is unlocked
                        //and we have permission
                        if (!StationConnection.Instance.TrainOnLine && controller.Po.BlockState && controller.Poz.BlockState) {
                            signalboxSegment.SegmentAnimator.SetBool("KlawiszDown", true);
                            signalboxSegment.KlawiszDown = true;
                            StationConnection.Instance.AnimatePoChange(sbNum, signalboxSegment);

                            //Train goes on the line
                            StationConnection.Instance.TrainOnLine = true;
                        }
                        else {
                            signalboxSegment.SegmentAnimator.SetBool("KlawiszLockedDown", true);
                            signalboxSegment.KlawiszLockedDown = true;
                        }
                        break;
                    case BlockType.Ko:
                        //We can block Ko only if there IS train on the line and the train arrived to station and Ko unlocked
                        if (StationConnection.Instance.TrainOnLine && !controller.Ko.BlockState && !controller.Poz.BlockState) {
                            signalboxSegment.SegmentAnimator.SetBool("KlawiszDown", true);
                            signalboxSegment.KlawiszDown = true;
                            StationConnection.Instance.AnimateKoChange(sbNum, signalboxSegment);

                            //Train arrives from the line
                            StationConnection.Instance.TrainOnLine = false;
                        }
                        else {
                            signalboxSegment.SegmentAnimator.SetBool("KlawiszLockedDown", true);
                            signalboxSegment.KlawiszLockedDown = true;
                        }
                        break;
                }
            }
         
            //Dorazny odblok zastawki

        }
    }

    private void OnMouseReleased() {
        //Klawisz blokowy
        if (signalboxSegment) {
            if (signalboxSegment.KlawiszDown) {
                signalboxSegment.SegmentAnimator.SetBool("KlawiszDown", false);
                signalboxSegment.KlawiszDown = false;
            } else if (signalboxSegment.KlawiszLockedDown) {
                signalboxSegment.SegmentAnimator.SetBool("KlawiszLockedDown", false);
                signalboxSegment.KlawiszLockedDown = false;
            }
            StationConnection.Instance.PauseAnimatingBlockChange(signalboxSegment);
        }
    }
}

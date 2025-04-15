using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIReader : MonoBehaviour {

    [SerializeField]
    private GraphicRaycaster raycaster;
    [SerializeField]
    private EventSystem eventSystem;
    [SerializeField]
    private InputReader inputReader;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            PointerEventData pointerData = new PointerEventData(eventSystem) {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            //Cycle through all raycasted objects
            foreach (RaycastResult result in results) {

                //Pick clicked semaphore and send it to master input reader
                if (result.gameObject.name.ContainsInsensitive("Sem")) {
                    inputReader.OnClickLeft(result.gameObject.GetComponent<Semaphore>());
                    return;
                }
                continue;
            }
        }
    }

}

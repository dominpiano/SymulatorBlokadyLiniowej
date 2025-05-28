using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StationManager : MonoBehaviour {

    [SerializeField]
    private InputReader inputReader;

    //Sem context menu
    [SerializeField]
    private UIDocument semContextMenuDocument;
    private GameObject semContextMenu;
    private VisualElement semContextMenuContainer;
    private Button allowSignal;
    private Button substituteSignal;
    private Button releaseSignal;

    [SerializeField]
    private List<Semaphore> semaphores;

    private Semaphore selectedSemaphore;

    private void Awake() {
        inputReader.UILeftMouseClickSem += OnUILeftMouseClickSem;
        inputReader.UILeftMouseClick += OnUILeftMouseClick;

        //Get buttons from semaphore context menu
        semContextMenu = semContextMenuDocument.gameObject;
        semContextMenuContainer = semContextMenuDocument.rootVisualElement.Q("Container");
        allowSignal = semContextMenuDocument.rootVisualElement.Q("SygZezwalajacy") as Button;
        substituteSignal = semContextMenuDocument.rootVisualElement.Q("SygZastepczy") as Button;
        releaseSignal = semContextMenuDocument.rootVisualElement.Q("SygDorZw") as Button;

        allowSignal.RegisterCallback<ClickEvent>(PodajSygnalZezwalajacy);
    }

    private void OnUILeftMouseClickSem(Semaphore sem) {
        semContextMenuContainer.style.visibility = Visibility.Visible;
        semContextMenuContainer.style.left = Input.mousePosition.x;
        semContextMenuContainer.style.bottom = Input.mousePosition.y;
        selectedSemaphore = sem;
    }

    private void OnUILeftMouseClick() {
        semContextMenuContainer.style.visibility = Visibility.Hidden;
        selectedSemaphore = null; //WARNING!!!! NULLABLE!!!
    }

    //Semaphore context menu options
    private void PodajSygnalZezwalajacy(ClickEvent evt) {
        Debug.Log("clicked zezwalajacy");
    }

}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class StationManager : MonoBehaviour {

    [SerializeField] private InputReader inputReader;

    [SerializeField] private GameObject trainCanvas;
    [SerializeField] private GameObject trainPrefab;

    //Sem context menu
    [SerializeField] private UIDocument semContextMenuDocument;
    private VisualElement semContextMenuContainer;
    private Button allowSignalButton;
    private Button substituteSignalButton;
    private Button releaseSignalButton;

    //Main Station View
    private UIDocument stationDocument;
    private VisualElement selectedSemaphoreElement;
    private Button spawnTrainButton;
    private GameObject currentTrain;

    private void Start() {

        //Initialize all semaphores
        RouteChecker.Initialize();

        //Get buttons from semaphore context menu
        semContextMenuContainer = semContextMenuDocument.rootVisualElement.Q("Container");
        allowSignalButton = semContextMenuDocument.rootVisualElement.Q("SygZezwalajacy") as Button;
        substituteSignalButton = semContextMenuDocument.rootVisualElement.Q("SygZastepczy") as Button;
        releaseSignalButton = semContextMenuDocument.rootVisualElement.Q("SygDorZw") as Button;

        allowSignalButton.RegisterCallback<ClickEvent>(PodajSygnalZezwalajacy);
        substituteSignalButton.RegisterCallback<ClickEvent>(PodajSygnalZastepczy);
        releaseSignalButton.RegisterCallback<ClickEvent>(ZwolnijSygnal);


        //Get all stuff from station
        stationDocument = GetComponent<UIDocument>();
        stationDocument.rootVisualElement.RegisterCallback<PointerDownEvent>(OnUILeftMouseClick);
        spawnTrainButton = stationDocument.rootVisualElement.Q("SpawnTrainButton") as Button;
        spawnTrainButton.RegisterCallback<ClickEvent>(evt => SpawnTrain(evt, RouteChecker.SemE));
    }

    private void OnUILeftMouseClick(PointerDownEvent evt) {

        VisualElement clickedElement = evt.target as VisualElement;
        string semName = clickedElement.name.Contains("Sem") ? clickedElement.name : (clickedElement.hierarchy.parent.name.Contains("Sem") ? clickedElement.hierarchy.parent.name : "");
        if (semName == "") {
            semContextMenuContainer.style.visibility = Visibility.Hidden;
            selectedSemaphoreElement = null;
            return;
        }

        selectedSemaphoreElement = stationDocument.rootVisualElement.Q(semName);

        semContextMenuContainer.style.visibility = Visibility.Visible;
        semContextMenuContainer.style.left = Input.mousePosition.x;
        semContextMenuContainer.style.bottom = Input.mousePosition.y;
    }

    //Semaphore context menu options
    private void PodajSygnalZezwalajacy(ClickEvent evt) {
        int iloscKomor = int.Parse(Regex.Replace(selectedSemaphoreElement.name, @"\D", ""));
        Semaphore selSem = selectedSemaphoreElement.userData as Semaphore;

        //Logic for checking routes
        if (selSem.IsLocked) {
            semContextMenuContainer.style.visibility = Visibility.Hidden;
            return;
        }

        switch (iloscKomor) {
            case 3:
                ChangeLight(selectedSemaphoreElement, SemaphoreSignal.S2);
                break;
            case 4:
                ChangeLight(selectedSemaphoreElement, SemaphoreSignal.S10);
                break;
            case 5:
                ChangeLight(selectedSemaphoreElement, SemaphoreSignal.S5);
                break;
        }
        selSem.State = SemaphoreState.Go;
        semContextMenuContainer.style.visibility = Visibility.Hidden;
    }

    //TODO: implement!
    private void PodajSygnalZastepczy(ClickEvent evt) {
        
        semContextMenuContainer.style.visibility = Visibility.Hidden;
    }

    private void ZwolnijSygnal(ClickEvent evt) {
        ChangeLight(selectedSemaphoreElement, SemaphoreSignal.S1);
        Semaphore selSem = selectedSemaphoreElement.userData as Semaphore;
        selSem.State = SemaphoreState.Stop;
        semContextMenuContainer.style.visibility = Visibility.Hidden;
    }

    private void ChangeLight(VisualElement semaphore, SemaphoreSignal toSignal) {
        var lights = semaphore.Children();

        switch (toSignal) {
            case SemaphoreSignal.S1:
                lights.Where(l => l.name.Contains("Red", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => !l.name.Contains("Red", StringComparison.OrdinalIgnoreCase)).ToList().ForEach(light => light.AddToClassList("semLightOff"));
                break;
            case SemaphoreSignal.S2:
                lights.Where(l => l.name.Contains("Green", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => !l.name.Contains("Green", StringComparison.OrdinalIgnoreCase)).ToList().ForEach(light => light.AddToClassList("semLightOff"));
                break;
            case SemaphoreSignal.S5:
                lights.Where(l => l.name.Contains("OrangeLight", StringComparison.OrdinalIgnoreCase) || l.name.Contains("Orange1Light", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => !(l.name.Contains("OrangeLight", StringComparison.OrdinalIgnoreCase) || l.name.Contains("Orange1Light", StringComparison.OrdinalIgnoreCase))).ToList().ForEach(light => light.AddToClassList("semLightOff"));
                break;
            case SemaphoreSignal.S10:
                if (lights.Count(l => l.name.Contains("Orange")) == 0) return;

                lights.Where(l => l.name.Contains("Green", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => l.name.Contains("OrangeLight", StringComparison.OrdinalIgnoreCase) || l.name.Contains("Orange1Light", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => !(l.name.Contains("OrangeLight", StringComparison.OrdinalIgnoreCase) || l.name.Contains("Orange1Light", StringComparison.OrdinalIgnoreCase) ||
                                    l.name.Contains("Green", StringComparison.OrdinalIgnoreCase))).ToList().ForEach(light => light.AddToClassList("semLightOff"));
                break;
        }
    }

    //Canvas and trains
    private void SpawnTrain(ClickEvent evt, Semaphore sem) {
        //If there is a train, don't make another one
        if (currentTrain) return;

        currentTrain = Instantiate(trainPrefab, trainCanvas.transform, false);
        currentTrain.name = "Train" + Guid.NewGuid().ToString().Substring(0, 5);
        currentTrain.GetComponent<RectTransform>().anchoredPosition = new Vector2(190f, 200f);
        currentTrain.GetComponent<Train>().StartSem = sem;
    }
}

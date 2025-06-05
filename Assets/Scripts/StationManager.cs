using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class StationManager : MonoBehaviour {

    //Input
    [SerializeField] private InputReader inputReader;

    //Train
    [SerializeField] private GameObject trainCanvas;
    [SerializeField] private GameObject trainPrefab;
    private Button spawnTrainEButton, spawnTrainFButton, spawnTrainCButton, spawnTrainDButton;
    private static GameObject currentTrainGO;
    private static Train currentTrain;
    //debug
    //[SerializeField] private Train trainObject;

    
    //UI TOOLKIT VISUALS
    //Main Station View
    private UIDocument stationDocument;

    //Sem context menu
    [SerializeField] private UIDocument semContextMenuDocument;
    private VisualElement semContextMenuContainer;
    private Button allowSignalButton;
    private Button substituteSignalButton;
    private Button releaseSignalButton;

    //Semaphores
    public static Dictionary<string, Semaphore> Semaphores = new Dictionary<string, Semaphore>();
    private void InitializeSemaphores() {
        //Get all semaphores
        var semaphores = new List<VisualElement>();
        var stationContainer = stationDocument.rootVisualElement.Q("Container");
        stationContainer.Query<VisualElement>().Where(el => el.name.Contains("Sem")).ToList().ForEach(sem => {
            semaphores.Add(sem);
        });
        Semaphores.Add("SemE", new Semaphore(semaphores.Find(s => s.name.Contains("SemE")), "SemE"));
        Semaphores.Add("SemF", new Semaphore(semaphores.Find(s => s.name.Contains("SemF")), "SemF"));
        Semaphores.Add("SemH", new Semaphore(semaphores.Find(s => s.name.Contains("SemH")), "SemH"));
        Semaphores.Add("SemA", new Semaphore(semaphores.Find(s => s.name.Contains("SemA")), "SemA"));
        Semaphores.Add("SemC", new Semaphore(semaphores.Find(s => s.name.Contains("SemC")), "SemC"));
        Semaphores.Add("SemD", new Semaphore(semaphores.Find(s => s.name.Contains("SemD")), "SemD"));
    }
    private VisualElement selectedSemaphoreElement;
    private Semaphore selectedSemaphore;

    private void Start() {

        //Get buttons from semaphore context menu
        semContextMenuContainer = semContextMenuDocument.rootVisualElement.Q("Container");
        allowSignalButton = semContextMenuDocument.rootVisualElement.Q("SygZezwalajacy") as Button;
        substituteSignalButton = semContextMenuDocument.rootVisualElement.Q("SygZastepczy") as Button;
        releaseSignalButton = semContextMenuDocument.rootVisualElement.Q("SygDorZw") as Button;

        allowSignalButton.RegisterCallback<ClickEvent>(PodajSygnalZezwalajacy);
        substituteSignalButton.RegisterCallback<ClickEvent>(PodajSygnalZastepczy);
        releaseSignalButton.RegisterCallback<ClickEvent>(ZwolnijSygnal);

        //Get all visual stuff from station
        stationDocument = GetComponent<UIDocument>();
        stationDocument.rootVisualElement.RegisterCallback<PointerDownEvent>(OnUILeftMouseClick);
        //Train spawning buttons
        spawnTrainEButton = stationDocument.rootVisualElement.Q("SpawnTrainE") as Button;
        spawnTrainEButton.RegisterCallback<ClickEvent>(evt => SpawnTrain(evt, Semaphores["SemE"]));
        spawnTrainFButton = stationDocument.rootVisualElement.Q("SpawnTrainF") as Button;
        spawnTrainFButton.RegisterCallback<ClickEvent>(evt => SpawnTrain(evt, Semaphores["SemF"]));
        spawnTrainCButton = stationDocument.rootVisualElement.Q("SpawnTrainC") as Button;
        spawnTrainCButton.RegisterCallback<ClickEvent>(evt => SpawnTrain(evt, Semaphores["SemC"]));
        spawnTrainDButton = stationDocument.rootVisualElement.Q("SpawnTrainD") as Button;
        spawnTrainDButton.RegisterCallback<ClickEvent>(evt => SpawnTrain(evt, Semaphores["SemD"]));

        //Initialize all semaphores
        InitializeSemaphores();

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
        selectedSemaphore = Semaphores[Regex.Replace(selectedSemaphoreElement.name, @"\d", "")];

        semContextMenuContainer.style.visibility = Visibility.Visible;
        semContextMenuContainer.style.left = Input.mousePosition.x;
        semContextMenuContainer.style.bottom = Input.mousePosition.y;
    }

    //Semaphore context menu options
    private void PodajSygnalZezwalajacy(ClickEvent evt) {
        int iloscKomor = int.Parse(Regex.Replace(selectedSemaphoreElement.name, @"\D", ""));

        //Logic for checking routes
        if (selectedSemaphore.IsLocked) {
            semContextMenuContainer.style.visibility = Visibility.Hidden;
            return;
        }

        switch (iloscKomor) {
            case 3:
                selectedSemaphore.ChangeLight(SemaphoreSignal.S2);
                break;
            case 4:
                selectedSemaphore.ChangeLight(SemaphoreSignal.S10);
                break;
            case 5:
                selectedSemaphore.ChangeLight(SemaphoreSignal.S5);
                break;
        }
        selectedSemaphore.State = SemaphoreState.Go;
        semContextMenuContainer.style.visibility = Visibility.Hidden;

        if (!currentTrain) return;

        if (selectedSemaphore == currentTrain.StartSem)
            currentTrain.StartTrain();
        if (selectedSemaphore == currentTrain.EndSem)
            currentTrain.TrainGoToEnd();

        
    }

    //TODO: implement!
    private void PodajSygnalZastepczy(ClickEvent evt) {
        
        semContextMenuContainer.style.visibility = Visibility.Hidden;
    }

    private void ZwolnijSygnal(ClickEvent evt) {
        selectedSemaphore.ChangeLight(SemaphoreSignal.S1);
        selectedSemaphore.State = SemaphoreState.Stop;
        semContextMenuContainer.style.visibility = Visibility.Hidden;
    }

    

    //Canvas and trains
    private void SpawnTrain(ClickEvent evt, Semaphore sem) {
        //If there is a train, don't make another one
        if (currentTrainGO) return;

        currentTrainGO = Instantiate(trainPrefab, trainCanvas.transform, false);
        currentTrainGO.name = "Train" + Guid.NewGuid().ToString().Substring(0, 5);

        switch (sem.Name) {
            case "SemE":
                currentTrainGO.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(190f, 200f);
                break;
            case "SemF":
                currentTrainGO.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(190f, 122f);
                break;
            case "SemC":
                currentTrainGO.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(1750f, 199f);
                break;
            case "SemD":
                currentTrainGO.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(1750f, 272f);
                break;

        }

        currentTrain = currentTrainGO.transform.GetChild(0).GetComponentInChildren<Train>();
        currentTrain.StartSem = sem;
        if (sem.Name == "SemE" || sem.Name == "SemF")
            currentTrain.EndSem = Semaphores["SemA"];
        else if (sem.Name == "SemC" || sem.Name == "SemD")
            currentTrain.EndSem = Semaphores["SemH"];
    }

    public static void DeleteTrain() {
        Destroy(currentTrainGO);
        currentTrain = null;
    }
}

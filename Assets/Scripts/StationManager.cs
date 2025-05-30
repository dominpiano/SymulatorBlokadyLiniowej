using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class StationManager : MonoBehaviour {

    [SerializeField]
    private InputReader inputReader;

    //Sem context menu
    [SerializeField]
    private UIDocument semContextMenuDocument;
    private VisualElement semContextMenuContainer;
    private Button allowSignalButton;
    private Button substituteSignalButton;
    private Button releaseSignalButton;

    //Main Station View
    private UIDocument stationDocument;

    private VisualElement selectedSemaphore;

    private void Awake() {

        //Get all stuff from station
        stationDocument = GetComponent<UIDocument>();
        stationDocument.rootVisualElement.RegisterCallback<PointerDownEvent>(OnUILeftMouseClick);

        //Get buttons from semaphore context menu
        semContextMenuContainer = semContextMenuDocument.rootVisualElement.Q("Container");
        allowSignalButton = semContextMenuDocument.rootVisualElement.Q("SygZezwalajacy") as Button;
        substituteSignalButton = semContextMenuDocument.rootVisualElement.Q("SygZastepczy") as Button;
        releaseSignalButton = semContextMenuDocument.rootVisualElement.Q("SygDorZw") as Button;

        allowSignalButton.RegisterCallback<ClickEvent>(PodajSygnalZezwalajacy);
        substituteSignalButton.RegisterCallback<ClickEvent>(PodajSygnalZastepczy);
        releaseSignalButton.RegisterCallback<ClickEvent>(ZwolnijSygnal);
    }

    private void OnUILeftMouseClick(PointerDownEvent evt) {

        VisualElement clickedElement = evt.target as VisualElement;
        string semName = clickedElement.name.Contains("Sem") ? clickedElement.name : (clickedElement.hierarchy.parent.name.Contains("Sem") ? clickedElement.hierarchy.parent.name : "");
        if (semName == "") {
            semContextMenuContainer.style.visibility = Visibility.Hidden;
            selectedSemaphore = null;
            return;
        }

        selectedSemaphore = stationDocument.rootVisualElement.Q(semName);

        semContextMenuContainer.style.visibility = Visibility.Visible;
        semContextMenuContainer.style.left = Input.mousePosition.x;
        semContextMenuContainer.style.bottom = Input.mousePosition.y;
    }

    //Semaphore context menu options
    private void PodajSygnalZezwalajacy(ClickEvent evt) {
        int iloscKomor = int.Parse(Regex.Replace(selectedSemaphore.name, @"\D", ""));

        //Logic for checking routes

        switch (iloscKomor) {
            case 3:
                ChangeLight(selectedSemaphore, SemaphoreSignal.S2);
                break;
            case 4:
                ChangeLight(selectedSemaphore, SemaphoreSignal.S10);
                break;
            case 5:
                ChangeLight(selectedSemaphore, SemaphoreSignal.S5);
                break;
        }
        semContextMenuContainer.style.visibility = Visibility.Hidden;
    }

    private void PodajSygnalZastepczy(ClickEvent evt) {
        semContextMenuContainer.style.visibility = Visibility.Hidden;
    }

    private void ZwolnijSygnal(ClickEvent evt) {
        ChangeLight(selectedSemaphore, SemaphoreSignal.S1);
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

}

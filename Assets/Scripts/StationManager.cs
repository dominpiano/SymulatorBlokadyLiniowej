using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour {

    [SerializeField]
    private InputReader inputReader;

    [SerializeField]
    private List<Semaphore> semaphores;

    private void OnEnable() {
        inputReader.UILeftMouseClick += OnUILeftMouseClick;
    }

    private void OnUILeftMouseClick(Semaphore sem) {
        if(sem.CurrentSignal == SemaphoreSignal.S1) {
            StartCoroutine(sem.LightS2());
        }
        else if(sem.CurrentSignal == SemaphoreSignal.S2) {
            StartCoroutine(sem.LightS1());
        }
    }

}

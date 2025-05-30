using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

class Semaphore {

    public SemaphoreState state;

    public Semaphore() { }
}

public class StationInitializer : MonoBehaviour {

    private UIDocument stationDocument;
    private VisualElement stationContainer;

    void Start() {
        stationDocument = GetComponent<UIDocument>();
        stationContainer = stationDocument.rootVisualElement.Q("Container");
        stationContainer.Query<VisualElement>().Where(el => el.name.Contains("Sem")).ToList().ForEach(sem => {
            sem.userData = new Semaphore();
        });
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public class StationInitializer : MonoBehaviour {

    private UIDocument stationDocument;
    private VisualElement stationContainer;

    public static List<Semaphore> semaphores = new List<Semaphore>();

    void Start() {
        stationDocument = GetComponent<UIDocument>();
        stationContainer = stationDocument.rootVisualElement.Q("Container");
        stationContainer.Query<VisualElement>().Where(el => el.name.Contains("Sem")).ToList().ForEach(sem => {
            Semaphore semData = new Semaphore(Regex.Replace(sem.name, @"\d", ""));
            semaphores.Add(semData);
            sem.userData = semData;
        });
    }
}

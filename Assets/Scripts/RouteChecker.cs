using System.Linq;
using System.Threading;
using Unity.VisualScripting;

public static class RouteChecker {

    public static Semaphore SemE, SemF, SemH, SemA, SemC, SemD;

    public static void Initialize() {
        SemE = StationInitializer.semaphores.Where(s => s.Name == "SemE").First();
        SemF = StationInitializer.semaphores.Where(s => s.Name == "SemF").First();
        SemH = StationInitializer.semaphores.Where(s => s.Name == "SemH").First();
        SemA = StationInitializer.semaphores.Where(s => s.Name == "SemA").First();
        SemC = StationInitializer.semaphores.Where(s => s.Name == "SemC").First();
        SemD = StationInitializer.semaphores.Where(s => s.Name == "SemD").First();
    }

    public static bool CheckIfLocked(Semaphore sem) {
        return sem.Name switch {
            "SemE" => SemF.IsOn || SemH.IsOn || !StationConnection.Instance.Signalbox1.Poz.BlockState || !StationConnection.Instance.Signalbox1.Po.BlockState,
            "SemF" => SemE.IsOn || SemH.IsOn || !StationConnection.Instance.Signalbox1.Poz.BlockState || !StationConnection.Instance.Signalbox1.Po.BlockState,
            "SemH" => SemE.IsOn || SemF.IsOn || StationConnection.Instance.Signalbox1.Poz.BlockState,
            _ => false,
        };
    }

}
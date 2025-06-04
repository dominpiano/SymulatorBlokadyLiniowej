using System.Linq;
using System.Threading;
using Unity.VisualScripting;

public static class RouteChecker {

    public static bool CheckIfLocked(Semaphore sem) {
        return sem.Name switch {
            "SemE" => StationManager.Semaphores["SemF"].IsOn || StationManager.Semaphores["SemH"].IsOn || !StationConnection.Instance.Signalbox1.Poz.BlockState || !StationConnection.Instance.Signalbox1.Po.BlockState,
            "SemF" => StationManager.Semaphores["SemE"].IsOn || StationManager.Semaphores["SemH"].IsOn || !StationConnection.Instance.Signalbox1.Poz.BlockState || !StationConnection.Instance.Signalbox1.Po.BlockState,
            "SemH" => StationManager.Semaphores["SemE"].IsOn || StationManager.Semaphores["SemF"].IsOn || StationConnection.Instance.Signalbox1.Poz.BlockState,
            _ => false,
        };
    }

}
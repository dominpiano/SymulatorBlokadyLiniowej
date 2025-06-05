using UnityEngine.UIElements;
using UnityEngine;

public class Train : MonoBehaviour {

	public Semaphore StartSem;
	public Semaphore EndSem;
	private Animator trainAnim;

	private void Start() {

		trainAnim = GetComponent<Animator>();
	}

    public void StartTrain() {
		trainAnim.SetTrigger("GoForward");
    }

	public void TrainGoToEnd() {
		trainAnim.SetBool("GoToEnd", true);
	}

	//Animation methods
	public void TrainPassedSemaphore(int semNum) { //There are only two - dont worry
		if (semNum == 1)
			StartSem.ChangeLight(SemaphoreSignal.S1);
		else if (semNum == 2)
			EndSem.ChangeLight(SemaphoreSignal.S1);
		else
			throw new System.Exception("There is no such number of a semaphore!");
	}

	public void ReleaseZastawka() {
		//NIE DZIALA ANIMACJA ZASTAWKI POPRAWIC - PEWNIE NIE DODALEM ANIMACJI ZASTAWKI DO DRUGIEGO SIGNALBOXA
		if (EndSem.Name == "SemA")
			StationConnection.Instance.AnimateZastawkaChange(StationConnection.Instance.Signalbox2.Ko, true);
        if (EndSem.Name == "SemH")
            StationConnection.Instance.AnimateZastawkaChange(StationConnection.Instance.Signalbox1.Ko, true);
    }

	public void TrainStopped() {
		trainAnim.SetTrigger("TrainStopped");
	}

	public void DestroyTrain() {
		StationManager.DeleteTrain();
	}
}
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

	public void TrainStopped() {
		trainAnim.SetTrigger("TrainStopped");
	}
}
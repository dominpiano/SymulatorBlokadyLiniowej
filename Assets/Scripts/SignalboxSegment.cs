using UnityEngine;

public class SignalboxSegment : MonoBehaviour {

    public Animator SegmentAnimator;
    public TarczkaBloku Tarczka;
    public bool KlawiszDown;

    private void Start() {
        SegmentAnimator = GetComponent<Animator>();
        Tarczka = GetComponentInChildren<TarczkaBloku>();
    }

    //Animation stuff
    public void ChangeTarczkaState(int state) {
        Tarczka.TarczkaState = state == 1;
    }

}

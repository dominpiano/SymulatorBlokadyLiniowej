using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour {

    private TarczkaBloku tb;

    private void Awake() {
        tb = GetComponentInParent<TarczkaBloku>();
    }

    //0 - false, 1 - true
    public void ChangeTarczkaState(int state) {
        tb.TarczkaState = state == 1;
    }

}

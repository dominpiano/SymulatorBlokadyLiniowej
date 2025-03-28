using UnityEngine;

public class TarczkaBloku : MonoBehaviour {

    /// <summary>
    /// It is the state of tarczka, true means red (locked), false means white (unlocked)
    /// </summary>
    public bool TarczkaState;

    private void Awake() {
        TarczkaState = false;
    }

}

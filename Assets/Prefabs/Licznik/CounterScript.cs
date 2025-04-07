using TMPro;
using UnityEngine;

public class CounterScript : MonoBehaviour {

    [SerializeField]
    TextMeshPro tmp;

    private int counter;
    
    public void CounterUp() {

        if(counter >= 9999) {
            counter = 0;
        }

        tmp.text = counter.ToString("D4");
        counter++;
    }

}

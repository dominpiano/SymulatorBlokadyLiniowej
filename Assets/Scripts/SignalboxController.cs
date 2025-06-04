using System.Collections.Generic;
using UnityEngine;

public class SignalboxController : MonoBehaviour {

    [SerializeField]
    public int SignalboxNumber;

    [SerializeField]
    public SignalboxSegment Po;
    [SerializeField]
    public SignalboxSegment Poz;
    [SerializeField]
    public SignalboxSegment Ko;

    public List<SignalboxSegment> Segments;

    //For our purposes - locked means red and unlocked means white.
    //In PKP locked Ko is white, and unlocked (ready to be handled) is red, but this in ONLY for Ko
    private void Start() {
        Segments.AddRange(new[] { Po, Poz, Ko });
        Po.BlockState = true; //With Po it means unlocked
        Ko.BlockState = true; //With Ko it means locked - it unlocks when turning red
    }

}

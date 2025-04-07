using System;
using UnityEngine;

public class SignalboxSegment : MonoBehaviour {

    [SerializeField]
    public BlockType Type;

    public bool BlockState;
    public Animator SegmentAnimator;
    public bool KlawiszDown;
    public bool KlawiszLockedDown;

    private void Start() {
        SegmentAnimator = GetComponent<Animator>();
    }

    //Animation stuff
    public void ChangeTarczkaState(int state) {
        BlockState = (state == 1);
    }
}

using System.Linq;
using UnityEngine;

public class StationConnection : MonoBehaviour {

    public static StationConnection Instance;

    [SerializeField]
    public SignalboxController Signalbox1;
    [SerializeField]
    public SignalboxController Signalbox2;

    public bool TrainOnLine;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(Instance);
    }

    private void Start() {
        if (!Signalbox1.Poz.BlockState) {
            Signalbox1.Poz.BlockState = true;
            //Set inital position of Poz2 to locked
            Signalbox2.Poz.SegmentAnimator.Play("Tarczka.tarczka Down", -1, 0);
            Signalbox2.Poz.SegmentAnimator.Update(0);
        }
    }

    public void AnimatePozChange(int sbNum, SignalboxSegment sbSegment) {
        SignalboxSegment sbSegMain = sbNum == 1 ? Signalbox1.Poz : Signalbox2.Poz;
        SignalboxSegment sbSegSec = sbNum == 1 ? Signalbox2.Poz : Signalbox1.Poz;

        HandleBlockAnimation(sbSegMain, sbSegSec, sbSegment);
    }

    public void AnimatePoChange(int sbNum, SignalboxSegment sbSegment) {
        SignalboxSegment sbSegMain = sbNum == 1 ? Signalbox1.Po : Signalbox2.Po;
        SignalboxSegment sbSegSec = sbNum == 1 ? Signalbox2.Ko : Signalbox1.Ko;

        HandleBlockAnimation(sbSegMain, sbSegSec, sbSegment);
    }

    public void AnimateKoChange(int sbNum, SignalboxSegment sbSegment) {
        SignalboxSegment sbSegMain = sbNum == 1 ? Signalbox1.Ko : Signalbox2.Ko;
        SignalboxSegment sbSegSec = sbNum == 1 ? Signalbox2.Po : Signalbox1.Po;

        HandleBlockAnimation(sbSegMain, sbSegSec, sbSegment);
    }

    public void PauseAnimatingBlockChange(SignalboxSegment sbSegment) {
        foreach (var seg in Signalbox1.Segments) {
            seg.SegmentAnimator.SetFloat("TarczkaSpeed", 0);
            seg.SegmentAnimator.SetBool("TarczkaChangeEnable", false);
        }
        foreach (var seg in Signalbox2.Segments) {
            seg.SegmentAnimator.SetFloat("TarczkaSpeed", 0);
            seg.SegmentAnimator.SetBool("TarczkaChangeEnable", false);
        }
    }

    //Helper methods
    private void HandleBlockAnimation(SignalboxSegment sb1, SignalboxSegment sb2, SignalboxSegment sbChosen) {
        sb1.SegmentAnimator.SetFloat("TarczkaSpeed", 1);
        sb2.SegmentAnimator.SetFloat("TarczkaSpeed", 1);
        sb1.SegmentAnimator.SetBool("TarczkaChangeEnable", true);
        sb2.SegmentAnimator.SetBool("TarczkaChangeEnable", true);

        //For Poz they work inversly, for Po and Ko they work parallel
        if (sb1.Type == BlockType.Poz) {
            if (sbChosen.BlockState) {
                sb1.SegmentAnimator.SetBool("TarczkaUp", true);
                sb2.SegmentAnimator.SetBool("TarczkaUp", false);
            }
            else {
                sb1.SegmentAnimator.SetBool("TarczkaUp", false);
                sb2.SegmentAnimator.SetBool("TarczkaUp", true);
            }
        }
        else {
            if (sbChosen.BlockState) {
                sb1.SegmentAnimator.SetBool("TarczkaUp", true);
                sb2.SegmentAnimator.SetBool("TarczkaUp", true);
            }
            else {
                sb1.SegmentAnimator.SetBool("TarczkaUp", false);
                sb2.SegmentAnimator.SetBool("TarczkaUp", false);
            }
        }
    }
}

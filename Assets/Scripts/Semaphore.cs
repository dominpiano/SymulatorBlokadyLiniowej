using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Semaphore : MonoBehaviour {

    [SerializeField]
    private SemaphoreType type;
    //Getter to protect 
    public SemaphoreType SemType {
        get { return type; }
    }

    public SemaphoreSignal CurrentSignal;

    private List<GameObject> semaphoreLights = new List<GameObject>();

    private void Start() {
        switch (type) {
            case SemaphoreType.Sem3:
                semaphoreLights.Add(transform.Find("GreenLight").gameObject);
                semaphoreLights.Add(transform.Find("RedLight").gameObject);
                semaphoreLights.Add(transform.Find("WhiteLight").gameObject);
                break;
            case SemaphoreType.Sem4:
                semaphoreLights.Add(transform.Find("GreenLight").gameObject);
                semaphoreLights.Add(transform.Find("RedLight").gameObject);
                semaphoreLights.Add(transform.Find("OrangeLight").gameObject);
                semaphoreLights.Add(transform.Find("WhiteLight").gameObject);
                break;
            case SemaphoreType.Sem5:
                semaphoreLights.Add(transform.Find("GreenLight").gameObject);
                semaphoreLights.Add(transform.Find("Orange1Light").gameObject);
                semaphoreLights.Add(transform.Find("RedLight").gameObject);
                semaphoreLights.Add(transform.Find("Orange2Light").gameObject);
                semaphoreLights.Add(transform.Find("WhiteLight").gameObject);
                break;
        }
        //Start state is always S1 (stop)
        CurrentSignal = SemaphoreSignal.S1;
    }

    public IEnumerator LightS1() {
        switch (type) {
            case SemaphoreType.Sem3:
                yield return SwitchToLight(1);
                break;
            case SemaphoreType.Sem4:
                yield return SwitchToLight(1);
                break;
            case SemaphoreType.Sem5:
                yield return SwitchToLight(2);
                break;
            default:
                Debug.Log("Unknown semaphore type!");
                break;
        }
        
        CurrentSignal = SemaphoreSignal.S1;
    }

    public IEnumerator LightS2() {
        switch (type) {
            case SemaphoreType.Sem3:
                yield return SwitchToLight(0);
                break;
            case SemaphoreType.Sem4:
                yield return SwitchToLight(0);
                break;
            case SemaphoreType.Sem5:
                yield return SwitchToLight(0);
                break;
            default:
                Debug.Log("Unknown semaphore type!");
                break;
        }
        CurrentSignal = SemaphoreSignal.S2;
    }

    public IEnumerator LightS5() {
        switch (type) {
            case SemaphoreType.Sem3:
                break;
            case SemaphoreType.Sem4:
                yield return SwitchToLight(2);
                break;
            case SemaphoreType.Sem5:
                yield return SwitchToLight(1);
                break;
            default:
                Debug.Log("Unknown semaphore type!");
                break;
        }
        CurrentSignal = SemaphoreSignal.S5;
    }

    public IEnumerator LightS10() {
        switch (type) {
            case SemaphoreType.Sem3:
                break;
            case SemaphoreType.Sem4:
                yield return SwitchToLight(0, 2);
                break;
            case SemaphoreType.Sem5:
                yield return SwitchToLight(0, 3);
                break;
            default:
                Debug.Log("Unknown semaphore type!");
                break;
        }
        CurrentSignal = SemaphoreSignal.S10;
    }

    public IEnumerator LightS13() {
        switch (type) {
            case SemaphoreType.Sem3:
                break;
            case SemaphoreType.Sem4:
                break;
            case SemaphoreType.Sem5:
                yield return SwitchToLight(1, 3);
                break;
            default:
                Debug.Log("Unknown semaphore type!");
                break;
        }
        CurrentSignal = SemaphoreSignal.S13;
    }

    public IEnumerator LightSz() {
        switch (type) {
            case SemaphoreType.Sem3:
                SwitchToSz(2);
                break;
            case SemaphoreType.Sem4:
                SwitchToSz(3);
                break;
            case SemaphoreType.Sem5:
                SwitchToSz(4);
                break;
            default:
                Debug.Log("Unknown semaphore type!");
                break;
        }
        CurrentSignal = SemaphoreSignal.Sz;
        yield break;
    }

    //Decide which lights to turn on or off
    private IEnumerator SwitchToLight(params int[] lightIndices) {
        if(lightIndices.Length == 1) {
            yield return DimLights(true, semaphoreLights.Where((light, i) => i != lightIndices[0]).ToArray());
            yield return DimLights(false, semaphoreLights[lightIndices[0]]);
        }
        else {
            yield return DimLights(true, semaphoreLights.Where((light, i) => !lightIndices.Contains(i)).ToArray());
            yield return DimLights(false, semaphoreLights[lightIndices[0]]);
        }
    }

    //Special function for Sz
    private IEnumerator SwitchToSz(int lightIndex) {
        yield break;
    }

    private IEnumerator DimLights(bool dimDown, params GameObject[] lights) {

        var seq = DOTween.Sequence();

        foreach (var light in lights) {
            seq.Join(light.GetComponent<Image>().DOColor(new Color(1, 1, 1, (dimDown ? 0 : 1)), 0.4f).SetEase(Ease.InOutCubic));
        }

        yield return seq.AsyncWaitForCompletion();

        foreach (var light in lights)
            light.SetActive(!dimDown);
    }
}

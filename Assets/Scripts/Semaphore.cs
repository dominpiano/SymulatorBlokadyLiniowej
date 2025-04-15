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
        int lightIndex;
        switch (type) {
            case SemaphoreType.Sem3:
                lightIndex = 1;
                yield return DimLights(true, semaphoreLights.Where((light, i) => i != lightIndex).ToArray());
                yield return DimLights(false, semaphoreLights[lightIndex]);
                break;
            case SemaphoreType.Sem4:
                lightIndex = 1;
                yield return DimLights(true, semaphoreLights.Where((light, i) => i != lightIndex).ToArray());
                yield return DimLights(false, semaphoreLights[lightIndex]);
                break;
            case SemaphoreType.Sem5:
                lightIndex = 2;
                yield return DimLights(true, semaphoreLights.Where((light, i) => i != lightIndex).ToArray());
                yield return DimLights(false, semaphoreLights[lightIndex]);
                break;
            default:
                Debug.Log("Unknown semaphore type!");
                break;
        }
        
        CurrentSignal = SemaphoreSignal.S1;
    }

    public IEnumerator LightS2() {
        int lightIndex;
        switch (type) {
            case SemaphoreType.Sem3:
                lightIndex = 0;
                yield return DimLights(true, semaphoreLights.Where((light, i) => i != lightIndex).ToArray());
                yield return DimLights(false, semaphoreLights[lightIndex]);
                break;
            case SemaphoreType.Sem4:
                lightIndex = 0;
                yield return DimLights(true, semaphoreLights.Where((light, i) => i != lightIndex).ToArray());
                yield return DimLights(false, semaphoreLights[lightIndex]);
                break;
            case SemaphoreType.Sem5:
                lightIndex = 0;
                yield return DimLights(true, semaphoreLights.Where((light, i) => i != lightIndex).ToArray());
                yield return DimLights(false, semaphoreLights[lightIndex]);
                break;
            default:
                Debug.Log("Unknown semaphore type!");
                break;
        }
        CurrentSignal = SemaphoreSignal.S2;
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

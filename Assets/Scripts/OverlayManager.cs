using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayManager : MonoBehaviour {
    public static OverlayManager Instance;
    
    [SerializeField]
    private UIDocument uiDocument;

    private VisualElement overlayElement;

    void OnEnable() {
        overlayElement = uiDocument.rootVisualElement.Q(className: "overlay");
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this) {
            Destroy(Instance);
        }
    }

    public async UniTask ShowOverlay() {
        overlayElement.AddToClassList("overlay-active");
        await UniTask.Delay(500); //Equal to USS declared value
    }
    
    public async UniTask HideOverlay() {
        await UniTask.Delay(500); //To run asynchronously
        overlayElement.RemoveFromClassList("overlay-active");
    }
}
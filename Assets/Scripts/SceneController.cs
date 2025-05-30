using Cysharp.Threading.Tasks;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneController : MonoBehaviour {

    //Instance of this class to have only one in project
    public static SceneController Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(Instance);
    }

    public async UniTask LoadScene(string scName) {
        await OverlayManager.Instance.ShowOverlay();
        await SceneManager.LoadSceneAsync(scName);
        await OverlayManager.Instance.HideOverlay();
    }

    public async UniTask LoadScene(int scIndex) {
        await OverlayManager.Instance.ShowOverlay();
        await SceneManager.LoadSceneAsync(scIndex);
        await OverlayManager.Instance.HideOverlay();
    }

    public async UniTask ReloadScene() {
        await OverlayManager.Instance.ShowOverlay();
        await SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        await OverlayManager.Instance.HideOverlay();
    }

}

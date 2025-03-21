using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneController : MonoBehaviour {

    //Instance of this class to have only one in project
    public static SceneController Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(Instance);
    }

    public void LoadScene(string scName) {
        SceneManager.LoadSceneAsync(scName);
    }

    public void LoadScene(int scIndex) {
        SceneManager.LoadSceneAsync(scIndex);
    }

}

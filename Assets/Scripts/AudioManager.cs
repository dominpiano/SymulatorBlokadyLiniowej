using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;

    private AudioSource audioSource;

    //Menu sounds
    [SerializeField]
    private AudioClip MouseOverSound, MouseClickSound;

    //Other sounds


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(Instance);
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOverSound(PointerOverEvent evt) {
        audioSource.PlayOneShot(MouseOverSound);
    }

    public void PlayClickSound(ClickEvent evt) {
        audioSource.PlayOneShot(MouseClickSound);
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Collections;

public class ChoiceMenuScript : MonoBehaviour {

    private UIDocument document;

    //UI buttons
    private Button singleplayerButton;
    private Button multiplayerButton;
    private Button exitButton;

    [SerializeField]
    private GameObject choiceMenu;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip overSound, clickSound;

    void Start() {
        document = GetComponent<UIDocument>();
        audioSource = GetComponent<AudioSource>();

        List<Button> buttons = document.rootVisualElement.Query<Button>().ToList();

        singleplayerButton = document.rootVisualElement.Q("SingleButton") as Button;
        multiplayerButton = document.rootVisualElement.Q("MultiButton") as Button;
        exitButton = document.rootVisualElement.Q("ExitButton") as Button;

        foreach (Button button in buttons) {
            button.RegisterCallback<PointerOverEvent>(SoundOver);
        }

        singleplayerButton.RegisterCallback<ClickEvent>(evt => ChooseMode(evt, 0));
        multiplayerButton.RegisterCallback<ClickEvent>(evt => ChooseMode(evt, 1));
        exitButton.RegisterCallback<ClickEvent>(ExitGame);
    }

    //mode 0 - singleplayer, mode 1 - multiplayer
    private void ChooseMode(ClickEvent evt, int mode) {
        StartCoroutine(ButtonClicked(mode));
    }

    private IEnumerator ButtonClicked(int mode) {
        Globals.GameMode = mode;
        audioSource.PlayOneShot(clickSound);
        yield return new WaitForSeconds(0.1f);
        choiceMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void SoundOver(PointerOverEvent evt) {
        audioSource.PlayOneShot(overSound);
    }

    private void ExitGame(ClickEvent evt) {
        Application.Quit();
    }

}

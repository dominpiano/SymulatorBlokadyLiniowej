using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

    private UIDocument document;

    //UI buttons
    private Button singleplayerButton;
    private Button multiplayerButton;
    private Button exitButton;

    [SerializeField]
    private GameObject choiceMenu;

    void OnEnable() {
        document = GetComponent<UIDocument>();

        List<Button> buttons = document.rootVisualElement.Query<Button>().ToList();
        foreach (Button button in buttons) {
            button.RegisterCallback<PointerOverEvent>(AudioManager.Instance.PlayOverSound);
            button.RegisterCallback<ClickEvent>(AudioManager.Instance.PlayClickSound);
        }

        singleplayerButton = document.rootVisualElement.Q("SingleButton") as Button;
        multiplayerButton = document.rootVisualElement.Q("MultiButton") as Button;
        exitButton = document.rootVisualElement.Q("ExitButton") as Button;

        singleplayerButton.RegisterCallback<ClickEvent>(evt => ChooseMode(evt, 0));
        multiplayerButton.RegisterCallback<ClickEvent>(evt => ChooseMode(evt, 1));
        exitButton.RegisterCallback<ClickEvent>(ExitGame);
    }

    //mode 0 - singleplayer, mode 1 - multiplayer
    private void ChooseMode(ClickEvent evt, int mode) {
        Globals.GameMode = mode;
        choiceMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void ExitGame(ClickEvent evt) {
        Application.Quit();
    }

}

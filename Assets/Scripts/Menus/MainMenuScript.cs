using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour {

    private UIDocument document;

    //UI buttons
    private Button singleplayerButton;
    private Button multiplayerButton;
    private Button exitButton;

    void Start() {
        document = GetComponent<UIDocument>();

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
        if (mode == 0) {

        }
        else if (mode == 1) {
        
        }
    }

    private void ExitGame(ClickEvent evt) {
        Application.Quit();
    }

}

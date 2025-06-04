using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Collections;
using System.Linq;
using Cysharp.Threading.Tasks;

public class ChoiceMenuScript : MonoBehaviour {

    private UIDocument document;

    [SerializeField]
    private GameObject mainMenu;

    //Track choice buttons
    List<Button> trackTypeButtons;

    //Interlocking devices type buttons
    List<Button> deviceTypeButtons;

    //Line block type buttons
    List<Button> lineBlockTypeButtons;

    //Other buttons
    private Button backButton;
    private Button playButton;

    private void OnEnable() {
        document = GetComponent<UIDocument>();

        //Clicking sounds
        List<Button> buttons = document.rootVisualElement.Query<Button>().ToList();
        foreach (Button button in buttons) {
            button.RegisterCallback<PointerOverEvent>(AudioManager.Instance.PlayOverSound);
            button.RegisterCallback<ClickEvent>(AudioManager.Instance.PlayClickSound);
        }

        trackTypeButtons = new List<Button> {
            document.rootVisualElement.Q("OneTrackTwoWayButton") as Button,
            document.rootVisualElement.Q("TwoTrackOneWayButton") as Button,
            document.rootVisualElement.Q("TwoTrackTwoWayButton") as Button
        };

        deviceTypeButtons = new List<Button> {
            document.rootVisualElement.Q("MechanicalTypeButton") as Button,
            document.rootVisualElement.Q("ElectricalTypeButton") as Button,
            document.rootVisualElement.Q("ComputerTypeButton") as Button
        };

        lineBlockTypeButtons = new List<Button> {
            document.rootVisualElement.Q("BlokadaCButton") as Button,
            document.rootVisualElement.Q("BlokadaEapButton") as Button,
            document.rootVisualElement.Q("BlokadaEacButton") as Button
        };

        backButton = document.rootVisualElement.Q("BackButton") as Button;
        playButton = document.rootVisualElement.Q("PlayButton") as Button;

        for (int i = 0; i < trackTypeButtons.Count; i++) {
            int ind = i; //For lambda variable capturing solve
            trackTypeButtons[i].RegisterCallback<ClickEvent>(evt => ChooseTrackTypeEvent(evt, ind));
        }

        for (int i = 0; i < deviceTypeButtons.Count; i++) {
            int ind = i;
            deviceTypeButtons[i].RegisterCallback<ClickEvent>(evt => ChooseDeviceTypeEvent(evt, ind));
        }

        for (int i = 0; i < lineBlockTypeButtons.Count; i++) {
            int ind = i;
            lineBlockTypeButtons[i].RegisterCallback<ClickEvent>(evt => ChooseLineBlockTypeEvent(evt, ind));
        }

        backButton.RegisterCallback<ClickEvent>(GoBackEvent);
        playButton.RegisterCallback<ClickEvent>(PlayEvent);

    }

    private void ChooseTrackTypeEvent(ClickEvent evt, int type) {
        Globals.TrackType = type;
        SelectOneOption(trackTypeButtons, type);
        foreach (var b in deviceTypeButtons)
            b.RemoveFromClassList("button-inactive");
    }

    private void ChooseDeviceTypeEvent(ClickEvent evt, int type) {
        Globals.DeviceType = type;
        SelectOneOption(deviceTypeButtons, type);
        //Depending on the device type there is different set of line blocks
        switch (type) {
            case 0:
                lineBlockTypeButtons[0].RemoveFromClassList("button-inactive");
                lineBlockTypeButtons[1].AddToClassList("button-inactive");
                lineBlockTypeButtons[2].AddToClassList("button-inactive");
                RemoveSelection(lineBlockTypeButtons);
                break;
            default:
                foreach (var b in lineBlockTypeButtons)
                    b.RemoveFromClassList("button-inactive");
                RemoveSelection(lineBlockTypeButtons);
                break;
        }
    }

    private void ChooseLineBlockTypeEvent(ClickEvent evt, int type) {
        Globals.LineBlockType = type;
        SelectOneOption(lineBlockTypeButtons, type);
        playButton.RemoveFromClassList("button-inactive");
    }

    private void GoBackEvent(ClickEvent evt) {
        mainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private async void PlayEvent(ClickEvent evt) {
        if (Globals.GameMode == 0)
            await SceneController.Instance.LoadScene("PlaySingleScene");
        //else if(Globals.GameMode == 1)
        //    SceneController.Instance.LoadScene("PlayMultiScene");
    }

    //Helper methods
    private void SelectOneOption(List<Button> btns, int indx) {
        foreach (var btn in btns.Select((val, i) => new { val, i })) {
            if (btn.i == indx)
                btn.val.AddToClassList("button-selected");
            else
                btn.val.RemoveFromClassList("button-selected");
        }
    }

    private void RemoveSelection(List<Button> btns) {
        foreach (var btn in btns)
            btn.RemoveFromClassList("button-selected");
    }
}

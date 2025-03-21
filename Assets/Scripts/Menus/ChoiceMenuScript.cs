using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Collections;
using System.Linq;

public class ChoiceMenuScript : MonoBehaviour {

    private UIDocument document;

    //Track choice buttons
    private int trackType; //0:one-two, 1:two-one, 2:two-two
    List<Button> trackButtons;

    //Interlocking devices type buttons
    private int deviceType; //0:mech, 1:electr, 2:comp
    List<Button> deviceTypeButtons;

    //Line block type buttons
    private int lineBlockType; //0:C, 1:eap, 2:eac
    List<Button> lineBlockTypeButtons;

    //Other buttons
    private Button backButton;
    private Button playButton;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip overSound, clickSound;

    void Start() {
        document = GetComponent<UIDocument>();
        audioSource = GetComponent<AudioSource>();

        //Clicking sounds
        List<Button> buttons = document.rootVisualElement.Query<Button>().ToList();
        foreach (Button button in buttons) {
            button.RegisterCallback<PointerOverEvent>(SoundOver);
        }

        trackButtons = new List<Button> {
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

        for(int i = 0; i < trackButtons.Count; i++)
            trackButtons[i].RegisterCallback<ClickEvent>(evt => ChooseTrackType(evt, i));

        for (int i = 0; i < deviceTypeButtons.Count; i++)
            deviceTypeButtons[i].RegisterCallback<ClickEvent>(evt => ChooseDeviceType(evt, i));

        for (int i = 0; i < lineBlockTypeButtons.Count; i++)
            lineBlockTypeButtons[i].RegisterCallback<ClickEvent>(evt => ChooseLineBlockType(evt, i));

    }

    private void ChooseTrackType(ClickEvent evt, int type) {
        trackType = type;
        SelectOneOption(trackButtons, type);
        foreach (var b in deviceTypeButtons)
            b.RemoveFromClassList("button-inactive");
    }

    private void ChooseDeviceType(ClickEvent evt, int type) {

    }

    private void ChooseLineBlockType(ClickEvent evt, int type) {

    }

    private void SelectOneOption(List<Button> btns, int indx) {
        foreach(var btn in btns.Select((val, i) => new {val, i})) {
            if (btn.i == indx)
                btn.val.AddToClassList("button-selected");
            else
                btn.val.RemoveFromClassList("button-selected");
        }
    }

    private void SoundOver(PointerOverEvent evt) {
        audioSource.PlayOneShot(overSound);
    }

}

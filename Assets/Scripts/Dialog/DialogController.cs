using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;
using AdaGraves_Dialog;

public class DialogController : MonoBehaviour
{
    public static DialogController dialogController;
    public GameObject dialogUI;
    public GameObject dialogContainer;
    public Image portrait;
    public GameObject dialogLinePrefab;
    public GameObject dialogChoiceContainer;
    public Button dialogChoicePrefab;
    public Button continueButtonPrefab;
    private List<KeyValuePair<string, Button>> choices;
    private List<KeyValuePair<string, DialogLine>> lines;
    private NPCDialog myDialogInteraction;
    private DialogObject dialogObject;
    public PlayerInput playerInput;
    public Sprite playerPortrait;
    private Sprite npcSprite;

    public void PlayDialog(Sprite charPortrait, DialogObject dialogObject, NPCDialog inter)
    {
        npcSprite = charPortrait;
        this.dialogObject = dialogObject;
        myDialogInteraction = inter;
        playerInput.SwitchCurrentActionMap("UI");
        GameObject dialogLineUI = GameObject.Instantiate(dialogLinePrefab);
        Text dialogText = dialogLineUI.GetComponent<Text>();
        dialogLineUI.transform.SetParent(dialogContainer.transform, false);
        DialogLine line = dialogObject.nextLines()[0];
        dialogObject.play(line);
        dialogText.text = "(" + line.speakerID + ") " + line.line;
        dialogUI.SetActive(true);
        if (line.speakerID == "player") {
            dialogText.color = dialogObject.playerTextColor;
            portrait.sprite = playerPortrait;
        } else {
            dialogText.color = dialogObject.npcTextColor;
            portrait.sprite = npcSprite;
        }
        Button continueButton = GameObject.Instantiate<Button>(continueButtonPrefab);
        continueButton.transform.SetParent(dialogContainer.transform, false);
        continueButton.onClick.AddListener(() => DialogChosen(false, continueButton.gameObject));
    }

    public void StopDialog()
    {
        dialogUI.SetActive(false);
        playerInput.SwitchCurrentActionMap("Player");
        myDialogInteraction.DialogEnded();
    }

    public void DialogChosen(bool wasChoice, GameObject buttons = null, DialogLine chosen=null)
    {
        GameObject.Destroy(buttons);
        if (chosen != null) {
            GameObject dialogLineUI = GameObject.Instantiate(dialogLinePrefab);
            Text dialogText = dialogLineUI.GetComponent<Text>();
            dialogLineUI.transform.SetParent(dialogContainer.transform, false);
            dialogObject.play(chosen);
            dialogText.text = "(" + chosen.speakerID + ") " + chosen.line;
            if (chosen.speakerID == "Willow") {
                dialogText.color = dialogObject.playerTextColor;
                portrait.sprite = playerPortrait;
            } else {
                dialogText.color = dialogObject.npcTextColor;
                portrait.sprite = npcSprite;
            }
            Button continueButton = GameObject.Instantiate<Button>(continueButtonPrefab);
            continueButton.transform.SetParent(dialogContainer.transform, false);
            continueButton.onClick.AddListener(() => DialogChosen(false, continueButton.gameObject));

            return;
        }
        DialogLine[] lines = dialogObject.nextLines();
        if (lines.Length == 1) {
            GameObject dialogLineUI = GameObject.Instantiate(dialogLinePrefab);
            Text dialogText = dialogLineUI.GetComponent<Text>();
            dialogLineUI.transform.SetParent(dialogContainer.transform, false);
            dialogObject.play(lines[0]);
            dialogText.text = "(" + lines[0].speakerID + ") " + lines[0].line;
            if (lines[0].speakerID == "Willow") {
                dialogText.color = dialogObject.playerTextColor;
                portrait.sprite = playerPortrait;
            } else {
                dialogText.color = dialogObject.npcTextColor;
                portrait.sprite = npcSprite;
            }
            Button continueButton = GameObject.Instantiate<Button>(continueButtonPrefab);
            continueButton.transform.SetParent(dialogContainer.transform, false);
            continueButton.onClick.AddListener(() => DialogChosen(false, continueButton.gameObject));
        } else if (lines.Length > 1) {
            GameObject buttonContainer = GameObject.Instantiate(dialogChoiceContainer);
            foreach (DialogLine line in lines) {
                Button choice = GameObject.Instantiate<Button>(dialogChoicePrefab);
                Text choiceText = choice.GetComponentInChildren<Text>();
                choiceText.text = line.line;
                choice.onClick.AddListener(() => DialogChosen(true, buttonContainer, line));
                choice.transform.SetParent(buttonContainer.transform, false);
            }
            buttonContainer.transform.SetParent(dialogContainer.transform, false);
        } else { 
            StopDialog();
        }
        /*foreach (KeyValuePair<string, DialogLine> line in lines) {
            Debug.Log(line.Key);
        }
        KeyValuePair<string, Button> choicePair = choices.SingleOrDefault(x => x.Key == question);
        KeyValuePair<string, DialogLine> linePair = lines.SingleOrDefault(x => x.Key == question);
        Button dialogChosen = choicePair.Value;
        DialogLine dialogLine = linePair.Value;
        choices.Remove(choicePair);
        lines.Remove(linePair);

        dialogChosen.transform.SetParent(null);
        GameObject.Destroy(dialogChosen);

        if(dialogLine.isExit) {
            foreach (KeyValuePair<string, Button> notChosen in choices) {
                notChosen.Value.transform.SetParent(null);
                GameObject.Destroy(notChosen.Value);
            }
            choices.Clear();
            lines.Clear();
            StopDialog();
        }

        dialogText.text = dialogLine.response;

        foreach (DialogLine line in dialogLine.additionalLines) {
            Button dialogChoice = GameObject.Instantiate<Button>(dialogChoicePrefab);
            Text choiceText = dialogChoice.GetComponentInChildren<Text>();
            dialogChoice.onClick.AddListener(() => DialogChosen(line.question));
            choiceText.text = line.question;
            dialogChoice.transform.SetParent(dialogChoiceContainer.transform);
            dialogChoice.transform.SetAsFirstSibling();
            dialogChoice.transform.localScale = new Vector3(1, 1, 1);
            choices.Add(new KeyValuePair<string, Button>(line.question, dialogChoice));
            lines.Add(new KeyValuePair<string, DialogLine>(line.question, line));
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogController = this;
        choices = new List<KeyValuePair<string, Button>>();
        lines = new List<KeyValuePair<string, DialogLine>>();
        dialogUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

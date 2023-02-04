using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogController : MonoBehaviour
{
    public static DialogController dialogController;
    public Image portrait;
    public Text dialogText;
    public GameObject dialogChoiceContainer;
    public Button dialogChoicePrefab;
    public GameObject dialogUI;
    private List<KeyValuePair<string, Button>> choices;
    private List<KeyValuePair<string, DialogLine>> lines;
    private Interaction myInteraction;

    public void PlayDialog(Sprite charPortrait, DialogObject dialogObject, Interaction inter)
    {
        myInteraction = inter;
        //InputManager.instance.PauseGameplay();
        dialogText.text = dialogObject.startingLine;
        dialogUI.SetActive(true);
        portrait.sprite = charPortrait;
        foreach (DialogLine line in dialogObject.additionalLines) {
            Button dialogChoice = GameObject.Instantiate<Button>(dialogChoicePrefab);
            Text choiceText = dialogChoice.GetComponentInChildren<Text>();
            dialogChoice.onClick.AddListener(() => DialogChosen(line.question));
            choiceText.text = line.question;
            dialogChoice.transform.SetParent(dialogChoiceContainer.transform, false);
            choices.Add(new KeyValuePair<string, Button>(line.question, dialogChoice));
            lines.Add(new KeyValuePair<string, DialogLine>(line.question, line));
        }
    }

    public void StopDialog()
    {
        dialogUI.SetActive(false);
        //InputManager.instance.UnpauseGameplay();
        myInteraction.DialogEnded();
    }

    public void DialogChosen(string question)
    {
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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogController = this;
        choices = new List<KeyValuePair<string, Button>>();
        lines = new List<KeyValuePair<string, DialogLine>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

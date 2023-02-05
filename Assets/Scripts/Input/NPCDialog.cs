using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdaGraves_Dialog;

public class NPCDialog : Interaction
{
    public DialogObject dialog;
    public Sprite portrait;
    InteractableItem item;
    public override void OnInteract(InteractableItem item)
    {
        this.item = item;
        DialogController.dialogController.PlayDialog(portrait, dialog, this);
    }
    public void DialogEnded() {
        item.active = false;
    }

    public void Start()
    {
        dialog = Instantiate(dialog);
        for (int i = 0; i < dialog.npcDialogs.Length; i++) {
            dialog.npcDialogs[i] = Instantiate(dialog.npcDialogs[i]);
        }
        for (int i = 0; i < dialog.playerDialogs.Length; i++) {
            dialog.playerDialogs[i] = Instantiate(dialog.playerDialogs[i]);
        }
        dialog.reset();
    }
}
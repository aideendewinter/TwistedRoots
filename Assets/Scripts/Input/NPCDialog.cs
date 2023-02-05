using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : Interaction
{
    public DialogObject dialog;
    public Sprite portrait;
    public override void OnInteract(InteractableItem item)
    {
        DialogController.dialogController.PlayDialog(portrait, dialog, this);
    }
    public void DialogEnded() {

    }
}

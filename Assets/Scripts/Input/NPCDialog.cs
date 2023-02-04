using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : Interaction
{
    public override void DialogEnded()
    {
        throw new System.NotImplementedException();
    }

    public override void OnInteract(InteractableItem item)
    {
        Debug.Log("Interacted with NPC.");
}
}

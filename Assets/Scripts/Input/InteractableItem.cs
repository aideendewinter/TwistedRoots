using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour {
    public Material baseMaterial;
    public Material highlightMaterial;
    public Interaction interaction;
    public bool active=true;

    public void HighlightOn()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = highlightMaterial;
    }

    public void HighlightOff()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = baseMaterial;
    }

    public void Interact()
    {
        interaction.OnInteract(this);
    }
}

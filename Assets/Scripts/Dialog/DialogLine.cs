using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogLine", order = 2)]
public class DialogLine : ScriptableObject
{
    public string question, response;
    public DialogLine[] additionalLines;
    public bool isExit = false;
}

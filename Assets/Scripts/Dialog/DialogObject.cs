using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogObject", order = 1)]
public class DialogObject : ScriptableObject
{
    public string startingLine;
    public DialogLine[] additionalLines;
}

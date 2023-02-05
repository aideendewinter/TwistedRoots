using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdaGraves_Dialog;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogCollection", order = 2)]
public class DialogCollection : ScriptableObject
{
    public DialogLine[] dialogLines;
    public Language lang = Language.ENGLISH;
}

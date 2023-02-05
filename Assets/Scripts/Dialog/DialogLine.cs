using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdaGraves_Dialog;

[System.Serializable]
public class DialogLine
{
    public string id, line, speakerID;
    public AudioClip lineAudio;
    public Language lang = Language.ENGLISH;
    public bool played = false;
    public bool repeatable;
    public string[] requirements;
}

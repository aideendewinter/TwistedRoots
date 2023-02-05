using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaGraves_Dialog {
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogObject", order = 1)]
    public class DialogObject : ScriptableObject {
        public DialogCollection[] npcDialogs;
        public DialogCollection[] playerDialogs;
        public Color npcTextColor;
        public Color playerTextColor = Color.white;
        public string startingLineID;
        public Language lang = Language.ENGLISH;
        private DialogLine lastLine;

        public bool checkRequirements(DialogLine line)
        {
            bool passed = true;
            foreach (string req in line.requirements) {
                if (req.Contains("PREV_LINE"))
                    passed = passed && (("PREV_LINE:" + lastLine.id) == req);
            }
            return (!line.played || line.repeatable) && passed;
        }

        public void reset()
        {
            lastLine = null;
        }

        public void play(DialogLine line)
        {
            line.played = true;
            lastLine = line;
        }

        public DialogLine[] nextLines()
        {
            if(lastLine != null) {
                List<DialogLine> possibleLines = new List<DialogLine>();
                foreach (DialogCollection collection in playerDialogs) {
                    if (collection.lang != lang)
                        continue;
                    foreach (DialogLine line in collection.dialogLines) {
                        if (checkRequirements(line)) {
                            possibleLines.Add(line);

                        }
                    }
                }
                foreach (DialogCollection collection in npcDialogs) {
                    if (collection.lang != lang)
                        continue;
                    foreach (DialogLine line in collection.dialogLines) {
                        if (checkRequirements(line)) {
                            if (line.requirements[0] == "PREV_LINE:" + lastLine.id)
                                return new[] { line };
                            possibleLines.Add(line);
                        }
                    }
                }

                if (possibleLines.Count > 0)
                    return possibleLines.ToArray();
                else return new DialogLine[0];
            }
            
            lastLine = GetDialogLine(startingLineID);
            return new []{lastLine};
        }
        
        public DialogLine GetDialogLine(string lineID) {
            if(lineID.Contains("Player")) {
                foreach (DialogCollection collection in playerDialogs) {
                    if (collection.lang != lang)
                        continue;
                    foreach(DialogLine line in collection.dialogLines) {
                        if (line.id == lineID)
                            return line;
                    }
                }
            }
            foreach (DialogCollection collection in npcDialogs) {
                if (collection.lang != lang)
                    continue;
                foreach (DialogLine line in collection.dialogLines) {
                    if (line.id == lineID)
                        return line;
                }
            }
            DialogLine errorLine = new DialogLine();
            errorLine.line = "Error Line-O-line: Line not found.";
            return errorLine;
        }
    }
    public enum Language {
        ENGLISH
    }
}
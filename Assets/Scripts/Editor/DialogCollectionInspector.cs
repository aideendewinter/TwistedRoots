using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(DialogCollection))]
public class DialogCollectionInspector : Editor
{
    public VisualTreeAsset visualTree;
    public override VisualElement CreateInspectorGUI()
    {
        // Create a new VisualElement to be the root of our inspector UI
        VisualElement myInspector = new VisualElement();

        visualTree.CloneTree(myInspector);

        // Return the finished inspector UI
        return myInspector;
    }
}
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugData))]
public class DebugDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var character = (DebugData)target;
        if (GUILayout.Button("Attack")) character.Attack();
    }
}
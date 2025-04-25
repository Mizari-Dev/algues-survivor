using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CustomButton),true)]
public class CustomButtonEditor : ButtonEditor
{
    SerializedProperty prop;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        DrawProperty("_view");
        DrawProperty("_rect");
        serializedObject.ApplyModifiedProperties();
    }
    
    private void DrawProperty(string name)
    {
        prop = serializedObject.FindProperty(name);
        EditorGUILayout.PropertyField(prop);
    }
}

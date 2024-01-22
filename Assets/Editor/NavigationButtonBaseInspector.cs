using CodeBase.UI.Buttons.NavigationButtons;
using UnityEditor;
using UnityEditor.UI;

namespace Editor
{
    [CustomEditor(typeof(NavigationButtonBase), true)]
    public class NavigationButtonBaseInspector : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
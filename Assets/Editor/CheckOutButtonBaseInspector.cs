using CodeBase.UI.Buttons;
using UnityEditor;
using UnityEditor.UI;

namespace Editor
{
    [CustomEditor(typeof(CheckOutButton), true)]
    public class CheckOutButtonBaseInspector : ButtonEditor
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
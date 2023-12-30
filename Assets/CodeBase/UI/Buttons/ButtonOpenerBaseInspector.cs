using UnityEditor;

namespace CodeBase.UI.Buttons
{
    [CustomEditor(typeof(ButtonOpenerBase), true)]
    public class ButtonOpenerBaseInspector : UnityEditor.UI.ButtonEditor
    {
        private SerializedProperty _windowServiceProperty;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
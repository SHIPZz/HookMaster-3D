using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResetDataEditor))]
public class ResetDataEditor : UnityEditor.Editor
{
    [MenuItem("Tools/Clear data")]
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Data cleared");
    }
}
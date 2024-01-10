using CodeBase.EmployeeSpawners;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EmployeeSpawner))]
public class EmployeeSpawnerEditor : UnityEditor.Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(EmployeeSpawner spawner, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawner.transform.position, 1f);
    }
}
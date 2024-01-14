using CodeBase.Services.Clients;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClientSpawnerEditor))]
public class ClientSpawnerEditor : UnityEditor.Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(ClientSpawner spawner, GizmoType gizmoType)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(spawner.transform.position, 0.5f);
    }
}
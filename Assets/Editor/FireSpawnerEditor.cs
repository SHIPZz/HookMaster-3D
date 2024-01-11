using CodeBase.Gameplay.Fire;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FireSpawnerEditor))]
public class FireSpawnerEditor : UnityEditor.Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(FireSpawner spawner, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawner.transform.position, 0.5f);
    }
}
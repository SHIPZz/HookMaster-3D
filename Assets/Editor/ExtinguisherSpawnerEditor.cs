using CodeBase.EmployeeSpawners;
using CodeBase.Gameplay.Extinguisher;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ExtinguisherSpawner))]
    public class ExtinguisherSpawnerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(ExtinguisherSpawner spawner, GizmoType gizmoType)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(spawner.transform.position, 0.5f);
        }
    }
}

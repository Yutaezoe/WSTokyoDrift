using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Takap.Utility.Algorithm
{
    // Gizmo�`�揈��
    public partial class WayPoint
    {
        // Gizmo�ŕ\������e�L�X�g
        public string GizmoText { get; set; }
        // Gizmo�̐��̐F
        public Color GizmoColor { get; set; } = Color.white;


    }
}
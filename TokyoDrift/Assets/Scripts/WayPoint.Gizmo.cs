using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Takap.Utility.Algorithm
{
    // Gizmo描画処理
    public partial class WayPoint
    {
        // Gizmoで表示するテキスト
        public string GizmoText { get; set; }
        // Gizmoの線の色
        public Color GizmoColor { get; set; } = Color.white;


    }
}
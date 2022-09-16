// WayPoint.cs

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// ゲーム中のある地点を表します。
    /// </summary>
    [ExecuteAlways]
    public partial class WayPoint : MonoBehaviour
    {
        // Inspector

        // 移動可能な隣接ノード
        [SerializeField] List<WayPoint> _relations;
        // 通行可能かどうか
        [SerializeField] bool _canMove = true;

        // Fields

        // 直前のノード隣接ノード状態を記録するリスト
        readonly List<WayPoint> _previousList = new List<WayPoint>();

        // Props

        // 移動可能な隣接ノードのリストを取得する
        public List<WayPoint> Rerations => _relations;

        // この地点が移動可能かどうかを取得します。
        public bool CanMove => _canMove;

        // Rintime impl

        private void Awake()
        {
            SynchronizeRelationsToPrevious();
            //GizmoText = name;
        }

        // Methods

        public void SynchronizeRelationsToPrevious()
        {
            _previousList.Clear();
            foreach (var p in _relations)
            {
                _previousList.Add(p);
            }
        }
    }
}
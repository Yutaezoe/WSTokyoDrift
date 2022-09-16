namespace Takap.Utility.Algorithm
{
    public class AStarNode
    {
        // 親ノード
        public AStarNode Parent { get; private set; }

        // 対応する位置
        public readonly WayPoint WayPoint; // Derived from Monobehaiour

        // ノードのステータス
        public NodeStatus Status { get; set; }

        // 開始地点からの総移動距離
        public float Total { get; private set; }

        // ゴールまでの推定移動距離
        public float Estimated { get; private set; }

        // ノードのスコア
        public float Score => Total + Estimated;

        // Constructors

        public AStarNode(WayPoint wayPoint)
        {
            WayPoint = wayPoint;
        }

        // Public Methods

        public void Open(AStarNode previous, AStarNode goal)
        {
            if (previous is not null)
            {
                Total = previous.Total +
                    UnityEngine.Vector3.Distance(previous.WayPoint.transform.position,
                        WayPoint.transform.position);
            }
            Estimated = UnityEngine.Vector3.Distance(WayPoint.transform.position,
                goal.WayPoint.transform.position);

            Parent = previous;
            Status = NodeStatus.Open;
        }
    }

    public enum NodeStatus
    {
        None = 0,
        Open,
        Close,
        Exclude, // 探索対象にしない
    }
}
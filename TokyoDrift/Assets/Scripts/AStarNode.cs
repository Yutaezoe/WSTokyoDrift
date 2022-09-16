namespace Takap.Utility.Algorithm
{
    public class AStarNode
    {
        // �e�m�[�h
        public AStarNode Parent { get; private set; }

        // �Ή�����ʒu
        public readonly WayPoint WayPoint; // Derived from Monobehaiour

        // �m�[�h�̃X�e�[�^�X
        public NodeStatus Status { get; set; }

        // �J�n�n�_����̑��ړ�����
        public float Total { get; private set; }

        // �S�[���܂ł̐���ړ�����
        public float Estimated { get; private set; }

        // �m�[�h�̃X�R�A
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
        Exclude, // �T���Ώۂɂ��Ȃ�
    }
}
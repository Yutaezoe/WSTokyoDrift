using System;
using System.Collections.Generic;
using System.Linq;

namespace Takap.Utility.Algorithm
{
    public class AStarWayPoint
    {
        // Fields

        // �I�[�v���ς݂̃m�[�h���L�����郊�X�g
        readonly List<AStarNode> _openList = new();
        // �J�n�m�[�h
        public readonly AStarNode StartPoint;
        // �I���m�[�h
        public readonly AStarNode EndPoint;
        // ���݂̌����������
        SearchState _searchState;

        // Props

        // �T���Ώۂ̃f�[�^�Ǘ�
        public readonly Dictionary<WayPoint, AStarNode> NodeTable = new();

        // Constructors

        public AStarWayPoint(IEnumerable<WayPoint> wayPoints, WayPoint startPoint, WayPoint endPoint)
        {
            foreach (var p in wayPoints)
            {
                if (p is null)
                {
                    continue;
                }

                AStarNode sn = new(p);
                if (!p.CanMove)
                {
                    sn.Status = NodeStatus.Exclude;
                }
                NodeTable[p] = sn;
            }

            StartPoint = NodeTable[startPoint];
            EndPoint = NodeTable[endPoint];

            if (!NodeTable.ContainsKey(startPoint))
            {
                throw new ArgumentException($"{nameof(wayPoints)}�̒���" +
                    $"{nameof(startPoint)}���܂܂�Ă��܂���B");
            }

            // �ŏ��̃^�C����Open�ɐݒ肷��
            AStarNode node = NodeTable[startPoint];
            node.Open(null, NodeTable[endPoint]);
            _openList.Add(node);
        }

        // Public Methods

        // ���[�g������S�Ď��s����
        public SearchState SearchAll()
        {
            SearchState state;
            while (true)
            {
                state = SearchOneStep();
                if (state != SearchState.Searching)
                {
                    break;
                }
            }

            return state;
        }

        // ���[�g������1�X�e�b�v���s����
        public SearchState SearchOneStep()
        {
            // �������������Ă���ꍇ�X�e�[�^�X��Ԃ��Č����������Ȃ�
            if (_searchState != SearchState.Searching)
            {
                return _searchState;
            }

            AStarNode parentNode = GetMinCostNode();
            if (parentNode is null)
            {
                // ���ɃI�[�v��������̂�������������O�ɂȂ��Ȃ����ꍇ
                _searchState = SearchState.Incomplete;
                return _searchState;
            }

            // �I�[�v���ς݃��X�g����m�[�h���폜���đΏۊO�ɂ���
            parentNode.Status = NodeStatus.Close;
            _openList.Remove(parentNode);

            if (parentNode.WayPoint == EndPoint.WayPoint)
            {
                // ���ɊJ�����m�[�h���S�[���n�_������
                _searchState = SearchState.Completed;
                return _searchState;
            }

            // �אڃm�[�h�����ׂăI�[�v������
            foreach (WayPoint aroundPoint in parentNode.WayPoint.Rerations)
            {
                if (!NodeTable.ContainsKey(aroundPoint))
                {
                    // �R���X�g���N�^�Ŏw�肵���|�C���g�̒��ɗאڃm�[�h���܂܂�Ă��Ȃ�������G���[
                    _searchState = SearchState.Error;
                    return _searchState;
                }

                AStarNode tmpNode = NodeTable[aroundPoint];
                if (tmpNode.Status != NodeStatus.None)
                {
                    continue;
                }

                tmpNode.Open(parentNode, EndPoint);

                _openList.Add(tmpNode);

                // �J�����m�[�h���I�_�������炻�̏�ŏ�����ł��؂��ď����I��
                if (tmpNode.WayPoint == EndPoint.WayPoint)
                {
                    tmpNode.Status = NodeStatus.Close;
                    _openList.Remove(tmpNode);

                    _searchState = SearchState.Completed;
                    return _searchState;
                }
            }

            return _searchState;
        }

        // �������ʂ̃��[�g���擾����
        public WayPoint[] GetRoute()
        {
            if (_searchState != SearchState.Completed)
            {
                throw new InvalidOperationException("�����������ł̓��[�g���擾�ł��܂���B");
            }

            AStarNode tmpNode = EndPoint;
            IEnumerable<WayPoint> f()
            {
                while (tmpNode.Parent != null)
                {
                    yield return tmpNode.WayPoint;
                    tmpNode = tmpNode.Parent;
                }

                yield return StartPoint.WayPoint;
            }

            var resultArray = f().ToArray();
            Array.Reverse(resultArray);
            return resultArray;
        }

        // �I�[�v���ς݂̃m�[�h���X�g����ŏ��R�X�g�̃m�[�h���擾���� 
        public AStarNode GetMinCostNode()
        {
            if (_openList.Count == 0)
            {
                return null;
            }

            // ���X�g����ŏ��R�X�g�̃m�[�h��I������
            AStarNode minCostNode = _openList[0];
            if (_openList.Count > 1)
            {
                for (int i = 1; i < _openList.Count; i++)
                {
                    AStarNode tmpNode = _openList[i];
                    if (minCostNode.Score > tmpNode.Score)
                    {
                        minCostNode = tmpNode;
                    }
                }
            }
            return minCostNode;
        }

        public enum SearchState
        {
            Searching = 0,
            Completed,
            Incomplete,
            Error, // �T���Ώۂɂ��Ȃ�
        }

    }
}
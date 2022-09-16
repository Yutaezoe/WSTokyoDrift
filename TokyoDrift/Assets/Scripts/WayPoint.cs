// WayPoint.cs

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Takap.Utility.Algorithm
{
    /// <summary>
    /// �Q�[�����̂���n�_��\���܂��B
    /// </summary>
    [ExecuteAlways]
    public partial class WayPoint : MonoBehaviour
    {
        // Inspector

        // �ړ��\�ȗאڃm�[�h
        [SerializeField] List<WayPoint> _relations;
        // �ʍs�\���ǂ���
        [SerializeField] bool _canMove = true;

        // Fields

        // ���O�̃m�[�h�אڃm�[�h��Ԃ��L�^���郊�X�g
        readonly List<WayPoint> _previousList = new List<WayPoint>();

        // Props

        // �ړ��\�ȗאڃm�[�h�̃��X�g���擾����
        public List<WayPoint> Rerations => _relations;

        // ���̒n�_���ړ��\���ǂ������擾���܂��B
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
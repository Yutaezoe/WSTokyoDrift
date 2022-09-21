using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SearchTarget : MonoBehaviour
{
    public int N { get; }               // ���_�̐�
    private List<Edge>[] _graph;        // �O���t�̕ӂ̃f�[�^

    [SerializeField]
    private GameObject gc3;
    private Transform[] lineChildren;
    private GameObject LineGO;
    private Line LineArray;
    private GameObject NodeGO;
    private Node NodeArray;

    private void Start()
    {

        lineChildren = ComFunctions.GetChildren(gc3.transform);
        int[] pointA = new int[lineChildren.Length];
        int[] pointB = new int[lineChildren.Length];
        int[] lineWeight = new int[lineChildren.Length];
 
        int i = 0;
        foreach (Transform setTrans in lineChildren)
        { 
            //�q���C������A���C���d�݁ApoinA,B��z��Ɋi�[
            LineGO = setTrans.gameObject;
            LineArray = LineGO.GetComponent<Line>();
            lineWeight[i] = LineArray.getLineWeight;
            NodeGO = LineArray.getAPosition.gameObject;
            NodeArray = NodeGO.GetComponent<Node>();
            pointA[i] = NodeArray.getNodeUID;
            NodeGO = LineArray.getBPosition.gameObject;
            NodeArray = NodeGO.GetComponent<Node>();
            pointB[i] = NodeArray.getNodeUID;
            i++;

        };
        // �_�C�N�X�g���̃C���X�^���X���쐬
        // ()���͈ꎞ�I�Ȑݒ�,�����K�v
        var graph = new SearchTarget(lineChildren.Length+1);

        // �ӂ̏���ǉ�����(�����O���t�Ȃ̂ŗ����̌�����)
        int j = 0;
        foreach (Transform setTrans in lineChildren)
        {
            graph.Add(pointA[j], pointB[j], lineWeight[j]);
            graph.Add(pointB[j], pointA[j], lineWeight[j]);
            //Debug.Log($"({pointA[j]},{pointB[j]},{lineWeight[j]})");
            j++;
        }
        // �ŒZ�����𒲂ׂ�
        var minDistaces = graph.GetMinCost(0);

        for (int k = 0; k < minDistaces.Length; k++) Debug.Log($"Vertex[{k}] = {minDistaces[k]}");
    }
    /// <summary>
    /// ������
    /// </summary>
    /// <param name="n">���_��</param>
    public SearchTarget(int n)
    {
        N = n;
        _graph = new List<Edge>[n];
        for (int i = 0; i < n; i++) _graph[i] = new List<Edge>();
    }

    /// <summary>
    /// �ӂ�ǉ�
    /// </summary>
    /// <param name="a">�ڑ����̒��_</param>
    /// <param name="b">�ڑ���̒��_</param>
    /// <param name="cost">�R�X�g</param>
    public void Add(int a, int b, long cost = 1)
            => _graph[a].Add(new Edge(b, cost));

    /// <summary>
    /// �ŒZ�o�H�̃R�X�g���擾
    /// </summary>
    /// <param name="start">�J�n���_</param>
    public long[] GetMinCost(int start)
    {
        // �R�X�g���X�^�[�g���_�ȊO�𖳌����
        var cost = new long[N];
        for (int i = 0; i < N; i++) cost[i] = 1000000000000000000;
        cost[start] = 0;

        // ���m��̒��_���i�[����D��x�t���L���[(�R�X�g���������قǗD��x������)
        var q = new PriorityQueue<Vertex>(N * 10, Comparer<Vertex>.Create((a, b) => b.CompareTo(a)));
        q.Push(new Vertex(start, 0));

        while (q.Count > 0)
        {
            var v = q.Pop();

            // �L�^����Ă���R�X�g�ƈقȂ�(�R�X�g�����傫��)�ꍇ�͖���
            if (v.cost != cost[v.index]) continue;

            // ����m�肵�����_����Ȃ��钸�_�ɑ΂��čX�V���s��
            foreach (var e in _graph[v.index])
            {
                if (cost[e.to] > v.cost + e.cost)
                {
                    // ���ɋL�^����Ă���R�X�g��菬������΃R�X�g���X�V
                    cost[e.to] = v.cost + e.cost;
                    q.Push(new Vertex(e.to, cost[e.to]));
                }
            }
        }

        // �m�肵���R�X�g��Ԃ�
        return cost;
    }

    public struct Edge
    {
        public int to;                      // �ڑ���̒��_
        public long cost;                   // �ӂ̃R�X�g

        public Edge(int to, long cost)
        {
            this.to = to;
            this.cost = cost;
        }
    }

    public struct Vertex : IComparable<Vertex>
    {
        public int index;                   // ���_�̔ԍ�
        public long cost;                   // �L�^�����R�X�g

        public Vertex(int index, long cost)
        {
            this.index = index;
            this.cost = cost;
        }

        public int CompareTo(Vertex other)
            => cost.CompareTo(other.cost);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SearchTarget : MonoBehaviour
{
    public int N { get; }               // 頂点の数
    private List<Edge>[] _graph;        // グラフの辺のデータ

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
            //子ラインから、ライン重み、poinA,Bを配列に格納
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
        // ダイクストラのインスタンスを作成
        // ()内は一時的な設定,検討必要
        var graph = new SearchTarget(lineChildren.Length+1);

        // 辺の情報を追加する(無向グラフなので両方の向きに)
        int j = 0;
        foreach (Transform setTrans in lineChildren)
        {
            graph.Add(pointA[j], pointB[j], lineWeight[j]);
            graph.Add(pointB[j], pointA[j], lineWeight[j]);
            //Debug.Log($"({pointA[j]},{pointB[j]},{lineWeight[j]})");
            j++;
        }
        // 最短距離を調べる
        var minDistaces = graph.GetMinCost(0);

        for (int k = 0; k < minDistaces.Length; k++) Debug.Log($"Vertex[{k}] = {minDistaces[k]}");
    }
    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="n">頂点数</param>
    public SearchTarget(int n)
    {
        N = n;
        _graph = new List<Edge>[n];
        for (int i = 0; i < n; i++) _graph[i] = new List<Edge>();
    }

    /// <summary>
    /// 辺を追加
    /// </summary>
    /// <param name="a">接続元の頂点</param>
    /// <param name="b">接続先の頂点</param>
    /// <param name="cost">コスト</param>
    public void Add(int a, int b, long cost = 1)
            => _graph[a].Add(new Edge(b, cost));

    /// <summary>
    /// 最短経路のコストを取得
    /// </summary>
    /// <param name="start">開始頂点</param>
    public long[] GetMinCost(int start)
    {
        // コストをスタート頂点以外を無限大に
        var cost = new long[N];
        for (int i = 0; i < N; i++) cost[i] = 1000000000000000000;
        cost[start] = 0;

        // 未確定の頂点を格納する優先度付きキュー(コストが小さいほど優先度が高い)
        var q = new PriorityQueue<Vertex>(N * 10, Comparer<Vertex>.Create((a, b) => b.CompareTo(a)));
        q.Push(new Vertex(start, 0));

        while (q.Count > 0)
        {
            var v = q.Pop();

            // 記録されているコストと異なる(コストがより大きい)場合は無視
            if (v.cost != cost[v.index]) continue;

            // 今回確定した頂点からつながる頂点に対して更新を行う
            foreach (var e in _graph[v.index])
            {
                if (cost[e.to] > v.cost + e.cost)
                {
                    // 既に記録されているコストより小さければコストを更新
                    cost[e.to] = v.cost + e.cost;
                    q.Push(new Vertex(e.to, cost[e.to]));
                }
            }
        }

        // 確定したコストを返す
        return cost;
    }

    public struct Edge
    {
        public int to;                      // 接続先の頂点
        public long cost;                   // 辺のコスト

        public Edge(int to, long cost)
        {
            this.to = to;
            this.cost = cost;
        }
    }

    public struct Vertex : IComparable<Vertex>
    {
        public int index;                   // 頂点の番号
        public long cost;                   // 記録したコスト

        public Vertex(int index, long cost)
        {
            this.index = index;
            this.cost = cost;
        }

        public int CompareTo(Vertex other)
            => cost.CompareTo(other.cost);
    }
}

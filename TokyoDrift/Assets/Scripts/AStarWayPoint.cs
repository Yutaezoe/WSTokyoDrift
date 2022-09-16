using System;
using System.Collections.Generic;
using System.Linq;

namespace Takap.Utility.Algorithm
{
    public class AStarWayPoint
    {
        // Fields

        // オープン済みのノードを記憶するリスト
        readonly List<AStarNode> _openList = new();
        // 開始ノード
        public readonly AStarNode StartPoint;
        // 終了ノード
        public readonly AStarNode EndPoint;
        // 現在の検索処理状態
        SearchState _searchState;

        // Props

        // 探索対象のデータ管理
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
                throw new ArgumentException($"{nameof(wayPoints)}の中に" +
                    $"{nameof(startPoint)}が含まれていません。");
            }

            // 最初のタイルをOpenに設定する
            AStarNode node = NodeTable[startPoint];
            node.Open(null, NodeTable[endPoint]);
            _openList.Add(node);
        }

        // Public Methods

        // ルート検索を全て実行する
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

        // ルート検索を1ステップ実行する
        public SearchState SearchOneStep()
        {
            // 処理が完了している場合ステータスを返して検索処理しない
            if (_searchState != SearchState.Searching)
            {
                return _searchState;
            }

            AStarNode parentNode = GetMinCostNode();
            if (parentNode is null)
            {
                // 次にオープンするものが検索完了する前になくなった場合
                _searchState = SearchState.Incomplete;
                return _searchState;
            }

            // オープン済みリストからノードを削除して対象外にする
            parentNode.Status = NodeStatus.Close;
            _openList.Remove(parentNode);

            if (parentNode.WayPoint == EndPoint.WayPoint)
            {
                // 次に開いたノードがゴール地点だった
                _searchState = SearchState.Completed;
                return _searchState;
            }

            // 隣接ノードをすべてオープンする
            foreach (WayPoint aroundPoint in parentNode.WayPoint.Rerations)
            {
                if (!NodeTable.ContainsKey(aroundPoint))
                {
                    // コンストラクタで指定したポイントの中に隣接ノードが含まれていなかったらエラー
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

                // 開いたノードが終点だったらその場で処理を打ち切って処理終了
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

        // 検索結果のルートを取得する
        public WayPoint[] GetRoute()
        {
            if (_searchState != SearchState.Completed)
            {
                throw new InvalidOperationException("検索未完了ではルートを取得できません。");
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

        // オープン済みのノードリストから最小コストのノードを取得する 
        public AStarNode GetMinCostNode()
        {
            if (_openList.Count == 0)
            {
                return null;
            }

            // リストから最小コストのノードを選択する
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
            Error, // 探索対象にしない
        }

    }
}
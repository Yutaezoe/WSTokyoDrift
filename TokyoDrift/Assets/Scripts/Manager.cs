using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using Common;
using System.Diagnostics;
using System.IO;
using System.Text;
using static Target;


public class Manager : MonoBehaviour
{

    public GameObject busMaster;
    public GameObject bus1;
    public GameObject bus2;
    //add hashimoto
    public GameObject mainCamera;
    [SerializeField] private GameObject target1;
    [SerializeField] private GameObject target2;
    [SerializeField] private GameObject logger;
    [SerializeField] private GameObject timer;


    private Transform[] moverGC;
    private GameObject moverGO;
    private Mover moverArray;
    private List<int> _moverID = new List<int>();
    private List<int> _targetID = new List<int>();
    private List<int> _distance = new List<int>();
    private List<int> assignMoverID = new List<int>();
    private List<int> assignTargetID = new List<int>();

    //kito added
    private Transform[] arrayMover;
    private Transform[] arrayTarget;
    private int countMover;
    private int countTarget;
    private int countKickOfdistancePassive ;
    [SerializeField]
    private GameObject targetMaster;
    //private string pyExePath = @"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\Python\tst.exe";
    //private string pyCodePath = @"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\Python\tst.py";

    private string pyExePath = @"C:\Users\13068\dojo\WSTokyoDrift0928\WSTokyoDrift\TokyoDrift\Assets\Python\sample_select_03_sako.exe";
    private string pyCodePath = @"C:\Users\13068\dojo\WSTokyoDrift0928\WSTokyoDrift\TokyoDrift\Assets\Python\sample_select_03_sako.py";


    //kito added 2
    bool assignComplete = false;
    bool assignStatus = false;

    public bool PropertyAssign
    {
        get { return assignStatus; }
    }

    public int GetAssignTarget(int moverID)
    {
        return assignTargetID[moverID];
    }

    private void Start()
    {
        moverGC = ComFunctions.GetChildren(busMaster.transform);
        moverGO = moverGC[0].gameObject;
        moverArray = moverGO.GetComponent<Mover>();
        //Debug.Log(moverArray.moverID);
        //added by kito
        //confirm mover and Target's count
        confirmMoverTargetCount();

    }
    private void Update()
    {
        //added by kito
        //Check if Assign Buber is kicked by ALL Mover every frame
        countDistancePassiveKick();

    }
    //moverクラスから各距離情報の変数を受け取ってManager上にデータを格納していく
    //moverIDの取得順に懸念あり→実装後確認
    //changed hashimoto
    public bool distancePassive(int moverID, int[] targetID, int[] distance)
    {

        //もらうtargetIDは全て同じ値想定
        for (int i = 0; i < targetID.Length; i++)
        {
            _moverID.Add(moverID);
            _targetID.Add(targetID[i]);
            _distance.Add(distance[i]);
        }
        //added by kito
        //coutup if Mover kicks passiveDistance

        countKickOfdistancePassive += 1;
        return true;
    }
    //added by kito
    void confirmMoverTargetCount()
    {
        arrayMover = ComFunctions.GetChildren(busMaster.transform);
        countMover = arrayMover.Length;
        arrayTarget = ComFunctions.GetChildren(targetMaster.transform);
        countTarget = arrayTarget.Length;
    }
    void countDistancePassiveKick()
    {

        if (countMover == countKickOfdistancePassive)
        {
            //assignMover is called once only.
            if (assignComplete == false)
            {
                aasignMover();

                assignComplete = true;
            }
        }
    }
    //add Hashimoto
    void aasignMover()
    {

        //make a CSVfile.
        MakeCSVdistancePassive();

        //start python.
      var ignore =  pythonInterface();

        //read assing result.
        ReadCSVAssignMover();

        //start logger
       var loggerCompo=logger.GetComponent<Logger>();
        loggerCompo.StartLogger();

        //start timer(timer is used by HUD)
        var timerCompo = timer.GetComponent<Timer>();
        timerCompo.StartTimer();
    }
     int pythonInterface()
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo()
        {
            FileName = pyExePath, //実行するファイル(python)
            UseShellExecute = false,//シェルを使うかどうか
            CreateNoWindow = true, //ウィンドウを開くかどうか
            RedirectStandardOutput = true, //テキスト出力をStandardOutputストリームに書き込むかどうか
            Arguments = pyCodePath, //実行するスクリプト 引数(複数可)        + " " + "MOID=0" + " " + "TaID=3" + " " + "Dis=32"
        };
        //外部プロセスの開始
        Process process = Process.Start(processStartInfo);
        //ストリームから出力を得る
        StreamReader streamReader = process.StandardOutput;
        string str = streamReader.ReadLine();
        //外部プロセスの終了
        process.WaitForExit();
        process.Close();
        //実行
        print(str);

        return 0;
    }


    void MakeCSVdistancePassive()
    {

        using (var fileStream = new FileStream(@"C:\Users\13068\dojo\WSTokyoDrift0928\WSTokyoDrift\TokyoDrift\Assets\CSV\toPython.csv", FileMode.Open))
        {
            //Delate toPython.csv
            // ストリームの長さを0に設定します。
            // 結果としてファイルのサイズが0になります。
            fileStream.SetLength(0);
        }


        StreamWriter file = new StreamWriter(@"C:\Users\13068\dojo\WSTokyoDrift0928\WSTokyoDrift\TokyoDrift\Assets\CSV\toPython.csv", true, Encoding.UTF8);
        for (int i = 0; i < _moverID.Count; i++)
        {
            file.WriteLine(string.Format("{0},{1},{2}", _moverID[i], _targetID[i], _distance[i]));
        }
        file.Close();
        print("complete to write toPython");
    }


    void ReadCSVAssignMover()
    {
        // CSVファイルの読み込み
        string filePath = @"C:\Users\13068\dojo\WSTokyoDrift0928\WSTokyoDrift\TokyoDrift\Assets\CSV\toCS.csv";
        // StreamReaderクラスをインスタンス化
        StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
        // 最後まで読み込む
        while (reader.Peek() >= 0)
        {
            // 読み込んだ文字列をカンマ区切りで配列に格納
            string[] cols = reader.ReadLine().Split(',');
            for (int n = 3; n < cols.Length + 3; n++)
            {
                if (n % 2 != 0)
                {
                    assignMoverID.Add(int.Parse(cols[n-3]));
                }
                else
                {
                    assignTargetID.Add(int.Parse(cols[n-3]));

                }
            }
            Console.ReadLine();
        }
        reader.Close();

        ChangeOrderOfArray();

      //for(int i = 0; i < assignTargetID.Count; i++)
      //  {
      //      print("TargetID"+assignTargetID[i]+":"+i);
      //      print("MoverID" + assignMoverID[i] + ":" + i);

      //  }

        assignStatus = true;
    }

    void ChangeOrderOfArray()
    {

        int iterationNum = 0;

        for (int i = 0; i < assignTargetID.Count; i++)
        {
            for (int j = 1; j < assignMoverID.Count - i; j++)
            {
                // 処理回数の値を増やします。
                iterationNum++;

                // 隣り合う要素と比較し、順序が逆であれば入れ替えます。
                if (assignMoverID[j] < assignMoverID[j - 1])
                {

                    // 配列の要素の交換を行います。
                    int moverTemp = assignMoverID[j];
                    assignMoverID[j] = assignMoverID[j - 1];
                    assignMoverID[j - 1] = moverTemp;

                    int targetTemp = assignTargetID[j];
                    assignTargetID[j] = assignTargetID[j - 1];
                    assignTargetID[j - 1] = targetTemp;
                }
            }

        }
    }


    //void MakeCSV()
    //{
    //    print("1111111111111111111111111111111111111111111");
    //        using (var fileStream = new FileStream(@"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\CSV\test.csv", FileMode.Open))
    //    {
    //        // ストリームの長さを0に設定します。
    //        // 結果としてファイルのサイズが0になります。
    //        fileStream.SetLength(0);
    //    }


    //    StreamWriter file = new StreamWriter(@"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\CSV\test.csv", true, Encoding.UTF8);
    //    for (int i = 0; i < assignMoverID.Count; i++)
    //    {
    //        file.WriteLine(string.Format("{0},{1}", assignMoverID[i], assignTargetID[i]));
    //    }
    //    file.Close();
    //}

}



//ver.before hashimotosan added 

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering;
//using System.Linq;
//using static UnityEngine.GraphicsBuffer;
//using Common;
//using System.Diagnostics;
//using System.IO;


//public class Manager : MonoBehaviour
//{
//    public GameObject busMaster;
//    public GameObject bus1;
//    public GameObject bus2;

//    private Transform[] moverGC;
//    private GameObject moverGO;
//    private Mover moverArray;

//    private List<int> _moverID = new List<int>();
//    private List<int> _targetID = new List<int>();
//    private List<int> _distance = new List<int>();

//    //kito added
//    private Transform[] arrayMover;
//    private Transform[] arrayTarget;
//    private int countMover;
//    private int countTarget;
//    private int countKickOfdistancePassive = 0;
//    [SerializeField]
//    private GameObject targetMaster;

//    private string pyExePath = @"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\Python\tst.exe";
//    private string pyCodePath = @"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\Python\tst.py";


//    private void Start()
//    {
//        moverGC = ComFunctions.GetChildren(busMaster.transform);
//        moverGO = moverGC[0].gameObject;
//        moverArray = moverGO.GetComponent<Mover>();
//        //Debug.Log(moverArray.moverID);

//        //added by kito
//        //confirm mover and Target's count
//        confirmMoverTargetCount();
//        pythonInterface();
//    }

//    private void Update()
//    {
//        for (int i = 0; i < _moverID.Count; i++) { UnityEngine.Debug.Log(_moverID[i]); }
//        for (int i = 0; i < _targetID.Count; i++) { UnityEngine.Debug.Log(_targetID[i]); }
//        for (int i = 0; i < _distance.Count; i++) { UnityEngine.Debug.Log(_distance[i]); }

//        //added by kito
//        //Check if Assign Buber is kicked by ALL Mover every frame
//        countDistancePassiveKick();
//    }


//    //moverクラスから各距離情報の変数を受け取ってManager上にデータを格納していく
//    public void distancePassive(int moverID, int[] targetID, int[] distance)
//    {
//        //1つのMoverに対し
//        _moverID.Add(moverID);

//        //複数のtargetと距離　moverで計算した配列を引数に入れていく

//        //リスト_targetIDにデータがなければ格納
//        //今入っている値と同じなら格納されない 異なるtargetIDが来ると入ってしまう
//        if (!_targetID.Contains(targetID[0]))
//        {
//            for (int i = 0; i < targetID.Length; i++)
//            {
//                _targetID.Add(targetID[i]);
//            }
//        }

//        //distanceリスト末尾からデータ格納
//        for (int i = 0; i < distance.Length; i++)
//        {
//            _distance.Add(distance[i]);
//        }

//        //added by kito
//        //coutup if Mover kicks passiveDistance
//        countKickOfdistancePassive += 1;

//    }


//    //added by kito
//    void confirmMoverTargetCount()
//    {
//        arrayMover = ComFunctions.GetChildren(busMaster.transform);
//        countMover = arrayMover.Length;

//        arrayTarget = ComFunctions.GetChildren(targetMaster.transform);
//        countTarget = arrayTarget.Length;
//    }

//    void countDistancePassiveKick()
//    {

//        if (countMover == countKickOfdistancePassive)
//        {
//            aasignMover();
//        }
//    }

//    void aasignMover()
//    {


//    }

//    void pythonInterface()
//    {
//        ProcessStartInfo processStartInfo = new ProcessStartInfo()
//        {
//            FileName = pyExePath, //実行するファイル(python)
//            UseShellExecute = false,//シェルを使うかどうか
//            CreateNoWindow = true, //ウィンドウを開くかどうか
//            RedirectStandardOutput = true, //テキスト出力をStandardOutputストリームに書き込むかどうか
//            Arguments = pyCodePath + " " + "MOID=0" +" "+ "TaID=3" + " "+ "Dis=32", //実行するスクリプト 引数(複数可)
//        };

//        //外部プロセスの開始
//        Process process = Process.Start(processStartInfo);

//        //ストリームから出力を得る
//        StreamReader streamReader = process.StandardOutput;
//        string str = streamReader.ReadLine();

//        //外部プロセスの終了
//        process.WaitForExit();
//        process.Close();

//        //実行
//        print(str);
//        UnityEngine.Debug.Log("python_---------------------------------------------------------");
//    }
//}


// using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering;
//using System.Linq;
//using static UnityEngine.GraphicsBuffer;
//using Common;

//public class Manager : MonoBehaviour
//{
//    public GameObject busMaster;
//    public GameObject bus1;
//    public GameObject bus2;

//    private Transform[] moverGC;
//    private GameObject moverGO;
//    private Mover moverArray;

//    private List<int> _moverID = new List<int>();
//    private List<int> _targetID = new List<int>();
//    private List<int> _distance = new List<int>();

//    private int[] targetID = new int[3] { 1, 2, 3 };
//    private int[] distance = new int[6] { 5, 6, 7, 8, 9, 10 };

//    //kito added
//    private Transform[] arrayMover;
//    private Transform[] arrayTarget;
//    private int countMover;
//    private int countTarget;
//    private int countKickOfdistancePassive;
//    [SerializeField]
//    private GameObject targetMaster;



//    private void Start()
//    {
//        moverGC = ComFunctions.GetChildren(busMaster.transform);
//        moverGO = moverGC[0].gameObject;
//        moverArray = moverGO.GetComponent<Mover>();
//        //Debug.Log(moverArray.moverID);

//        distancePassive(moverArray.PropertyMoveID, targetID, distance);
//        Debug.Log(_moverID[0]);
//        Debug.Log(_targetID[0]);
//        Debug.Log(_targetID[1]);
//        Debug.Log(_targetID[2]);
//        Debug.Log(_distance[0]);
//        Debug.Log(_distance[1]);
//        Debug.Log(_distance[2]);
//    }

//    //各Moverが向かうターゲットを決めて各moverに渡す
//    public void assignMover()
//    {
//        //全てのMover分実行する distance最小を探してどのtargetか判断する
//        for (int i = 1; i < _moverID.Count; i++)
//        {

//            {
//                //moverにtarget情報を渡す処理
//            }
//        }
//    }


//    public void distancePassive(int moverID, int[] targetID, int[] distance)
//    {

//        _moverID.Add(moverID);

//        if (!_targetID.Contains(targetID[0]))
//        {
//            for (int i = 0; i < targetID.Length; i++)
//            {
//                _targetID.Add(targetID[i]);
//            }
//        }


//        for (int i = 0; i < distance.Length; i++)
//        {
//            _distance.Add(distance[i]);
//        }
//    }

//    //added by kito
//    void confirmMoverTargetCount()
//    {
//        arrayMover = ComFunctions.GetChildren(busMaster.transform);
//        countMover = arrayMover.Length;

//        arrayTarget = ComFunctions.GetChildren(targetMaster.transform);
//        countTarget = arrayTarget.Length;
//    }

//    void countDistancePassiveKick()
//    {

//        if (countMover == countKickOfdistancePassive)
//        {
//            aasignMover();
//        }
//    }

//    void aasignMover()
//    {


//    }

//}

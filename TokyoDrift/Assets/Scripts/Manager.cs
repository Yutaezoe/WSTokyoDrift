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
    private Transform[] moverGC;
    private GameObject moverGO;
    private Mover moverArray;
    private List<int> _moverID = new List<int>();
    private List<int> _targetID = new List<int>();
    private List<int> _distance = new List<int>();
    //kito added
    private Transform[] arrayMover;
    private Transform[] arrayTarget;
    private int countMover;
    private int countTarget;
    private int countKickOfdistancePassive = 0;
    [SerializeField]
    private GameObject targetMaster;
    private string pyExePath = @"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\Python\tst.exe";
    private string pyCodePath = @"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\Python\tst.py";
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
        for (int i = 0; i < _moverID.Count; i++) { UnityEngine.Debug.Log("----------------------------------------------"+_moverID[i]); }
        for (int i = 0; i < _targetID.Count; i++) { UnityEngine.Debug.Log(_targetID[i]); }
        for (int i = 0; i < _distance.Count; i++) { UnityEngine.Debug.Log(_distance[i]); }
        //added by kito
        //Check if Assign Buber is kicked by ALL Mover every frame
        countDistancePassiveKick();
    }
    //moverクラスから各距離情報の変数を受け取ってManager上にデータを格納していく
    //moverIDの取得順に懸念あり→実装後確認
    //changed hashimoto
    public void distancePassive(int moverID, int[] targetID, int[] distance)
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
            aasignMover();
        }
    }
    //add Hashimoto
    void aasignMover()
    {

        //make a CSVfile.
        MakeCSVdistancePassive();

        //start python.
      var i =  pythonInterface();

        //read assing result.
        readCSVAssignMover();




        //var testComponent = mainCamera.GetComponent<test>();
        //var test2Component = mainCamera.GetComponent<test2>();
        //var target1Component = target1.GetComponent<Target>();
        //var target2Component = target2.GetComponent<Target>();
        ////moverの数だけ繰り返す
        //for (int i = 0; i < _moverID.Count; i++)
        //{
        //    switch (_moverID[i])
        //    {
        //        case 0:
        //            //moverに渡す値たち
        //            testComponent.TargetIDPropertyInt = _targetID[i];
        //            UnityEngine.Debug.Log("これはManager側のtest1:" + testComponent.mover1targetID1propertyInt);
        //            //情報渡したらtargetステータスをCOMPLETED
        //            target1Component.SetStatusOfPikkingCOMPLETED();
        //            break;
        //        case 1:
        //            test2Component.TargetID2PropertyInt = _targetID[i];
        //            UnityEngine.Debug.Log("これはManager側のtest2:" + test2Component.mover2targetID1propertyInt);
        //            target2Component.SetStatusOfPikkingCOMPLETED();
        //            break;
        //    }
        //}
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

        using (var fileStream = new FileStream(@"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\CSV\toPython.csv", FileMode.Open))
        {
            // ストリームの長さを0に設定します。
            // 結果としてファイルのサイズが0になります。
            fileStream.SetLength(0);
        }


        StreamWriter file = new StreamWriter(@"C:\Users\13074\Desktop\newWorkspace\WSTokyoDrift\TokyoDrift\Assets\CSV\toPython.csv", true, Encoding.UTF8);
        for (int i = 0; i < _moverID.Count; i++)
        {
            file.WriteLine(string.Format("{0},{1},{2}", _moverID[i], _targetID[i], _distance[i]));
        }
        file.Close();
    }


    void readCSVAssignMover()
    {

    }
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

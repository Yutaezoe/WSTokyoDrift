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
    //add hashimoto
    [SerializeField] private GameObject logger;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject targetMaster;

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

    //state relative path
    static string path = Directory.GetCurrentDirectory();

    private string pyExePath = path + @"\Assets\Python\sample_select_04.exe";
    private string pyCodePath = path + @"\Assets\Python\sample_select_04.py";


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
    public bool distancePassive(int moverID, int[] targetID, int[] distance)
    {
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

        using (var fileStream = new FileStream(path+ @"\Assets\Python\csv\toPython.csv", FileMode.Open))
        {
            //Delate toPython.csv
            // ストリームの長さを0に設定します。
            // 結果としてファイルのサイズが0になります。
            fileStream.SetLength(0);
        }


        StreamWriter file = new StreamWriter(path+@"\Assets\Python\csv\toPython.csv", true, Encoding.UTF8);
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
        string filePath = path + @"\Assets\CSV\toCS.csv";
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

}
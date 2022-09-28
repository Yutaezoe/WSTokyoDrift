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
    //mover�N���X����e�������̕ϐ����󂯎����Manager��Ƀf�[�^���i�[���Ă���
    //moverID�̎擾���Ɍ��O���聨������m�F
    //changed hashimoto
    public bool distancePassive(int moverID, int[] targetID, int[] distance)
    {

        //���炤targetID�͑S�ē����l�z��
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
            FileName = pyExePath, //���s����t�@�C��(python)
            UseShellExecute = false,//�V�F�����g�����ǂ���
            CreateNoWindow = true, //�E�B���h�E���J�����ǂ���
            RedirectStandardOutput = true, //�e�L�X�g�o�͂�StandardOutput�X�g���[���ɏ������ނ��ǂ���
            Arguments = pyCodePath, //���s����X�N���v�g ����(������)        + " " + "MOID=0" + " " + "TaID=3" + " " + "Dis=32"
        };
        //�O���v���Z�X�̊J�n
        Process process = Process.Start(processStartInfo);
        //�X�g���[������o�͂𓾂�
        StreamReader streamReader = process.StandardOutput;
        string str = streamReader.ReadLine();
        //�O���v���Z�X�̏I��
        process.WaitForExit();
        process.Close();
        //���s
        print(str);

        return 0;
    }


    void MakeCSVdistancePassive()
    {

        using (var fileStream = new FileStream(@"C:\Users\13068\dojo\WSTokyoDrift0928\WSTokyoDrift\TokyoDrift\Assets\CSV\toPython.csv", FileMode.Open))
        {
            //Delate toPython.csv
            // �X�g���[���̒�����0�ɐݒ肵�܂��B
            // ���ʂƂ��ăt�@�C���̃T�C�Y��0�ɂȂ�܂��B
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
        // CSV�t�@�C���̓ǂݍ���
        string filePath = @"C:\Users\13068\dojo\WSTokyoDrift0928\WSTokyoDrift\TokyoDrift\Assets\CSV\toCS.csv";
        // StreamReader�N���X���C���X�^���X��
        StreamReader reader = new StreamReader(filePath, Encoding.GetEncoding("UTF-8"));
        // �Ō�܂œǂݍ���
        while (reader.Peek() >= 0)
        {
            // �ǂݍ��񂾕�������J���}��؂�Ŕz��Ɋi�[
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
                // �����񐔂̒l�𑝂₵�܂��B
                iterationNum++;

                // �ׂ荇���v�f�Ɣ�r���A�������t�ł���Γ���ւ��܂��B
                if (assignMoverID[j] < assignMoverID[j - 1])
                {

                    // �z��̗v�f�̌������s���܂��B
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
    //        // �X�g���[���̒�����0�ɐݒ肵�܂��B
    //        // ���ʂƂ��ăt�@�C���̃T�C�Y��0�ɂȂ�܂��B
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


//    //mover�N���X����e�������̕ϐ����󂯎����Manager��Ƀf�[�^���i�[���Ă���
//    public void distancePassive(int moverID, int[] targetID, int[] distance)
//    {
//        //1��Mover�ɑ΂�
//        _moverID.Add(moverID);

//        //������target�Ƌ����@mover�Ōv�Z�����z��������ɓ���Ă���

//        //���X�g_targetID�Ƀf�[�^���Ȃ���Ίi�[
//        //�������Ă���l�Ɠ����Ȃ�i�[����Ȃ� �قȂ�targetID������Ɠ����Ă��܂�
//        if (!_targetID.Contains(targetID[0]))
//        {
//            for (int i = 0; i < targetID.Length; i++)
//            {
//                _targetID.Add(targetID[i]);
//            }
//        }

//        //distance���X�g��������f�[�^�i�[
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
//            FileName = pyExePath, //���s����t�@�C��(python)
//            UseShellExecute = false,//�V�F�����g�����ǂ���
//            CreateNoWindow = true, //�E�B���h�E���J�����ǂ���
//            RedirectStandardOutput = true, //�e�L�X�g�o�͂�StandardOutput�X�g���[���ɏ������ނ��ǂ���
//            Arguments = pyCodePath + " " + "MOID=0" +" "+ "TaID=3" + " "+ "Dis=32", //���s����X�N���v�g ����(������)
//        };

//        //�O���v���Z�X�̊J�n
//        Process process = Process.Start(processStartInfo);

//        //�X�g���[������o�͂𓾂�
//        StreamReader streamReader = process.StandardOutput;
//        string str = streamReader.ReadLine();

//        //�O���v���Z�X�̏I��
//        process.WaitForExit();
//        process.Close();

//        //���s
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

//    //�eMover���������^�[�Q�b�g�����߂Ċemover�ɓn��
//    public void assignMover()
//    {
//        //�S�Ă�Mover�����s���� distance�ŏ���T���Ăǂ�target�����f����
//        for (int i = 1; i < _moverID.Count; i++)
//        {

//            {
//                //mover��target����n������
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

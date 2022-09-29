using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Common
{

    [Serializable]
    public struct SaveData
    {
        public int[] moverUid;
        public Vector3[] moverVector;
        public Vector3[] targetVector;
        public Vector3 targetGoalVector;

    }

    public static class SaveManager
    {

        public static SaveData saveData;
        const string SAVE_FILE_PATH = "save.json";


        public static void SaveMoverVector(Vector3[] _moverVector)
        {
            saveData.moverVector = _moverVector;
            Save();
        }

        public static void SaveTargetVector(Vector3[] _targetVector)
        {
            saveData.targetVector = _targetVector;
            Save();
        }

        public static void SaveMoverUID(int[] _moverUid)
        {
            saveData.moverUid = _moverUid;
            Save();
        }

        public static void SaveGoalTargetVector(Vector3 _targetGoalVector)
        {
            saveData.targetGoalVector = _targetGoalVector;
            Save();
        }



        public static void Save()
        {
            string json = JsonUtility.ToJson(saveData);
#if UNITY_EDITOR
            string path = Directory.GetCurrentDirectory();
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif
            //Debug.Log(path);
            path += ("/" + SAVE_FILE_PATH);
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(json);
            writer.Flush();
            writer.Close();
        }

        public static void Load()
        {
            try
            {
#if UNITY_EDITOR
                string path = Directory.GetCurrentDirectory();
#else
        string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#endif
                FileInfo info = new FileInfo(path + "/" + SAVE_FILE_PATH);
                StreamReader reader = new StreamReader(info.OpenRead());
                string json = reader.ReadToEnd();
                saveData = JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception)
            {
                saveData = new SaveData();
            }
        }
    }

}

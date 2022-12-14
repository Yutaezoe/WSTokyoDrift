using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

	[SerializeField]
	private int minute;
	[SerializeField]
	private float seconds;
	//　前のUpdateの時の秒数
	private float oldSeconds;
	//　タイマー表示用テキスト
	private string timerText="00:00";
	bool timerStatus = false;

	void Start()
	{
		minute = 0;
		seconds = 0f;
		oldSeconds = 0f;
	}

	void Update()
	{
		if (timerStatus == true)
		{
			seconds += Time.deltaTime;
			if (seconds >= 60f)
			{
				minute++;
				seconds = seconds - 60;
			}
			//　値が変わった時だけテキストUIを更新
			if ((int)seconds != (int)oldSeconds)
			{
				timerText = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
			}
			oldSeconds = seconds;
		}
	}

	public string GetTimerCount
    {
        get { return timerText; }
    }

	public void StartTimer()
    {
		timerStatus = true;
    }
}
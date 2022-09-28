using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

	[SerializeField]
	private int minute;
	[SerializeField]
	private float seconds;
	//�@�O��Update�̎��̕b��
	private float oldSeconds;
	//�@�^�C�}�[�\���p�e�L�X�g
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
			//�@�l���ς�����������e�L�X�gUI���X�V
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
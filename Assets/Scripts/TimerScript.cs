using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    public decimal time = 0.00M;
    bool timerActive = false;
    public Text timerText;

	// Use this for initialization
	void Start () {
        timerText.text = "Time\n" + time.ToString();
        startTimer();
	}
	
	// Update is called once per frame
	void Update () {
        if (timerActive)
        {
            time += Math.Round((decimal)Time.deltaTime,2);
            timerText.text = "Time\n" + time.ToString();
        }
	}

    public void startTimer()
    {
        timerActive = true;
    }

    public void stopTimer()
    {
        timerActive = false;
    }
}

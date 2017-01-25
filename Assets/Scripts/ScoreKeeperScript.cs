using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeperScript : MonoBehaviour {

    public string time;
    public string moves;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void getScores()
    {
        time = GameObject.Find("Timer").GetComponent<TimerScript>().time.ToString();
        moves = GameObject.Find("MoveCounter").GetComponent<MoveCounterScript>().moves.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour {

    public Text timeText;
    public Text movesText;

    GameObject scoreKeeper;
    string time;
    string moves;

	// Use this for initialization
	void Start () {
        scoreKeeper = GameObject.Find("ScoreKeeper");
        ScoreKeeperScript scoreKeeperScript = scoreKeeper.GetComponent<ScoreKeeperScript>();
        time = scoreKeeperScript.time;
        moves = scoreKeeperScript.moves;
        timeText.text = "Time: " + time;
        movesText.text = "Moves: " + moves;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("4"))
        {
            SceneManager.LoadScene("Tile Game 4 Letter");
        }

        if (Input.GetKeyDown("5"))
        {
            SceneManager.LoadScene("Tile Game 5 Letter");
        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    public void Load4By4()
    {
        SceneManager.LoadScene("Tile Game 4 Letter");
    }

    public void Load5By5()
    {
        SceneManager.LoadScene("Tile Game 5 Letter");
    }
}

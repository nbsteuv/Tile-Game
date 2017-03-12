using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WinScript : MonoBehaviour {

    public GameObject playAgainText;

    GameObject scoreKeeper;
    string time;
    string moves;

	// Use this for initialization
	void Start () {
        scoreKeeper = GameObject.Find("ScoreKeeper");
        ScoreKeeperScript scoreKeeperScript = scoreKeeper.GetComponent<ScoreKeeperScript>();
        time = scoreKeeperScript.time;
        moves = scoreKeeperScript.moves;
        playAgainText.GetComponent<TextMesh>().text = "It only took you " + time + " seconds\nand " + moves + " moves.\nPress 4 or 5 to play again.";
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
}

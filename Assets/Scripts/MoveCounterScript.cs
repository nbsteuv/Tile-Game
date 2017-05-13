using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCounterScript : MonoBehaviour {

    public int moves = 0;
    public Text movesText;

	// Use this for initialization
	void Start () {
        movesText.text = "Moves\n" + moves.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void incrementMoves()
    {
        moves++;
        movesText.text = "Moves\n" + moves.ToString();
    }
}

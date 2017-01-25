using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCounterScript : MonoBehaviour {

    public int moves = 0;
    TextMesh textMesh;

	// Use this for initialization
	void Start () {
        textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = "Moves\n" + moves.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void incrementMoves()
    {
        moves++;
        textMesh.text = "Moves\n" + moves.ToString();
    }
}

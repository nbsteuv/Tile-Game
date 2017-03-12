using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public GameObject tileTextPrefab;

	// Use this for initialization
	void Start () {
        GameObject tileText = (GameObject)Instantiate(tileTextPrefab, transform.position, Quaternion.identity);
        TextMesh tileTextMesh = tileText.GetComponent<TextMesh>();
        tileTextMesh.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

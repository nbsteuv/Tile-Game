using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public GameObject tileTextPrefab;
    public string tileTextContent;

    // Use this for initialization
    void Start () {
        GameObject tileText = (GameObject)Instantiate(tileTextPrefab, transform.position, Quaternion.identity);
        TextMesh tileTextMesh = tileText.GetComponent<TextMesh>();
        tileTextMesh.text = tileTextContent;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public delegate void TileClickedEventHandler(object source, EventArgs args);
    public event TileClickedEventHandler TileClicked;
    public virtual void OnTileClicked()
    {
        if(TileClicked != null)
        {
            TileClicked(this, EventArgs.Empty);
        }
        //Tileclicked?.Invoke(this, EventArgs.Empty);--Better?
    }

    private void OnMouseDown()
    {
        OnTileClicked();
    }

}

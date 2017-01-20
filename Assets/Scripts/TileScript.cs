using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public GameObject tileTextPrefab;
    public string tileTextContent;
    public float moveSpeed;

    Vector3 targetPosition;

    // Use this for initialization
    void Start () {
        targetPosition = transform.position;
        GameObject tileText = (GameObject)Instantiate(tileTextPrefab, transform.position, Quaternion.identity);
        TextMesh tileTextMesh = tileText.GetComponent<TextMesh>();
        tileTextMesh.text = tileTextContent;
	}
	
	// Update is called once per frame
	void Update () {
		if(targetPosition != transform.position)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
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

    public void move(Vector3 target)
    {
        targetPosition = target;
    }



}

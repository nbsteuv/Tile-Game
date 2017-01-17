using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    public TextAsset threeWordList;
    public TextAsset fourWordList;

	// Use this for initialization
	void Start () {
        string word = getRandomWordFromList(threeWordList);
        Debug.Log(word);
        string secondWord = getRandomWordFromList(fourWordList);
        Debug.Log(secondWord);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    string getRandomWordFromList(TextAsset wordList)
    {
        string[] wordArray = getArrayFromWordList(wordList);
        string randomWord = getRandomWordFromArray(wordArray);
        return randomWord;
    }

    string[] getArrayFromWordList(TextAsset wordList)
    {
        string[] wordArray = wordList.text.Split("\n"[0]);
        return wordArray;
    }

    string getRandomWordFromArray(string[] wordArray)
    {
        int randomIndex = Random.Range(0, wordArray.Length);
        string randomWord = wordArray[randomIndex].Trim();
        return randomWord;
    }

}

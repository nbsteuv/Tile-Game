using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    public TextAsset threeWordList;
    public TextAsset fourWordList;

    List<string> gameWords = new List<string>();

	// Use this for initialization
	void Start () {
        setGameWords();
        for(int i = 0; i < gameWords.Count; i++)
        {
            Debug.Log(gameWords[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setGameWords()
    {
        //TODO: Replace i and wordList values based on selected number of squares 
        for(int i = 0; i < 3; i++)
        {
            setUniqueWord(fourWordList);
        }
        setUniqueWord(threeWordList);
    }

    void setUniqueWord(TextAsset wordList)
    {
        bool gameWordInserted = false;
        while (!gameWordInserted)
        {
            string newWord = getRandomWordFromList(wordList);
            gameWordInserted = insertGamewordIfUnique(newWord);
        }
    }

    bool insertGamewordIfUnique(string word)
    {
        if (gameWords.Contains(word))
        {
            return false;
        } else
        {
            gameWords.Add(word);
            return true;
        }
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

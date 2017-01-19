using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    public TextAsset threeWordList;
    public TextAsset fourWordList;
    public GameObject tilePrefab;

    List<string> gameWords = new List<string>();
    List<char> letterKey = new List<char>();
    List<char> randomizedLetterKey = new List<char>();
    System.Random rnd = new System.Random();

	// Use this for initialization
	void Start () {
        setGameWords();
        setLetterKey();
        setRandomizedLetterKey();
        instantiateTiles();

        //Test code
        string test = "";
        foreach(char letter in randomizedLetterKey)
        {
            test += letter + " - ";
        }
        Debug.Log(test);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Instantiate all blocks----------------------------------------------------------------------
    
    void instantiateTiles()
    {
        Vector3 tilePosition = calculateInitialVector();
        for(int i = 0; i < randomizedLetterKey.Count; i++)
        {
            generateTile(tilePosition, randomizedLetterKey[i]);
            tilePosition = incrementTilePosition(tilePosition);
        }
    }

    void generateTile(Vector3 tilePosition, char tileText)
    {
        GameObject tile = (GameObject)Instantiate(tilePrefab, tilePosition, Quaternion.Euler(-90, 0, 0));
        TileScript tileScript = tile.GetComponent<TileScript>();
        tileScript.tileTextContent = tileText.ToString();
    }

    //TODO: Calculate based on screen size

    Vector3 calculateInitialVector()
    {
        Vector3 startingPosition = new Vector3(0, 0, -1);
        return startingPosition;
    }

    Vector3 incrementTilePosition(Vector3 currentPosition)
    {
        Vector3 positionShift = new Vector3(1, 1, 0);
        Vector3 newPosition = currentPosition + positionShift;
        return newPosition;
    }

    //Randomize answer key to create game tiles----------------------------------------------------

    void setRandomizedLetterKey()
    {
        randomizedLetterKey = shuffleList(letterKey);
    }

    List<char> shuffleList(List<char> listToShuffle)
    {
        List<char> listClone = cloneList(listToShuffle);
        for(int i = 0; i < listToShuffle.Count; i++)
        {
            listSwap(listClone, i, rnd.Next(i, listClone.Count));
        }
        return listClone;
    }
    
    List<char> cloneList(List<char> listToClone)
    {
        List<char> listClone = new List<char>();
        for(int i = 0; i < listToClone.Count; i++)
        {
            listClone.Add(listToClone[i]);
        }
        return listClone;
    }
    
    void listSwap(List<char> listToSwap, int i, int j)
    {
        char temp = listToSwap[i];
        listToSwap[i] = listToSwap[j];
        listToSwap[j] = temp;
    }

    //Create answer key for tile game---------------------------------------------------------------

    void setLetterKey()
    {
        for(int i = 0; i < gameWords.Count; i++)
        {
            string gameWord = gameWords[i];
            char[] wordCharacters = gameWord.ToCharArray();
            for(int j = 0; j < wordCharacters.Length; j++)
            {
                char wordCharacter = wordCharacters[j];
                letterKey.Add(wordCharacter);
            }
        }
    }
    
    //Create word list from text assets--------------------------------------------------------------

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
        int randomIndex = UnityEngine.Random.Range(0, wordArray.Length);
        string randomWord = wordArray[randomIndex].Trim();
        return randomWord;
    }

}

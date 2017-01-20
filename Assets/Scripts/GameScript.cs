using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    public TextAsset threeWordList;
    public TextAsset fourWordList;
    public GameObject tilePrefab;
    public int gridSquares;
    public float gridBuffer;
    public float screenPercentageToGrid;

    float screenHeight;
    float gridSize;
    int bufferCount;
    float squareSize;
    Vector3 emptyPosition;

    List<string> gameWords = new List<string>();
    List<char> letterKey = new List<char>();
    List<char> randomizedLetterKey = new List<char>();
    Dictionary<int, GameObject> squarePositions = new Dictionary<int, GameObject>();
    Dictionary<int, int> indexAtPosition = new Dictionary<int, int>();
    System.Random rnd = new System.Random();

	// Use this for initialization
	void Start () {
        setScreenValues();
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
        Debug.Log(emptyPosition);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Move tile and reset empty position marker-------------------------------------------------------

    void swapPositionWithEmpty(TileScript tileScript)
    {
        Vector3 tilePosition = tileScript.transform.position;
        int tileIndex = indexAtPosition[tilePosition.GetHashCode()];
        int emptyIndex = indexAtPosition[emptyPosition.GetHashCode()];
        listSwap(randomizedLetterKey, tileIndex, emptyIndex);
        tileScript.move(emptyPosition);
        squarePositions[emptyPosition.GetHashCode()] = tileScript.gameObject;
        emptyPosition = tilePosition;
    }

    //Fire event on tile click-----------------------------------------------------------------------

    void onTileClicked(object source, EventArgs args)
    {
        TileScript tileScript = (TileScript)source;
        swapPositionWithEmpty(tileScript);
    }

    //Keep track of empty placeholder----------------------------------------------------------------

    void addEndingPlaceHolder(Vector3 placeHolderPosition)
    {
        char space = ' ';
        letterKey.Add(space);
        randomizedLetterKey.Add(space);
        GameObject emptyGameObject = null;
        squarePositions.Add(placeHolderPosition.GetHashCode(), emptyGameObject);
        indexAtPosition.Add(placeHolderPosition.GetHashCode(), indexAtPosition.Count);
        emptyPosition = placeHolderPosition;
    }

    //Get screen values for block instantiation-------------------------------------------------------

    void setScreenValues()
    {
        setScreenHeight();
        setGridSize();
        setSquareSize();
    }
    
    void setScreenHeight()
    {
        screenHeight = Camera.main.orthographicSize * 2;
    }

    void setGridSize()
    {
        gridSize = (screenPercentageToGrid / 100) * screenHeight;
    }

    void setSquareSize()
    {
        int bufferCount = gridSquares + 1;
        squareSize = (gridSize - (gridBuffer * bufferCount)) / gridSquares;
    }

    //Instantiate all blocks and subscribe to click events----------------------------------------------------------------------

    void instantiateTiles()
    {
        Vector3 tilePosition = calculateInitialVector();
        for(int i = 0; i < randomizedLetterKey.Count; i++)
        {
            GameObject newTile = generateTile(tilePosition, randomizedLetterKey[i]);
            subscribeToTileClickEvent(newTile);
            indexAtPosition.Add(tilePosition.GetHashCode(), i);
            //squarePositions dictionary might not be necessary after all, but could be used to highlight posible moves in tutorial
            squarePositions.Add(tilePosition.GetHashCode(), newTile);
            tilePosition = incrementTilePosition(tilePosition);
        }
        addEndingPlaceHolder(tilePosition);
    }

    void subscribeToTileClickEvent(GameObject tile)
    {
        tile.GetComponent<TileScript>().TileClicked += onTileClicked;
    }

    GameObject generateTile(Vector3 tilePosition, char tileText)
    {
        GameObject tile = (GameObject)Instantiate(tilePrefab, tilePosition, Quaternion.Euler(-90, 0, 0));
        float squareScale = squareSize / 10;
        tile.transform.localScale = new Vector3(squareScale, squareScale, squareScale);
        TileScript tileScript = tile.GetComponent<TileScript>();
        tileScript.tileTextContent = tileText.ToString().ToUpper();
        return tile;
    }
    
    Vector3 calculateInitialVector()
    {
        //Calculate Y value of first square, first find origin relative to screen bottom
        float originY = 0.5f * screenHeight;
        float squareY = gridSize - (originY + gridBuffer + (0.5f * squareSize));

        //Calculate X value of first square
        float squareX = -((0.5f * gridSize) - (gridBuffer + (0.5f * squareSize)));

        Vector3 startingPosition = new Vector3(squareX, squareY, -1);
        return startingPosition;
    }

    Vector3 incrementTilePosition(Vector3 currentPosition)
    {
        float positionShift = gridBuffer + squareSize;
        float newX;
        float newY;

        if(currentPosition.x + positionShift > 0.5f * gridSize)
        {
            newX = currentPosition.x - (positionShift * (gridSquares - 1));
            newY = currentPosition.y - positionShift;
        } else
        {
            newX = currentPosition.x + positionShift;
            newY = currentPosition.y;
        }

        return new Vector3(newX, newY, -1);
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

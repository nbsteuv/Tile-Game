using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour {

    public TextAsset threeWordList;
    public TextAsset fourWordList;
    public GameObject tilePrefab;
    public GameObject wordDisplayTextPrefab;
    public GameObject timerDisplayPrefab;
    public int gridSquares;
    public float gridBuffer;
    public float screenPercentageToGrid;
    public float pauseSecondsAfterWin;
    public bool limitMovement;

    float screenHeight;
    float gridSize;
    int bufferCount;
    float squareSize;
    float displayHeight;
    float displayPanelWidth;
    float displayDistanceAboveOrigin;
    Vector3 emptyPosition;
    int squareMapX = 0;
    int squareMapY = 0;

    List<string> gameWords = new List<string>();
    List<char> letterKey = new List<char>();
    List<char> randomizedLetterKey = new List<char>();
    Dictionary<int, GameObject> squarePositions = new Dictionary<int, GameObject>();
    Dictionary<int, int> indexAtPosition = new Dictionary<int, int>();
    Dictionary<int, Vector2> squareMap = new Dictionary<int, Vector2>();
    System.Random rnd = new System.Random();

	// Use this for initialization
	void Start () {
        setScreenValues();
        setGameWords();
        setLetterKey();
        setRandomizedLetterKey();
        instantiateTiles();
        generateDisplayPanels();

        //Test code
        foreach (string word in gameWords)
        {
            Debug.Log(word);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            toggleLimitMovement();
        }

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
	}

    //Displayer timer------------------------------------------------------------------------------

    void instantiateTimer()
    {
        Vector3 timerPosition = calculateTimerPosition();
        GameObject timer = (GameObject)Instantiate(timerDisplayPrefab, timerPosition, Quaternion.identity);
        timer.gameObject.name = "Timer";
        timer.GetComponent<TimerScript>().startTimer();
    }

    Vector3 calculateTimerPosition()
    {
        float positionX = 0;
        Debug.Log(displayHeight);
        float positionY = displayDistanceAboveOrigin + (displayHeight / 2);
        float positionZ = -1;
        Vector3 position = new Vector3(positionX, positionY, positionZ);
        return position;
    }

    //Limit swap to positions next to the empty square---------------------------------------------

    bool checkEmptyAdjacent(TileScript tileScript)
    {
        if (!limitMovement)
        {
            return true;
        }
        Vector3 tilePosition = tileScript.transform.position;
        Vector2 mappedTilePosition = squareMap[tilePosition.GetHashCode()];
        Vector2 mappedEmptyPosition = squareMap[emptyPosition.GetHashCode()];

        float differenceFound = Math.Abs(mappedTilePosition.x - mappedEmptyPosition.x) + Math.Abs(mappedTilePosition.y - mappedEmptyPosition.y);

        if(differenceFound == 1)
        {
            return true;
        } else
        {
            return false;
        }
    }

    void addPositionToSquareMap(Vector3 position)
    {
        squareMap.Add(position.GetHashCode(), new Vector2(squareMapX, squareMapY));
        squareMapX++;
        if(squareMapX > gridSquares - 1)
        {
            squareMapX = 0;
            squareMapY++;
        }
    }

    void toggleLimitMovement()
    {
        limitMovement = !limitMovement;
    }

    //Create display for words, time, and move count-----------------------------------------------

    void generateDisplayPanels()
    {
        instantiateGameWordDisplay();
        instantiateTimer();
    }

    void instantiateGameWordDisplay()
    {
        Vector3 wordPosition = calculateInitialWordPosition();
        foreach(string word in gameWords)
        {
            GameObject gameWord = (GameObject)Instantiate(wordDisplayTextPrefab, wordPosition, Quaternion.identity);
            gameWord.GetComponent<TextMesh>().text = word.ToUpper();
            wordPosition = incrementWordPosition(wordPosition);
        }
    }

    Vector3 calculateInitialWordPosition()
    {
        float initialWordPositionX = -((Camera.main.aspect * screenHeight) / 2);
        float initialWordPositionY = screenHeight / 2;
        float initialWordPositionZ = -1;
        Vector3 wordPosition = new Vector3(initialWordPositionX, initialWordPositionY, initialWordPositionZ);
        return wordPosition;
    }

    Vector3 incrementWordPosition(Vector3 currentPosition)
    {
        float currentX = currentPosition.x;
        float currentY = currentPosition.y;
        float currentZ = currentPosition.z;
        float newY = currentY - 2;
        Vector3 newPosition = new Vector3(currentX, newY, currentZ);
        return newPosition;
    }
    
    //Create win condition and effect---------------------------------------------------------------

    void compareBoardToKey()
    {
        if(letterKey.SequenceEqual(randomizedLetterKey))
        {
            Invoke("winGame", pauseSecondsAfterWin);
        }
    }

    void winGame()
    {
        SceneManager.LoadScene("Win Screen");
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
        compareBoardToKey();
    }

    //Fire event on tile click-----------------------------------------------------------------------

    void onTileClicked(object source, EventArgs args)
    {
        TileScript tileScript = (TileScript)source;
        if (checkEmptyAdjacent(tileScript))
        {
            swapPositionWithEmpty(tileScript);
        }
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
        addPositionToSquareMap(placeHolderPosition);
        emptyPosition = placeHolderPosition;
    }

    //Get screen values for block instantiation-------------------------------------------------------

    void setScreenValues()
    {
        setScreenHeight();
        setGridSize();
        setSquareSize();
        setDisplayHeight();
        setDisplayDistanceAboveOrigin();
        setDisplayPanelWidth();

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

    void setDisplayHeight()
    {
        displayHeight = ((100 - screenPercentageToGrid) / 100) * screenHeight;
    }

    void setDisplayPanelWidth()
    {
        displayPanelWidth = (Camera.main.aspect * screenHeight) / 3;
    }

    void setDisplayDistanceAboveOrigin()
    {
        displayDistanceAboveOrigin = gridSize - (screenHeight / 2);
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
            addPositionToSquareMap(tilePosition);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// give a question
/// </summary>
public class SettingQuestion : IntEventInvoker {

    #region Field
    //save for efficiency
    GameObject cupPrefab;

    Timer spawnPickupTimer;

    //support speed down
    bool isSpeedDown;

    //support cup level (difficulty)
    int cupNumber;
    int gemNumber;// number of cup which contain gem(s)

    readonly Vector3[][] originalCupPositions = {
        // 2 cups
        new Vector3[] { new Vector3(-3, 0, 0), new Vector3(3, 0, 0)}, 
        // 3 cups
        new Vector3[] { new Vector3(0, 2, 0), new Vector3(-3, -2, 0), new Vector3(3, -2, 0) },
        // 4 cups
        new Vector3[] { new Vector3(3, 2, 0), new Vector3(-3, 2, 0),
                        new Vector3(3, -2, 0), new Vector3(-3, -2, 0)},
        // 5 cups
        new Vector3[] { new Vector3(3, 2, 0), new Vector3(-3, 2, 0),
                        new Vector3(0, -2, 0), new Vector3(-4.6f, -2, 0), new Vector3(4.6f, -2, 0)},
        // 6 cups
        new Vector3[] { new Vector3(-4.6f, 2, 0), new Vector3(0, 2, 0), new Vector3(4.6f, 2, 0),
                        new Vector3(-4.6f, -2, 0), new Vector3(0, -2, 0), new Vector3(4.6f, -2, 0)},
        // 7 cups
        new Vector3[] { new Vector3(-4.6f, 2, 0), new Vector3(0, 2, 0), new Vector3(4.6f, 2, 0),
                        new Vector3(-5, -2, 0), new Vector3(-1.6f, -2, 0),
                        new Vector3(1.6f, -2, 0), new Vector3(5, -2, 0),
                        }
    };
    
    #endregion

    #region Properties
    /// <summary>
    /// return how many cups contain gems
    /// </summary>
    public int AnswerNumber
    {
        get { return gemNumber; }
    }

    /// <summary>
    /// return how many cups in the question
    /// </summary>
    public int CupNumber
    {
        get { return cupNumber; }
    }
    #endregion

    #region Private Methods
    // Use this for initialization
    void Start () {
        cupPrefab = Resources.Load<GameObject>("OpenedCup");
        //InitailizeCupPosition();
        EventManager.AddListener(EventName.GiveNewQuestionEvent, SpawnQuestion);


        //support spawning pickup
        spawnPickupTimer = gameObject.AddComponent<Timer>();
        spawnPickupTimer.AddTimerFinishedEventListener(HandleSpawnPickupTimerFinishedEvent);
        //show cup duration + close cup duration + rand - pickup life time
        float spawntiming = 3.0f + Random.Range(0, DifficultyUtilities.MovingDuration) - DifficultyUtilities.PickupLifeTime;
        spawnPickupTimer.Duration = spawntiming;

        unityEvents.Add(EventName.SpawnPickupEvent, new SpawnPickupEvent());
        EventManager.AddInvoker(EventName.SpawnPickupEvent, this);

        //spawn a question
        int StartGameLevel = PlayerPrefs.GetInt(GameDataName.NowLevel.ToString(), 1);
        SpawnQuestion(StartGameLevel);


        isSpeedDown = false;
        EventManager.AddListener(EventName.SpeedDownEvent, HandleSpeedDownPickupEvent);
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    /// <summary>
    /// decide putting gems into which cups. 
    /// </summary>
    /// <returns>a array which contain the number of gems in each cup</returns>
    int[] SetGems(int level)
    {
        if (cupNumber == 0 || gemNumber == 0)
        {
            print("Gems, SettingDifficulty must execute first");
            return null;
        }

        int[] gemNumbers = new int[cupNumber];

        //give initial value
        for(int i = 0; i < gemNumber; i++)
        {
            gemNumbers[i] = 1;//i+1;
        }
        for (int i = gemNumber; i < cupNumber; i++)
        {
            gemNumbers[i] = 0;
        }

        //shuffle
        ShuffleArray(gemNumbers);

        return gemNumbers;
    }

    /// <summary>
    /// set the position which cups will return.
    /// </summary>
    /// <returns></returns>
    Vector3[] SetBackPosition()
    {
        if (cupNumber == 0 || gemNumber == 0)
        {
            print("BackPosition, SettingDifficulty must execute first");
            return null;
        }

        Vector3[] backPosition = new Vector3[cupNumber];

        //give initial value
        for (int i = 0; i < cupNumber; i++)
        {
            backPosition[i] = originalCupPositions[cupNumber - 2][i];
        }

        //shuffle
        ShuffleArray(backPosition);

        return backPosition;
    }

    /// <summary>
    /// shuffle a array
    /// </summary>
    /// <param name="array">array</param>
    void ShuffleArray<T>(T[] source)
    {
        if (source == null) return;

        int len = source.Length;
        int rand;
        T temp;

        for (int i=0; i < len-1; i++)
        {
            rand = Random.Range(i, len);

            if (i == rand)// same, does have to swap
                continue;

            temp = source[rand];
            source[rand] = source[i];
            source[i] = temp;
        }
    }


    /// <summary>
    /// sets Difficulty by setting cup Number and gem Number
    /// </summary>
    /// <param name="level">level of the player</param>
    void SettingDifficulty(int level)
    {
        if (level <= 0)
        {
            print("Error, level isn't in the range, level = " + level);
            return;
        }

        if (level <= 2)//1~2 (2)
        {
            cupNumber = 2;
            gemNumber = 1;
        }
        else if (level <= 5)// 3~5 (3)
        {
            cupNumber = 3;
            gemNumber = 1;
        }
        else if (level <= 8)// 6~8 (3)
        {
            cupNumber = 4;
            gemNumber = 1;
        }
        else if (level <= 11)// 9~11 (3)
        {
            cupNumber = 5;
            gemNumber = 1;
        }
        else if (level <= 15)// 12~15 (4)
        {
            cupNumber = 5;
            gemNumber = 2;
        }
        else if (level <= 19)// 16~19 (4)
        {
            cupNumber = 6;
            gemNumber = 2;
        }
        else if (level <= 24)// 20~24 (5)
        {
            cupNumber = 7;
            gemNumber = 2;
        }
        else if (level >= 25)// 25~
        {
            cupNumber = 7;
            gemNumber = 3;
        }

        //cupNumber = 7;
        //gemNumber = 3;
    }
    void TestSettingDifficulty(int level)
    {
        if (level <= 0)
        {
            print("Error, level isn't in the range, level = " + level);
            return;
        }

        if (level <= 1)//1
        {
            cupNumber = 2;
            gemNumber = 1;
        }
        else if (level <= 2)// 2
        {
            cupNumber = 3;
            gemNumber = 1;
        }
        else if (level <= 3)// 3
        {
            cupNumber = 4;
            gemNumber = 1;
        }
        else if (level <= 4)// 4
        {
            cupNumber = 5;
            gemNumber = 1;
        }
        else if (level <= 5)// 5
        {
            cupNumber = 5;
            gemNumber = 2;
        }
        else if (level <= 6)// 6
        {
            cupNumber = 6;
            gemNumber = 2;
        }
        else if (level <= 7)// 7
        {
            cupNumber = 7;
            gemNumber = 2;
        }
        else if (level >= 8)// 8~
        {
            cupNumber = 7;
            gemNumber = 3;
        }
        else
        {
            print("level is over");
        }

        //cupNumber = 7;
        //gemNumber = 3;
    }

    /// <summary>
    /// Handle the event for SpawnPickupTimer Finished
    /// </summary>
    void HandleSpawnPickupTimerFinishedEvent()
    {
        //spawn a pickup
        unityEvents[EventName.SpawnPickupEvent].Invoke(0);
    }

    /// <summary>
    /// Handle SpeedDown Pickup Event
    /// </summary>
    /// <param name="unuse">unuse</param>
    void HandleSpeedDownPickupEvent(int unuse)
    {
        isSpeedDown = true;
        //print("pick speed down");
    }

    #endregion



    #region Public Methods

    /// <summary>
    /// Spawn a new Question according to given level
    /// </summary>
    /// <param name="level">level of the player</param>
    public void SpawnQuestion(int level)
    {
        int[] gemNumberInCups;
        Vector3[] returnPositions;

        //SettingDifficulty(level);
        TestSettingDifficulty(level);
        gemNumberInCups = SetGems(level);
        returnPositions = SetBackPosition();

        //create cups on the scenes
        for (int i = 0; i < cupNumber; i++)
        {
            GameObject cup = GameObject.Instantiate(cupPrefab, originalCupPositions[cupNumber - 2][i], Quaternion.identity);
            Cup cupScript = cup.GetComponent<Cup>();
            cupScript.DistributeGem(gemNumberInCups[i]);
            cupScript.ReturnPosition = returnPositions[i];

            //print("i = "+i+", value = "+gemNumberInCups[i]);
            //change all cup's speed
            if (isSpeedDown)
            {
                //tell the cup to reduce speed
                cupScript.IsSpeedDown = true;
            }
            else
            {
                //tell the cup to use original speed
                cupScript.IsSpeedDown = false;
            }
        }

        if (isSpeedDown)
        {
            //only this game reduce speed
            isSpeedDown = false;
        }

        //show where gems are : 2s ,
        //close cup and wait : 1s
        spawnPickupTimer.Run();
    }

    #endregion




    /*
      /// <summary>
    /// gets a Cup Position
    /// </summary>
    /// <param name="cupNum"> how many cup in the scene</param>
    /// <param name="cupIdx"> cup idx </param>
    /// <returns></returns>
    public Vector3 OriginalCupPositions(int cupNum, int cupIdx)
    {
        return originalCupPositions[cupNum][cupIdx];
    }
      /// <summary>
    /// Initailize cup position
    /// </summary>
    void InitailizeCupPosition()
    {
        //announce array size
        originalCupPositions = new Vector3[6][];

        for(int i = 0; i < originalCupPositions.Length; i++)
        {
            originalCupPositions[i] = new Vector3[2 + i];
        }

        // 2 cups
        originalCupPositions[0][0] = new Vector3(-3, 0, 0);
        originalCupPositions[0][1] = new Vector3(3, 0, 0);

        // 3 cups
        originalCupPositions[1][0] = new Vector3(0, 2, 0);
        originalCupPositions[1][1] = new Vector3(-3, -2, 0);
        originalCupPositions[1][2] = new Vector3(3, -2, 0);

        // 4 cups
        originalCupPositions[2][0] = new Vector3(3, 2, 0);
        originalCupPositions[2][1] = new Vector3(-3, 2, 0);

        originalCupPositions[2][2] = new Vector3(3, -2, 0);
        originalCupPositions[2][3] = new Vector3(-3, -2, 0);

        // 5 cups
        originalCupPositions[3][0] = new Vector3(3, 2, 0);
        originalCupPositions[3][1] = new Vector3(-3, 0, 0);
        originalCupPositions[3][2] = new Vector3(0, -2, 0);

        originalCupPositions[3][3] = new Vector3(-4.6f, -2, 0);
        originalCupPositions[3][4] = new Vector3(4.6f, -2, 0);

        // 6 cups
        originalCupPositions[4][0] = new Vector3(-4.6f, 2, 0);
        originalCupPositions[4][1] = new Vector3(0, 2, 0);
        originalCupPositions[4][2] = new Vector3(4.6f, 2, 0);

        originalCupPositions[4][3] = new Vector3(-4.6f, -2, 0);
        originalCupPositions[4][4] = new Vector3(0, -2, 0);
        originalCupPositions[4][5] = new Vector3(4.6f, -2, 0);

        // 7 cups
        originalCupPositions[5][0] = new Vector3(-4.6f, 2, 0);
        originalCupPositions[5][1] = new Vector3(0, 2, 0);
        originalCupPositions[5][2] = new Vector3(4.6f, 2, 0);

        originalCupPositions[5][3] = new Vector3(-5, -2, 0);
        originalCupPositions[5][4] = new Vector3(-1.6f, -2, 0);

        originalCupPositions[5][5] = new Vector3(1.6f, -2, 0);
        originalCupPositions[5][6] = new Vector3(5, -2, 0);
    }*/
}

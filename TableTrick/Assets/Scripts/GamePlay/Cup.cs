using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a cup
/// </summary>
public class Cup : IntEventInvoker
{
    #region Field

    //save for efficiency
    Sprite OpenCupSprite;
    Sprite CloseCupSprite;
    Sprite[] gemSprites;
    SpriteRenderer sRenderer;
    Rigidbody2D rb2d;
    GameObject gemPrefab;

    //support for cup state
    CupState cupState;

    //save original position
    Vector2 returnPosition;
    float startMagnitude;

    //support for moving time
    Timer flowTimer;

    //Number of gems
    int gemNumber;
    // has this cup been Selected?
    bool isSelected = false;

    //support pickup effect
    bool isSpeedDown;
    float speedFactor = 0.5f;

    //support smooth Line
    Vector3[] smoothLine;
    int targetIndex = 0;
    float speed = 100.0f;

    #endregion

    #region Properties

    /// <summary>
    /// set return Position of cup
    /// </summary>
    public Vector2 ReturnPosition
    {
        set { returnPosition = value; }
    }

    public bool IsSpeedDown
    {
        set { isSpeedDown = value; }
    }

    #endregion


    private void Awake()
    {
        // Loads resources
        OpenCupSprite = Resources.Load<Sprite>("OpenedCup");
        CloseCupSprite = Resources.Load<Sprite>("ClosedCup");
        gemPrefab = Resources.Load<GameObject>("Gem");
        gemSprites = new Sprite[4];
        gemSprites[0] = Resources.Load<Sprite>("Blue");
        gemSprites[1] = Resources.Load<Sprite>("Green");
        gemSprites[2] = Resources.Load<Sprite>("Red");
        gemSprites[3] = Resources.Load<Sprite>("Yellow");

        //save for efficiency
        sRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        //add event for CheckAnswerEvent
        unityEvents.Add(EventName.CheckAnswerEvent, new CheckAnswerEvent());
        EventManager.AddInvoker(EventName.CheckAnswerEvent, this);

        //add event for WaitAllback event
        unityEvents.Add(EventName.WaitAllBackEvent, new WaitAllbackEvent());
        EventManager.AddInvoker(EventName.WaitAllBackEvent, this);
            
        EventManager.AddChangingCupStateListener(ChangeState);
    }

    // Use this for initialization
    void Start () {
        //save original position
        //returnPosition = transform.position;


        //gameObject.layer = 8;
        //Physics2D.IgnoreLayerCollision(8, 8, true);
        //int randNum = Random.Range(1, 4);
        //SetGem(randNum);

        //Game state
        cupState = CupState.ShowGems;
        
        //add timer
        flowTimer = gameObject.AddComponent<Timer>();
        flowTimer.AddTimerFinishedEventListener(CupFlow);
        flowTimer.Duration = 2.0f;//show opened cups for 2 seconds
        
        flowTimer.Run();
    }

    /// <summary>
    /// Initializes object. We don't use Start for this because
    /// we want to set up the points added event when we
    /// create the object in the french fries pool
    /// </summary>
    public void Initialize()
    {

        // add as invoker for PointsAddedEvent
    }


    // Update is called once per frame
    void Update () {
        if (DifficultyUtilities.MoveMode == Difficulty.Curve)
        {
            //move along the curve line
            if (cupState == CupState.CupMoving)
            {
                if (targetIndex < smoothLine.Length)
                {
                    //not move to the back position
                    if (isSpeedDown)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, smoothLine[targetIndex], 
                            speed * Time.deltaTime * speedFactor);
                        //print("reduce speed");
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, smoothLine[targetIndex], 
                            speed * Time.deltaTime);
                    }

                    
                    if (transform.position == smoothLine[targetIndex])
                    {
                        targetIndex++;
                    }
                }
                else
                {
                    //arrived back position
                    cupState = CupState.WaitAllBack;
                    unityEvents[EventName.WaitAllBackEvent].Invoke(0);
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (DifficultyUtilities.MoveMode == Difficulty.Normal)
        {
            //for bouncing movement
            if (cupState == CupState.MovingBack)
            {
                if (rb2d.position != returnPosition)
                {
                    Vector3 newPosition;
                    if (isSpeedDown)
                    {
                        newPosition = Vector3.MoveTowards(rb2d.position, returnPosition, startMagnitude * Time.deltaTime * speedFactor);
                        //print("reduce speed");
                    }
                    else
                    {
                        newPosition = Vector3.MoveTowards(rb2d.position, returnPosition, startMagnitude * Time.deltaTime);
                    }

                    rb2d.MovePosition(newPosition);
                }
                else
                {
                    //cupState = CupState.Choose_Ans;
                    cupState = CupState.WaitAllBack;
                    unityEvents[EventName.WaitAllBackEvent].Invoke(0);
                    //print("GameState.Choose_Ans, allow open");
                }
            }
        }
    }


    /// <summary>
    /// Starts the Cup moving
    /// </summary>
    public void StartMoving()
    {
        // apply impulse force to get teddy bear moving
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector2 direction = new Vector2(
            Mathf.Cos(angle), Mathf.Sin(angle));
        startMagnitude = Random.Range(DifficultyUtilities.MinSpeed, DifficultyUtilities.MaxSpeed);
        Vector2 force = direction * startMagnitude;
        

        if (isSpeedDown)
        {
            rb2d.AddForce(force * speedFactor, ForceMode2D.Impulse);
            //print("reduce speed");
        }
        else
        {
            rb2d.AddForce(force, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// according to cup state decide how cup do 
    /// </summary>
    void CupFlow()
    {
        if (cupState == CupState.ShowGems)
        {
            //close the cup
            sRenderer.sprite = CloseCupSprite;
            sRenderer.sortingOrder = 1;

            //change state to close cup
            cupState = CupState.CoverGemsDelay;
            flowTimer.Duration = 1.0f;
            flowTimer.Run();
        }
        else if (cupState == CupState.CoverGemsDelay)
        {
            //change state to move
            cupState = CupState.CupMoving;
            if (DifficultyUtilities.MoveMode == Difficulty.Normal)
            {
                StartMoving();
                flowTimer.Duration = DifficultyUtilities.MovingDuration; ;
                flowTimer.Run();
            }
            else
            {
                //move curve
                Vector3[] originalLine = DLGCurve.randomLine(13, transform.position, returnPosition);
                smoothLine = DLGCurve.DLG(originalLine, 3);
                speed = Random.Range(DifficultyUtilities.MinSpeed, DifficultyUtilities.MaxSpeed);

                //DLGCurve.DrawALine(smoothLine, Color.red);
                //checkInScreen(smoothLine);
            }
        }
        else if (cupState == CupState.CupMoving)
        {
            rb2d.velocity = Vector2.zero;
            cupState = CupState.MovingBack;
        }
    }

    /// <summary>
    /// sets this cup has num-th gum(s)
    /// </summary>
    /// <param name="num">the number of gums (Max = 3)</param>
    public void DistributeGem(int num)
    {
        if (num > 3 || num < 0)
        {
            num = 3;
            print("The number of gums should be 0~3");
        }

        //save number of gem
        gemNumber = num;

        //
        GameObject[] gems = new GameObject[num];
        Vector3[] gemsPosition = GetGemsPosition(num);
        
        for(int i = 0; i < num; i++)
        {
            gems[i] = Instantiate(gemPrefab);
            gems[i].GetComponent<SpriteRenderer>().sprite = gemSprites[Random.Range(0, 4)];
            gems[i].transform.parent = gameObject.transform;
            gems[i].transform.localPosition = gemsPosition[i];
        }
    }

    /// <summary>
    /// gets Position of Gems
    /// </summary>
    /// <param name="num"> the num of gem</param>
    /// <returns>Positions</returns>
    private Vector3[] GetGemsPosition(int num)
    {
        Vector3[] gemsPosition = new Vector3[num];

        if (num == 1)
        {
            gemsPosition[0] = new Vector3(0.02f, -0.35f, 0);
        }
        else if (num == 2)
        {
            gemsPosition[0] = new Vector3(-0.13f, -0.39f, 0);
            gemsPosition[1] = new Vector3(0.17f, -0.39f, 0);
        }
        else if(num == 3)
        {
            gemsPosition[0] = new Vector3(0.02f, -0.35f, 0);
            gemsPosition[1] = new Vector3(-0.28f, -0.39f, 0);
            gemsPosition[2] = new Vector3(0.3f, -0.39f, 0);
        }

        return gemsPosition;
    }

    /// <summary>
    /// open this cup (guess gems in it)
    /// </summary>
    private void OnMouseDown()
    {
        if (cupState == CupState.Choose_Ans)
        {
            if (!isSelected)
            {
                isSelected = true;

                sRenderer.sprite = OpenCupSprite;
                sRenderer.sortingOrder = -1;

                //print("open");
                unityEvents[EventName.CheckAnswerEvent].Invoke(gemNumber);
            }
            else
            {
                print("This cup has been selected(opened)");
            }
        }
    }

    /// <summary>
    /// Called when the cup become invisible
    /// </summary>
    virtual protected void OnBecameInvisible()
    {
        EventManager.RemoveInvoker(EventName.CheckAnswerEvent, this);
        EventManager.RemoveInvoker(EventName.WaitAllBackEvent, this);
        //print("cup become invisible");
    }

    /// <summary>
    /// Change the state of this cup 
    /// </summary>
    void ChangeState(CupState state)
    {
        cupState = state;
        //print("cup state = " + cupState);

        if (cupState == CupState.ShowResultDelay && gemNumber >= 1)
        {
            //open the cup if it contain gems
            sRenderer.sprite = OpenCupSprite;
            sRenderer.sortingOrder = -1;
        }

        if (cupState == CupState.ShowResult)
        {
            Destroy(gameObject);
        }
    }
    
    /*
    /// <summary>
    /// Starts the person moving toward the target Waypoint
    /// </summary>
    void GoToWaypoint(Transform targetWaypoint)
    {
        // calculate direction to target pickup and start moving toward it
        Vector2 direction = new Vector2(
            targetWaypoint.position.x - transform.position.x,
            targetWaypoint.position.y - transform.position.y);

        direction.Normalize();
        rb2d.velocity = Vector2.zero;

        rb2d.AddForce(direction * 20,
            ForceMode2D.Impulse);
    }*/

    void checkInScreen(Vector3[] points)
    {
        float halfWidth, halfLength;
        float minSpawnX, maxSpawnX, minSpawnY, maxSpawnY;

        GameObject tempCup = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("OpenedCup"));
        BoxCollider2D bc2d = tempCup.GetComponent<BoxCollider2D>();

        halfWidth = bc2d.size.x / 2;
        halfLength = bc2d.size.y / 2;
        UnityEngine.Object.Destroy(tempCup);

        //calculate spawn area
        minSpawnX = ScreenUtils.ScreenLeft + halfWidth;
        maxSpawnX = ScreenUtils.ScreenRight - halfWidth;
        minSpawnY = ScreenUtils.ScreenBottom + halfLength;
        maxSpawnY = ScreenUtils.ScreenTop - halfLength;

        int sum = 0;
        foreach (Vector3 point in points)
        {
            if (point.x > maxSpawnX || point.x < minSpawnX
                || point.y > maxSpawnY || point.y < minSpawnY)
            {
                print("out = "+point);
                sum++;
            }
        }
        print("Total out = " + sum);
    }
}

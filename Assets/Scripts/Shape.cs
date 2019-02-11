using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Shape : MonoBehaviour 
{
    //touch vars
	public bool isYellow = false;
	public int startingAngle = 0;
	private Vector2 fp; // first finger position
	private Vector2 lp; // last finger position
	private int value = 0;
	private bool Tch = false;

	//touch vars
	public float speedDownNormal = 0.05f;
	float speedDownFast = 0.5f;
	public float speedDownCurrent = 0.05f;
	public List<Block> childBlocks;
	public int blockHeight;
	bool collide = false;

    bool isDirty = false;
    bool isMoving = false;
    bool isNewShape = false;
    bool isRotate = false;

    int sign = 1;
    private float speed = 255.0f;
    private float rotation = 0.0f;
    private Quaternion qTo = Quaternion.identity;

    public int[,,] wholeBlock;

	public string name;
    public Color myColor;
	public Texture myTexture;

    public bool isStart = false;

//    bool isShapeCollsionTriggered = false;

    GameManager gameManager;

	// Use this for initialization
	public void Start ()
	{
        int levelNum = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);
        LevelManager levelManager = LevelManager.getInstance();
        LevelData levelData = levelManager.getDataForLevel(levelNum);
		//
        speedDownNormal = levelData.speed;
		speedDownFast = 50;
        speedDownCurrent = 2.5f;
		//
        for (int i = 0; i < transform.childCount; i++)
        {
            childBlocks.Add(transform.GetChild(i).GetComponent<Block>());
        }

//		childBlocks.Clear();
		speedDownCurrent = speedDownNormal;
//        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        wholeBlock = new int[GameConstants.TOWER_X, GameConstants.TOWER_Y, blockHeight];

        for (int h = 0; h < blockHeight; h++)
        {
            for (int i = 0; i < GameConstants.TOWER_X; i++)
            {
                for (int j = 0; j < GameConstants.TOWER_Y; j++)
                {
                    wholeBlock[i,j,h] = 0;
                }
            }
        }

        for (int i = 0; i < childBlocks.Count; i++)
        {
            Block child = childBlocks[i];
            Vector3 position = child.getPosition();
            wholeBlock[(int)position.x, (int)position.y, (int)position.z] = 1;
        }

        gameManager.showShapeShadow(this);
        changeColorOfBlocks();

//        startingAngle = 0;
		if (startingAngle == 90)
		{
            rotate();
		}
		else if (startingAngle == 180)
		{
            rotate();
            rotate();
		}
		else if (startingAngle == 270)
		{
            rotate();
            rotate();
            rotate();
		}

		if (isMoving)
		{
			moveNewShapeDown();
		}

	}

    public void redrawWholeShape()
    {
        for (int h = 0; h < 1; h++)
        {
            for (int i = 0; i < GameConstants.TOWER_X; i++)
            {
                for (int j = 0; j < GameConstants.TOWER_Y; j++)
                {
                    wholeBlock[i,j,h] = 0;
                }
            }
        }

        for (int i = 0; i < childBlocks.Count; i++)
        {
            Block child = childBlocks[i];
            Vector3 position = child.getPosition();
            wholeBlock[(int)position.x, (int)position.y, (int)position.z] = 1;
        }
    }

	public int[,] getMyShape()
	{
		List<Block> childs = new List<Block>();
		for (int i = 0; i < transform.childCount; i++)
		{
			//Debug.Log("childCount : "+transform.childCount);
			childs.Add(transform.GetChild(i).GetComponent<Block>());
		}
		int[,] mysh = new int[3,3];
		for (int i = 0; i < GameConstants.TOWER_X; i++)
		{
			for (int j = 0; j < GameConstants.TOWER_Y; j++)
			{
				mysh[i,j] = 0;
			}
		}
		for (int i = 0; i < childs.Count; i++)
		{
			Block child = childs[i];
			Vector3 position = child.getPosition();
			mysh[(int)position.x, (int)position.y] = 1;
		}
		return mysh;

	}

    public void setGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    public bool getIsMoving()
    {
        return isMoving;
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (gameManager.isPaused)
		{
			speedDownCurrent = speedDownNormal;
            return;
		}
        if (isNewShape && isMoving)
        {
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				rotateShape(-90);
			}
			if (Input.GetKeyDown ("space"))
			{
				speedDownCurrent = speedDownFast;
				moveNewShapeDown();

			}
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				rotateShape(90);
			}

			if (value == 1)
			{
				rotateShape(-90);
				value = 0;
			}

			if (value == 2)
			{
				rotateShape(90);
				value = 0;

			}

			foreach (Touch touch in Input.touches) {

				if (touch.phase == TouchPhase.Began) 
				{
					fp = touch.position;
					lp = touch.position;
					Tch = true;
				}
				if (touch.phase == TouchPhase.Moved) 
				{
					lp = touch.position;
					Tch = false;
				}
				if (touch.phase == TouchPhase.Ended) {
					if (Tch == true) 
					{
						speedDownCurrent = speedDownFast;

						moveNewShapeDown();
						SoundManager.Instance.PlayFastBlockSound();

					} 
					else if (((fp.x - lp.x) >= 0 && (fp.x - lp.x) <= 10 || (fp.x - lp.x) <= 0 && (fp.x - lp.x) >= -10)) 
					{
						speedDownCurrent = speedDownFast;
						Tch = true;

						moveNewShapeDown();
						SoundManager.Instance.PlayFastBlockSound();

					} 
					else if ((fp.x - lp.x) > 80) 
					{ // left swipe
						value = 2;
						Tch = true;
					} 
					else if ((fp.x - lp.x) < -80) 
					{ // right swipe
						value = 1;
						Tch = true;
					}
					else if ((fp.y - lp.y) > 80) 
					{
						speedDownCurrent = speedDownFast;
						Tch = true;

						moveNewShapeDown();
						SoundManager.Instance.PlayFastBlockSound();


					}
				}
			}

           // rotateShape(90);
        }


//        if (isRotate)
//        {
//            qTo = Quaternion.Euler(0.0f, rotation, 0.0f);
//
//            if (transform.rotation == qTo)
//            {
//                isRotate = false;
//                rotation = 0;
//                return;
//            }
//            transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, speed * Time.deltaTime);
//        }
	}


//	void FixedUpdate()
//	{
//		
//	}

	public void changeColorOfBlocks()
	{
        if (isStart)
        {
            myColor = new Color(255f/255f, 215f/255f, 0f/255f);
            for (int i = 0; i < childBlocks.Count; i++)
            {
                Block child = childBlocks[i];
				ColorPallete.getInstance().setDollarBlockTex(child.gameObject.GetComponent<Renderer>());

                //child.GetComponent<Block>().changeColor(myColor);
            }
        }
        else
        {
			
            int levelNum = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);
			myColor = ColorPallete.getInstance().getColor(levelNum);
			myTexture = ColorPallete.getInstance().getTetxure(levelNum);
         //   myColor = ColorPallete.getInstance().getColor(levelNum);
            for (int i = 0; i < childBlocks.Count; i++)
            {
                Block child = childBlocks[i];

				ColorPallete.getInstance().changeColorOrTex(child.gameObject.GetComponent<Renderer>(),levelNum, myColor);
              //  child.GetComponent<Block>().changeColor(myColor);
            }
        }

	}

    public void setShapeMoving(bool moving, bool isNew)
    {
        isNewShape = isNew;
        isMoving = moving;
		if(moving)
		{
			moveNewShapeDown();
		}
		else
		{
			stopShape();
		}
    }

    void moveNewShapeDown()
    {
		//transform.Translate(Vector3.down * Time.deltaTime * speedDownCurrent);
//		Debug.Log("moveNewShapeDown");
		for (int i = 0; i < childBlocks.Count; i++)
		{
			Block child = childBlocks[i];
			child.setVeloc(speedDownCurrent);
		}
		//transform.position = new Vector3(transform.position.x, (transform.position.y - speedDownCurrent), transform.position.z);
    }

	void stopShape()
	{
//		Debug.Log("stopShape");

		for (int i = 0; i < childBlocks.Count; i++)
		{
			Block child = childBlocks[i];
			child.setVeloc(0);
		}
	}

	public bool checkBelow()
	{
        bool shouldBlockMoveDown = true;

        for (int i = 0; i < GameConstants.TOWER_Y; i++)
        {
            Block block = getBlockFromPosition(new Vector3(0, i, 0));
            if (block != null)
            {
                Vector3 vec = block.getPositionInTower();
                if (vec.z <= 1 && gameManager.tower[(int)vec.x, (int)vec.y, (int)vec.z - 1] != null)
                {
                    return false;
                }
            }
        }

        return true;
	}



    //TODO: only done with height 1
    public void rotateShape(int angle)
    {
		
        int maxHeight = gameManager.currentMaxHeight+1;
		if ((childBlocks[0].transform.position.y+5.14) < maxHeight + (maxHeight * 0.05) + 0.5f)
        {
//			Debug.Log("not possible");
            return;
        }
		SoundManager.Instance.PlayRotateSound();
        gameManager.rotateBase(angle, this);
        return;

        int n = 3;
        int[,,] ret = new int[n, n, blockHeight];

        Dictionary<string, Vector3> positions = new Dictionary<string, Vector3>();

        if (angle == 90)
        {
//            rotation = 90.0f;
//            isRotate = true;
            transform.Rotate(0, 90, 0);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Block block = getBlockFromPosition(new Vector3(n - j - 1, i, 0));
                    ret[i, j, 0] = wholeBlock[n - j - 1, i, 0];

                    positions.Add(""+ (n - j - 1) + i , new Vector3(i, j, 0));

//                    if(block != null && ret[i, j, 0] == 1)
//                        block.position = new Vector3(i, j, 0);
                }
            }
        }
        else
        {
//            rotation = -90.0f;
//            isRotate = true;
            transform.Rotate(0, -90, 0);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Block block = getBlockFromPosition(new Vector3(j, n - i - 1, 0));
                    ret[i, j, 0] = wholeBlock[j, n - i - 1, 0];
                    positions.Add(""+ j + (n - i - 1), new Vector3(i, j, 0));

//                    if(block != null && ret[i, j, 0] == 1)
//                        block.position = new Vector3(i, j, 0);
                }
            }
        }

        wholeBlock = ret;

        for (int i = 0; i < childBlocks.Count; i++)
        {
            Block block = childBlocks[i];
            Vector3 oldPos = block.getPosition();
            Vector3 newPos = positions["" + oldPos.x + oldPos.y];

            block.position = newPos;
        }

        gameManager.showShapeShadow(this);
    }

    private void rotate()
    {
        int angle = 90;
        int n = 3;
        int[,,] ret = new int[n, n, blockHeight];

        Dictionary<string, Vector3> positions = new Dictionary<string, Vector3>();

        if (angle == 90)
        {
            //            rotation = 90.0f;
            //            isRotate = true;
            transform.Rotate(0, 90, 0);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Block block = getBlockFromPosition(new Vector3(n - j - 1, i, 0));
                    ret[i, j, 0] = wholeBlock[n - j - 1, i, 0];

                    positions.Add(""+ (n - j - 1) + i , new Vector3(i, j, 0));

                    //                    if(block != null && ret[i, j, 0] == 1)
                    //                        block.position = new Vector3(i, j, 0);
                }
            }
        }
        else
        {
            //            rotation = -90.0f;
            //            isRotate = true;
            transform.Rotate(0, -90, 0);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Block block = getBlockFromPosition(new Vector3(j, n - i - 1, 0));
                    ret[i, j, 0] = wholeBlock[j, n - i - 1, 0];
                    positions.Add(""+ j + (n - i - 1), new Vector3(i, j, 0));

                    //                    if(block != null && ret[i, j, 0] == 1)
                    //                        block.position = new Vector3(i, j, 0);
                }
            }
        }

        wholeBlock = ret;

        for (int i = 0; i < childBlocks.Count; i++)
        {
            Block block = childBlocks[i];
            Vector3 oldPos = block.getPosition();
            Vector3 newPos = positions["" + oldPos.x + oldPos.y];

            block.position = newPos;
        }

//        gameManager.showShapeShadow(this);
    }

    public void removeBlock(Block block)
    {
        wholeBlock[(int)block.getPosition().x, (int)block.getPosition().y, (int)block.getPosition().z] = 0;
        childBlocks.Remove(block);
		SoundManager.Instance.PlayPopBlockSound();
        Destroy(block.gameObject);
        isDirty = true;

        if (childBlocks.Count == 0)
        {
            gameManager.removeShapeFromList(this);
            Destroy(this.gameObject);
        }

    }

    public void rearrangeShape()
    {
        if (!isDirty)
            return;
        
        for (int z = 0; z < blockHeight - 1; z++)
        {
            for (int x = 0; x < GameConstants.TOWER_X; x++)
            {
                bool shouldMoveDown = true;
                for (int y = 0; x < GameConstants.TOWER_Y; y++)
                {
                    if (wholeBlock[x, y, z] == 1)
                    {
                        shouldMoveDown = false;
                        break;
                    }
                }


                if (shouldMoveDown)
                {
                    for (int y = 0; x < GameConstants.TOWER_Y; y++)
                    {
                        wholeBlock[x, y, z] = wholeBlock[x, y, z+1];
                        wholeBlock[x, y, z+1] = 0;
                    }
                }
            }
        }
    }

    public Block getBlockFromPosition(Vector3 position)
    {
        for (int i = 0; i < childBlocks.Count; i++)
        {
            Block child = childBlocks[i];
            if (child.getPosition().z == position.z &&
                child.getPosition().x == position.x &&
                child.getPosition().y == position.y)
            {
                return child;
            }
        }

        return null;
    }

    public void setBlocksHeights(int collisionHeight, int localHeightOfBlock)
	{
		// loop heights
		// update tower height and list of shapes in game manager
		// bool check fill call gamemanger
		// 
		//Destroy(other.gameObject);


        for (int i = 0; i < childBlocks.Count; i++)
        {
            Block child = childBlocks[i];
            int globalHeight = (collisionHeight + child.getInternalHeight()) - localHeightOfBlock;

            child.setHeight(globalHeight);
        }
	}




    public IEnumerator onBottomCollisionEnter(Collision other, Block thisBlock)
    {
        if (!isMoving)// || gameManager.newShape != gameObject) //isShapeCollsionTriggered && 
            yield break;

        if (thisBlock.getGlobalHeight() != -1 && other.gameObject.tag != "Base" && other.gameObject.GetComponent<Block>().getGlobalHeight() >= thisBlock.getGlobalHeight())
        {
         //   Debug.Log("Ignore this collision");
            yield break;
        }
		SoundManager.Instance.PlayPlaceBlockSound();
        speedDownCurrent = speedDownNormal;
        isMoving = false;
		stopShape();
        while (gameManager.rotating)
        {
            yield return 0;
        }

        transform.parent = gameManager.baseObject.transform;
//        transform.eulerAngles = new Vector3(0f, -gameManager.finalAngle.y, 0f);
        // get other height and send height to parent
        if(other.gameObject.tag == "Base")
        {
            setBlocksHeights(1, 1);
        }
        else
        {
            int localHeightOfBlock  = thisBlock.getInternalHeight();
            int globalHeightOfBlock = other.gameObject.GetComponent<Block>().getGlobalHeight() + 1;

            setBlocksHeights(globalHeightOfBlock, localHeightOfBlock);
        }

        Block testing = other.gameObject.GetComponent<Block>();
        Shape shapeTest = testing != null ? testing.getParent() : null;

        setShapePosition();

        gameManager.addNewShapeAndUpdateData(this, isNewShape);

        isNewShape = false;
    }

    public IEnumerator onBottomCollisionEnterStartLevel(int globalHeight, bool isBase, bool isLast)
    {
        yield return 0;
//        if (thisBlock.getGlobalHeight() != -1 && other.gameObject.tag != "Base" && other.gameObject.GetComponent<Block>().getGlobalHeight() >= thisBlock.getGlobalHeight())
//        {
//            //   Debug.Log("Ignore this collision");
//            yield break;
//        }
		gameManager.yellowCounts += childBlocks.Count;
        speedDownCurrent = speedDownNormal;
        isMoving = false;
		stopShape();

        while (gameManager.rotating)
        {
            yield return 0;
        }

        transform.parent = gameManager.baseObject.transform;
        //        transform.eulerAngles = new Vector3(0f, -gameManager.finalAngle.y, 0f);
        // get other height and send height to parent
        if(isBase)
        {
            setBlocksHeights(1, 1);
        }
        else
        {
            int localHeightOfBlock = 1;
            int globalHeightOfBlock = globalHeight;

            setBlocksHeights(globalHeightOfBlock, localHeightOfBlock);
        }

        setShapePosition();

        gameManager.addNewShapeAndUpdateData(this, true, true);

        isNewShape = false;

        if (isLast)
        {
            gameManager.createNewShape();
        }
    }


    public void setPositionsAfterRearrange()
    {
//        setShapePosition();
//        int maxInternalHeight = getTopBlockInternalHeight();
//        blockHeight = maxInternalHeight + 1;
    }

    public void setShapePosition()
    {
        int minheight = getBottomBlockHeight();
        float newHeightInScene = minheight + (0.05f * (minheight + 1));
        transform.position = new Vector3(gameManager.baseObject.transform.position.x, newHeightInScene, gameManager.baseObject.transform.position.z);

    }

    int getBottomBlockHeight()
    {
        int minHeight = 99;
        for (int i = 0; i < childBlocks.Count; i++)
        {
            int height = childBlocks[i].getGlobalHeight() - 1;
            if(height < minHeight)
            {
                minHeight = height;
            }
        }

        if (minHeight == 99)
        {
            Debug.Log("height = " + minHeight);
        }

        return minHeight;
    }

}

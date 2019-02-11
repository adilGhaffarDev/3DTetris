using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public int yellowCounts = 0;
	public int currentMaxHeight = 0;
	public int currentLevel = 1;
    public Block[,,] tower = new Block[GameConstants.TOWER_X, GameConstants.TOWER_Y, GameConstants.TOWER_Z];
	public int[,] maxXYheights = new int[3, 3];
    [SerializeField]
    List<Shape> allShapes = new List<Shape>();


    public List<GameObject> shapePrefabs;
    public GameObject greenShadowPrefab;
	public Transform spawnPoint;
    private Dictionary<string, GameObject> shadowList = new Dictionary<string, GameObject>();

	public MenuManager menuManager;
    public GameObject redBlocksParent;

    int counter = 0;
    public bool isPaused = false;

    /// <rotateangle members>
    public bool rotating = false;
    Vector3 curAngle = Vector3.zero;
    public Vector3 finalAngle = Vector3.zero;
    float speed = 500f;
    public GameObject baseObject;
    public GameLevelManager gameLevelManager;
    /// </rotateangle members>

    public ParticleSystem particleSystem;
    public ParticleSystem bombParticle;
    Shape shapeScript;
    public GameObject bomb;
    public GameObject howToPanel;

	bool shouldZoomCamera = true;
    bool isGameOver = false;

    void Awake()
    {
//        PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, 2);
    }

	// Use this for initialization
	void Start () 
	{
        isGameOver = false;
		//initialize tower with zero
		for (int h = 0; h < GameConstants.TOWER_Z; h++)
		{
			for (int i = 0; i < GameConstants.TOWER_X; i++)
			{
				for (int j = 0; j < GameConstants.TOWER_Y; j++)
				{
                    tower[i,j,h] = null;
				}
			}
		}

        for (int h = 0; h < GameConstants.TOWER_Y; h++)
		{
            for (int i = 0; i < GameConstants.TOWER_X; i++)
			{
				maxXYheights[i,h] = 0;
			}
		}
		int bombs = PlayerPrefs.GetInt(GameConstants.BOMBS,GameConstants.START_BOMB_COUNT);
		bomb.GetComponentInChildren<Text>().text = bombs.ToString();

        bomb.SetActive(true);
        LevelManager.getInstance();
        //createNewShape();

        setLevelBase();

	}

    public void setLevelBase()
    {
        int levelNum = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);
        baseObject = gameLevelManager.getLevelBase(levelNum);

        for (int i = 0; i < gameLevelManager.bgList.Count; i++)
        {
            gameLevelManager.bgList[i].CrossFadeColor(Color.clear, .01f, true, true);
        }

        gameLevelManager.bgList[levelNum-1].CrossFadeColor(Color.white, .01f, true, true);
    }

    public void createLevel()
    {
		yellowCounts = 0;
		int bombs = PlayerPrefs.GetInt(GameConstants.BOMBS,GameConstants.START_BOMB_COUNT);
		bomb.GetComponentInChildren<Text>().text = bombs.ToString();
        int levelNum = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);
        LevelManager levelManager = LevelManager.getInstance();

        LevelData levelData = levelManager.getDataForLevel(levelNum);

        LevelHandler levelHandler = baseObject.GetComponent<LevelHandler>();

        for (int i = 0; i < levelData.shapesData.Count; i++)
        {
            LevelObject a = levelData.shapesData[i];

            GameObject prefab = shapePrefabs[a.shapeIndex - 1];

            GameObject newShape = GameObject.Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity);
			newShape.name = "" + i;
//            newShape.transform.rotation = Quaternion.Euler(new Vector3(0f, levelHandler.startingAngle - 45f, 0f));
            newShape.transform.position = new Vector3(baseObject.transform.position.x, spawnPoint.transform.position.y, baseObject.transform.position.z);
            shapeScript = newShape.GetComponent<Shape>();
            shapeScript.setGameManager(this);
            shapeScript.startingAngle = a.rotation;
            shapeScript.isStart = true;
			shapeScript.isYellow = true;


            StartCoroutine(shapeScript.onBottomCollisionEnterStartLevel(a.height + 1, a.height == 0 ? true : false, a.isLast)); 
        }


    }

    public IEnumerator showHowTo()
    {
        
        yield return 0;


        GameObject rotate = howToPanel.transform.Find("rotate").gameObject;
        iTween.RotateTo(rotate, Vector3.zero, 0.5f);

    }

    public void continueClicked()
    {
        howToPanel.SetActive(false);
        PlayerPrefs.SetInt("HOW_TO", 1);
        createNewShape();
    }

    public void createNewShape()
    {
        int howTo = PlayerPrefs.GetInt("HOW_TO", 0);
        if (howTo == 0)
        {
            StartCoroutine(showHowTo());
//            showHowTo();
            return;
        }
        else
        {
            howToPanel.SetActive(false);
        }


		if (currentMaxHeight > 10)
		{
			//gameover
			return;
		}
        counter++;

//		if(counter > 1)
//		{
//			return;
//		}

		int index = 0;
		int angle = 0;

        if(currentMaxHeight == 0)// || true)
		{
           index = Random.Range(0, shapePrefabs.Count);


		}
		else
		{
			AIData ad = getOptimizedShape();
			index = ad.index;
			angle = ad.rotation;
		//	index =7;
		}
        LevelHandler levelHandler = baseObject.GetComponent<LevelHandler>();

		GameObject prefab = shapePrefabs[index];
		//		Debug.Log("ad.index "+ ad.index);
//        Quaternion.Euler(new Vector3(0f, levelHandler.startingAngle - 45f, 0f)
        GameObject newShape = GameObject.Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity);
        newShape.transform.position = new Vector3(baseObject.transform.position.x, spawnPoint.transform.position.y, baseObject.transform.position.z);
		shapeScript = newShape.GetComponent<Shape>();
		shapeScript.setGameManager(this);
		shapeScript.setShapeMoving(true, true);
		shapeScript.startingAngle = 0;
        newShape.name = "" + index + "-" + counter;
    }

    void gameOver()
    {
        redBlocksParent.SetActive(false);
        foreach(var pair in shadowList)
        {
            GameObject g = pair.Value;
            g.SetActive(false);
        }

        bomb.SetActive(false);
    }




	// after fill layer
	bool checkBelowEmpty(Shape shape)
	{

		for(int i = 0; i < shape.childBlocks.Count; i++)
		{
			Block block = shape.childBlocks[i];

			if(block.globalheight == 1)
			{
				return false;
			}

			if(tower[(int)block.position.x, (int)block.position.y, block.globalheight - 2] != null)
			{
				return false;
			}

		}

		return true;
	}


	// on  any collision
    int checkFillLayer()
	{
//		for(int i = 0; i<blockArray.)
//		{}

		// if fill destroy blocks(update shpaes array and global tower and global list), fall blocks

        bool isLayerFilled = true;
        int height = -1;
        for(int z = 0; z < GameConstants.TOWER_Z; z++)
        {
            for(int x = 0; x < GameConstants.TOWER_X; x++)
            {
                for(int y = 0; y < GameConstants.TOWER_Y; y++)
                {
                    Block block = tower[x, y, z];

                    if (block == null)
                    {
                        isLayerFilled = false;
                        break;
                    }
                }

                if (!isLayerFilled)
                {
                    break;
                }

            }

            if (isLayerFilled)
            {
                height = z;
                break;
            }
            else
            {
                isLayerFilled = true;
            }
        }

        return height;
	}

    void destroyLayerAndAddScore()
    {
        if (isGameOver)
        {
            return;
        }

		shouldZoomCamera = true;
		if (allShapes.Count == 0 || yellowCounts == 0)
        {
            isGameOver = true;
            gameLevelManager.isGameOverSucess = true;
            int levelNum = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);
			//if(PlayerPrefs.GetInt(GameConstants.ALL_LEVELS_UNLOCKED,0)==1)
			{
				if (levelNum <= 12 && levelNum >= 1)
				{
//                    Debug.Log("destroyLayerAndAddScore");
					PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, levelNum + 1);
				}
			}
//            else if (levelNum <= 3 && levelNum >= 1)
//            {
//                PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, levelNum+1);
//            }
            gameOver();
			menuManager.gameOver(true);
			SoundManager.Instance.PlayLevelWinSound();

            return;
        }

        setMaxXYheights();
        int height = checkFillLayer();
        if (height == -1)
        {
            if (allShapes.Count == 0 || !allShapes.Any(shape => shape.getIsMoving()))
            {
                createNewShape();
            }
//            else if (allShapes.Count == 0 && !allShapes.Any(shape => shape.getIsMoving()))
//            {
//                createNewShape();
//            }
            return;
        }

        float newHeightInScene = -5.54f + height + (0.05f * (height + 1));
        particleSystem.transform.position = new Vector3(baseObject.transform.position.x, newHeightInScene, baseObject.transform.position.z);
        particleSystem.Stop();
        particleSystem.Play();

        currentMaxHeight -= 1;

        Camera.main.GetComponent<CameraScript>().startZoom(currentMaxHeight, -1);

        for (int x = 0; x < GameConstants.TOWER_X; x++)
        {
            for (int y = 0; y < GameConstants.TOWER_Y; y++)
            {
                Block block = tower[x, y, height];
                block.getParent().removeBlock(block);
				if(block.getParent().isYellow)
				{
					yellowCounts-- ;
					menuManager.updateDiamond(10);

				}
                tower[x, y, height] = null;
            }
        }
        bool shouldRecall = true;
        //TODO: move below if require

        for (int i = 0; i < allShapes.Count; i++)
        {
            Shape shape = allShapes[i];
            shape.rearrangeShape();

			if (checkBelowEmpty(shape))
            {
				shouldZoomCamera = false;
				Debug.Log("ARSLAN::ARSLAN");
                shouldRecall = false;
                removeShapeFromTower(shape);
                shape.setShapeMoving(true, false);
            }
        }

        if(shouldRecall)
            destroyLayerAndAddScore();
    }

    void removeShapeFromTower(Shape shape)
    {
        List<Block> childBlocks = shape.childBlocks;
        for (int i = 0; i < childBlocks.Count; i++)
        {
            Block child = childBlocks[i];
            Vector3 pos = child.getPositionInTower();

            tower[(int)pos.x, (int)pos.y, (int)pos.z - 1] = null;
        }
    }

    public void addNewShapeAndUpdateData(Shape shape, bool isNew, bool isStart = false)
    {

        foreach(var pair in shadowList)
        {
            GameObject g = pair.Value;
            g.SetActive(false);
        }

        List<Block> childBlocks = shape.childBlocks;

        if(!isStart)
    		menuManager.updateScore(childBlocks.Count);
        
		int heightBlock = currentMaxHeight;
        for (int i = 0; i < childBlocks.Count; i++)
        {
            Block child = childBlocks[i];

            int x = (int)child.getPosition().x;
            int y = (int)child.getPosition().y;
            int z = child.getGlobalHeight() - 1;

            if (z >= GameConstants.TOWER_Z)
			{
                gameOver();
				menuManager.gameOver(false);
				SoundManager.Instance.PlayLevelFailSound();
				return;
			}

            if (tower[x, y, z] != null)
            {
//	                Debug.Log("Something is wrong here");
	                Debug.LogError("Something is wrong here");
            }

            tower[x, y, z] = child;

			if(child.getGlobalHeight()>heightBlock)
			{
				heightBlock = child.getGlobalHeight();
			}
        }
		currentMaxHeight = heightBlock;

//        if(currentMaxHeight != 0)
		if(shouldZoomCamera)
	        Camera.main.GetComponent<CameraScript>().startZoom(currentMaxHeight, 1);

        if (isStart)
        {
            allShapes.Add(shape);
        }
        else
        {
            if (isNew)
            {
                allShapes.Add(shape);
                destroyLayerAndAddScore();
            }
            else
            {
                setMaxXYheights();
                destroyLayerAndAddScore();
            }
        }
    }

    public void removeShapeFromList(Shape shape)
    {
        if (allShapes.Count != 0)
        {
            allShapes.Remove(shape);
        }
    }

    void setMaxXYheights()
    {
        for (int i = 0; i < GameConstants.TOWER_X; i++)
        {
            for (int j = 0; j < GameConstants.TOWER_Y; j++)
            {
                int heightXY = 0;
                for (int h = GameConstants.TOWER_Z-1; h >= 0; h--)
                {
                    Block block = tower[i, j, h];

                    if (block)
                    {
                        heightXY = block.getGlobalHeight();
                        break;
                    }
                }

                maxXYheights[i, j] = heightXY;

                if (heightXY >= 7)
                {
                    redBlocksParent.transform.Find("" + i + j).gameObject.SetActive(true);
                }
                else
                {
                    redBlocksParent.transform.Find("" + i + j).gameObject.SetActive(false);
                }
            }
        }
    }

    public void showShapeShadow(Shape shape)
    {
        foreach(var pair in shadowList)
        {
            GameObject g = pair.Value;
            g.SetActive(false);
        }

        for (int h = 0; h < shape.blockHeight; h++)
        {
            for (int i = 0; i < GameConstants.TOWER_X; i++)
            {
                for (int j = 0; j < GameConstants.TOWER_Y; j++)
                {
                    if (shape.wholeBlock[i, j, h] == 1)
                    {
                        int heightXY = maxXYheights[i, j];
                        float position = heightXY + (0.05f * (heightXY)) - 5.54f;

                        GameObject shadow;
                        Transform b = shape.getBlockFromPosition(new Vector3(i, j, h)).transform;

                        if (!shadowList.ContainsKey("" + i + j))
                        {
                            shadow = GameObject.Instantiate(greenShadowPrefab, new Vector3(b.position.x, position, b.position.z), Quaternion.Euler(new Vector3(0f, 45f, 0f)));
                            shadowList.Add("" + i + j, shadow);
                        }
                        else
                        {
                            shadow = shadowList["" + i + j];
                            shadow.transform.position = new Vector3(b.position.x, position, b.position.z);
                            shadow.SetActive(true);
                        }
                    }
                }
            }
        }
    }

	AIData getOptimizedShape()
	{
		List<AIData> AIDataList = new List<AIData>();
		int maxH = currentMaxHeight-1;
		int[,] topArray = new int[3,3];
		for (int h = 0; h < GameConstants.TOWER_Y; h++)
		{
			for (int i = 0; i < GameConstants.TOWER_X; i++)
			{
//				Debug.Log("Logog");
				if(tower[i,h,maxH] != null)
				{
					topArray[i,h] = 1;
				}
				else
				{
					topArray[i,h] = 0;
				}

			}
		}
		int gIndex = 0;
		foreach(GameObject g in shapePrefabs)
		{
			//Debug.Log(g.name);		

			int[,] prefabArray = g.GetComponent<Shape>().getMyShape();
			for(int y = 0; y<4;y++)
			{
				int[,] rotatedPrefab = new int[3,3];
				if(y==0)
				{
					rotatedPrefab = prefabArray;
				}
				else if(y==1)
				{
					rotatedPrefab = GameConstants.rotateShape(prefabArray);
					//rotatedPrefab = GameConstants.rotateShape(rotatedPrefab);

				}
				else if(y==2)
				{
					rotatedPrefab = GameConstants.rotateShape(prefabArray);
					rotatedPrefab = GameConstants.rotateShape(rotatedPrefab);

				}
				else if(y==3)
				{
					rotatedPrefab = GameConstants.rotateShape(prefabArray);
					rotatedPrefab = GameConstants.rotateShape(rotatedPrefab);
					rotatedPrefab = GameConstants.rotateShape(rotatedPrefab);

				}



//				for(int i = 0;i<y;i++)
//				{
//				}
				AIData ad = new AIData();
				compareMats(topArray,rotatedPrefab,y*90,gIndex,ref ad);
				AIDataList.Add(ad);
			}
			gIndex++;

		}
		AIData optimizedShapeIndex = getTopNotchShape(AIDataList);
		return optimizedShapeIndex;
	}

	AIData getTopNotchShape(List<AIData> AIdataList)
	{
		int minRank = 99;
		List<AIData> perfectFits = new List<AIData>();
		List<AIData> bestFits = new List<AIData>();
		List<AIData> rankeds = new List<AIData>();
		foreach(AIData ad in AIdataList)
		{
			if(ad.perfectFit)
			{
				perfectFits.Add(ad);
			}
			else if(ad.bestFit)
			{
				bestFits.Add(ad);
			}
			else
			{
				if(minRank > ad.rank)
				{
					minRank = ad.rank;
				}
				rankeds.Add(ad);
			}
		}

        int rand = Random.Range(1, 3);

        if(currentMaxHeight >= rand)
		{
			if(perfectFits.Count >0)
			{
				int r = Random.Range(0,perfectFits.Count);
				return perfectFits[r];
			}
			else if(bestFits.Count >0)
			{
				int r = Random.Range(0,bestFits.Count);
				return bestFits[r];
			}
			else if(rankeds.Count >0)
			{
				
				{
					List<AIData> MinRankeds = new List<AIData>();
					foreach(AIData ad in AIdataList)
					{
						if(ad.rank == minRank)
						{
							MinRankeds.Add(ad);
						}
					}
					int r = Random.Range(0,MinRankeds.Count);
					return MinRankeds[r];
				}
			}

		}
		else
		{
			if(bestFits.Count >0)
			{
				int r = Random.Range(0,bestFits.Count);
				return bestFits[r];
			}
			else if(perfectFits.Count >0)
			{
				int r = Random.Range(0,perfectFits.Count);
				return perfectFits[r];
			}
			else if(rankeds.Count >0)
			{
				
				{
				List<AIData> MinRankeds = new List<AIData>();
				foreach(AIData ad in AIdataList)
				{
					if(ad.rank == minRank)
					{
						MinRankeds.Add(ad);
					}
				}
				int r = Random.Range(0,MinRankeds.Count);
				return MinRankeds[r];
				}
			}
		}

		AIData aa= new AIData();
		return aa;
	}

	void compareMats(int[,] top, int[,] prefab, int rotate, int index,ref AIData ad)
	{
		int[,] ranked = new int[3,3];
		for (int h = 0; h < GameConstants.TOWER_Y; h++)
		{
			for (int i = 0; i < GameConstants.TOWER_X; i++)
			{
				ranked[i,h] = prefab[i,h] - top[i,h];

			}
		}


		ad.rotation = rotate;
		ad.index = index;
		calculateFits(ranked,top,prefab,ref ad);

		// best fit
		// perfect fit
	}

	void calculateFits(int[,] ranked, int[,] top,int[,] prefab, ref AIData ad)
	{
		
		int oneToMinusOne = 0;
		int oneToZero = 0;
		int zeroToOne = 0;

		int fill = 0;
		int noChange = 0;
		int zeroCovered = 0;

		int prefabOnes = 0;
		int topOnes = 0;


		for (int h = 0; h < GameConstants.TOWER_Y; h++)
		{
			for (int i = 0; i < GameConstants.TOWER_X; i++)
			{
				if(top[i,h] == 1 && ranked[i,h] == 0) 
				{
					oneToZero++;
				}
				else if(top[i,h] == 0 && ranked[i,h] == 1) 
				{
					zeroToOne++;
				}
				else if(top[i,h] == 1 && ranked[i,h] == -1) 
				{
					oneToMinusOne++;
				}
				if(prefab[i,h] == 1)
				{
					prefabOnes++;
				}
				if(top[i,h] == 1)
				{
					topOnes++;
				}
			}
		}
//		Debug.Log("=========================================");
//		Debug.Log("Index "+ad.index);
//		Debug.Log("rotation "+ad.rotation);
//		Debug.Log("oneToZero "+oneToZero);
//		Debug.Log("zeroToOne "+zeroToOne);
//		Debug.Log("oneToMinusOne "+oneToMinusOne);
//		Debug.Log("prefabOnes "+prefabOnes);
//		Debug.Log("topOnes "+topOnes);
//		Debug.Log("=========================================");
		if(prefabOnes == zeroToOne && oneToZero == 0)
		{
			ad.perfectFit = true;
			fill = zeroToOne;
			zeroCovered = oneToZero;
		}
		else
		{
			ad.perfectFit = false;

			fill = oneToZero;
			zeroCovered = zeroToOne;

			noChange = oneToMinusOne;
			if(noChange <= topOnes && zeroCovered == 0)
			{
				ad.bestFit = true;
			}
			else
			{
				ad.bestFit = false;
			}
		}



		ad.rank = zeroCovered;
//		Debug.Log("topOnes "+zeroCovered);
	}

    public void rotateBase(int angle, Shape shapeForShadow)
    {
        if (rotating)
            return;
        int n = 3;
        Block[,,] ret = new Block[GameConstants.TOWER_X, GameConstants.TOWER_Y, GameConstants.TOWER_Z];
        Dictionary<string, Vector3> positions = new Dictionary<string, Vector3>();

        if (angle == 90)
        {
            //            baseObject.transform.Rotate(0, 90, 0);
            StartCoroutine(RotateAngle(90f, baseObject.transform, shapeForShadow.transform));

            for (int h = 0; h < GameConstants.TOWER_Z; ++h)
            {
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        ret[i, j, h] = tower[n - j - 1, i, h];
                        positions.Add("" + (n - j - 1) + i + h, new Vector3(i, j, 0));
                    }
                }
            }

        }
        else
        {
            //            baseObject.transform.Rotate(0, -90, 0);
            StartCoroutine(RotateAngle(-90f, baseObject.transform, shapeForShadow.transform));

            for (int h = 0; h < GameConstants.TOWER_Z; ++h)
            {
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        ret[i, j, h] = tower[j, n - i - 1, h];
                        positions.Add(""+ j + (n - i - 1) + h, new Vector3(i, j, 0));
                    }
                }
            }
        }

        for (int h = 0; h < GameConstants.TOWER_Z; ++h)
        {
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    tower[i, j, h] = ret[i, j, h];
                }
            }
        }

        for (int x = 0; x < allShapes.Count; x++)
        {
            Shape shape = allShapes[x];
            for (int i = 0; i < shape.childBlocks.Count; i++)
            {
                Block block = shape.childBlocks[i];
                Vector3 oldPos = block.getPosition();
                Vector3 newPos = positions["" + oldPos.x + oldPos.y + (block.getGlobalHeight() - 1)];
                block.position = new Vector3(newPos.x, newPos.y, block.position.z);
            }
            shape.redrawWholeShape();
        }
        setMaxXYheights();
        showShapeShadow(shapeForShadow);
    }

    IEnumerator RotateAngle(float angle, Transform rotateObject, Transform shape)
    {
        if (rotating) yield break; // ignore calls to RotateAngle while rotating
        rotating = true;

        curAngle = rotateObject.eulerAngles;

        finalAngle = rotateObject.eulerAngles;
        finalAngle.y = finalAngle.y + angle;

        var newAngle = curAngle.y+angle;

        if (angle > 0)
        {
            while (curAngle.y < newAngle)
            {
                curAngle.y = Mathf.MoveTowards(curAngle.y, newAngle, speed * Time.deltaTime);
                rotateObject.eulerAngles = curAngle;
                yield return 0; // and let Unity free till the next frame
            }
        }
        else
        {
            while (curAngle.y > newAngle)
            {
                curAngle.y = Mathf.MoveTowards(curAngle.y, newAngle, speed * Time.deltaTime);
                rotateObject.eulerAngles = curAngle;
                yield return 0; // and let Unity free till the next frame
            }
        }
        rotating = false;
    }

    public void onBombClicked()
    {
		if (isPaused)
		{
			return;
		}
		int bombs = PlayerPrefs.GetInt(GameConstants.BOMBS,GameConstants.START_BOMB_COUNT);
		if(bombs <= 0)
		{
			//Time.timeScale = 0;
			menuManager.notEnoughBombs();
			return;
		}
		bombs -= 1;
		PlayerPrefs.SetInt(GameConstants.BOMBS,bombs);
		bomb.GetComponentInChildren<Text>().text = bombs.ToString();

		SoundManager.Instance.PlayClickSound();

		bombParticle.transform.position = new Vector3(baseObject.transform.position.x, shapeScript.childBlocks[0].transform.position.y, baseObject.transform.position.z);
        bombParticle.Play();
        bombParticle.gameObject.GetComponent<Renderer> ().material.color = shapeScript.myColor;
		bombParticle.gameObject.GetComponent<Renderer> ().material.mainTexture = shapeScript.myTexture;

        Destroy(shapeScript.gameObject);
		SoundManager.Instance.PlayBombSound();

        createNewShape();
    }

	public void resetBombs()
	{
		int bombs = PlayerPrefs.GetInt(GameConstants.BOMBS,GameConstants.START_BOMB_COUNT);

		bomb.GetComponentInChildren<Text>().text = bombs.ToString();

	}
}

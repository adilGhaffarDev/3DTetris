using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLevelManager : MonoBehaviour
{
    private Vector2 fp; // first finger position
    private Vector2 lp; // last finger position
    private int value = 0;
    private bool Tch = false;

    public List<GameObject> levels;
    public List<Image> bgList;

    public bool isWorldsPanelShowing = false;
    bool isRotating = false;
    public int currentLevel = 1;

	public BuyAllLevelScript bals;
    MenuManager _menuManager;
    public bool isGameOverSucess = false;

	void Start ()
    {
        _menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
        int levelNum = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);

        GameObject level = levels[levelNum - 1];
        LevelHandler levelHandler = level.GetComponent<LevelHandler>();

        transform.rotation = Quaternion.Euler(new Vector3(0f, 45f - levelHandler.startingAngle, 0f));

        level.SetActive(true);
        currentLevel = levelNum;
        scrollSettings(levelNum);
	}

    void scrollSettings(int levelNum)
    {
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject level = levels[i];
            if (i == levelNum - 2 || i == levelNum - 1 || i == levelNum - 0)
            {
                level.SetActive(true);
            }
            else
            {
                level.SetActive(false);
            }
        }
    }

    public GameObject getLevelBase(int levelNum)
    {
        GameObject level = levels[levelNum - 1];
        return level;
    }
	
    public void showWorldsScrollView(bool isWorldMenu)
    {
        Debug.Log("Adil:showWorldsScrollView");

        isWorldsPanelShowing = isWorldMenu;
    }

	void Update ()
    {
        if (isWorldsPanelShowing)
        {
            if (Input.GetKeyDown ("space"))
            {
				if(canPlayOrNotBool(currentLevel))
				{
                	isWorldsPanelShowing = false;
            	   PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, currentLevel);
                	_menuManager.backFromWorldScene();
				}
				else
				{
					// buy level
//					Debug.Log("buy level");
					//UnityIAP.instance.BuyItem(GameConstants.BUY_LEVELS_IAP,onInappSuccess);

				}
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentLevel >= levels.Count)
                {
                    return;
                }


                rotateBase(-90);
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentLevel <= 1)
                {
                    return;
                }


                rotateBase(90);
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
						if(canPlayOrNotBool(currentLevel))
						{
							isWorldsPanelShowing = false;
							   PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, currentLevel);
							_menuManager.backFromWorldScene();
						}
						else
						{
							// buy level
							//UnityIAP.instance.BuyItem(GameConstants.BUY_LEVELS_IAP,onInappSuccess);

						//	Debug.Log("buy level");
						}
                       
                    } 
                    else if (((fp.x - lp.x) >= 0 && (fp.x - lp.x) <= 10 || (fp.x - lp.x) <= 0 && (fp.x - lp.x) >= -10)) 
                    {
						if(canPlayOrNotBool(currentLevel))
						{
							isWorldsPanelShowing = false;
							PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, currentLevel);
							_menuManager.backFromWorldScene();
						}
						else
						{
							// buy level
							//UnityIAP.instance.BuyItem(GameConstants.BUY_LEVELS_IAP,onInappSuccess);

					//		Debug.Log("buy level");
						}
                        Tch = true;
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
						if(canPlayOrNotBool(currentLevel))
						{
							isWorldsPanelShowing = false;
							PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, currentLevel);
							_menuManager.backFromWorldScene();
						}
						else
						{
							// buy level
							//UnityIAP.instance.BuyItem(GameConstants.BUY_LEVELS_IAP,onInappSuccess);

				//			Debug.Log("buy level");
						}
                        Tch = true;
                    }
                }
            }

            if (value == 2)
            {
                Debug.Log("Adil:value1");

                if (currentLevel >= levels.Count)
                {
                    Debug.Log("Adil:value1:return");

                    return;
                }
                rotateBase(-90);
                value = 0;
            }

            if (value == 1)
            {
                if (currentLevel <= 1)
                {
                    Debug.Log("Adil:value2:return");

                    return;
                }
                rotateBase(90);
                value = 0;

            }

            // rotateShape(90);
//        }


        }
	}

    public void rotateBase(int angle)
    {
        if (isRotating)
            return;
		SoundManager.Instance.PlaySwipeLevelSound();
        if (angle == 90)
        {
            currentLevel--;
            scrollSettings(currentLevel);
			bals.canPlayOrNot(currentLevel);
//            Debug.Log(currentLevel);
//            GetComponent<Image>().CrossFadeColor(greenColor,.3f,true,true);
//            bgList[currentLevel].CrossFadeColor(Color.clear, .1f, true, true);
//            bgList[currentLevel-1].CrossFadeColor(Color.white, .1f, true, true);

//            for (int i = 0; i < bgList.Count; i++)
//            {
//                bgList[i].CrossFadeColor(Color.clear, .1f, true, true);
//            }
//
//            bgList[currentLevel-1].CrossFadeColor(Color.white, .1f, true, true);

            StartCoroutine(RotateAngle(90f, transform));
        }
        else
        {
            currentLevel++;
//            Debug.Log(currentLevel);

            scrollSettings(currentLevel);
			bals.canPlayOrNot(currentLevel);

//            bgList[currentLevel-2].CrossFadeColor(Color.clear, .1f, true, true);
//            bgList[currentLevel-1].CrossFadeColor(Color.white, .1f, true, true);
//            for (int i = 0; i < bgList.Count; i++)
//            {
//                bgList[i].CrossFadeColor(Color.clear, .1f, true, true);
//            }
//
//            bgList[currentLevel-1].CrossFadeColor(Color.white, .1f, true, true);

            StartCoroutine(RotateAngle(-90f, transform));
        }
    }

    IEnumerator RotateAngle(float angle, Transform rotateObject)
    {
        if (isRotating) yield break; // ignore calls to RotateAngle while rotating
        isRotating = true;

        Color clear = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
        for (int i = 0; i < bgList.Count; i++)
        {
            if (i == currentLevel - 1)
            {
                continue;
            }
            bgList[i].CrossFadeColor(Color.clear, .1f, true, true);
        }

        bgList[currentLevel-1].CrossFadeColor(Color.white, .1f, true, true);

        Vector3 curAngle = rotateObject.eulerAngles;

        Vector3 finalAngle = rotateObject.eulerAngles;
        finalAngle.y = finalAngle.y + angle;
        int speed = 500;

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
        isRotating = false;
    }

	bool canPlayOrNotBool(int levl)
	{
		if(PlayerPrefs.GetInt(GameConstants.ALL_LEVELS_UNLOCKED,0)==0)
		{
			if(levl <= GameConstants.BASE_UNLOCKED_LEVELS)
			{
				return true;

			}
			else
			{
				return false;

			}
		}
		else
		{
			return true;
		}
	}

	public bool onInappSuccess()
	{
		bals.canPlayOrNot(currentLevel);
		return true;
	}
}

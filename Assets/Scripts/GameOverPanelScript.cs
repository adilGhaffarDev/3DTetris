using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanelScript : MonoBehaviour 
{
	public GameObject levelSuccess;
	public GameObject levelFailed;

	public GameObject currentScore;
	public GameObject bestScore;
	public GameObject diamondsPanel;
	public GameObject botPanel;


	public Transform botPanHidePos;
	public Transform botPanShowPos;

    public GameLevelManager _levelManager;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	public void showGameOverPan(bool win)
	{
		gameObject.SetActive(true);
		if(win)
		{
			levelFailed.SetActive(false);
			levelSuccess.SetActive(true);

			iTween.ScaleTo(levelSuccess,iTween.Hash(
				"scale",Vector3.one,
				"time",0.25f,
				"loopType",iTween.LoopType.none,
				"easeType","easeInOutQuad",
				"ignoretimescale",true
			));
		}
		else
		{
			levelFailed.SetActive(true);
			levelSuccess.SetActive(false);

			iTween.ScaleTo(levelFailed,iTween.Hash(
				"scale",Vector3.one,
				"time",0.25f,
				"loopType",iTween.LoopType.none,
				"easeType","easeInOutQuad",
				"ignoretimescale",true));
		}
		int diamonds = PlayerPrefs.GetInt(GameConstants.DIAMONDS,0);
		int currScore = PlayerPrefs.GetInt(GameConstants.CURRENTSCORE,0);
		//GPSInstanceX.instance.UpdateLeaderboardForID(currScore);
		int highScore = PlayerPrefs.GetInt(GameConstants.HIGHSCORE,0);
		if(currScore > highScore)
		{
			PlayerPrefs.SetInt(GameConstants.HIGHSCORE,currScore);
			highScore = currScore;
		}

		currentScore.GetComponent<Text>().text = currScore.ToString();
		bestScore.GetComponent<Text>().text = "BEST  "+ highScore.ToString();
		diamondsPanel.GetComponentInChildren<Text>().text = diamonds.ToString();

		ShowPanel();

		iTween.ScaleTo(currentScore,iTween.Hash(
			"scale",Vector3.one,
			"time",0.25f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));
		
		iTween.ScaleTo(bestScore,iTween.Hash(
			"scale",Vector3.one,
			"time",0.25f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));

		iTween.ScaleTo(diamondsPanel,iTween.Hash(
			"scale",Vector3.one,
			"time",0.25f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));
	}

    public void onTap()
    {
        StartCoroutine(onTapRoutine());
    }

    public IEnumerator onTapRoutine()
	{
//        int levelNum = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);
//
//        if (_levelManager.isGameOverSucess)
//        {
////            PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, levelNum);
////            _levelManager.rotateBase(-90);
//        }
        yield return new WaitForSeconds(0.01f);

		SceneManager.LoadScene("gameplay");
	}

	public void ShowPanel()
	{
		//gameObject.SetActive(true);
		StartCoroutine("showRoutine");
	}

	public void HidelPanel()
	{
		StartCoroutine("hideRoutine");

	}

	IEnumerator showRoutine()
	{


		iTween.MoveTo(botPanel,iTween.Hash(
			"position",botPanShowPos.position,
			"time",0.5f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));

		yield return new WaitForSeconds(0.5f);
		botPanel.transform.position = botPanShowPos.position;
	}

	IEnumerator hideRoutine()
	{

		iTween.MoveTo(botPanel,iTween.Hash(
			"position",botPanHidePos.position,
			"time",0.5f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));

		yield return new WaitForSeconds(0.5f);
		botPanel.transform.position = botPanHidePos.position;
		//gameObject.SetActive(false);


	}
}

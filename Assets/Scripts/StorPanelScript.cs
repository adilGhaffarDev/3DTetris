using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum StoreType
{
	InGame,
	Global
}

public class StorPanelScript : MonoBehaviour {

	public StoreType myType; 
	public GameObject coinsBarPan;
	public GameObject BombsPan;

	public GameObject botPanel;


	public Transform botPanHidePos;
	public Transform botPanShowPos;

	public GameObject notBombsPan;

	//public GameLevelManager _levelManager;

	// Use this for initialization
//	void Start () {
//
//	}

	public void showStorePan(StoreType type)
	{
		myType = type;
		if(myType == StoreType.InGame)
		{
			notBombsPan.SetActive(true);
		}
		else
		{
			notBombsPan.SetActive(false);

		}
		gameObject.SetActive(true);
		int diamonds = PlayerPrefs.GetInt(GameConstants.DIAMONDS,0);
		int bombs = PlayerPrefs.GetInt(GameConstants.BOMBS,GameConstants.START_BOMB_COUNT);



		coinsBarPan.GetComponentInChildren<Text>().text = diamonds.ToString();
		BombsPan.GetComponentInChildren<Text>().text = bombs.ToString();

		ShowPanel();

		iTween.ScaleTo(coinsBarPan,iTween.Hash(
			"scale",Vector3.one,
			"time",0.25f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));

		iTween.ScaleTo(BombsPan,iTween.Hash(
			"scale",new Vector3(0.8f,0.8f,1),
			"time",0.25f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));


	}

	public void hideStorePan()
	{
		HidelPanel();

	}

//	public void onTap()
//	{
//		StartCoroutine(onTapRoutine());
//	}
//
//	public IEnumerator onTapRoutine()
//	{
//		int levelNum = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);
//
//		if (_levelManager.isGameOverSucess)
//		{
//			PlayerPrefs.SetInt(GameConstants.CURRENT_LEVEL, levelNum);
//			_levelManager.rotateBase(-90);
//		}
//		yield return new WaitForSeconds(0.2f);
//
//		SceneManager.LoadScene("gameplay");
//	}

	public void ShowPanel()
	{
		//gameObject.SetActive(true);
		StartCoroutine("showRoutine");
	}

	void HidelPanel()
	{
		StartCoroutine("hideRoutine");

	}

	IEnumerator showRoutine()
	{


		iTween.MoveTo(botPanel,iTween.Hash(
			"position",botPanShowPos.position,
			"time",0.25f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));

		yield return new WaitForSeconds(0.5f);
		botPanel.transform.position = botPanShowPos.position;
	}

	IEnumerator hideRoutine()
	{
		coinsBarPan.transform.localScale = Vector3.zero;
		BombsPan.transform.localScale = Vector3.zero;

		iTween.MoveTo(botPanel,iTween.Hash(
			"position",botPanHidePos.position,
			"time",0.1f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));

		yield return new WaitForSeconds(0.2f);
		botPanel.transform.position = botPanHidePos.position;
		gameObject.SetActive(false);

		//gameObject.SetActive(false);


	}

	public void BuyBomb(int i)
	{
		int userCoins = PlayerPrefs.GetInt(GameConstants.DIAMONDS,0);
		if(i*100 <= userCoins)
		{
			userCoins -= (i*100);
			int bombs = PlayerPrefs.GetInt(GameConstants.BOMBS,GameConstants.START_BOMB_COUNT);
			bombs += i;
			PlayerPrefs.SetInt(GameConstants.BOMBS,bombs);
			PlayerPrefs.SetInt(GameConstants.DIAMONDS,userCoins);

			coinsBarPan.GetComponentInChildren<Text>().text = userCoins.ToString();
			BombsPan.GetComponentInChildren<Text>().text = bombs.ToString();

			iTween.PunchScale(BombsPan,new Vector3(1.5f,1.5f,1),1);

			iTween.PunchScale(coinsBarPan,new Vector3(1.5f,1.5f,1),1);

		}
	}

	public void BuyCoins()
	{
		//UnityIAP.instance.BuyItem(GameConstants.BUY_COINS_IAP,BuyCoinsSuccess);

	}


	public bool BuyCoinsSuccess()
	{
		int userCoins = PlayerPrefs.GetInt(GameConstants.DIAMONDS,0);

		PlayerPrefs.SetInt(GameConstants.DIAMONDS,userCoins+GameConstants.BUY_COINS_NUMBER);
        coinsBarPan.GetComponentInChildren<Text>().text = (userCoins+GameConstants.BUY_COINS_NUMBER).ToString();
		iTween.PunchScale(coinsBarPan,new Vector3(1.5f,1.5f,1),1);
		return true;
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
	public GameObject PauseCanvas;
	public GameObject BuyLevelCanvas;

	public GameObject StoreCanvas;
	public GameObject MainMenuCanvas;
	public GameObject GameOverCanvas;
	public GameObject ScoreCanvas;
	public GameObject TapeToPlayCanvas;
	public GameObject SettingsBtnPanel;
	public GameObject MainMenuBtnPanel;	

	public GameObject ScorePanel;	
	public GameObject TitlePanel;
	public GameObject MainCamera;

	public GameObject scoreBox;
	public GameObject diamondBox;	
	public GameObject nextShape;

	public GameObject musicIcon;

	public GameObject buyLevelBtn;
	public GameObject playLevelBtn;


	public GameManager _gameManager;
    public GameLevelManager _levelManager;

	bool playing = false;
    bool isWorldMenu = false;
	int score = 0;
	int diamonds = 0;

	// Use this for initialization
	void Start () 
	{
		//PlayerPrefs.SetInt(GameConstants.ALL_LEVELS_UNLOCKED,1);

		//PlayerPrefs.SetInt(GameConstants.DIAMONDS, 100000);
		diamonds = PlayerPrefs.GetInt(GameConstants.DIAMONDS , 0);	
		MainMenuCanvas.SetActive(true);
		setTapPlayBtn();
		BuyLevelCanvas.SetActive(false);
		StoreCanvas.SetActive(false);
	}

	void setTapPlayBtn()
	{
		TapeToPlayCanvas.SetActive(true);
		int levelNum = PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL, 1);
		if(PlayerPrefs.GetInt(GameConstants.ALL_LEVELS_UNLOCKED,0)==1)
		{
			playLevelBtn.SetActive(true);
			buyLevelBtn.SetActive(false);

		}
		else
		{
			if (levelNum <= 4 && levelNum >= 1)
			{
				playLevelBtn.SetActive(true);
				buyLevelBtn.SetActive(false);
			}
			else
			{
				playLevelBtn.SetActive(false);
				buyLevelBtn.SetActive(true);
			}
		}
	}

	public void onTapPlay()
	{
		Time.timeScale = 1;

		SoundManager.Instance.PlayClickSound();
		if(!playing)
		{
			StartCoroutine("startPlayRoutine");
		}
		else
		{
			PauseCanvas.SetActive(false);
			TapeToPlayCanvas.SetActive(false);
			ScorePanel.GetComponent<PanelScript>().ShowPanel();
            _gameManager.isPaused = false;
		}

	}

	public void onSettingsClick()
	{
		SoundManager.Instance.PlayClickSound();

		StartCoroutine("showSettingsRoutine");
	}

	public void onSettingsBackClick()
	{
		SoundManager.Instance.PlayClickSound();

		StartCoroutine("hideSettingsRoutine");
	}


	public void notEnoughBombs()
	{
		StartCoroutine("notEnoughBombsROut");

	}

	IEnumerator notEnoughBombsROut()
	{
		//PauseCanvas.SetActive(true);
	//	Time.timeScale = 0;
		Time.timeScale = 0;
		_gameManager.isPaused = true;

		StoreCanvas.GetComponent<StorPanelScript>().showStorePan(StoreType.InGame);

		ScorePanel.GetComponent<PanelScript>().HidelPanel();
		ScoreCanvas.SetActive(false);

		yield return new WaitForSeconds(0.3f);


	}

	public void onPause()
	{
		SoundManager.Instance.PlayClickSound();

		StartCoroutine("startPauseRoutine");
	}

	IEnumerator startPauseRoutine()
	{
		PauseCanvas.SetActive(true);
		ScorePanel.GetComponent<PanelScript>().HidelPanel();
		yield return new WaitForSeconds(0.2f);

		_gameManager.isPaused = true;
		Time.timeScale = 0;
	}

	public void gameOver(bool win)
	{
		PlayerPrefs.SetInt(GameConstants.CURRENTSCORE, score);
		PlayerPrefs.SetInt(GameConstants.DIAMONDS, diamonds);
		ScorePanel.GetComponent<PanelScript>().HidelPanel();
//		yield return new WaitForSeconds(0.5f);
//		ScoreCanvas.SetActive(false);
		GameOverCanvas.GetComponent<GameOverPanelScript>().showGameOverPan(win);
	}

	IEnumerator showSettingsRoutine()
	{
		MainMenuBtnPanel.GetComponent<PanelScript>().HidelPanel();
		yield return new WaitForSeconds(0.25f);
		SettingsBtnPanel.GetComponent<PanelScript>().ShowPanel();
	}

	IEnumerator hideSettingsRoutine()
	{
		SettingsBtnPanel.GetComponent<PanelScript>().HidelPanel();
		yield return new WaitForSeconds(0.25f);
		MainMenuBtnPanel.GetComponent<PanelScript>().ShowPanel();
	}

	IEnumerator startPlayRoutine()
	{
		MainCamera.GetComponent<CameraScript>().startSceneChangeMove(1);
		playing = true;
		TapeToPlayCanvas.SetActive(false);
		TitlePanel.GetComponent<PanelScript>().HidelPanel();
		MainMenuBtnPanel.GetComponent<PanelScript>().HidelPanel();
		ScoreCanvas.SetActive(true);
		ScorePanel.GetComponent<PanelScript>().ShowPanel();
		scoreBox.GetComponent<Text>().text = score.ToString();
		diamondBox.GetComponent<Text>().text = diamonds.ToString();
		yield return new WaitForSeconds(0.2f);
//        yield return 0;
		MainMenuCanvas.SetActive(false);

        _gameManager.createLevel();
	}

	IEnumerator startMainMenuOpenRoutine()
	{
		MainCamera.GetComponent<CameraScript>().startSceneChangeMove(0);

		//TapeToPlayCanvas.SetActive(true);
		setTapPlayBtn();
		MainMenuCanvas.SetActive(true);

		TitlePanel.GetComponent<PanelScript>().ShowPanel();
		MainMenuBtnPanel.GetComponent<PanelScript>().ShowPanel();

		ScorePanel.GetComponent<PanelScript>().HidelPanel();
		yield return new WaitForSeconds(0.5f);
		ScoreCanvas.SetActive(false);
	}

	public void updateScore(int sc)
	{
		score += sc;
		scoreBox.GetComponent<Text>().text = score.ToString();

	}

	public void updateDiamond(int dia)
	{
		iTween.PunchScale(diamondBox,new Vector3(1.5f,1.5f,1),1);
		diamonds += dia;
		diamondBox.GetComponent<Text>().text = diamonds.ToString();

	}

	public void changeNextShapeTex(string s)
	{
		nextShape.GetComponent<Image>().overrideSprite  = Resources.Load (s,typeof(Sprite)) as Sprite;
	}


    public void changeWorldButtonPressed()
    {
        Debug.Log("Adil:changeWorldButtonPressed");

        StartCoroutine("changeWorldButtonPressedRout");
    }

    IEnumerator changeWorldButtonPressedRout()
    {
        isWorldMenu = true;
        TapeToPlayCanvas.SetActive(false);
        TitlePanel.GetComponent<PanelScript>().HidelPanel();
        MainMenuBtnPanel.GetComponent<PanelScript>().HidelPanel();
        yield return new WaitForSeconds(0.6f);

        MainMenuCanvas.SetActive(false);
		BuyLevelCanvas.GetComponent<BuyAllLevelScript>().showSelf();
        _levelManager.showWorldsScrollView(isWorldMenu);
    }

    public void backFromWorldScene()
    {
        isWorldMenu = false;
		BuyLevelCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
       // TapeToPlayCanvas.SetActive(true);
		setTapPlayBtn();

        TitlePanel.GetComponent<PanelScript>().ShowPanel();
        MainMenuBtnPanel.GetComponent<PanelScript>().ShowPanel();
        _gameManager.setLevelBase();
    }

	public void onSoundSwitch()
	{
		SoundManager.Instance.PlayClickSound();

		if (AudioListener.volume == 0.0f)
		{
			AudioListener.volume = 1.0f;
			Sprite temp = Resources.Load ("sound_on",typeof(Sprite)) as Sprite;
			musicIcon.GetComponent<Image>().overrideSprite  =  temp;//Resources.Load ("DressingScene/Body/body"+id.ToString()+".png",typeof(Sprite)) as Sprite;
			//musicOn.GetComponent<UISprite>().spriteName = "SoundOn-button";
		}
		else
		{
			AudioListener.volume = 0.0f;
			Sprite temp = Resources.Load ("sound_off",typeof(Sprite)) as Sprite;
			musicIcon.GetComponent<Image>().overrideSprite  =  temp;
			//musicOn.GetComponent<UISprite>().spriteName = "SoundOff-button";

		}
	}

	public void onLeaderboard()
	{
		//GPSInstanceX.instance.ShowLeaderboardForID();
	}

	public void onRateUs()
	{
		#if UNITY_ANDROID
		Application.OpenURL("market://details?id=" + Application.bundleIdentifier);
		#elif UNITY_IPHONE
		Application.OpenURL("https://itunes.apple.com/app/id"+GameConstants.APPLE_ID);
		#endif
	}

	public void onShare()
	{

	}

	public void onRestoreIAP()
	{
		//UnityIAP.instance.RestorePurchase();

	}

	public void onStoreOpen()
	{
		StartCoroutine("showStoreRout");
	}

	public void onStoreClose()
	{
		StartCoroutine("hideStoreRout");
	}

	IEnumerator showStoreRout()
	{
		TapeToPlayCanvas.SetActive(false);
		TitlePanel.GetComponent<PanelScript>().HidelPanel();
		MainMenuBtnPanel.GetComponent<PanelScript>().HidelPanel();
		yield return new WaitForSeconds(0.6f);

		MainMenuCanvas.SetActive(false);
		StoreCanvas.GetComponent<StorPanelScript>().showStorePan(StoreType.Global);

	}

	IEnumerator hideStoreRout()
	{
		Time.timeScale = 1;

		StoreCanvas.GetComponent<StorPanelScript>().hideStorePan();
		yield return new WaitForSeconds(0.25f);
		if(StoreCanvas.GetComponent<StorPanelScript>().myType == StoreType.Global)
		{
			MainMenuCanvas.SetActive(true);

		//	TapeToPlayCanvas.SetActive(true);
			setTapPlayBtn();

			TitlePanel.GetComponent<PanelScript>().ShowPanel();
			MainMenuBtnPanel.GetComponent<PanelScript>().ShowPanel();
		}
		else
		{
			_gameManager.isPaused = false;
			ScoreCanvas.SetActive(true);

			ScorePanel.GetComponent<PanelScript>().ShowPanel();
			_gameManager.resetBombs();
		}

	}

	public void onHomeClick()
	{
		Time.timeScale = 1;

		SceneManager.LoadScene("gameplay");
	}

	public void onBuyLevel()
	{
		//UnityIAP.instance.BuyItem(GameConstants.BUY_LEVELS_IAP,onSuccessBuyLev);

	}

	public bool onSuccessBuyLev()
	{
		setTapPlayBtn();
		return true;
	}

}

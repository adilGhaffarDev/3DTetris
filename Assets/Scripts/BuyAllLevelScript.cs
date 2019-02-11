using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAllLevelScript : MonoBehaviour 
{
	public GameObject taptoPlayPan;
	public GameObject buyLevelPan;

	public GameObject left;
	public GameObject right;
	// Use this for initialization
	void Start () 
	{
		taptoPlayPan.transform.localScale = Vector3.zero;
		buyLevelPan.transform.localScale = Vector3.zero;
	}
	
	public void showSelf()
	{
		gameObject.SetActive(true);

		canPlayOrNot(PlayerPrefs.GetInt(GameConstants.CURRENT_LEVEL,1));
//		iTween.ScaleTo(taptoPlayPan,iTween.Hash(
//			"scale",Vector3.one,
//			"time",0.25f,
//			"loopType",iTween.LoopType.none,
//			"easeType","easeInOutQuad"));
//
//		iTween.ScaleTo(buyLevelPan,iTween.Hash(
//			"scale",Vector3.one,
//			"time",0.25f,
//			"loopType",iTween.LoopType.none,
//			"easeType","easeInOutQuad"));
		
	}

	public void hideSelf()
	{
		taptoPlayPan.transform.localScale = Vector3.zero;
		buyLevelPan.transform.localScale = Vector3.zero;
		gameObject.SetActive(false);

	}



	public void canPlayOrNot(int levl)
	{
		if(levl == 1)
		{
			left.SetActive(false);
			right.SetActive(true);
		}
		else if(levl == 13)
		{
			left.SetActive(true);
			right.SetActive(false);
		}
		else
		{
			left.SetActive(true);
			right.SetActive(true);
		}
		if(PlayerPrefs.GetInt(GameConstants.ALL_LEVELS_UNLOCKED,0)==0)
		{
			if(levl <= GameConstants.BASE_UNLOCKED_LEVELS)
			{
				iTween.ScaleTo(taptoPlayPan,iTween.Hash(
					"scale",Vector3.one,
					"time",0.05f,
					"loopType",iTween.LoopType.none,
					"easeType","easeInOutQuad"));

				buyLevelPan.transform.localScale = Vector3.zero;
			}
			else
			{
				taptoPlayPan.transform.localScale = Vector3.zero;



				iTween.ScaleTo(buyLevelPan,iTween.Hash(
					"scale",Vector3.one,
					"time",0.05f,
					"loopType",iTween.LoopType.none,
					"easeType","easeInOutQuad"));
			}
		}
		else
		{
			iTween.ScaleTo(taptoPlayPan,iTween.Hash(
				"scale",Vector3.one,
				"time",0.05f,
				"loopType",iTween.LoopType.none,
				"easeType","easeInOutQuad"));

			buyLevelPan.transform.localScale = Vector3.zero;

		}
	}

//	public void onBuyLevels()
//	{
//		//inapp
//	}
//
//	public void onTapPlay()
//	{
//		//play
//	}
}

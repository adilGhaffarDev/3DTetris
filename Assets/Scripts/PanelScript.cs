using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScript : MonoBehaviour 
{
	public Transform showPosition;
	public Transform hidePosition;

	public void ShowPanel()
	{
		gameObject.SetActive(true);
		StartCoroutine("showRoutine");
	}

	public void HidelPanel()
	{
		StartCoroutine("hideRoutine");

	}
	
	IEnumerator showRoutine()
	{
        yield return 0;

		iTween.MoveTo(gameObject,iTween.Hash(
			"position",showPosition.position,
			"time",0.1f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));

		yield return new WaitForSeconds(0.1f);
		gameObject.transform.position = showPosition.position;
	}

	IEnumerator hideRoutine()
	{

		iTween.MoveTo(gameObject,iTween.Hash(
			"position",hidePosition.position,
			"time",0.1f,
			"loopType",iTween.LoopType.none,
			"easeType","easeInOutQuad",
			"ignoretimescale",true
		));

		yield return new WaitForSeconds(0.1f);
		gameObject.transform.position = hidePosition.position;
		gameObject.SetActive(false);


	}
}

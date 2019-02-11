using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public AudioSource BackgroundMusic;


	public AudioClip clickSound;
	public AudioClip PopSound;
	public AudioClip[] RotateSound;
	public AudioClip BombSound;
	public AudioClip levelWin;
	public AudioClip levelFail;
	public AudioClip swipeLevel;

	public AudioClip fastBlock;
	public AudioClip blockPlaceSound;

//	public AudioClip TwoXUPSound;
//	public AudioClip SheildUPSound;
//	int indexPopSound = 0;
	int indexRotateSound = 0;

    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();

                //Tell unity not to destroy this object when loading a new scene!
           //     DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake() 
    {
//        Debug.Log("Awake Called");
        if(_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
          //  DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if(this != _instance)
                Destroy(gameObject);
        }
    }



	public void PlayPopBlockSound( )
	{
		
		AudioSource.PlayClipAtPoint(PopSound,transform.position);
			
	}

	public void PlayRotateSound( )
	{
		if(PlayerPrefs.GetInt("Sound",1) == 1)
		{
			if(indexRotateSound < RotateSound.Length)
			{
				AudioSource.PlayClipAtPoint(RotateSound[indexRotateSound],transform.position);
			}
			else
			{
				indexRotateSound = 0;
				AudioSource.PlayClipAtPoint(RotateSound[indexRotateSound],transform.position);

			}
			indexRotateSound++;
		}
	}

	public void PlayClickSound()
	{
		if(PlayerPrefs.GetInt("Sound",1) == 1)
		{
			AudioSource.PlayClipAtPoint(clickSound,transform.position);
		}
	}

	public void PlayLevelFailSound()
	{
		if(PlayerPrefs.GetInt("Sound",1) == 1)
		{
			AudioSource.PlayClipAtPoint(levelFail,transform.position);
		}
	}

	public void PlayLevelWinSound( )
	{
		if(PlayerPrefs.GetInt("Sound",1) == 1)
		{
			AudioSource.PlayClipAtPoint(levelWin,transform.position);
		}
	}

	public void PlaySwipeLevelSound( )
	{
		if(PlayerPrefs.GetInt("Sound",1) == 1)
		{
			AudioSource.PlayClipAtPoint(swipeLevel,transform.position);
		}
	}

	public void PlayBombSound( )
	{
		if(PlayerPrefs.GetInt("Sound",1) == 1)
		{
			AudioSource.PlayClipAtPoint(BombSound,transform.position);
		}
	}

	public void PlayFastBlockSound( )
	{
//		if(PlayerPrefs.GetInt("Sound",1) == 1)
//		{
//			AudioSource.PlayClipAtPoint(fastBlock,transform.position);
//		}
	}

	public void PlayPlaceBlockSound( )
	{
		if(PlayerPrefs.GetInt("Sound",1) == 1)
		{
			AudioSource.PlayClipAtPoint(blockPlaceSound,transform.position);
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPallete : MonoBehaviour 
{
	
	public Dictionary<int,Color[]> colorDictionary = new Dictionary<int,Color[]>();

	public Dictionary<int,Texture> texDictionary = new Dictionary<int,Texture>();

	[SerializeField]
	Texture[] textures;

	[SerializeField]
	Texture dollarBlockTex;

	[SerializeField]
	int[] levelWithTex;

	[SerializeField]
	Color[] Level1 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level2 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level3 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level4 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level5 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level6 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level7 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level8 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level9 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level10 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level11 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};


	[SerializeField]
	Color[] Level12 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	[SerializeField]
	Color[] Level13 = new Color[] { 
		new Color(220f/255f, 190f/255f, 45.0f/255f),
		new Color(207f/255f, 135f/255f, 56.0f/255f),
		new Color(204f/255f, 111f/255f, 75f/255f),
		new Color(200f/255f, 100f/255f, 110f/255f),
		new Color(200f/255f, 90.0f/255f, 145f/255f)
	};

	static ColorPallete m_instance = null;

	public static ColorPallete getInstance()
	{
		if(m_instance == null)
		{
			m_instance = GameObject.FindObjectOfType<ColorPallete>();
			DontDestroyOnLoad(m_instance.gameObject);
		}

		return m_instance;
	}

	void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//m_instance.count = 0;
			Destroy(gameObject);
		}
	}

	void Start()
	{
		
		colorDictionary.Add(1,Level1);
		colorDictionary.Add(2,Level2);
		colorDictionary.Add(3,Level3);
		colorDictionary.Add(4,Level4);
		colorDictionary.Add(5,Level5);
		colorDictionary.Add(6,Level6);
		colorDictionary.Add(7,Level7);
		colorDictionary.Add(8,Level8);
		colorDictionary.Add(9,Level9);
		colorDictionary.Add(10,Level10);
		colorDictionary.Add(11,Level11);
		colorDictionary.Add(12,Level12);
		colorDictionary.Add(13,Level13);

		for(int i = 0; i < levelWithTex.Length; i++)
		{
			texDictionary.Add(levelWithTex[i],textures[i]);
		}
	}

	public Color getColor(int levelNo)
	{
		bool istexLev = false;
		for(int i = 0; i < levelWithTex.Length; i++)
		{
			if(levelNo == levelWithTex[i])
			{
				istexLev = true;
			}
		}
		if(!istexLev)
		{
			Color[] levelColors;
			if(colorDictionary.TryGetValue(levelNo,out levelColors))
			{
				int rand = Random.Range(0,5);
				return levelColors[rand];
			}
		}
		else
		{
			return Color.white;
		}
//		if(levelNo == 1)
//		{
//			int rand = Random.Range(0,4);
//			return Level1[rand];
//		}
//		else if(levelNo == 2)
//		{
//			int rand = Random.Range(0,4);
//			return Level2[rand];
//		}
		return Color.white;

	}

	public Texture getTetxure(int levelNo)
	{
		bool istexLev = false;
		for(int i = 0; i < levelWithTex.Length; i++)
		{
			if(levelNo == levelWithTex[i])
			{
				istexLev = true;
			}
		}
		if(istexLev)
		{
			Texture tex;

			if(texDictionary.TryGetValue(levelNo,out tex))
			{
				
				return tex;
			}
		}
		else
		{
			return null;
		}
		//		if(levelNo == 1)
		//		{
		//			int rand = Random.Range(0,4);
		//			return Level1[rand];
		//		}
		//		else if(levelNo == 2)
		//		{
		//			int rand = Random.Range(0,4);
		//			return Level2[rand];
		//		}
		return null;

	}

	public void changeColorOrTex(Renderer rend, int levNo, Color color)
	{
		bool istexLev = false;
		for(int i = 0; i < levelWithTex.Length; i++)
		{
			if(levNo == levelWithTex[i])
			{
				istexLev = true;
			}
		}
		if(istexLev)
		{
			Texture tex;

			if(texDictionary.TryGetValue(levNo,out tex))
			{
				rend.material.color = Color.white;
				rend.material.mainTexture = tex;
			}
		}
		else
		{
			rend.material.color = color;
//			Color[] levelColors;
//			if(colorDictionary.TryGetValue(levNo,out levelColors))
//			{
//				int rand = Random.Range(0,5);
//				rend.material.color = levelColors[rand];
//				//return levelColors[rand];
//			}
		}
	}

	public void setDollarBlockTex(Renderer rend)
	{
		bool istexLev = false;
		rend.material.color = Color.white;
		rend.material.mainTexture = dollarBlockTex;
		
	}

}

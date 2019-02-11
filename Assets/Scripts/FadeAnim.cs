using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnim : MonoBehaviour 
{
	Image _image;
	float count = 0.1f;
	public float duration = 0.5F;
	Color color0;
	public Color color1 = Color.white;
	// Use this for initialization
	void Start()
	{
		_image = GetComponent<Image>();
		color0 = _image.color;
		//color1 = new Color(color0.r,color0.g,color0.b,color0.a/4);
	}
	void Update() 
	{
		count += 0.02f;
		float t = Mathf.PingPong(count, duration) / duration;
		_image.color = Color.Lerp(color0, color1, t);
	}
}

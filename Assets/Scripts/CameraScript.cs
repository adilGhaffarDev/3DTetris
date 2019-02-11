using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour 
{

	public Transform CameraMainPos;
	public Transform CameraPlayPos;
	public bool startMove = false;
    public bool zoomCamera = false;

	Vector3 targetPosition;
    Vector3 targetPositionZoom;
	public float speed;

    public float cameraZ   = 0.5f;
    public float zoomValue = 0.5f;

    public float targetCameraZ = 0;
    public float targetZoomValue = 0;

    public float currentCameraZ = 9;
    public float currentZoomValue = 7;
    public int sign;

    float maxLimit;
	// Use this for initialization
	void Start () 
	{
		targetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(startMove)
		{
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
		}

        if (zoomCamera)
        {
            

            float step = speed * Time.deltaTime;

//            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + cameraZ);

            if (sign > 0)
            {
                if (transform.position.y < targetPositionZoom.y)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPositionZoom, step);
                }
                if (Camera.main.orthographicSize < maxLimit)
                {
                    Camera.main.orthographicSize += (step * sign);
                }
            }
            else if (sign < 0)
            {
                if (transform.position.y > targetPositionZoom.y)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPositionZoom, step);
                }
                if (Camera.main.orthographicSize > maxLimit)
                {
                    Camera.main.orthographicSize += (step * sign);
                }
            }
        }
	}

	public void startSceneChangeMove(int scene)
	{
		startMove = true;
		if(scene == 0)
		{
			targetPosition = CameraMainPos.position;
		}
		else if(scene == 1)
		{
			targetPosition = CameraPlayPos.position;
		}
	}

    public void startZoom(int maxH, int sign)
    {
//		Debug.Log("ZOOM:: " + maxH);
        startMove = false;
//        Debug.Log("maxH: " + maxH);
        targetCameraZ = 0;
        zoomCamera = true;
        float offset = maxH * 0.5f;
        float offset2 = maxH * 0.25f;
        maxLimit = offset2 + currentZoomValue;
        this.sign = sign;
        targetPositionZoom = new Vector3(transform.position.x, currentCameraZ + offset, transform.position.z);
    }
}

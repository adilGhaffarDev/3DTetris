using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    public int internalHeight;
	public int globalheight = -1;//starts from 1
	public Vector3 position;
	Shape parent;

    public int getInternalHeight()
    {
        return internalHeight;
    }

    public int getGlobalHeight()
    {
        return globalheight;
    }

    public Vector3 getPosition()
    {
        return position;
    }

    public Vector3 getPositionInTower()
    {
        return new Vector3(position.x, position.y, globalheight);
    }

    public Shape getParent()
    {
        return parent;
    }

	// Use this for initialization
	void Start ()
    {
//		Debug.Log("Start : block");


		parent = transform.GetComponentInParent<Shape>();
        GetComponent<BoxCollider>().isTrigger = false;
//        Debug.Log("Arslan::Block::Start");

//        transform.GetComponent<Rigidbody>().AddForce(Vector3.down*500);
	}
	

	public void setHeight(int h)
	{
        globalheight = h;
	}

    void OnTriggerEnter(Collider other)
	{
        Debug.Log("Arslan::OnTriggerEnter");
//        StartCoroutine(parent.onBottomCollisionEnter(other, this));
	}

    void OnCollisionEnter(Collision other)
    {
//        Debug.Log("Arslan::OnCollisionEnter");
//		transform.localPosition = new Vector3(transform.localPosition.x, -5.14f, transform.localPosition.z);
//		gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
//		gameObject.GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.None;
//		gameObject.GetComponent<Rigidbody> ().velocity = Vector3.down*0;

        StartCoroutine(parent.onBottomCollisionEnter(other, this));

    }

	public void changeColor(Color col)
	{
		gameObject.GetComponent<Renderer> ().material.color = col;
	}

    public void setVeloc(float velo)
    {
		if(velo > 0)
		{
			gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation ;
			gameObject.GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Interpolate;
		}
		else
		{
			transform.localPosition = new Vector3(transform.localPosition.x, -5.14f, transform.localPosition.z);
			gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
			gameObject.GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.None;
		}
        gameObject.GetComponent<Rigidbody> ().velocity = Vector3.down*velo;
    }

}

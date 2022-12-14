using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class EditMover : MonoBehaviour
{
	[SerializeField]
	private float speed = 5.0f;

	[SerializeField]
	private Material originalColor;


	private GameObject childGameObject;


	private bool _isMoveAllow = false;
	
	private bool _isDelAllowMover = false;


	

	public bool IsMoveAllow
	{
		set
		{
			this._isMoveAllow = value;
		}
	}


	public bool IsDelAllowMover
	{
		set
		{
			this._isDelAllowMover = value;
		}
	}




    private void Start()
    {

		Transform[] childTrans;
		childTrans = ComFunctions.GetChildren(gameObject.transform);

		foreach(Transform child in childTrans)
        {
            if (child.gameObject.name == "Head")
            {
				childGameObject = child.gameObject;
			}
		}

		

	}

    // Update is called once per frame
    void Update()
	{


		MovingObject();

		//Debug.Log(_isDelAllowMover);



	}

	

	private void MovingObject()
	{

		if (_isMoveAllow)
		{

			GetComponent<Renderer>().material.color = Color.blue;

			childGameObject.GetComponent<Renderer>().material.color = Color.blue;

			if (Input.GetKey("up"))
			{
				transform.position += transform.forward * speed * Time.deltaTime;
			}
			if (Input.GetKey("down"))
			{
				transform.position -= transform.forward * speed * Time.deltaTime;
			}
			if (Input.GetKey("right"))
			{
				transform.position += transform.right * speed * Time.deltaTime;
			}
			if (Input.GetKey("left"))
			{
				transform.position -= transform.right * speed * Time.deltaTime;
			}


            if (_isDelAllowMover)
            {
				_isDelAllowMover = false;
				//Debug.Log("why");
				Destroy(gameObject);
			}


		}
		else
		{
			GetComponent<Renderer>().material.color = originalColor.color;
			childGameObject.GetComponent<Renderer>().material.color = originalColor.color;
		}

	}
}

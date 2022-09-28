using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class EditTarget : MonoBehaviour
{
	[SerializeField]
	private float speed = 5.0f;

	[SerializeField]
	private Material originalColor;

	private GameObject childGameObject;

	private bool _isTargetAllow = false;
	private bool _isDelAllowTarget = false;

	public bool IsTargetAllow	{set{this._isTargetAllow = value;}}

	public bool IsDelAllowTarget	{ set{this._isDelAllowTarget = value;}}


	private void Start()
	{
		Transform[] childTrans;
		childTrans = ComFunctions.GetChildren(gameObject.transform);

		foreach (Transform child in childTrans)
		{
			if (child.gameObject.name == "Cube (1)")
			{
				childGameObject = child.gameObject;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		MovingObject();
	}

	private void MovingObject()
	{

		if (_isTargetAllow)
		{

			GetComponent<Renderer>().material.color = Color.red;

			childGameObject.GetComponent<Renderer>().material.color = Color.red;

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

			if (_isDelAllowTarget)
			{
				_isDelAllowTarget = false;
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

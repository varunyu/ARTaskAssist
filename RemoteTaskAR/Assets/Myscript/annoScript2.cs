﻿using UnityEngine;
using System.Collections;

public class annoScript2 : MonoBehaviour {

	private int annotype;
	public Material mat1;
	public Material mat2;
	public Vector3 initCam;

	private GameObject axisObject;
	// Use this for initialization

	private Vector3 initPos;


	void Start () {
		initPos = gameObject.transform.position;
		axisObject = gameObject.transform.GetChild (0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SelectedAnnotation()
	{
		ChangeMatColor(mat2);
		axisObject.SetActive (true);
	}
	public void DeselectedAnnotatin()
	{
		ChangeMatColor(mat1);
		axisObject.SetActive (false);
	}
	private void ChangeMatColor(Material mat)
	{
		if (annotype >= 3)
		{
			Transform c = gameObject.transform.GetChild(1).gameObject.transform.GetChild(0);
			c.GetComponent<MeshRenderer>().material = mat;
		}
		else
		{
			Transform c = gameObject.transform.GetChild(1);
			c.GetComponent<MeshRenderer>().material = mat;
		}

	}
	public void SetEuler(){
		gameObject.transform.eulerAngles = new Vector3 (0, 0, 0);
	}
	public void SetInitCam(Vector3 cam){
		initCam = cam;
	}
	public Vector3 GetInitCam(){
		return initCam;
	}
	public void SetPos(Vector3 p){
		initPos = p;
	}
	public Vector3 GetPos(){
		return initPos;
	}
	public void SetAnnoType(int t)
	{
		annotype = t;
	}

	public void SetOrientation(float deg,string Axis){
		
		if(Axis.Equals("X"))
		{
			gameObject.transform.Rotate (Vector3.right, deg, Space.Self);
		}
		else if(Axis.Equals("Y"))
		{
			gameObject.transform.Rotate (Vector3.up, deg, Space.Self);

		}
		else if(Axis.Equals("Z"))
		{
			gameObject.transform.Rotate (Vector3.forward, deg, Space.Self);
		}		
	}



}

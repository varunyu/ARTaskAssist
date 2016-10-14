using UnityEngine;
using System.Collections;

public class annoScript2 : MonoBehaviour {

	private int annotype;
	public Material mat1;
	public Material mat2;
	private Vector3 initCam;

	// Use this for initialization

	private Vector3 pos;

	void Start () {
		pos = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SelectedAnnotation()
	{
		ChangeMatColor(mat2);
	}
	public void DeselectedAnnotatin()
	{
		ChangeMatColor(mat1);
	}
	private void ChangeMatColor(Material mat)
	{
		if (annotype > 4)
		{
			Transform c = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0);
			c.GetComponent<MeshRenderer>().material = mat;
		}
		else
		{
			Transform c = gameObject.transform.GetChild(0);
			c.GetComponent<MeshRenderer>().material = mat;
		}

	}
	public void SetInitCam(Vector3 cam){
		initCam = cam;
	}
	public Vector3 GetInitCam(){
		return initCam;
	}
	public void SetPos(Vector3 p){
		pos = p;
	}
	public Vector3 GetPos(){
		return pos;
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

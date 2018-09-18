using UnityEngine;
using System.Collections;

public class AnnoScript : MonoBehaviour {


	private Vector3 initCamPos;
	private Vector3 initObjPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Rotate90Deg(){
		gameObject.transform.Rotate (new Vector3 (90, 0, 0));
	}

	public void SetInitCamPos(Vector3 cam){
		initCamPos = cam;
	}
	public Vector3 GetInitCamPos(){
		return initCamPos;
	}
	public void SetInitObjPos(Vector3 obj){
		initObjPos = obj;
	}
	public Vector3 GetInitObjPos(){
		return initObjPos;
	}

	public void SetInitPos(Vector3 init){
		gameObject.transform.eulerAngles = init;
	}
}

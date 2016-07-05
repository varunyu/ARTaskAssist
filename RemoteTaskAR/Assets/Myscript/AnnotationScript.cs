using UnityEngine;
using System.Collections;

public class AnnotationScript : MonoBehaviour {

	public Vector3 gravity;
	public Vector3 vGra;
	public Vector3 v3Current;
	public bool parallel;

	public Vector3 initCamPos;
	public Vector3 initPos;


	// Use this for initialization
	void Start () {
		gravity = Input.gyro.gravity;

		Input.compass.enabled = true;


		Input.gyro.enabled = true;
//		//print (Input.gyro.enabled);
//		VGra = new Vector3(0,0,0);
//		v3current = this.gameObject.transform.eulerAngles;

		SetRotation ();
	}

	public void SetRotation(){
		

		Vector3 camAngle = Camera.main.transform.eulerAngles;

		Quaternion rotation = Quaternion.Euler (camAngle);
		Vector3 grav = new Vector3 (Input.acceleration.x,Input.acceleration.y,-Input.acceleration.z);


		Vector3 rotaGrav = rotation * grav;
		float toNorth = Camera.main.transform.eulerAngles.y-Input.compass.magneticHeading;
	


		if (rotaGrav.y < -0 ) {
			//print ("test");
			gameObject.transform.rotation = Quaternion.LookRotation (rotaGrav, Vector3.back);


		} else {
			gameObject.transform.rotation = Quaternion.LookRotation(rotaGrav,Vector3.up);
		}

		if (parallel) {
			//gameObject.transform.rotation =  (new Vector3 (90, 0, 0));
			gameObject.transform.rotation = Quaternion.Euler(90,0,toNorth);
		} else {
			gameObject.transform.rotation = Quaternion.Euler(0,-toNorth,0);
		}
		//print (Camera.main.transform.eulerAngles.y+" : "+Input.compass.magneticHeading);


	}
	
	// Update is called once per frame
	void Update () {
		
		//SetRotation ();
	}

	public void SetState(bool state){
		parallel = state ;
	}

	public void InitCamaraPosition(Vector3 pos){
		initCamPos = pos;
		initPos = gameObject.transform.position;
	}

	public Vector3 GetAnnoPos(){
		return initPos;
	}
	public Vector3 GetInitCamPos(){
		return initCamPos;
	}
	public void SetPos(Vector3 currentPos){
		gameObject.transform.position = currentPos;
	}
}


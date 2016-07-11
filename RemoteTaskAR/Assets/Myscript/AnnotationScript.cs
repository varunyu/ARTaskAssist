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


		//Input.gyro.enabled = true;
		SetRotation ();
	}

	public void SetRotation(){
		

		Vector3 camAngle = Camera.main.transform.eulerAngles;

		Quaternion rotation = Quaternion.Euler (camAngle);
		Vector3 grav = new Vector3 (Input.acceleration.x,Input.acceleration.y,-Input.acceleration.z);


		Vector3 rotaGrav = rotation * grav;
		float toNorth = Input.compass.magneticHeading;
	
		//print (rotaGrav);

		if (rotaGrav.y < -0 ) {
			//print ("test");
			gameObject.transform.rotation = Quaternion.LookRotation (rotaGrav, Vector3.back);


		} else {
			gameObject.transform.rotation = Quaternion.LookRotation(rotaGrav,Vector3.up);
		}
		Vector3 cEuler = gameObject.transform.eulerAngles;
		if (parallel) {
			gameObject.transform.Rotate (new Vector3 (camAngle.x+90, camAngle.y, camAngle.z),Space.Self);
			//gameObject.transform.Rotate(new Vector3(cEuler.x+90,cEuler.y,cEuler.z));
			//gameObject.transform.rotation = Quaternion.Euler(cEuler.x+90,cEuler.y,cEuler.z+toNorth);
		
		} else {
			gameObject.transform.Rotate (new Vector3 (camAngle.x, camAngle.y, camAngle.z),Space.Self);


			//gameObject.transform.rotation = Quaternion.Euler(cEuler.x,cEuler.y-toNorth,cEuler.z);
			//gameObject.transform.Rotate(new Vector3(cEuler.x,cEuler.y,cEuler.z));
		}
		//print (Camera.main.transform.eulerAngles.y+" : "+Input.compass.magneticHeading);
		//print (gameObject.transform.eulerAngles);

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
	public void SetOrientation(){

	}
}


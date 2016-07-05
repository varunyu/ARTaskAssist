using UnityEngine;
using System.Collections;

public class Touchinput : MonoBehaviour {



	public GameObject Prefab;
	private Camera camera;
	private AnnotationScript annoScrip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		foreach(Touch touch in Input.touches){


			//if(Input.touchCount > 0 && Input.GetTouch(0).phase==TouchPhase.Began){				

			if (touch.phase == TouchPhase.Began) {
				
				print ("position "+ touch.position);
				//Vector3 fingerPos = touch.position;
				//fingerPos.z = 8;
				Vector3 fingerPos = new Vector3 (touch.position.x,touch.position.y,8);

				Vector3 objePos = Camera.current.ScreenToWorldPoint (fingerPos);

				GameObject annotation = Instantiate (Prefab,objePos,transform.rotation) as GameObject;
				annotation.transform.parent = this.gameObject.transform;

				print (annotation.ToString ());
				annoScrip = (AnnotationScript)annotation.GetComponent (typeof(AnnotationScript));
				//GyroScrip.setRotation ();

			}
		}
	}
}

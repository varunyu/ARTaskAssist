using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AR_DeviceEventController : MonoBehaviour {

	private float depth;
	private float scHeight;
	private float scWidth;

	public GameObject selectedObject;
	private Vector3 rayEnd;
	private float rayDis;
	private Ray ray;

	// Use this for initialization
	private float minPitcgDis = 10f;
	private float minAngle = 10f;
	void Start () {
		scHeight = Screen.height;
		scWidth = Screen.width;
	}
	public Image target;
	// Update is called once per frame

	private Vector2 t1PrevPos;
	private Vector2 t2PrevPos;
	private Vector2 prevDir;
	private Vector2 currentDir;

	float prevMagnitude;
	float cMagnitude;
	float diffMagnitude;
	float rotaSpeed = 20.0f;

	/*
	 * NONE
	 * ADD
	 * EDIT
	 * */
	private string state;

	private AnnotationControl AC;
	private AR_DeviceUIController ARDUi;
	private UserStudy uS;

	void Awake(){
		AC = (AnnotationControl)transform.parent.gameObject.GetComponent(typeof(AnnotationControl));
		AC.SetGravityMode(false);
		SetState ("NONE");

		ARDUi = (AR_DeviceUIController)gameObject.GetComponent(typeof(AR_DeviceUIController));
		uS = (UserStudy)transform.parent.gameObject.GetComponent(typeof(UserStudy));
	}

	void Update () {

		if (Input.touches.Length > 0) {
			Touch touch = Input.GetTouch (0);

			if (!EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {

				if (state.Equals ("EDIT")) {
					ray = Camera.main.ScreenPointToRay (target.transform.position);
					//rayEnd = ray.GetPoint (rayDis);


					if (touch.phase == TouchPhase.Began) {
						//ray = Camera.main.ScreenPointToRay(GameObject);
						foreach (RaycastHit hit  in Physics.RaycastAll(ray)) {
							if (hit.collider.tag.Equals ("annotation")) {

								rayDis = Vector3.Distance (Camera.main.transform.position, hit.transform.position);
								selectedObject = hit.transform.gameObject;
								AC.SetSelectedAnnotation (selectedObject);
							}

						}
					} else {
						if (selectedObject != null) {
							rayEnd = ray.GetPoint (rayDis);

							selectedObject.transform.position = rayEnd;
						}
					}
					if (selectedObject != null) {
						if (Input.touchCount == 1) {
							if (touch.phase == TouchPhase.Moved) {
								if (Mathf.Abs (touch.deltaPosition.x) >= 5f) {
									selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.up,
										touch.deltaPosition.x * rotaSpeed * Time.deltaTime * -1);
								}

								if (Mathf.Abs (touch.deltaPosition.y) >= 5f) {
									//selectedObject.transform.Rotate (Camera.main.transform.up,touch.deltaPosition.y*rotaSpeed*Time.deltaTime);
									selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.right,
										touch.deltaPosition.y * rotaSpeed * Time.deltaTime);
								}
								uS.SetRotateMode (true);
							}

						}
						if (Input.touchCount == 2) {
							if( (touch.phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Stationary))
							{
								//if (Mathf.Abs (touch.deltaPosition.x) >= 5f) {
									selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.up,
										touch.deltaPosition.x * rotaSpeed * Time.deltaTime * -1);
								//}

								//if (Mathf.Abs (touch.deltaPosition.y) >= 5f) {
									//selectedObject.transform.Rotate (Camera.main.transform.up,touch.deltaPosition.y*rotaSpeed*Time.deltaTime);
									selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.right,
										touch.deltaPosition.y * rotaSpeed * Time.deltaTime);
								//}
								uS.SetRotateMode (true);
							}else if(touch.phase == TouchPhase.Stationary && Input.GetTouch(1).phase == TouchPhase.Moved){
								if (Mathf.Abs (Input.GetTouch (1).deltaPosition.x) >= 5f) {
									selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.up,
										Input.GetTouch (1).deltaPosition.x * rotaSpeed * Time.deltaTime * -1);
								}

								if (Mathf.Abs (Input.GetTouch (1).deltaPosition.y) >= 5f) {
									//selectedObject.transform.Rotate (Camera.main.transform.up,touch.deltaPosition.y*rotaSpeed*Time.deltaTime);
									selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.right,
										Input.GetTouch (1).deltaPosition.y * rotaSpeed * Time.deltaTime);
								}
								uS.SetRotateMode (true);
							}else{
								t1PrevPos = touch.position - touch.deltaPosition;
								t2PrevPos = Input.GetTouch (1).position - Input.GetTouch (1).deltaPosition;

								prevMagnitude = (t1PrevPos - t2PrevPos).magnitude;
								cMagnitude = (touch.position - Input.GetTouch (1).position).magnitude;

								diffMagnitude = (prevMagnitude - cMagnitude);
								//print (diffMagnitude);

								if (Mathf.Abs (diffMagnitude) >= minPitcgDis) {
									rayDis += diffMagnitude * 0.05f;
									//Pitch finger
								} else {

									prevDir = t2PrevPos - t1PrevPos;
									currentDir = Input.GetTouch (1).position - touch.position;
									float angle = Vector2.Angle (prevDir, currentDir);
									Vector3 LR = Vector3.Cross (prevDir, currentDir);
									//if (Mathf.Abs(angle) >= minAngle) {

									if (LR.z > 0) {
										selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.forward,
											angle*2f);
									} else {
										selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.forward,
											-angle*2f);
									}
								}
								uS.SetRotateMode (true);
							}
							if((touch.phase == TouchPhase.Stationary && Input.GetTouch(1).phase == TouchPhase.Stationary)){
								uS.SetRotateMode (false);
							}
						}
					}
					if (selectedObject == null) {
						uS.SetRotateMode (false);
					}
					///
					 /*
					if (touch.phase == TouchPhase.Moved) {
						if (selectedObject != null) {
							if (Input.touchCount == 1) {

								if (Mathf.Abs (touch.deltaPosition.x) >= 5f) {
									selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.up,
										touch.deltaPosition.x * rotaSpeed * Time.deltaTime * -1);
								}

								if (Mathf.Abs (touch.deltaPosition.y) >= 5f) {
									//selectedObject.transform.Rotate (Camera.main.transform.up,touch.deltaPosition.y*rotaSpeed*Time.deltaTime);
									selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.right,
										touch.deltaPosition.y * rotaSpeed * Time.deltaTime);
								}
							} else if (Input.touchCount >= 2) {

								t1PrevPos = touch.position - touch.deltaPosition;
								t2PrevPos = Input.GetTouch (1).position - Input.GetTouch (1).deltaPosition;

								prevMagnitude = (t1PrevPos - t2PrevPos).magnitude;
								cMagnitude = (touch.position - Input.GetTouch (1).position).magnitude;

								diffMagnitude = (prevMagnitude - cMagnitude);
								print (diffMagnitude);

								if (Mathf.Abs (diffMagnitude) >= minPitcgDis) {
									rayDis += diffMagnitude * 0.05f;
									//Pitch finger
								} else {

									prevDir = t2PrevPos - t1PrevPos;
									currentDir = Input.GetTouch (1).position - touch.position;
									float angle = Vector2.Angle (prevDir, currentDir);
									Vector3 LR = Vector3.Cross (prevDir, currentDir);
									//if (Mathf.Abs(angle) >= minAngle) {

									if (LR.z > 0) {
										selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.forward,
											angle*2f);
									} else {
										selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.forward,
											-angle*2f);
									}
								}

							}
						}


					}*/
					///


					if (touch.phase == TouchPhase.Ended) {
						selectedObject = null;
						AC.DeselectedAnnotation ();
					}			

				} else if (state.Equals ("ADD")) {
					if (touch.phase == TouchPhase.Began) {
						ray = Camera.main.ScreenPointToRay (touch.position);
						AC.ObjectInstantiate (ray.GetPoint (90f));
						SetState ("EDIT");
					}
					
				}
					

			}
		}


	}

	public void SetState(string text){
		state = text;
	}
	public void RemoveObject(){
		Destroy (selectedObject);
		AC.DeselectedAnnotation ();
		selectedObject = null;
	}
	public void PitchFinger(){
		
	}
	public void RotateFinger(){
		
	}

}

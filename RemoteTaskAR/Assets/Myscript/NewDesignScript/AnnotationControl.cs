using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnnotationControl : MonoBehaviour {

	// Use this for initialization
	private GameObject selectedObject;
	private GameObject annoPrefab;
	public GameObject marker;

	public bool gravityMode ;

	public GameObject[] arrowPrefab = new GameObject[2];
	public GameObject[] halfCPrefab = new GameObject[2];

	public Text debugText;
	private UserStudyUIController usUI;
	private UserStudy usScript;

	private Vector3 worldCoor;
	/*
	 * case 1 Arrow point down
	 * case 2 Arrow parallel to ground
	 * case 10 Circle parallel to ground
	 * case 11 Circle perpendicular to ground
	 */

	private AnnoScript aScript;
	private int annoType;


	public GameObject debugFinger;

	public void ShowDebugText(){
		if (selectedObject != null) {
			debugText.text = "Position X :"+selectedObject.transform.position.x +
				" Y :"+selectedObject.transform.position.y +
				" Z :"+selectedObject.transform.position.z + "\n"+
				" Rotate X :" +selectedObject.transform.eulerAngles.x +
				" Y :" +selectedObject.transform.eulerAngles.y +
				" Z :" +selectedObject.transform.eulerAngles.z ;
		} else {
			debugText.text = "Selected Object first";
		}
	}
	void Awake(){
		usUI = (UserStudyUIController)gameObject.GetComponent(typeof(UserStudyUIController));
		usScript = (UserStudy)gameObject.GetComponent(typeof(UserStudy));
	}
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
		if (selectedObject != null) {
			usScript.CheckCorrectness (selectedObject);
		}
		/*
		if(Input.touches.Length > 0 ) {
			
			for (int i = 0; i < Input.touches.Length; i++) {
				ShowInput(Input.GetTouch(i),i);

			}
		}*/
	}

	private void ShowInput(Touch t,int num){
		if (t.phase == TouchPhase.Began) {
			debugFinger.transform.GetChild (num).gameObject.SetActive (true);
			debugFinger.transform.GetChild (num).gameObject.transform.position 
			= new Vector3 (t.position.x, t.position.y, 0);
		} else if (t.phase == TouchPhase.Moved) {
			debugFinger.transform.GetChild (num).gameObject.transform.position 
			= new Vector3 (t.position.x, t.position.y, 0);
		} else if (t.phase == TouchPhase.Ended) {
			debugFinger.transform.GetChild (num).gameObject.SetActive (false);
		}
	}

	public void SwitchMode(){
		if (gravityMode) {
			gravityMode = false;
		} else
			gravityMode = true;
	}
	public void SetGravityMode(bool t){
		usUI.SetMode (t);
		gravityMode = t;
	}

	public void DeselectedAnnotation (){
		if (selectedObject != null){
			selectedObject.transform.GetChild (0).gameObject.SetActive (true);
			selectedObject.transform.GetChild (1).gameObject.SetActive (false);
			selectedObject = null;
			aScript = null;
		}
	}
	public void SetSelectedAnnotation(GameObject anno){
		selectedObject = anno;
		selectedObject.transform.GetChild (0).gameObject.SetActive (false);
		selectedObject.transform.GetChild (1).gameObject.SetActive (true);
		aScript = (AnnoScript)selectedObject.GetComponent(typeof(AnnoScript));
	}
	public GameObject GetSelectedAnnotation(){
		return selectedObject;
	}
	public bool perpToG;
	public void  ObjectInstantiate(Vector3 objPos){
		perpToG = false;
		annoPrefab = null;
		switch (annoType) {
		case 1:
			annoPrefab = arrowPrefab[0];
			perpToG = false;
			break;
		case 2:
			annoPrefab = arrowPrefab[0];
			perpToG = true;
			break;
		case 10:
			annoPrefab = halfCPrefab[0];
			perpToG = false;
			break;
		case 11:
			annoPrefab = halfCPrefab[0];
			perpToG = true;
			break;
		}
		GameObject newAnnotation = Instantiate (annoPrefab, objPos, Camera.main.transform.rotation) as GameObject;
		newAnnotation.transform.parent = marker.transform;
		//selectedObject = newAnnotation;
		aScript = (AnnoScript)newAnnotation.GetComponent(typeof(AnnoScript));
		if (worldCoor != null) {
			aScript.SetInitPos (worldCoor);
		}
		//aScript.SetInitCamPos (Camera.main.transform.position);
		SetSelectedAnnotation(newAnnotation);


		if (gravityMode) {
			SetInitialRotation ();

			if (perpToG) {
				aScript.Rotate90Deg ();
				//selectedObject.transform.Rotate(new Vector3 (90, 0, 0));
				//selectedObject.transform.Rotate(90,0,0);
			}
		}
	}
	public void SetWorldCoor(Vector3 pos){
		worldCoor = pos;
	}
	public void SetAnnoType(int i){
		annoType = i;
	}
	private Vector3 GetGravityVector(){
		Vector3 camAngle = Camera.main.transform.eulerAngles;

		Quaternion rotation = Quaternion.Euler (camAngle);
		Vector3 grav = new Vector3 (Input.acceleration.x,Input.acceleration.y,-Input.acceleration.z);


		Vector3 rotaGrav = rotation * grav;
		return rotaGrav;
	}

	private void SetInitialRotation(){
		Vector3 rotaGrav = GetGravityVector ();
		selectedObject.transform.rotation = Quaternion.FromToRotation (Vector3.down,rotaGrav);
	}


	public void SetObjInitPos(){
		if (aScript != null && selectedObject != null) {
			aScript.SetInitCamPos (Camera.main.transform.position);
			aScript.SetInitObjPos (selectedObject.transform.position);
		}
	}
	public Vector3 GetObjInitPos(){
		
		return aScript.GetInitObjPos ();
		
	}
	public Vector3 GetObjInitCamPos(){
		
		return aScript.GetInitCamPos ();

	}


	public void RotateObjRelateToGX(float deg){
		Vector3 rotaGrav = GetGravityVector ();
		selectedObject.transform.RotateAround (selectedObject.transform.position,rotaGrav,deg);
	}
	public void RotateObjRelateToGY(float deg){
		
	}
	public void RotateObjRelateToGZ(float deg){
		
	}
	public void RotateObjRelateToCameraX(float angle){
		selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.up,angle);
	}
	public void RotateObjRelateToCameraY(float angle){
		selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.right,angle);
	}
	public void RotateObjRelateToCameraZ(float angle){
		selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.forward,
			angle);
	}

	public void BackToMain(){
		SceneManager.LoadScene ("Main");
	}
}

using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class eventcontroller2 : MonoBehaviour {

	public GameObject marker;
	public GameObject[] annoPrefab;
	private int annotationtype;
	private string state;    

	private annoScript2 annoScrip;
	private Ray ray;
	private Vector3 grav;
	private GameObject selectedGameobject;

	private bool[] input;
	private float[] targetPos;
	private float[] targetOren;
	private float[] targetScale;

	// Use this for initialization
	void Start () {		
		state = "NONE";
		input = new bool[10];

		TargetSetUp ();

	}
	private void TargetSetUp(){
		targetPos = new float[3];
		targetOren = new float[3];
		targetScale = new float[3];

		targetPos [0] = 0.150f;
		targetPos [1] = -0.020f;
		targetPos [2] = -0.132f;

		targetOren [0] = 0f;
		targetOren [1] = 0f;
		targetOren [2] = 0f;

		targetScale [0] = 0.472f;
		targetScale [1] = 0.472f;
		targetScale [2] = 0.472f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touches.Length >0) {
			Touch touch = Input.GetTouch(0);

			if(!EventSystem.current.IsPointerOverGameObject (touch.fingerId))
			{
				if(touch.phase == TouchPhase.Began) 
				{

					ray = Camera.main.ScreenPointToRay (touch.position);
					Vector3 rayEnd = ray.GetPoint (4);

					if(state.Equals("NONE") || state.Equals ("EDIT"))
					{
						
						foreach(RaycastHit hit  in Physics.RaycastAll(ray) ) 
						{

							if(hit.collider.tag.Equals ("annotation"))
							{
								print (hit.transform.name);

								if (selectedGameobject != null)
								{
									SelectedAnnotationMat(false);
								}

								selectedGameobject = hit.transform.gameObject;
								SelectedAnnotationMat(true);
								PrepareData ();
								break;
							}
						}
					}
					if (state.Equals("ADDANNO"))
					{

						GameObject selectPrefab = null;						                         
						switch (annotationtype)
						{
						case 1:
							selectPrefab = annoPrefab[0];
							break;
						case 2:
							selectPrefab = annoPrefab[1];
							break;
						case 3:
							selectPrefab = annoPrefab[2];
							break;
						case 4:
							selectPrefab = annoPrefab[3];
							break;
						case 5:
							selectPrefab = annoPrefab[4];
							break;
						}
						print (selectPrefab);
						if (selectPrefab != null){
							CreateAnnotation(selectPrefab,rayEnd,annotationtype);
							annotationtype = 99;
						}


					}
				}
			}

			if (touch.phase == TouchPhase.Stationary) {
				
				if (state.Equals ("EDIT")) {

					if (input [0]) {
						SetOrientationX(5);
					}
					else if (input [1]) {
						SetOrientationX(-5);
					}
					if (input [2]) {
						SetOrientationY(5);
					}
					else if (input [3]) {
						SetOrientationY(-5);
					}
					if (input [4]) {
						SetOrientationZ(5);
					}
					else if (input [5]) {
						SetOrientationZ(-5);
					}
					if (input [6]) {
						ScaleButton (0.05f);
					}
					else if (input [7]) {
						ScaleButton (-0.05f);
					}
					if (input [8]) {
						PositionButton (0.025f);
					}
					else if (input [9]) {
						PositionButton(-0.025f);
					}
				}
			}


		}

		if (selectedGameobject != null) {
			CheckCorrectness ();
		}
	}

	private void CheckCorrectness(){

		float posX = selectedGameobject.transform.position.x;
		float posY = selectedGameobject.transform.position.y;
		float posZ = selectedGameobject.transform.position.z;

		float rotX = selectedGameobject.transform.eulerAngles.x;
		float rotY = selectedGameobject.transform.eulerAngles.y;
		float rotZ = selectedGameobject.transform.eulerAngles.z;

		float scale = selectedGameobject.transform.localScale.x;
		//print (posX + posY + posZ);


		if(posX <= targetPos[0]+0.1f && posX >= targetPos[0]-0.1f &&
			posY <= targetPos[1]+0.1f && posY >= targetPos[1]-0.1f &&
			posZ <= targetPos[2]+0.1f && posZ >= targetPos[2]-0.1f &&
			rotX <= targetOren[0] + 4.5f && rotX >= targetOren[0] - 4.5f &&
			rotY <= targetOren[1] + 4.5f && rotY >= targetOren[1] - 4.5f &&
			rotZ <= targetOren[2] + 4.5f && rotZ>= targetOren[2] - 4.5f &&
			scale <= targetScale[0] + 0.2f && scale>= targetScale[0] - 0.2f 
		
		){
			print("correct");
		}
	}

	public void OnHoldButton(int index){
		input [index] = true;
	}
	public void OnReleaseButton(int index){
		input [index] = false;
	}
	public void ChangeState(string st){
		state = st;
		print (state);
	}
	public void SeclectAnnoType(int i){
		annotationtype = i;
	}

	private void CreateAnnotation(GameObject annoPrefab,Vector3 objePos,int annotype){

		GameObject newAnnotation = Instantiate (annoPrefab,objePos,transform.rotation) as GameObject;
		newAnnotation.transform.parent = marker.transform;
		annoScrip = (annoScript2)newAnnotation.GetComponent (typeof(annoScript2));
		annoScrip.SetInitCam(Camera.main.transform.position);
		annoScrip.SetAnnoType (annotype);

	}

	private Vector3 camPos;
	private Vector3 objPos;

	public void PrepareData(){
		annoScript2 tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		camPos = tmp.GetInitCam ();
	}
	private void SelectedAnnotationMat(bool seleted)
	{
		annoScript2 tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		if (seleted)
		{
			tmp.SelectedAnnotation();
		}
		else
		{
			tmp.DeselectedAnnotatin();
		}
	}
	public void RemoveSelectedAnno(){

		if (selectedGameobject != null) {
			Destroy (selectedGameobject);
			selectedGameobject = null;

		}

	}
	public void PositionButton(float input){
		/*
		annoScript2 tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		Vector3 camPos = tmp.GetInitCam ();
		*/
		objPos = selectedGameobject.transform.position;

		Vector3 V1 = objPos - camPos;
		float d = 1f + input;

		selectedGameobject.transform.position = camPos + (d * V1);


	}
	public void ScaleButton(float d){
		float tmpscale = selectedGameobject.transform.localScale.x+d;
		Vector3 newScale = new Vector3 (tmpscale,tmpscale,tmpscale);
		if (newScale.x <= 0 || newScale.y <= 0 || newScale.z <= 0) {
			newScale.x = 0.1f;
			newScale.y = 0.1f;
			newScale.z = 0.1f;
		}
		selectedGameobject.transform.localScale = newScale;
	}
	public void SetOrientationY(float deg){

		annoScript2 tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		tmp.SetOrientation(deg,"Y");
	}
	public void SetOrientationZ(float deg){

		annoScript2 tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		tmp.SetOrientation(deg,"Z");
	}
	public void SetOrientationX(float deg){

		annoScript2 tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		tmp.SetOrientation(deg,"X");
	}
}

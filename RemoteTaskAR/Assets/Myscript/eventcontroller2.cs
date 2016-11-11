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
	private UserStudy USScrip;

	public bool IsUserStudy;
	public Text debugText;


	private bool[] input;

	/*
	 * SlidAR
	 * */
	float scHeight;
	float scWidth;
	private bool slidARMode;

	/*
	 * Scale mode
	 * */

	private bool scaleMode;

	// Use this for initialization
	void Start () {		

		scHeight = Screen.height;
		scWidth = Screen.width;
		slidARMode = false;

		state = "NONE";
		input = new bool[10];

		lr = gameObject.AddComponent<LineRenderer>();
		lr.enabled = false;

		USScrip = (UserStudy)gameObject.GetComponent (typeof(UserStudy));
		if (IsUserStudy) {
			USScrip.StartUserStudy ();
		}
	}
	private int counts = 0;
	// Update is called once per frame
	void Update () {
		if (Input.touches.Length >0) {
			Touch touch = Input.GetTouch(0);

			if (!EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {
				if (touch.phase == TouchPhase.Began) {

					ray = Camera.main.ScreenPointToRay (touch.position);
					Vector3 rayEnd = ray.GetPoint (28);

					if (state.Equals ("NONE") || state.Equals ("EDIT")) {
						
						foreach (RaycastHit hit  in Physics.RaycastAll(ray)) {

							if (hit.collider.tag.Equals ("annotation")) {
								print (hit.transform.name);

								if (selectedGameobject != null) {
									SelectedAnnotationMat (false);
								}

								selectedGameobject = hit.transform.gameObject;
								PrepareData ();
								SelectedAnnotationMat (true);

								break;
							}
						}
					}
					if (state.Equals ("ADDANNO")) {

						GameObject selectPrefab = null;						                         
						switch (annotationtype) {
						case 1:
							selectPrefab = annoPrefab [0];
							break;
						case 2:
							selectPrefab = annoPrefab [1];
							break;
						case 3:
							selectPrefab = annoPrefab [2];
							break;
						case 4:
							selectPrefab = annoPrefab [3];
							break;
						case 5:
							selectPrefab = annoPrefab [4];
							break;
						}
						print (selectPrefab);
						if (selectPrefab != null) {
							CreateAnnotation (selectPrefab, rayEnd, annotationtype);
							annotationtype = 99;
						}


					}
				}
			} else if (touch.phase == TouchPhase.Stationary) {
				
				if (state.Equals ("EDIT")) {

					if (input [0]) {
						SetOrientationX (1);
					} else if (input [1]) {
						SetOrientationX (-1);
					}
					if (input [2]) {
						SetOrientationY (1);
					} else if (input [3]) {
						SetOrientationY (-1);
					}
					if (input [4]) {
						SetOrientationZ (1);
					} else if (input [5]) {
						SetOrientationZ (-1);
					}
					if (input [6]) {
						ScaleButton (0.025f);
					} else if (input [7]) {
						ScaleButton (-0.025f);
					}
					/*
					if (input [8]) {
						PositionButton (0.025f);
					} else if (input [9]) {
						PositionButton (-0.025f);
					}
					*/
				}
			}
			if (touch.phase == TouchPhase.Moved) {
				if (state.Equals ("EDIT")) {
					if (selectedGameobject != null) {
						if (slidARMode) {
							
							MoveSlidAR (touch.position);

						} else if (scaleMode) {
							
							if (Input.touchCount >= 2) {
								
								Vector2 t1PrevPos = touch.position - touch.deltaPosition;
								Vector2 t2PrevPos = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;

								float prevMagnitude = (t1PrevPos - t2PrevPos).magnitude;
								float cMagnitude = (touch.position - Input.GetTouch(1).position).magnitude;

								float diffMagnitude = (prevMagnitude - cMagnitude)*0.01f;

								ScaleButton (-diffMagnitude);
							}

						}
					}


				}
			}


		}
		if (state.Equals ("EDIT")) {

			if (selectedGameobject != null) {
				//PrepareData();
				if (slidARMode)
				{
					counts++;
					//if (counts == 0) {
						DrawSlidAR();
					//	counts = 0;
					//}

				}
			}
		} 
		if (selectedGameobject != null) {
			
			if (IsUserStudy) {
				CallUserStudyScript();
			}

		}
	}
	public void ShowDebugText(){
		debugText.text = "Position:  X: "+selectedGameobject.transform.position.x+ " Y: "
			+ selectedGameobject.transform.position.y+ " Z: "
			+ selectedGameobject.transform.position.z+ ""
			+ "\nRotation: X "
			+ selectedGameobject.transform.eulerAngles.x+ " Y: "
			+ selectedGameobject.transform.eulerAngles.y+ " Z: "
			+ selectedGameobject.transform.eulerAngles.z+ ""
			+ "\nScale: " + selectedGameobject.transform.localScale.x+"\n";
	}
	private void CallUserStudyScript(){
		
		USScrip.CheckCorrectness (selectedGameobject);


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
	private annoScript2 tmp;

	public void PrepareData(){
		tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		camPos = tmp.GetInitCam ();
	}
	private void SelectedAnnotationMat(bool seleted)
	{
		//tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		if (seleted)
		{
			tmp.SelectedAnnotation();
		}
		else
		{
			tmp.DeselectedAnnotatin();
		}
	}
	public void DoneButton(){
		if (selectedGameobject != null) {
			SelectedAnnotationMat (false);
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
		/*
		float tmpscale = selectedGameobject.transform.localScale.x+d;

		Vector3 newScale = new Vector3 (tmpscale,tmpscale,tmpscale);
		*/
		Vector3 newScale = new Vector3 (d,d,d);
		/*
		if (newScale.x <= 0 || newScale.y <= 0 || newScale.z <= 0) {
			newScale.x = 0.1f;
			newScale.y = 0.1f;
			newScale.z = 0.1f;
		}
		*/
		selectedGameobject.transform.localScale += newScale;
	}


	public void SetOrientationY(float deg){

		//tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		tmp.SetOrientation(deg,"Y");
	}
	public void SetOrientationZ(float deg){

		//tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		tmp.SetOrientation(deg,"Z");
	}
	public void SetOrientationX(float deg){

		//tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		tmp.SetOrientation(deg,"X");
	}

	public void TriggerScale(bool bo){
		if (slidARMode) {
			TriggerSlidAR (false);
		} 
		scaleMode = bo;
	}

	/****
	 * 
	 * Slid AR
	 * */

	private Vector3 tmpCamPos ;
	private Vector3 tmpAnnoPos;
	public Material lineMat;
	private LineRenderer lr;

	private Vector3 tmpcam2 ;
	private Vector3 tmpAnno2 ;

	public void TriggerSlidAR(bool bo){
		if (scaleMode) {
			TriggerScale (false);
		}
		if (bo) {
			PrepareSlidARData ();
		} else {
			destroyLine ();
		}
		slidARMode = bo;
	}

	public void PrepareSlidARData(){
		//print ("prepare");
		tmp = (annoScript2)selectedGameobject.GetComponent (typeof(annoScript2));
		tmpCamPos = tmp.GetInitCam ();
		tmpAnnoPos = tmp.GetPos ();

	}

	private void MoveSlidAR(Vector3 pos){
		/*
		annoScrip = (AnnotationScript)selectedGameobject.GetComponent (typeof(AnnotationScript));
		Vector3 camPos = annoScrip.GetInitCamPos ();
		Vector3 objPos = annoScrip.GetAnnoPos ();
		*/

		Vector3 intCamToSc = Camera.main.WorldToScreenPoint (tmpCamPos);
		Vector3 objToSc = Camera.main.WorldToScreenPoint (tmpAnnoPos);

		float m = (intCamToSc.y-objToSc.y)/(intCamToSc.x-objToSc.x);
		float c = intCamToSc.y - (m*intCamToSc.x);

		float lx = pos.x + (m*pos.x-pos.y+c)/(m*m+1)*m;
		float ly = pos.y + (m*pos.x-pos.y+c)/(m*m+1); 
		//print (pos);
		//print (lx+" : "+ly);

		Vector3 touchPos = new Vector3 (lx,ly,pos.z+1f);

		//Vector3 newPos = new Vector3 (pos.x,(m*pos.x)+c ,1);
		touchPos = Camera.main.ScreenToWorldPoint (touchPos);



		Vector3 cCamPos = Camera.main.transform.position;
		//Ray ray2 = Camera.main.ScreenPointToRay (touchPos);

		Vector3 V1 = tmpAnnoPos - tmpCamPos;
		Vector3 V2 =  touchPos - cCamPos ;

		float[][] input = new float[3][];
		input[0] = new float[3] { -V1.x, V2.x, tmpCamPos.x - cCamPos.x };
		input[1] = new float[3] { -V1.y, V2.y, tmpCamPos.y - cCamPos.y };
		input[2] = new float[3] { -V1.z, V2.z, tmpCamPos.z - cCamPos.z };

		//		print (input[0][0]+":"+input[0][1]+":"+input[0][2]);
		//		print (input[1][0]+":"+input[1][1]+":"+input[1][2]);
		//		print (input[2][0]+":"+input[2][1]+":"+input[2][2]);

		float[] result = guassianElim(input);
		float d = result[0];
		//float t = result[1];

		//print (d+":"+t);
		//print(tmpCamPos + " : " + d + " : " + rayInit);
		//print(tmpCamPos + (d*rayInit));
		selectedGameobject.transform.position = tmpCamPos + (d * V1);


	}
	private void destroyLine(){
		if (lr.enabled) {
			lr.enabled = false;
		}
	}
	private void DrawSlidAR(){
		lr.enabled = true;
		int count = 2;
		bool isCamOnSc = false;
		bool isAnnoOnSc = false;
		/*
		annoScrip = (AnnotationScript)selectedGameobject.GetComponent (typeof(AnnotationScript));
		Vector3 tmpCamPos = annoScrip.GetInitCamPos ();
		Vector3 tmpAnnoPos = annoScrip.GetAnnoPos ();
		*/

		lr.material = lineMat;
		lr.SetColors (Color.red,Color.red);
		lr.SetWidth (0.005f,0.005f);


		tmpcam2 = Camera.main.WorldToScreenPoint (tmpCamPos);
		tmpAnno2 = Camera.main.WorldToScreenPoint (tmpAnnoPos);

		Vector2 camOnScr = new Vector2 (tmpcam2.x,tmpcam2.y); 
		Vector2 annoOnScr = new Vector2 (tmpAnno2.x,tmpAnno2.y); 
		Vector2 vCamToAnno = annoOnScr - camOnScr;

		if(camOnScr.x >=0 || camOnScr.x < scWidth)
		{
			if(camOnScr.y >=0 || camOnScr.y < scHeight)
			{
				//print ("1");
				isCamOnSc = true;
				count++;
			}
		}
		if(annoOnScr.x>=0||annoOnScr.x < scWidth)
		{
			if(annoOnScr.y >=0 || annoOnScr.y < scHeight)
			{
				//print ("2");
				isAnnoOnSc = true;
				count++;
			}
		}

		lr.SetVertexCount (count);	

		if(!isCamOnSc)
		{
			lr.SetPosition (0, Camera.main.ScreenToWorldPoint(new Vector3(camOnScr.x,camOnScr.y,1)));
			lr.SetPosition (1, Camera.main.ScreenToWorldPoint(new Vector3(annoOnScr.x,annoOnScr.y,1)));
		}
		else
		{
			Vector2 fpoint = camOnScr - (2*vCamToAnno);
			lr.SetPosition (0, Camera.main.ScreenToWorldPoint(new Vector3(fpoint.x,fpoint.y,1)));
			lr.SetPosition (1, Camera.main.ScreenToWorldPoint(new Vector3(camOnScr.x,camOnScr.y,1)));
			lr.SetPosition (2, Camera.main.ScreenToWorldPoint(new Vector3(annoOnScr.x,annoOnScr.y,1)));
		}
		if(isAnnoOnSc)
		{
			Vector2 lpoint = camOnScr + (2*vCamToAnno);
			lr.SetPosition (count-1, Camera.main.ScreenToWorldPoint(new Vector3(lpoint.x,lpoint.y,1)));
		}

	}
	public float[] guassianElim(float[][] rows)
	{
		int length = rows[0].Length;

		for (int i = 0; i < rows.Length - 1; i++)
		{
			if (rows[i][i] == 0 && !Swap(rows, i, i))
			{
				return null;
			}

			for (int j = i; j < rows.Length; j++)
			{
				float[] d = new float[length];
				for (int x = 0; x < length; x++)
				{
					d[x] = rows[j][x];
					if (rows[j][i] != 0)
					{
						d[x] = d[x] / rows[j][i];
					}
				}
				rows[j] = d;
			}

			for (int y = i + 1; y < rows.Length; y++)
			{
				float[] f = new float[length];
				for (int g = 0; g < length; g++)
				{
					f[g] = rows[y][g];
					if (rows[y][i] != 0)
					{
						f[g] = f[g] - rows[i][g];
					}

				}
				rows[y] = f;
			}
		}

		return CalculateResult(rows);
	}

	private bool Swap(float[][] rows, int row, int column)
	{
		bool swapped = false;
		for (int z = rows.Length - 1; z > row; z--)
		{
			if (rows[z][row] != 0)
			{
				float[] temp = new float[rows[0].Length];
				temp = rows[z];
				rows[z] = rows[column];
				rows[column] = temp;
				swapped = true;
			}
		}

		return swapped;
	}

	private float[] CalculateResult(float[][] rows)
	{
		float val = 0;
		int length = rows[0].Length;
		float[] result = new float[rows.Length];
		for (int i = rows.Length - 1; i >= 0; i--)
		{
			val = rows[i][length - 1];
			for (int x = length - 2; x > i - 1; x--)
			{
				val -= rows[i][x] * result[x];
			}
			result[i] = val / rows[i][i];

			if (!IsValidResult(result[i]))
			{
				return null;
			}
		}
		return result;
	}

	private bool IsValidResult(double result)
	{
		return result.ToString() != "NaN" || !result.ToString().Contains("Infinity");
	}
}

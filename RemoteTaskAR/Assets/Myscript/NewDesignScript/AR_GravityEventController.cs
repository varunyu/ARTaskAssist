using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AR_GravityEventController : MonoBehaviour {
	
	private GameObject selectedObject;
	private Vector3 rayEnd;
	private Ray ray;
	private Ray rayTouch;

	float scHeight;
	float scWidth;

	/*
	 * state
	 * 
	 *1.NONE
	 *2.ADDANNO
	 *3.ADJUST
	 *4.EDIT
	 * */
	private string state;   


	private float rotaSpeed = 20.0f;

	private bool slidARMode;
	private bool rotateMode;


	private AnnotationControl AC;
	private AR_GravityUIController UiC;
	private UserStudy uS;


	void Awake(){
		AC = (AnnotationControl)transform.parent.gameObject.GetComponent(typeof(AnnotationControl));
		AC.SetGravityMode(true);
		UiC = (AR_GravityUIController)gameObject.GetComponent(typeof(AR_GravityUIController));

		uS = (UserStudy)transform.parent.gameObject.GetComponent(typeof(UserStudy));

		lr = gameObject.AddComponent<LineRenderer>();
		lr.enabled = false;
	}

	// Use this for initialization
	void Start () {
		
		scHeight = Screen.height;
		scWidth = Screen.width;

		slidARMode = false;
		rotateMode = false;
		state = "NONE";
	}
	private Vector2 tmpScPos;
	// Update is called once per frame
	void Update () {
	
		if (Input.touches.Length > 0) 
		{
			Touch touch = Input.GetTouch(0);

			if (!EventSystem.current.IsPointerOverGameObject (touch.fingerId)) 
			{
				if (touch.phase == TouchPhase.Began) {

					tmpScPos = touch.position;
					tmpScPos.y += 100;

					rayTouch = Camera.main.ScreenPointToRay (touch.position);
					ray = Camera.main.ScreenPointToRay (tmpScPos);
					rayEnd = ray.GetPoint (90);

					if (state.Equals ("ADDANNO")) {

						CreateAnno (rayEnd);


					} else if (state.Equals ("NONE") || state.Equals ("EDIT")) {
						if (!slidARMode && !rotateMode) {
							
							foreach (RaycastHit hit  in Physics.RaycastAll(rayTouch)) {

								if (hit.collider.tag.Equals ("annotation")) {
									
									if (selectedObject != null) {
										AC.DeselectedAnnotation ();
										selectedObject = null;
									}

									selectedObject = hit.transform.gameObject;
									AC.SetSelectedAnnotation (selectedObject);

									break;
								}

							}
						}
					}


				} else if (touch.phase == TouchPhase.Moved) {
					if (state.Equals ("ADJUST")) {

						tmpScPos = touch.position;
						tmpScPos.y += 100;


						ray = Camera.main.ScreenPointToRay (tmpScPos);
						rayEnd = ray.GetPoint (90);

						selectedObject.transform.position = rayEnd;
					} else if (state.Equals ("EDIT")) {
						if (selectedObject != null) {
							if (slidARMode) {
								MoveSlidAR (touch.position);
							}
							if (rotateMode) {
								if (Input.touchCount == 1) {
									if (Mathf.Abs (touch.deltaPosition.x) > Mathf.Abs (touch.deltaPosition.y)) {
										SetObjectOrientation (touch.deltaPosition.x * rotaSpeed * Time.deltaTime, "X");
									} else {
										
										selectedObject.transform.RotateAround (selectedObject.transform.position, Camera.main.transform.right,
											touch.deltaPosition.y * rotaSpeed * Time.deltaTime);

									}
								} else if (Input.touchCount >= 2) {
									Vector2 t1PrevPos = touch.position - touch.deltaPosition;
									Vector2 t2PrevPos = Input.GetTouch (1).position - Input.GetTouch (1).deltaPosition;
									/*
									float prevMagnitude = (t1PrevPos - t2PrevPos).magnitude;
									float cMagnitude = (touch.position - Input.GetTouch(1).position).magnitude;
									*/
									Vector2 prevDir = t2PrevPos - t1PrevPos;
									Vector2 currentDir = Input.GetTouch (1).position - touch.position;
									float angle = Vector2.Angle (prevDir, currentDir);
									Vector3 LR = Vector3.Cross (prevDir, currentDir);

									if (LR.z > 0) {
										SetObjectOrientation (angle * 2f, "Z");
									} else {
										SetObjectOrientation (-angle * 2f, "Z");
									}
								}
								uS.SetRotateMode (true);
							}

						}
					}


				} 
				if (touch.phase == TouchPhase.Ended) {
					if (rotateMode) {
						uS.SetRotateMode (false);
					}
				}


			}
		}

		//

		if (slidARMode) {
			DrawSlidAR ();
		}

	}
	public void RemoveAnnotation(){
		AC.DeselectedAnnotation ();
		if (selectedObject != null) {
			Destroy (selectedObject);
			selectedObject = null;
		}
	}

	public void ChangeState(string t){
		state = t;
	}
	public void TriggerSildARMode(){
		if (rotateMode) {
			rotateMode = false;
		}

		if (slidARMode) {
			slidARMode = false;
			TriggerSlidAR (false);
		} else {
			slidARMode = true;
			TriggerSlidAR (true);
		}

	}
	public void TriggerRotateMode(){
		if (slidARMode) {
			slidARMode = false;
			TriggerSlidAR (false);
		}

		if (rotateMode) {
			rotateMode = false;
		} else {
			rotateMode = true;;
		}
	}
	public void FinishButton(){
		AC.DeselectedAnnotation ();
		selectedObject = null;

		if (slidARMode) {
			TriggerSlidAR (false);
		}
		slidARMode = false;
		rotateMode = false;

	}
	public void SetObjectOrientation(float input,string axis)
	{
		if (selectedObject != null) {
			switch (axis) {
			case "X":
				AC.RotateObjRelateToGX (input);
				break;
			case "Y":
				AC.RotateObjRelateToCameraY (input);
				break;
			case "Z":
				AC.RotateObjRelateToCameraZ (input);
				break;
			
			}
			//annoScrip = (AnnotationScript)selectedGameobject.GetComponent (typeof(AnnotationScript));
			//tmp.SetOrientation(input,axis);
		}
	}    

	private void CreateAnno(Vector3 pos){


		AC.ObjectInstantiate (pos);

		selectedObject = AC.GetSelectedAnnotation ();

		ChangeState ("ADJUST");
		UiC.SubAddPannelControl (3);


	}


	private Vector3 camPos;
	private Vector3 objPos;

	public void PrepareData(){
		/*
		tmp = (annoScript2)selectedObject.GetComponent(typeof(annoScript2));
		camPos = tmp.GetInitCam ();
		*/
	}


	private Vector3 tmpCamPos ;
	private Vector3 tmpAnnoPos;
	public Material lineMat;
	private LineRenderer lr;

	private Vector3 tmpcam2 ;
	private Vector3 tmpAnno2 ;

	public void TriggerSlidAR(bool bo){
		
		if (bo) {
			PrepareSlidARData ();

		} else {
			
			destroyLine ();
		}
		slidARMode = bo;
	}

	public void PrepareSlidARData(){
		print ("prepare");
		/*
		if(tmp!= null){
			tmp = (annoScript2)selectedObject.GetComponent (typeof(annoScript2));
			tmpCamPos = tmp.GetInitCam ();
			tmpAnnoPos = tmp.GetPos ();
		}
		*/
		tmpCamPos = AC.GetObjInitCamPos ();
		tmpAnnoPos = AC.GetObjInitPos ();

	}

	private void MoveSlidAR(Vector3 pos){
		
		Vector3 intCamToSc = Camera.main.WorldToScreenPoint (tmpCamPos);
		Vector3 objToSc = Camera.main.WorldToScreenPoint (tmpAnnoPos);

		float m = (intCamToSc.y-objToSc.y)/(intCamToSc.x-objToSc.x);
		float c = intCamToSc.y - (m*intCamToSc.x);

		float lx = pos.x + (m*pos.x-pos.y+c)/(m*m+1)*m;
		float ly = pos.y + (m*pos.x-pos.y+c)/(m*m+1); 


		Vector3 touchPos = new Vector3 (lx,ly,pos.z+1f);

		touchPos = Camera.main.ScreenToWorldPoint (touchPos);



		Vector3 cCamPos = Camera.main.transform.position;

		Vector3 V1 = tmpAnnoPos - tmpCamPos;
		Vector3 V2 =  touchPos - cCamPos ;

		float[][] input = new float[3][];
		input[0] = new float[3] { -V1.x, V2.x, tmpCamPos.x - cCamPos.x };
		input[1] = new float[3] { -V1.y, V2.y, tmpCamPos.y - cCamPos.y };
		input[2] = new float[3] { -V1.z, V2.z, tmpCamPos.z - cCamPos.z };

		float[] result = guassianElim(input);
		float d = result[0];

		selectedObject.transform.position = tmpCamPos + (d * V1);


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
			lr.SetPosition (0, Camera.main.ScreenToWorldPoint(new Vector3(camOnScr.x,camOnScr.y,1f)));
			lr.SetPosition (1, Camera.main.ScreenToWorldPoint(new Vector3(annoOnScr.x,annoOnScr.y,1f)));
		}
		else
		{
			Vector2 fpoint = camOnScr - (2*vCamToAnno);
			lr.SetPosition (0, Camera.main.ScreenToWorldPoint(new Vector3(fpoint.x,fpoint.y,1f)));
			lr.SetPosition (1, Camera.main.ScreenToWorldPoint(new Vector3(camOnScr.x,camOnScr.y,1f)));
			lr.SetPosition (2, Camera.main.ScreenToWorldPoint(new Vector3(annoOnScr.x,annoOnScr.y,1f)));
		}
		if(isAnnoOnSc)
		{
			Vector2 lpoint = camOnScr + (2*vCamToAnno);
			lr.SetPosition (count-1, Camera.main.ScreenToWorldPoint(new Vector3(lpoint.x,lpoint.y,1f)));
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

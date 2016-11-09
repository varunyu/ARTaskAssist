using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventController : MonoBehaviour {

	public GameObject marker;
	public GameObject annoPrefab;
	public GameObject[] arrowPrefab = new GameObject[2] ;
	public GameObject[] halfCPrefab = new GameObject[2];
	public GameObject pArrowPrefab;
	public GameObject[] cArrowPrefab = new GameObject[4];

    private AnnotationScript annoScrip;
    private UIController UIs;

	public GameObject circlePrefab;

	private string state;    

    private int annotationtype;
    /*
        case 1 = arrow up
        case 2 = arrow down
        case 3 = arrow forward

        case 11 = curve arrow up  
        case 12 = curve arrow left      
        case 13 = curve arrow down        
        case 14 = curve arrow right

        ** perpendicular to gravity
        case 21 = Half circle left
        case 22 = Half circle right

        ** parallel to gravity
        case 31 = Half circle left
        case 32 = Half circle right
    */


    private bool annoOption;

    public GameObject addAnnoPanel;
    public GameObject editPanel;
   
	public Text modeText;

    private bool slidARMode;
    private bool rotateMode;
    
	public Material lineMat;
	private LineRenderer lr;

	private GameObject selectedGameobject;

	private Ray ray;
	private Vector3 grav;

	private float rotaSpeed = 20.0f;

	private Vector3 tmpcam2 ;
	private Vector3 tmpAnno2 ;

	float scHeight;
	float scWidth;

	private Vector3 tmpCamPos ;
	private Vector3 tmpAnnoPos;



	private UserStudy USScrip;
	public bool IsUserStudy;
	public Text debugText;

	// Use this for initialization
	void Start () {

        UIs = (UIController)gameObject.GetComponent(typeof(UIController));

		scHeight = Screen.height;
		scWidth = Screen.width;

        slidARMode = false;
        rotateMode = false;
		state = "NONE";
        annoOption = false;
		lr = gameObject.AddComponent<LineRenderer>();
		lr.enabled = false;

		USScrip = (UserStudy)gameObject.GetComponent (typeof(UserStudy));
		if (IsUserStudy) {			
			USScrip.StartUserStudy ();
		}
	}
	/*
	float  oldAngle ;
	float  newAngle ;*/
	int counts =0;
	// Update is called once per frame
	void Update () {

		if (Input.touches.Length >0) 
		
		{
			Touch touch = Input.GetTouch(0);

			if(!EventSystem.current.IsPointerOverGameObject (touch.fingerId))
			{
				if(touch.phase == TouchPhase.Began) 
				{

					ray = Camera.main.ScreenPointToRay (touch.position);
					Vector3 rayEnd = ray.GetPoint (4);

					if(state.Equals("NONE") || state.Equals ("EDIT"))
					{
						if (!slidARMode && !rotateMode) {
							foreach(RaycastHit hit  in Physics.RaycastAll(ray) ) 
							{

								if(hit.collider.tag.Equals ("annotation"))
								{
									//print (hit.transform.name);

									if (selectedGameobject != null)
									{
										SelectedAnnotationMat(false);
									}

									selectedGameobject = hit.transform.gameObject;
									SelectedAnnotationMat(true);

									break;
								}

							}
						}

					}
                    if (state.Equals("ADDANNO"))
                    {
						
						GameObject selectPrefab = null;
                        
                        if (annotationtype > 0 && annotationtype < 10)
                        {                            
                            switch (annotationtype)
                            {
                                case 1:
                                    selectPrefab = arrowPrefab[0];
                                    annoOption = false;
                                    break;
                                case 2:
                                    selectPrefab = arrowPrefab[1];
                                    annoOption = false;
                                    break;
                                case 3:
                                    selectPrefab = arrowPrefab[1];
                                    annoOption = true;
                                    break;
                            }


                        }
                        else if (annotationtype > 10 && annotationtype < 20)
                        {                            
                            annoOption = true;
                            switch (annotationtype)
                            {
                                case 11:
                                    selectPrefab = cArrowPrefab[0];
                                    break;
                                case 12:
                                    selectPrefab = cArrowPrefab[1];
                                    break;
                                case 13:
                                    selectPrefab = cArrowPrefab[2];
                                    break;
                                case 14:
                                    selectPrefab = cArrowPrefab[3];
                                    break;
                            }
                        }
                        else if (annotationtype > 20)
                        {
                            switch (annotationtype)
                            {
                                case 21:
                                    selectPrefab = halfCPrefab[1];
                                    annoOption = true;
                                    break;
                                case 22:
                                    selectPrefab = halfCPrefab[0];
                                    annoOption = true;
                                    break;
                                case 31:
                                    selectPrefab = halfCPrefab[1];
                                    annoOption = false;
                                    break;
                                case 32:
                                    selectPrefab = halfCPrefab[0];
                                    annoOption = false;
                                    break;
                            }
                            
                        }
                        if (selectPrefab != null){
							CreateAnnotation(selectPrefab,rayEnd, annoOption);
						}

						
                    }
					if(state.Equals("EDIT"))
					{
						/*
						if(rotateMode)
						{
							if(Input.touchCount >=2)
							{
								//print ("Touch Count = "+Input.touchCount);
								Vector2 cVector = touch.position - Input.GetTouch(1).position;
								oldAngle = Mathf.Atan2(cVector.y,cVector.x);
							}
						}*/
					}

                    
				}
				else if(touch.phase == TouchPhase.Ended)
				{

				}
				else if(touch.phase == TouchPhase.Moved)
				{
					if (state.Equals ("EDIT")) 
					{

						if (selectedGameobject != null) 
						{
							if(slidARMode)
							{
								MoveSlidAR (touch.position);
							}
							if(rotateMode)
							{
								if(Input.touchCount == 1)
								{
									//print (touch.deltaPosition);
									if(Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
									{

										SetObjectOrientation(touch.deltaPosition.x*rotaSpeed*Time.deltaTime,"Y");
									}
									else
									{
										//SetObjectOrientation(touch.deltaPosition.y*rotaSpeed*Time.deltaTime,"X");
									}
								}
								else if(Input.touchCount >=2)
								{
									Vector2 t1PrevPos = touch.position - touch.deltaPosition;
									Vector2 t2PrevPos = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;

									float prevMagnitude = (t1PrevPos - t2PrevPos).magnitude;
									float cMagnitude = (touch.position - Input.GetTouch(1).position).magnitude;

									float diffMagnitude = (prevMagnitude - cMagnitude)*0.01f;

									//print (-diffMagnitude);
									SetObjectScale(-diffMagnitude);
								}
							}
							
						}	

					}
				}
			}


		}


/*
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)){
			

			if(!EventSystem.current.IsPointerOverGameObject ()){  

				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				Vector3 rayEnd = ray.GetPoint (4);

				if(state.Equals("NONE")){

					foreach(RaycastHit hit  in Physics.RaycastAll(ray) ) {

						if(hit.collider.tag.Equals ("annotation"))
						{
							//print (hit.transform.name);
							selectedGameobject = hit.transform.gameObject;
							
							break;
						}

					}
				}
                
			}

		}
		if (!EventSystem.current.IsPointerOverGameObject ()) {  
			if (Input.GetMouseButton (0)) {
			
				if (state.Equals ("EDIT")) {
					if (selectedGameobject != null) {

						if(slidARMode)
						{
							MoveSlidAR (Input.mousePosition);
						}
						if(rotateMode)
						{

						}

					}				
				}
			}
		}
		
#endif
*/
		if (state.Equals ("EDIT")) {

			if (selectedGameobject != null) {
                //PrepareData();
                if (slidARMode)
                {
					counts++;
					if (counts == 5) {
						DrawSlidAR();
						counts = 0;
					}

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
			+ " \nScale: " + selectedGameobject.transform.localScale.x + "\n";
	}
	private void CallUserStudyScript(){
		
		USScrip.CheckCorrectness (selectedGameobject);
			

	}
    public void SlidARActive()
    {
        UIs.OnSlidARTouch();
        slidARMode = true;
        rotateMode = false;
    }
    public void RotateActive()
    {
		destroyLine ();
        UIs.OnRotateTouch();
        slidARMode = false;
        rotateMode = true;
    }
    public void OpenAddPanel()
    {
        UIs.AddAnnoActive();
        addAnnoPanel.SetActive(true);
        state = "ADDANNO";
    }    
    public void OpenEditPanel()
    {
        UIs.EditButtonActive();
        editPanel.SetActive(true);
        state = "EDIT";
    }
    public void DoneButton()
    {
        UIs.OnEditDone();
        UIs.OnAddAnnoDone();        
        slidARMode = false;
        rotateMode = false;
        destroyLine ();
        state = "NONE";
    } 
   
    public void SetAnno(int type)
    {
        annotationtype = type;
    }
	/*
    public void ChangeAnnoOption()
    {
        if (annoOption)
        {
            annoOption = false;
			modeText.text = "Vertical";
        }
        else
        {
            annoOption = true;
			modeText.text = "Parallel";
        }
    }
	*/
    private void SelectedAnnotationMat(bool seleted)
    {
        AnnotationScript tmp = (AnnotationScript)selectedGameobject.GetComponent(typeof(AnnotationScript));
        if (seleted)
        {
            tmp.SelectedAnnotation();
        }
        else
        {
            tmp.DeselectedAnnotatin();
        }
    }

    public void SetObjectOrientation(float input,string axis)
	{
		if (selectedGameobject != null) {
			annoScrip = (AnnotationScript)selectedGameobject.GetComponent (typeof(AnnotationScript));
			annoScrip.SetOrientation(input,axis);
		}
	}    
	public void SetObjectScale(float mag)
	{
		if (selectedGameobject != null) {
			annoScrip = (AnnotationScript)selectedGameobject.GetComponent (typeof(AnnotationScript));
			annoScrip.SetObjectScale(new Vector3(mag,mag,mag));
		}
	}

	private void CreateAnnotation(GameObject annoPrefab,Vector3 objePos,bool paraOption){

		GameObject newAnnotation = Instantiate (annoPrefab,objePos,transform.rotation) as GameObject;
		newAnnotation.transform.parent = marker.transform;
		annoScrip = (AnnotationScript)newAnnotation.GetComponent (typeof(AnnotationScript));
		annoScrip.SetState (paraOption);
		annoScrip.InitCamaraPosition (Camera.main.transform.position);
        annoScrip.SetAnnoType(annotationtype);
    }
	/*
	private void CreateCircle(Vector3 objePos,bool paraOption){
		GameObject newAnnotation = Instantiate (circlePrefab,objePos,transform.rotation) as GameObject;
		newAnnotation.transform.parent = marker.transform;
		annoScrip = (AnnotationScript)newAnnotation.GetComponent (typeof(AnnotationScript));
		annoScrip.SetState (paraOption);
		annoScrip.InitCamaraPosition (Camera.main.transform.position);
		//TmpCircleGameobject = newAnnotation;
	}	*/



	public void PrepareSlidARData(){
		print ("prepare");
		annoScrip = (AnnotationScript)selectedGameobject.GetComponent (typeof(AnnotationScript));
		tmpCamPos = annoScrip.GetInitCamPos ();
		tmpAnnoPos = annoScrip.GetAnnoPos ();

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


	public void RemoveSelectedAnno(){
		
		if (selectedGameobject != null) {
			Destroy (selectedGameobject);
			selectedGameobject = null;

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
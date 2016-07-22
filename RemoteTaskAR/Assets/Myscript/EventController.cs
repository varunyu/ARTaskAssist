using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventController : MonoBehaviour {

	public GameObject marker;
	public GameObject annoPrefab;
	private AnnotationScript annoScrip;
    private UIController UIs;

	public GameObject circlePrefab;

	private string state;
    private string annotype;
    private bool annoOption;

    public GameObject addAnnoPanel;
    public GameObject editPanel;
    /*
    public Button addButton;
    public Button buttonEdit;
    public Button buttonRemove;
    
    public Button testModelButton;
    public Button buttonCircle;
	public Button annoOptionButton;
    */
	public Text modeText;
	//public Button addDone;


    private bool slidARMode;
    private bool rotateMode;
    /*
    public Button rotateLeftButton;
    public Button rotateRightButton;
    public Button editDone;
	*/

	public Material lineMat;
	private LineRenderer lr;

	private GameObject selectedGameobject;
	private GameObject TmpCircleGameobject;

	private Ray ray;
	private Vector3 grav;

    

	Vector3 tmpcam2 ;
	Vector3 tmpAnno2 ;

	float t ;

	// Use this for initialization
	void Start () {

        UIs = (UIController)gameObject.GetComponent(typeof(UIController));

        slidARMode = false;
        rotateMode = false;
		state = "NONE";
        annoOption = false;
		lr = gameObject.AddComponent<LineRenderer>();
		lr.enabled = false;

		t = 0;
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_IOS
		foreach (Touch touch in Input.touches) {

			if(!EventSystem.current.IsPointerOverGameObject (touch.fingerId)){
				if(touch.phase == TouchPhase.Began) {

					ray = Camera.main.ScreenPointToRay (touch.position);
					Vector3 rayEnd = ray.GetPoint (4);

					if(state.Equals("NONE")){
						foreach(RaycastHit hit  in Physics.RaycastAll(ray) ) {
							
							if(hit.collider.tag.Equals ("annotation"))
							{
								print (hit.transform.name);
								selectedGameobject = hit.transform.gameObject;
								//PrepareData();
								break;
							}
							
						}
					}
                    if (state.Equals("ADDANNO"))
                    {
                        if (annotype.Equals("DEBUG"))
                        {
                            CreateAnno(rayEnd, annoOption);
                        }
                        if (annotype.Equals("CIRCLE"))
                        {
                            CreateCircle(rayEnd, annoOption);
                        }
                    }

                    /*
					else if(state.Equals("90DEG")){
						CreateAnno (rayEnd,false);
					RemoveSelectedAnno();
					CreateCircle(rayEnd,false);
					}
					else if(state.Equals("180DEG")){
						CreateAnno (rayEnd,true);
						RemoveSelectedAnno();
						CreateCircle(rayEnd,true);
					}
					else if(state.Equals("CIRCLE")){

						RemoveSelectedAnno();
						CreateCircle(rayEnd,false);

					}
                    */
				


				}
				else if(touch.phase == TouchPhase.Ended){

				}
				else if(touch.phase == TouchPhase.Moved){
					if (state.Equals ("EDIT")) {
						if (selectedGameobject != null) {
							MoveSlidAR (touch.position);
							
						}
						
						
					}
				}
			}


		}
#endif
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)){
			

			if(!EventSystem.current.IsPointerOverGameObject ()){  

				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				Vector3 rayEnd = ray.GetPoint (4);

				//RaycastHit hit;



				if(state.Equals("NONE")){

					foreach(RaycastHit hit  in Physics.RaycastAll(ray) ) {

						if(hit.collider.tag.Equals ("annotation"))
						{
							print (hit.transform.name);
							selectedGameobject = hit.transform.gameObject;
							
							break;
						}

					}
				}
                if (state.Equals("ADDANNO"))
                {
                    if (annotype.Equals("DEBUG"))
                    {
                        CreateAnno(rayEnd, annoOption);
                    }
                    if (annotype.Equals("CIRCLE"))
                    {
                        CreateCircle(rayEnd, annoOption);
                    }
                }
                /*
				else if(state.Equals("90DEG")){

					CreateAnno (rayEnd,false);
					print ("create anno 90 deg");

				}
				else if(state.Equals("180DEG")){

					CreateAnno (rayEnd,true);
					print ("create anno 180 deg");

				}
				else if(state.Equals("CIRCLE")){
					CreateCircle(rayEnd,true);

					print ("Circle");
				}

                */
			}

		}
		if (!EventSystem.current.IsPointerOverGameObject ()) {  
			if (Input.GetMouseButton (0)) {
			
				if (state.Equals ("EDIT")) {
					if (selectedGameobject != null) {
						MoveSlidAR (Input.mousePosition);

					}				
				}
			}
		}
		
#endif
		if (state.Equals ("EDIT")) {

			if (selectedGameobject != null) {
                //PrepareData();
                if (slidARMode)
                {
                    DrawSlidAR();
                }
				
				//print ("edit on");
			}
		} 
	}
    public void SlidARActive()
    {
        UIs.OnSlidARTouch();
        slidARMode = true;
        rotateMode = false;
    }
    public void RotateActive()
    {
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
        //addAnnoPanel.SetActive(false);
        //editPanel.SetActive(false);
        slidARMode = false;
        rotateMode = false;
        destroyLine ();
        state = "NONE";
    }
    public void TestAnnoButton()
    {
        annotype = "DEBUG";
    }
    public void CircleAnnoButton()
    {
        annotype = "CIRCLE";
    }

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

	public void ChangeStateButton(string text){

        
		if (text.Equals ("EDIT")) {
			
			
		}
		
		print (state);
	}
    public void Rotate(float input)
    {
		if (selectedGameobject != null) {
			annoScrip.SetOrientation(input);
		}
    }
	private void CreateCircle(Vector3 objePos,bool paraOption){
		GameObject newAnnotation = Instantiate (circlePrefab,objePos,transform.rotation) as GameObject;
		newAnnotation.transform.parent = marker.transform;
		annoScrip = (AnnotationScript)newAnnotation.GetComponent (typeof(AnnotationScript));
		annoScrip.SetState (paraOption);
		annoScrip.InitCamaraPosition (Camera.main.transform.position);
		TmpCircleGameobject = newAnnotation;
	}
	private void CreateAnno(Vector3 objePos,bool paraOption){
		GameObject newAnnotation = Instantiate (annoPrefab,objePos,transform.rotation) as GameObject;
		newAnnotation.transform.parent = marker.transform;
		annoScrip = (AnnotationScript)newAnnotation.GetComponent (typeof(AnnotationScript));
		annoScrip.SetState (paraOption);
		annoScrip.InitCamaraPosition (Camera.main.transform.position);

	}

	private void MoveSlidAR(Vector3 pos){
		annoScrip = (AnnotationScript)selectedGameobject.GetComponent (typeof(AnnotationScript));
		Vector3 camPos = annoScrip.GetInitCamPos ();
		Vector3 objPos = annoScrip.GetAnnoPos ();

		Vector3 intCamToSc = Camera.main.WorldToScreenPoint (camPos);
		Vector3 objToSc = Camera.main.WorldToScreenPoint (objPos);


//		float disx = Mathf.Abs(tmpcam2.x - tmpAnno2.x);
//		float disy = Mathf.Abs(tmpcam2.y - tmpAnno2.y);
//
//	
//		float m = disy / disx;
//		float c = tmpcam2.y - m * tmpAnno2.x;

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
		Ray ray2 = Camera.main.ScreenPointToRay (touchPos);

		Vector3 V1 = objPos - camPos;
		Vector3 V2 =  touchPos - cCamPos ;

		//print (newPosToWorld);
		//print (Camera.main.transform.position);

		//print (tmpAnnoPos);
		//print (tmpCamPos);

		print ("touch : "+Camera.main.ScreenToWorldPoint(touchPos));
		print ("Camera Pos : "+Camera.main.transform.position);
		print ("InitCamPos : "+camPos);
		print ("ObjPos : "+objPos);


		float[][] input = new float[3][];
		input[0] = new float[3] { -V1.x, V2.x, camPos.x - cCamPos.x };
		input[1] = new float[3] { -V1.y, V2.y, camPos.y - cCamPos.y };
		input[2] = new float[3] { -V1.z, V2.z, camPos.z - cCamPos.z };

//		print (input[0][0]+":"+input[0][1]+":"+input[0][2]);
//		print (input[1][0]+":"+input[1][1]+":"+input[1][2]);
//		print (input[2][0]+":"+input[2][1]+":"+input[2][2]);

        float[] result = guassianElim(input);
        float d = result[0];
        float t = result[1];

		//print (d+":"+t);
        //print(tmpCamPos + " : " + d + " : " + rayInit);
        //print(tmpCamPos + (d*rayInit));
		selectedGameobject.transform.position = camPos + (d * V1);

        
	}

	private void destroyLine(){
		if (lr.enabled) {
			lr.enabled = false;
		}
	}
	private void DrawSlidAR(){
		lr.enabled = true;

		annoScrip = (AnnotationScript)selectedGameobject.GetComponent (typeof(AnnotationScript));
		Vector3 tmpCamPos = annoScrip.GetInitCamPos ();
		Vector3 tmpAnnoPos = annoScrip.GetAnnoPos ();

		lr.material = lineMat;
		lr.SetColors (Color.red,Color.red);
		lr.SetWidth (0.005f,0.005f);
		lr.SetVertexCount (2);	



		tmpcam2 = Camera.main.WorldToScreenPoint (tmpCamPos);
		tmpAnno2 = Camera.main.WorldToScreenPoint (tmpAnnoPos);

		//print (tmpcam2);

		Vector3 tmpcam3 = new Vector3 (tmpcam2.x,tmpcam2.y,1);
		Vector3 tmpAnno3 = new Vector3 (tmpAnno2.x,tmpAnno2.y,1);

		//print (Camera.main.ScreenToWorldPoint(tmpcam2));
		//print (Camera.main.ScreenToWorldPoint(tmpAnno3));

		lr.SetPosition (0, Camera.main.ScreenToWorldPoint(tmpcam2));
		lr.SetPosition (1, Camera.main.ScreenToWorldPoint(tmpAnno2));
//		lr.SetPosition (0, Camera.main.ScreenToWorldPoint(tmpcam3));
//		lr.SetPosition (1, Camera.main.ScreenToWorldPoint(tmpAnno3));			
	}
//	private void PrepareData(){
//		annoScrip = (AnnotationScript)selectedGameobject.GetComponent (typeof(AnnotationScript));
//		Vector3 tmpCamPos = annoScrip.GetInitCamPos ();
//		Vector3 tmpAnnoPos = annoScrip.GetAnnoPos ();
//		Vector3 tmpcam2 = Camera.main.WorldToScreenPoint (tmpCamPos);
//		Vector3 tmpAnno2 = Camera.main.WorldToScreenPoint (tmpAnnoPos);
//
//	}


	public void RemoveSelectedAnno(){
		
		if (selectedGameobject != null) {
			Destroy (selectedGameobject);
			selectedGameobject = null;

		}
//		if (TmpCircleGameobject != null) {
//			Destroy(TmpCircleGameobject);
//			TmpCircleGameobject = null;
//		}
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

//			Matrix4x4 cam_pos = Camera.main.worldToCameraMatrix;
//			Matrix4x4 loc_pos = Marker.transform.worldToLocalMatrix;
//			Matrix4x4 loc_in_cam_space = cam_pos.inverse * loc_pos;





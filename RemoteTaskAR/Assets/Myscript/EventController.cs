using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventController : MonoBehaviour {

	public GameObject marker;
	public GameObject annoPrefab;
	private AnnotationScript annoScrip;

	public GameObject circlePrefab;
	public string state;

	public Button button90;
	public Button button180;
	public Button buttonCircle;
	public Button buttonEdit;
	public Button buttonRemove;

	public Material lineMat;
	private LineRenderer lr;

	private GameObject selectedGameobject;
	private GameObject TmpCircleGameobject;

	private Ray ray;
	private Vector3 grav;


//	Vector3 tmpCamPos ;
//	Vector3 tmpAnnoPos ;
	Vector3 tmpcam2 ;
	Vector3 tmpAnno2 ;

	float t ;

	// Use this for initialization
	void Start () {
		state = "NONE";
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
					else if(state.Equals("90DEG")){
						CreateAnno (rayEnd,false);
//						RemoveSelectedAnno();
//						CreateCircle(rayEnd,false);
					}
					else if(state.Equals("180DEG")){
						CreateAnno (rayEnd,true);
//						RemoveSelectedAnno();
//						CreateCircle(rayEnd,true);
					}
					else if(state.Equals("CIRCLE")){

						RemoveSelectedAnno();
						CreateCircle(rayEnd,false);

					}
//					if (state.Equals ("CREATE")) {
//						ray = Camera.main.ScreenPointToRay (touch.position);
//						Vector3 rayEnd = ray.GetPoint (4);
//
//						createAnno (rayEnd);
//					}


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
		if(Input.GetMouseButtonDown(0)){
			

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
							//PrepareData();
							break;
						}

					}
				}
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
		if(Input.GetKey(KeyCode.DownArrow)){
			if (selectedGameobject != null) {
				MoveSlidAR(Input.mousePosition);
				t=t-0.01f;
			}

		}

		if(Input.GetKey(KeyCode.UpArrow)){

			if (selectedGameobject != null) {
				MoveSlidAR(Input.mousePosition);
				t=t+0.01f;
			}

		}
#endif
		if(state.Equals("EDIT")){

			if (selectedGameobject != null) {


				//PrepareData();
				DrawSlidAR ();
				//print ("edit on");
			}
		}


	}    

	public void ChangeStateButton(string text){


		if (text.Equals ("EDIT")) {
			
			if (!state.Equals ("EDIT")) {
				
				state = "EDIT";
				buttonEdit.image.color = Color.red;
				button90.enabled = false;
				button180.enabled = false;
				buttonCircle.enabled = false;
			} else {
				state = "NONE";
				buttonEdit.image.color = Color.white;
				button90.enabled = true;
				button180.enabled = true;
				buttonCircle.enabled = true;
			}
		}
		else if (text.Equals (state)) {
			state = "NONE";
			button90.image.color = Color.white;
			button180.image.color = Color.white;
			buttonCircle.image.color = Color.white;
			
		}else if (text.Equals ("90DEG")) {
			state = "90DEG";
			button90.image.color = Color.gray;
			button180.image.color = Color.white;
			buttonCircle.image.color = Color.white;

		}
		else if (text.Equals ("180DEG")) {
			state = "180DEG";
			button90.image.color = Color.white;
			button180.image.color = Color.gray;
			buttonCircle.image.color = Color.white;

		}
		else if (text.Equals ("CIRCLE")) {
			state = "CIRCLE";
			button90.image.color = Color.white;
			button180.image.color = Color.white;
			buttonCircle.image.color = Color.gray;

		}
		 else {
			button90.image.color = Color.white;
			button180.image.color = Color.white;
			buttonCircle.image.color = Color.white;
		}
		print (state);
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
		Vector3 tmpCamPos = annoScrip.GetInitCamPos ();
		Vector3 tmpAnnoPos = annoScrip.GetAnnoPos ();


		float disx = Mathf.Abs(tmpcam2.x - tmpAnno2.x);
		float disy = Mathf.Abs(tmpcam2.y - tmpAnno2.y);


		//////
		float distance = Mathf.Sqrt ((disx*disx)+(disy*disy));
		///////
	
		float m = disy / disx;
		float c = tmpcam2.y - m * tmpAnno2.x;

		float lx = pos.x + (m*pos.x-pos.y+c)/(m*m+1)*m;
		float ly = pos.y + (m*pos.x-pos.y+c)/(m*m+1); 
		//print (pos);
		//print (lx+" : "+ly);

		Vector3 newPos = new Vector3 (lx,ly,pos.z+1);
		Vector3 newPosToWorld = Camera.main.ScreenToWorldPoint (newPos);



		Vector3 cCamPos = Camera.main.transform.position;
		Ray ray2 = Camera.main.ScreenPointToRay (newPos);

		Vector3 rayInit = tmpAnnoPos - tmpCamPos;
		Vector3 ray2Vec =  newPosToWorld - cCamPos ;

		print (newPosToWorld);
		print (Camera.main.transform.position);

		print (tmpAnnoPos);
		print (tmpCamPos);


		float[][] input = new float[3][];
		input[0] = new float[3] {-rayInit.x,ray2Vec.x, tmpCamPos.x - cCamPos.x };
		input[1] = new float[3] { -rayInit.y, ray2Vec.y, tmpCamPos.y - cCamPos.y };
		input[2] = new float[3] { -rayInit.z, ray2Vec.z, tmpCamPos.z - cCamPos.z };

//		print (input[0][0]+":"+input[0][1]+":"+input[0][2]);
//		print (input[1][0]+":"+input[1][1]+":"+input[1][2]);
//		print (input[2][0]+":"+input[2][1]+":"+input[2][2]);

        float[] result = guassianElim(input);
        float d = result[0];
        float t = result[1];

//        print(tmpCamPos + " : " + d + " : " + rayInit);
//        print(tmpCamPos + (d*rayInit));
		selectedGameobject.transform.position = tmpCamPos + (d * rayInit);

        
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





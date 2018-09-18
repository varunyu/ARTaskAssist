using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserStudy : MonoBehaviour {

	public float[] targetPos = new float[3];
	public float[] targetOren = new float[3];

	public GameObject selectedGameobject;
	private GameObject tmp;

	private GameObject GuideLists;

	public GameObject list1;
	public GameObject list2;

	public int childNum;
	public int count;

	private bool timerStart;
	private float timeCount;

	public Text debugText;

	private Quaternion Q1;
	private Quaternion Q2;


	private AnnotationControl AC;

	private char[] stageList;
	public GameObject WorldCoordiList;
	private GameObject cWorldCoordi;

	public int cStage;

	public Scrollbar progressBar;
	public GameObject[] correctCheck;

	private Vector3 lastCamPos;
	private float deviceMovement;
	private float rotateTimer;
	private bool rotateMode;

	void Awake (){
		AC = (AnnotationControl)gameObject.GetComponent(typeof(AnnotationControl));

		string input = "12345678";

		if (PlayerPrefs.HasKey ("UserStudyOrder")) {
			input = PlayerPrefs.GetString ("UserStudyOrder");
		}
		cStage = 1;

		stageList = input.ToCharArray ();

	}

	void Start () {
		childNum = 5;
		count = 0;

		StageSetup ();
		TargetSetUp();
		ShowProgress ();


		deviceMovement = 0;
		lastCamPos = Camera.main.transform.position;
		timerStart = true;
	}

	private float totalDistace;

	void Update(){
		if (timerStart) {
			timeCount += Time.deltaTime;
			/*
			print("Dis "+Vector3.Distance (Camera.main.transform.position,lastCamPos)
				* Time.deltaTime);*/

			if (rotateMode) {
				rotateTimer += Time.deltaTime;
			}

			totalDistace = Vector3.Distance (Camera.main.transform.position, lastCamPos)
			* Time.deltaTime;

			if( totalDistace >=0.06f ){
				deviceMovement += totalDistace;
			}
			lastCamPos = Camera.main.transform.position;
		}

	}
	private void ResetTimer(){
		timeCount = 0;
		deviceMovement = 0f;
		rotateTimer = 0;
		if (rotateMode) {
			rotateMode = false;
		}
	}
	public void SetRotateMode(bool t){
		
		rotateMode = t;
	}
	private string savePrefsName;

	private void StageSetup(){
		if (cWorldCoordi != null) {
			cWorldCoordi.SetActive (false);
		}

		switch (stageList [cStage - 1]) {
		case '1':
			// setup world coordinate
			// setup method
			AC.SetGravityMode (true);
			cWorldCoordi = WorldCoordiList.transform.GetChild (0).gameObject;
			GuideLists = list1;
			savePrefsName = "Time000";
			break;
		case '2':
			AC.SetGravityMode(true);
			cWorldCoordi = WorldCoordiList.transform.GetChild (1).gameObject;
			GuideLists = list1;
			savePrefsName = "Time001";
			break;
		case '3':
			AC.SetGravityMode(true);
			cWorldCoordi = WorldCoordiList.transform.GetChild (0).gameObject;
			GuideLists = list2;
			savePrefsName = "Time010";
			break;
		case '4':
			AC.SetGravityMode(true);
			cWorldCoordi = WorldCoordiList.transform.GetChild (1).gameObject;
			GuideLists = list2;
			savePrefsName = "Time011";
			break;	
		case '5':
			AC.SetGravityMode(false);
			cWorldCoordi = WorldCoordiList.transform.GetChild (0).gameObject;
			GuideLists = list1;
			savePrefsName = "Time100";
			break;
		case '6':
			AC.SetGravityMode(false);
			cWorldCoordi = WorldCoordiList.transform.GetChild (1).gameObject;
			GuideLists = list1;
			savePrefsName = "Time101";
			break;
		case '7':
			AC.SetGravityMode(false);
			cWorldCoordi = WorldCoordiList.transform.GetChild (0).gameObject;
			GuideLists = list2;
			savePrefsName = "Time110";
			break;
		case '8':
			AC.SetGravityMode(false);
			cWorldCoordi = WorldCoordiList.transform.GetChild (1).gameObject;
			GuideLists = list2;
			savePrefsName = "Time111";
			break;
		case '0':
			AC.SetGravityMode (true);
			cWorldCoordi = WorldCoordiList.transform.GetChild (1).gameObject;
			break;
		
		}
		progressBar.size = (cStage) * 0.125f;
		//cWorldCoordi.SetActive (true);

		AC.SetWorldCoor (cWorldCoordi.transform.eulerAngles);

	}

	private void SaveTimeRecord(){
		print ("Save: " + savePrefsName);
		deviceMovement *= 10;
		PlayerPrefs.SetString (savePrefsName,timeCount.ToString());
		PlayerPrefs.SetString (savePrefsName+"R",rotateTimer.ToString());
		PlayerPrefs.SetString (savePrefsName+"D",deviceMovement.ToString());
		ResetTimer ();
	}

	public float GetTime(){
		return timeCount;
	}
	public void TargetSetUp(){

		if (!IsFinish ()) {
			
			GuideLists.transform.GetChild (count).gameObject.SetActive (true);
			tmp = GuideLists.transform.GetChild (count).gameObject;
		
			targetPos [0] = tmp.transform.position.x;
			targetPos [1] = tmp.transform.position.y;
			targetPos [2] = tmp.transform.position.z;

			targetOren [0] = tmp.transform.eulerAngles.x;
			targetOren [1] = tmp.transform.eulerAngles.y;
			targetOren [2] = tmp.transform.eulerAngles.z;

			Q1 = tmp.transform.rotation;


		}
	}
	public void StartUserStudy(){
		
		ShowProgress ();
		TargetSetUp ();
	}
	private void ShowProgress(){
		debugText.text = count + " / " + childNum;




	}

	public bool IsFinish(){
		

		if (cStage == 8 && count >= 5) {

			print ("finish");
			//print ("complete :" + cStage);
			if (timerStart) {
				SaveTimeRecord ();
			}

			timerStart = false;
			debugText.text = " Finish ";
			savePrefsName = "Done";
			return true;
		} else
			if (count == 5) {
				count = 0;
				//print ("complete :" + cStage);
				SaveTimeRecord ();
				cStage++;
				StageSetup ();
			}
			return false;
	}

	public Vector3 GetCurrentGuide(){
		return  GuideLists.transform.GetChild (count).gameObject.transform.position;
	}

	public void SaveData(){
		
	}

	private float posX ;
	private float posY ;
	private float posZ ;

	private float scale;

	private float angle;
	private float mAngle = 20f;
	/*
	 * Check correctness
	 * Hard code need to redesign ASAP
	 * */
	public void CheckCorrectness(GameObject obj){
		
		if (!IsFinish ()) {
			
			selectedGameobject = obj;

			posX = selectedGameobject.transform.position.x;
			posY = selectedGameobject.transform.position.y;
			posZ = selectedGameobject.transform.position.z;

			Q2 = selectedGameobject.transform.rotation;
				
			scale = selectedGameobject.transform.localScale.x;
			angle = Quaternion.Angle (Q1,Q2);

			if (posX <= targetPos [0] + 1f && posX >= targetPos [0] - 1f &&
			    posY <= targetPos [1] + 1f && posY >= targetPos [1] - 1f &&
			    posZ <= targetPos [2] + 1f && posZ >= targetPos [2] - 1f) {

				CorrectImageColor (0, 1);


				if (angle <= mAngle) {
					CorrectImageColor (1, 1);
					Correct ();
				}

			} else if (angle <= mAngle) {
				CorrectImageColor (1, 1);
			} else {
				CorrectImageColor (0, 0);
				CorrectImageColor (1, 0);
			}

		} 


	}

	public void Correct(){
		if (!IsFinish()) {
			GuideLists.transform.GetChild (count).gameObject.SetActive (false);
			count++;

			ShowProgress ();
			TargetSetUp ();
		}
	}

	public void CorrectImageColor(int index,int color){
		if (color == 0) {
			correctCheck [index].GetComponent<Image> ().color = Color.gray;
		} else if (color == 1) {
			correctCheck [index].GetComponent<Image> ().color = Color.blue;
		}
	}

}

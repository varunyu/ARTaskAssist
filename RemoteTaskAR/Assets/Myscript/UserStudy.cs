using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserStudy : MonoBehaviour {

	public float[] targetPos = new float[3];
	public float[] targetOren = new float[3];
	public float targetScale= 0f;
	public GameObject selectedGameobject;
	private GameObject tmp;

	private GameObject GuideLists;

	public GameObject list1;
	public GameObject list2;

	public int childNum;
	public int count;

	private bool timerStart;
	private float timeCount;

	private int inputCount;
	// 0 = app1_1
	// 1 = app1_2
	// 2 = app2_1
	// 3 = app2_2
	public int userStudyScene;
	// Use this for initialization

	public Text debugText;

	private Quaternion Q1;
	private Quaternion Q2;

	private AnnotationScript annoScrip;
	private annoScript2 annoScrip2;

	public float slidARTotalTime;
	public float slidARcount;
	public bool slidAREnable;
	public bool Helped;

	private int mode;
	void Awake (){
		mode = PlayerPrefs.GetInt ("STAGE");

		if (mode == 1|| mode ==4) {
			GuideLists = list1;

			if (mode == 1)
				userStudyScene = 0;
			if (mode == 4)
				userStudyScene = 2;

		} else if (mode == 2 || mode ==5) {
			GuideLists = list2;
			if (mode == 2)
				userStudyScene = 1;
			if (mode == 5)
				userStudyScene = 3;
		} else {
			
		}
	}

	void Start () {
		inputCount = 0;
		childNum = 5;
		count = 0;
		slidARTotalTime = 0;
		slidARcount = 0;
		Helped= false;
		//TargetSetUp();
	}
	void Update(){
		if (timerStart) {
			timeCount += Time.deltaTime;
		}
		if (slidAREnable) {
			slidARTotalTime += Time.deltaTime;

			if (!Helped) {
				slidARcount += Time.deltaTime;
			}


			if (slidARcount >= 40) {
				PositionHelper ();
				Helped = true;
				slidARcount = 0;
			}
		}
	}
	private Vector3 currentGuide;

	public void PositionHelper(){
		currentGuide = GetCurrentGuide ();

		if (mode == 1 || mode == 2) {
			annoScrip2 = (annoScript2)selectedGameobject.GetComponent (typeof(annoScript2));
			selectedGameobject.transform.position = currentGuide;
			annoScrip2.SetPos (currentGuide);

		} else if (mode == 4 || mode == 5) {
			annoScrip = (AnnotationScript)selectedGameobject.GetComponent(typeof(AnnotationScript));
			selectedGameobject.transform.position = currentGuide;
			annoScrip.SetPos (currentGuide);
		}
	}
	public void SetSlidAR(bool AR){
		slidAREnable = AR;
		if (AR) {
			slidARcount = 0;
		}
	}
	public void SetTimer(bool timer){
		timerStart = timer;
	}
	public float GetTime(){
		return timeCount;
	}
	public void InputAdd(){
		if (!IsFinish()) {
			inputCount++;
		}
	}
	public int InputCount(){
		return inputCount;
	}

	public void TargetSetUp(){

		if (!IsFinish ()) {
			Helped= false;
			SetSlidAR (false);
			GuideLists.transform.GetChild (count).gameObject.SetActive (true);
			tmp = GuideLists.transform.GetChild (count).gameObject;
		
			targetPos [0] = tmp.transform.position.x;
			targetPos [1] = tmp.transform.position.y;
			targetPos [2] = tmp.transform.position.z;

			targetOren [0] = tmp.transform.eulerAngles.x;
			targetOren [1] = tmp.transform.eulerAngles.y;
			targetOren [2] = tmp.transform.eulerAngles.z;

			Q1 = tmp.transform.rotation;

			targetScale = tmp.transform.localScale.x;
		}
	}
	public void StartUserStudy(){
		SetTimer (true);
		ShowProgress ();
		TargetSetUp ();
	}
	private void ShowProgress(){
		debugText.text = count + " / " + childNum;
	}
	public bool IsFinish(){
		
		if (count == childNum) {
			SetTimer (false);
			SvaeData ();
			print ("Finish");
			return true;
		}
		return false;
	}

	public Vector3 GetCurrentGuide(){
		return  GuideLists.transform.GetChild (count).gameObject.transform.position;
	}

	public void SvaeData(){
		switch(userStudyScene)
		{
		case 0:
			PlayerPrefs.SetFloat ("TimeApp1_1",timeCount);
			PlayerPrefs.SetFloat ("TimeSLIDAR1_1",slidARTotalTime);
			PlayerPrefs.SetInt ("InputApp1_1",inputCount);
			break;
		case 1:
			PlayerPrefs.SetFloat ("TimeApp1_2",timeCount);
			PlayerPrefs.SetFloat ("TimeSLIDAR1_2",slidARTotalTime);
			PlayerPrefs.SetInt ("InputApp1_2",inputCount);
			break;
		case 2:
			PlayerPrefs.SetFloat ("TimeApp2_1",timeCount);
			PlayerPrefs.SetFloat ("TimeSLIDAR2_1",slidARTotalTime);
			PlayerPrefs.SetInt ("InputApp2_1",inputCount);
			break;
		case 3:
			PlayerPrefs.SetFloat ("TimeApp2_2",timeCount);
			PlayerPrefs.SetFloat ("TimeSLIDAR2_2",slidARTotalTime);
			PlayerPrefs.SetInt ("InputApp2_2",inputCount);
			break;
		}
	}

	private float posX ;
	private float posY ;
	private float posZ ;
	/*
	private float rotX ;
	private float rotY ;
	private float rotZ ;
*/
	private float scale;

	private float angle;
	private float mAngle = 15f;
	/*
	 * Check correctness
	 * too Hard code need to redesign ASAP
	 * */
	public void CheckCorrectness(GameObject obj){
		
		if (!IsFinish ()) {
			
			selectedGameobject = obj;

			posX = selectedGameobject.transform.position.x;
			posY = selectedGameobject.transform.position.y;
			posZ = selectedGameobject.transform.position.z;
			/*
			rotX = selectedGameobject.transform.eulerAngles.x;
			rotY = selectedGameobject.transform.eulerAngles.y;
			rotZ = selectedGameobject.transform.eulerAngles.z;
*/
			Q2 = selectedGameobject.transform.rotation;
				
			scale = selectedGameobject.transform.localScale.x;
			//print (posX + posY + posZ);
			/*
			print(posX+ " "+posY+ " "+posZ);
			print (rotX + " " + rotY + " " + rotZ);
			print (scale);
*/
			if (posX <= targetPos [0] + 0.8f && posX >= targetPos [0] - 0.8f &&
			    posY <= targetPos [1] + 0.8f && posY >= targetPos [1] - 0.8f &&
			    posZ <= targetPos [2] + 0.8f && posZ >= targetPos [2] - 0.8f &&			    
			    scale <= targetScale + 0.07f && scale >= targetScale - 0.07f) {

				angle = Quaternion.Angle (Q1,Q2);

				if (angle <= mAngle) {
					Correct ();
				}

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

}

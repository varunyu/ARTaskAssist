using UnityEngine;
using System.Collections;

public class UserStudy : MonoBehaviour {

	public float[] targetPos;
	public float[] targetOren;
	public float targetScale;
	public GameObject selectedGameobject;
	private GameObject tmp;

	public GameObject GuideLists;
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
	void Start () {
		inputCount = 0;
		childNum = 3;
		count = 0;

		targetPos = new float[3];
		targetOren = new float[3];
		targetScale = 0f;

		TargetSetUp();
	}
	void Update(){
		if (timerStart) {
			timeCount += Time.deltaTime;
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
			GuideLists.transform.GetChild (count).gameObject.SetActive (true);
			tmp = GuideLists.transform.GetChild (count).gameObject;
			/*
		targetPos [0] = 0.150f;
		targetPos [1] = -0.020f;
		targetPos [2] = -0.132f;

		targetOren [0] = 0f;
		targetOren [1] = 0f;
		targetOren [2] = 0f;

		targetScale [0] = 0.472f;
		targetScale [1] = 0.472f;
		targetScale [2] = 0.472f;
		*/
			targetPos [0] = tmp.transform.position.x;
			targetPos [1] = tmp.transform.position.y;
			targetPos [2] = tmp.transform.position.z;

			targetOren [0] = tmp.transform.eulerAngles.x;
			targetOren [1] = tmp.transform.eulerAngles.y;
			targetOren [2] = tmp.transform.eulerAngles.z;

			targetScale = tmp.transform.localScale.x;
		}




	}
	public bool IsFinish(){
		if (count == 0) {
			SetTimer (true);
		}

		if (count == childNum) {
			SetTimer (false);
			SvaeData ();
			return true;
		}
		return false;
	}

	public void SvaeData(){
		switch(userStudyScene)
		{
		case 0:
			PlayerPrefs.SetFloat ("TimeApp1_1",timeCount);
			PlayerPrefs.SetInt ("InputApp1_1",inputCount);
			break;
		case 1:
			PlayerPrefs.SetFloat ("TimeApp1_2",timeCount);
			PlayerPrefs.SetInt ("InputApp1_2",inputCount);
			break;
		case 2:
			PlayerPrefs.SetFloat ("TimeApp2_1",timeCount);
			PlayerPrefs.SetInt ("InputApp2_1",inputCount);
			break;
		case 3:
			PlayerPrefs.SetFloat ("TimeApp2_2",timeCount);
			PlayerPrefs.SetInt ("InputApp2_2",inputCount);
			break;
		}
	}

	private float posX ;
	private float posY ;
	private float posZ ;

	private float rotX ;
	private float rotY ;
	private float rotZ ;

	private float scale;



	public bool CheckCorrectness(GameObject obj){

		if (!IsFinish ()) {

			selectedGameobject = obj;

			posX = selectedGameobject.transform.position.x;
			posY = selectedGameobject.transform.position.y;
			posZ = selectedGameobject.transform.position.z;

			rotX = selectedGameobject.transform.eulerAngles.x;
			rotY = selectedGameobject.transform.eulerAngles.y;
			rotZ = selectedGameobject.transform.eulerAngles.z;

			scale = selectedGameobject.transform.localScale.x;
			//print (posX + posY + posZ);


			if (posX <= targetPos [0] + 0.05f && posX >= targetPos [0] - 0.05f &&
			    posY <= targetPos [1] + 0.05f && posY >= targetPos [1] - 0.05f &&
			    posZ <= targetPos [2] + 0.05f && posZ >= targetPos [2] - 0.05f &&
			    rotX <= targetOren [0] + 4.5f && rotX >= targetOren [0] - 4.5f &&
			    rotY <= targetOren [1] + 4.5f && rotY >= targetOren [1] - 4.5f &&
			    rotZ <= targetOren [2] + 4.5f && rotZ >= targetOren [2] - 4.5f &&
			    scale <= targetScale + 0.05f && scale >= targetScale - 0.05f) {
				print ("correct");

				print (count);
				GuideLists.transform.GetChild (count).gameObject.SetActive (false);
				count++;
				TargetSetUp ();

			}

			return false;

		} else {
			return true;
		}


	}

}

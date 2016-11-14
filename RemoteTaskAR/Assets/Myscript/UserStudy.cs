using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserStudy : MonoBehaviour {

	public float[] targetPos = new float[3];
	public float[] targetOren = new float[3];
	public float targetScale= 0f;
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

	public bool isGravAR;
	public Text debugText;

	private Quaternion Q1;
	private Quaternion Q2;

	void Start () {
		inputCount = 0;
		childNum = 5;
		count = 0;


		//TargetSetUp();
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
	/*
	private float rotX ;
	private float rotY ;
	private float rotZ ;
*/
	private float scale;

	private float angle;
	private float mAngle = 10f;
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

				/*
				if (isGravAR) {

					if (count == 1 || count ==4) {
						if(rotY <= targetOren [1] + 5f && rotY >= targetOren [1] - 5f ){
							Correct ();
						}
					} else if (count == 3) {
						if(rotZ <= targetOren [1] + 5f && rotZ >= targetOren [1] - 5f ){
							Correct ();
						}
					}else {
						Correct ();
					}

					
				} else if(rotX <= targetOren [0] + 5f && rotX >= targetOren [0] - 5f &&
					rotY <= targetOren [1] + 5f && rotY >= targetOren [1] - 5f &&
					rotZ <= targetOren [2] + 5f && rotZ >= targetOren [2] - 5f){

					Correct ();
				}
*/

			} else {
				
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

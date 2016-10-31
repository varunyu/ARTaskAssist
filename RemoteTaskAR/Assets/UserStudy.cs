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

	private int inputCount;
	// Use this for initialization
	void Start () {
		inputCount = 0;
		childNum = 5;
		count = 1;

		targetPos = new float[3];
		targetOren = new float[3];
		targetScale = 0f;

		TargetSetUp();
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



		GuideLists.transform.GetChild (count - 1).gameObject.SetActive (true);
		tmp = GuideLists.transform.GetChild (count - 1).gameObject;
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
	public bool IsFinish(){
		if (count == childNum) {
			return true;
		}
		return false;
	}

	private float posX ;
	private float posY ;
	private float posZ ;

	private float rotX ;
	private float rotY ;
	private float rotZ ;

	private float scale;



	public void CheckCorrectness(GameObject obj){

		if (!IsFinish()) {

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

				GuideLists.transform.GetChild (count - 1).gameObject.SetActive (false);
				count++;
				TargetSetUp();

			} 

		}


	}

}

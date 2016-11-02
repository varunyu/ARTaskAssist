using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

	public GameObject logPannel;
	public Text[] timeList;
	public Text[] inputList;
	// Use this for initialization
	void Start () {
		
	}
	
	public void GotoARNormal(){
		SceneManager.LoadScene ("AR_Normal");
	}
	public void GotoARGrav(){
		SceneManager.LoadScene ("AR_Gravity");
	}
	public void GotoScenes(int i){
		switch (i) {
		case 1:
			SceneManager.LoadScene ("AR_Normal");
			break;
		case 2:
			SceneManager.LoadScene ("Tutorial_Normal");
			break;
		case 3:
			SceneManager.LoadScene ("EasyTask_Normal");
			break;
		case 4:
			SceneManager.LoadScene ("HardTask_Normal");
			break;
		case 5:
			SceneManager.LoadScene ("AR_Gravity");
			break;
		case 6:
			SceneManager.LoadScene ("Tutorial_Grav");
			break;
		case 7:
			SceneManager.LoadScene ("EasyTask_Grav");
			break;
		case 8:
			SceneManager.LoadScene ("HardTask_Grav");
			break;
		}
	}
	public void ShowLogPannel(bool b){
		logPannel.SetActive (b);
	}

	float time1_1;
	float time1_2;
	float time2_2;
	float time2_1;
	int input1_1;
	int input1_2;
	int input2_1;
	int input2_2;

	public void ShowLogInfo(){
		
		if (PlayerPrefs.HasKey ("TimeApp1_1")) {
			time1_1 = PlayerPrefs.GetFloat ("TimeApp1_1");
		} else {
			time1_1 = 0f;
		}
		timeList [0].text = time1_1.ToString();
		if (PlayerPrefs.HasKey ("TimeApp1_2")) {
			time1_2 = PlayerPrefs.GetFloat ("TimeApp1_2");
		} else {
			time1_2 = 0f;
		}
		timeList [1].text = time1_2.ToString();
		if (PlayerPrefs.HasKey ("TimeApp2_1")) {
			time2_1 = PlayerPrefs.GetFloat ("TimeApp2_1");
		} else {
			time2_1 = 0f;
		}
		timeList [2].text = time2_1.ToString();
		if (PlayerPrefs.HasKey ("TimeApp2_2")) {
			time2_2 = PlayerPrefs.GetFloat ("TimeApp2_2");
		} else {
			time2_2 = 0f;
		}
		timeList [3].text = time2_2.ToString ();
		/////////////////////////////////////////
		if (PlayerPrefs.HasKey ("InputApp1_1")) {
			input1_1 = PlayerPrefs.GetInt ("InputApp1_1");
		} else {
			input1_1 = 0;
		}
		inputList [0].text = input1_1.ToString();
		if (PlayerPrefs.HasKey ("InputApp1_2")) {
			input1_2 = PlayerPrefs.GetInt ("InputApp1_2");
		} else {
			input1_2 = 0;
		}
		inputList [1].text = input1_2.ToString();
		if (PlayerPrefs.HasKey ("InputApp2_1")) {
			input2_1 = PlayerPrefs.GetInt ("InputApp2_1");
		} else {
			input2_1 = 0;
		}
		inputList [2].text = input2_1.ToString();
		if (PlayerPrefs.HasKey ("InputApp2_2")) {
			input2_2 = PlayerPrefs.GetInt ("InputApp2_2");
		} else {
			input2_2 = 0;
		}
		inputList [3].text = input2_2.ToString();
	}

	public void ClearLog(){
		PlayerPrefs.DeleteAll ();
	}


}

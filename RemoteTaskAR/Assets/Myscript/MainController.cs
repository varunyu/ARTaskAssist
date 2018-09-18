using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

	public GameObject logPannel;

	public InputField userStudyOrder;
	public GameObject timeLists;
	private Text[] resultList;
	// Use this for initialization
	void Start () {
		resultList = timeLists.GetComponentsInChildren<Text> (true);
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
			PlayerPrefs.SetInt ("STAGE", 0);
			SceneManager.LoadScene ("AR_Normal");
			break;
		case 2:
			SceneManager.LoadScene ("Tutorial_Normal");
			break;
		case 3:
			PlayerPrefs.SetInt ("STAGE", 1);
			SceneManager.LoadScene ("AR_Normal");
			//SceneManager.LoadScene ("EasyTask_Normal");
			break;
		case 4:
			PlayerPrefs.SetInt ("STAGE", 2);
			SceneManager.LoadScene ("AR_Normal");
			//SceneManager.LoadScene ("HardTask_Normal");
			break;
		case 5:
			PlayerPrefs.SetInt ("STAGE", 3);
			SceneManager.LoadScene ("AR_Gravity");
			break;
		case 6:
			SceneManager.LoadScene ("Tutorial_Grav");
			break;
		case 7:
			PlayerPrefs.SetInt ("STAGE", 4);
			SceneManager.LoadScene ("AR_Gravity");
			//SceneManager.LoadScene ("EasyTask_Grav");
			break;
		case 8:
			PlayerPrefs.SetInt ("STAGE", 5);
			SceneManager.LoadScene ("AR_Gravity");
			//SceneManager.LoadScene ("HardTask_Grav");
			break;
		case 9:
			
			SceneManager.LoadScene ("AR_DeviceMOV");

			break;
		case 10:
			//print (userStudyOrder.text);
			PlayerPrefs.SetString ("UserStudyOrder", userStudyOrder.text);
			SceneManager.LoadScene ("UserStudy");

			break;
		}
	}
	public void GotoDemo(){
		PlayerPrefs.SetString ("UserStudyOrder", "0");
		SceneManager.LoadScene ("UserStudy");
	}
	public void ShowLogPannel(bool b){
		logPannel.SetActive (b);
	}



	public void ShowLogInfo(int mode){

		int i = 0;
		for (int j = 0; j < 2; j++) {

			for(int k=0;k<2;k++){
				
				for (int l = 0; l < 2; l++) {

					switch (mode) {
					case 0:
						if (PlayerPrefs.HasKey ("Time"+j+""+k+""+l)) {
							resultList [i].text = PlayerPrefs.GetString ("Time"+j+""+k+""+l);
						}
						break;
					case 1:
						print (PlayerPrefs.HasKey ("Time"+j+""+k+""+l+"R"));
						if (PlayerPrefs.HasKey ("Time"+j+""+k+""+l+"R")) {
							resultList [i].text = PlayerPrefs.GetString ("Time"+j+""+k+""+l+"R");
						}
						break;
					case 2:
						
						if (PlayerPrefs.HasKey ("Time"+j+""+k+""+l+"D")) {
							resultList [i].text = PlayerPrefs.GetString ("Time"+j+""+k+""+l+"D") +" cm";
						}
						break;

					}

					i++;
				}
			}


		}

	}
	public void ResetLog(){
		
		for(int i=0;i<8;i++){
			resultList [i].text = "0";
		}
	}
	public void ClearLog(){
		PlayerPrefs.DeleteAll ();
	}


}

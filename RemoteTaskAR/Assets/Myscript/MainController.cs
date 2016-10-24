using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {

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
}

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
}

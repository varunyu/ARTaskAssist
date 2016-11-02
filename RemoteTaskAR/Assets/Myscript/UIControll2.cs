using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIControll2 : MonoBehaviour {

	public GameObject AddPannel;
	public GameObject EditPannel;

	void Start () {
	
	}
	public void AddButtonClick(){
		AddPannel.SetActive (true);
		EditPannel.SetActive (false);
	}
	public void EditPannelClick(){
		AddPannel.SetActive (false);
		EditPannel.SetActive (true);
	}
	public void ClearAll(){
		AddPannel.SetActive (false);
		EditPannel.SetActive (false);
	}
	public void BackToMenu(){
		SceneManager.LoadScene ("Main");
	}
}

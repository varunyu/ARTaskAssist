using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UserStudyUIController : MonoBehaviour {
	public Canvas gravUI;
	public Canvas deviceUI;

	public GameObject worldCoordiList;
	public GameObject ObjectList;
	public int index;
	public int max;
	// Use this for initialization

	void Start () {
		index = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void EnableDeviceUI(bool e){
		deviceUI.gameObject.SetActive (e);
	}
	private void EnableGravUI(bool e){
		gravUI.gameObject.SetActive (e);
	}

	public void SwitchMode(){
		if (deviceUI.gameObject.activeSelf) {
			EnableDeviceUI (false);
			EnableGravUI (true);
		} else {
			EnableDeviceUI (true);
			EnableGravUI (false);
		}
	}
	/*
	 * true = Gravity
	 * false = Device
	 * */
	public void SetMode(bool t){
		if (t) {
			if (deviceUI.gameObject.activeSelf) {

				EnableDeviceUI (false);
			} 
			EnableGravUI (true);
		} else{
			if (gravUI.gameObject.activeSelf) {
				
				EnableGravUI (false);
			} 
			EnableDeviceUI (true);
		}
	}

	public void ChangeIndex(){
		
		if (index < max) {
			index++;
		}
		if (index < 15) {
			if (index <= 4) {
				CloseAllChild (ObjectList);
				OpenChild (ObjectList, index);
			} else if (index <= 9) {
				CloseAllChild (worldCoordiList);
				OpenChild (worldCoordiList, 1);

				CloseAllChild (ObjectList);
				OpenChild (ObjectList, index-5);

			} else if (index <= 14) {
				CloseAllChild (worldCoordiList);
				OpenChild (worldCoordiList, 2);

				CloseAllChild (ObjectList);
				OpenChild (ObjectList, index-10);
			}
		} else {
			if (index == 15) {
				CloseAllChild (worldCoordiList);
				OpenChild (worldCoordiList, 0);

				CloseAllChild (ObjectList);
				OpenChild (ObjectList, 5);
			}else if(index ==16){
				CloseAllChild (ObjectList);
				OpenChild (ObjectList, 6);
			}else if(index ==17){
				CloseAllChild (worldCoordiList);
				OpenChild (worldCoordiList, 1);

				CloseAllChild (ObjectList);
				OpenChild (ObjectList, 7);
			}else if(index ==18){
				CloseAllChild (ObjectList);
				OpenChild (ObjectList, 8);
			}
		}
	}
	private void CloseAllChild(GameObject list){
		int childNum = list.gameObject.transform.childCount;

		for (int i = 0; i < childNum; i++) {
			list.gameObject.transform.GetChild (i).gameObject.SetActive (false);
		}
	}

	private void OpenChild(GameObject list,int i){
		list.gameObject.transform.GetChild (i).gameObject.SetActive (true);
	}
}

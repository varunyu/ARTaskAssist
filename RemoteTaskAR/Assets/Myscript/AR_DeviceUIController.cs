using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AR_DeviceUIController : MonoBehaviour {


	public GameObject mainPannel;
	public GameObject addPannel;
	public GameObject editPannel;
	public GameObject target;

	private Button[] bList1;
	private Button[] bList2;
	// Use this for initialization
	void Start () {
		bList1 = mainPannel.GetComponentsInChildren<Button>(true);
		bList2 = editPannel.GetComponentsInChildren<Button>(true);
	}
	

	public void EnableAddPannel(bool t){
		addPannel.SetActive (t);
		if (editPannel.activeSelf) {
			editPannel.SetActive (false);
		}
	}

	public void EnableTarget(bool t){
		target.SetActive (t);
	}
	public void EnableEditPannel(bool t){
		editPannel.SetActive (t);
		if (addPannel.activeSelf) {
			addPannel.SetActive (false);
		}
	}

	public void PannelButtonController(int index,int type){
		
		if (type == 1) {
			ClearButtonColor (bList1);
			ChangeButtonColor (bList1[index]);
		} else if (type == 2) {
			ClearButtonColor (bList2);
			ChangeButtonColor (bList2[index]);
		}
	}

	private void ChangeButtonColor(Button b){
		b.image.color = Color.red;
	}
	private void ClearButtonColor(Button[] list){

		for (int i = 0; i < list.Length; i++) {
			list [i].image.color = Color.white;

		}
	}
}

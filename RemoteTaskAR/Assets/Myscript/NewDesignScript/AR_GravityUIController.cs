using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class AR_GravityUIController : MonoBehaviour {

	public GameObject addPannel;
	public GameObject editPannel;

	public GameObject arrowPannel;
	public GameObject hCirclePannel;
	public GameObject adjustPannel;

	private Button[] arrowButtonList;
	private Button[] hCircleButtonList;

	private Button[] addButtonList;
	private Button[] editButtonList;
	private Button addButton;
	private Button editButton;
	// Use this for initialization
	void Awake () {
		addButtonList = addPannel.GetComponentsInChildren<Button>(true);
		editButtonList = editPannel.GetComponentsInChildren<Button> (true);

		arrowButtonList = arrowPannel.GetComponentsInChildren<Button> (true);
		hCircleButtonList = arrowPannel.GetComponentsInChildren<Button> (true);


		addButton = gameObject.transform.GetChild (0).
			gameObject.transform.GetChild(0).GetComponent<Button>();
		editButton = gameObject.transform.GetChild (0).
			gameObject.transform.GetChild(1).GetComponent<Button>();
	}



	// open/close main pannel
	public void MainPannelControl(int i){
		//active addPannel
		if (i == 0) {
			addPannel.SetActive (true);
			editPannel.SetActive (false);
			MainPannelButtonColor (0);
			InteractableButton (0);
		} 
		// active editPannel
		else if (i == 1) {
			editPannel.SetActive (true);
			addPannel.SetActive (false);
			MainPannelButtonColor (1);
			InteractableButton (0);
		} 
		// close all main pannel
		else if (i == 99) {
			editPannel.SetActive (false);
			addPannel.SetActive (false);
			MainPannelButtonColor (2);
			InteractableButton (1);
		}

	}

	//open/close sub pannel
	public void SubAddPannelControl(int i){
		//active arrow pannel
		if (i == 0) {
			arrowPannel.SetActive (true);
			hCirclePannel.SetActive (false);
			AddPannelButtonColor (0);
		} 
		// active Half Circle Pannel
		else if (i == 1) {
			arrowPannel.SetActive (false);
			hCirclePannel.SetActive (true);
			AddPannelButtonColor (1);
		}
		// active adjust pannel
		else if (i == 3) {
			arrowPannel.SetActive (false);
			hCirclePannel.SetActive (false);
			adjustPannel.SetActive (true);
			InteractableButton (10);
		}
		// close all sub pannel
		else if (i == 99) {
			editPannel.SetActive (false);
			addPannel.SetActive (false);
			adjustPannel.SetActive (false);
			AddPannelButtonColor (2);
			InteractableButton (11);
		}
	}

	private void InteractableButton(int i){
		if (i == 0) {
			addButton.interactable = false;
			editButton.interactable = false;
		} else if (i == 1) {
			addButton.interactable = true;
			editButton.interactable = true;
		} else if (i == 10) {
			addButtonList [0].interactable = false;
			addButtonList [1].interactable = false;
			arrowButtonList [0].interactable = false;
			arrowButtonList [1].interactable = false;
			hCircleButtonList [0].interactable = false;
			hCircleButtonList [1].interactable = false;

		} else if (i == 11) {
			addButtonList [0].interactable = true;
			addButtonList [1].interactable = true;
			arrowButtonList [0].interactable = true;
			arrowButtonList [1].interactable = true;
			hCircleButtonList [0].interactable = true;
			hCircleButtonList [1].interactable = true;
		} else if (i == 99) {
			
		}
	}

	public void EditPannelButtonColor(int i){
		if (i == 0) {
			editButtonList [0].image.color = Color.red;
			editButtonList [1].image.color = Color.white;
		} else if (i == 1) {
			editButtonList [0].image.color = Color.white;
			editButtonList [1].image.color = Color.red;
		} else if (i == 2) {
			editButtonList [0].image.color = Color.white;
			editButtonList [1].image.color = Color.white;
		}
	}

	private void MainPannelButtonColor(int i){
		if (i == 0) {
			addButton.image.color = Color.red;
			editButton.image.color = Color.white;
		} else if (i == 1) {
			addButton.image.color = Color.white;
			editButton.image.color = Color.red;
		} else if (i == 2) {
			addButton.image.color = Color.white;
			editButton.image.color = Color.white;
		}

	}
	private void AddPannelButtonColor(int i){
		if (i == 0) {
			addButtonList [0].image.color = Color.red;
			addButtonList [1].image.color = Color.white;
		} else if (i == 1) {
			addButtonList [0].image.color = Color.white;
			addButtonList [1].image.color = Color.red;
		} else if (i == 2) {
			addButtonList [0].image.color = Color.white;
			addButtonList [1].image.color = Color.white;
		}
	}
}

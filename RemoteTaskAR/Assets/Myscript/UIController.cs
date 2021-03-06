﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public GameObject addAnnoPanel;
    public GameObject editPanel;

    public Button addButton;
    public Button buttonEdit;
    public Button buttonRemove;

    public Button testModelButton;
    public Button buttonCircle;
    public Button annoOptionButton;  
    public Button addDone;

    public Button slidARButton;
    public Button rotateButton;
    public Button rotateLeftButton;
    public Button rotateRightButton;
    public Button editDone;


    public GameObject ArrowPannel;
    public GameObject CArrowPannel;
    public GameObject HalfCirclePannel;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame

    public void ClearColor(Button b)
    {
        b.image.color = Color.white;
    }
	public void AddAnnoActive()
    {
        addAnnoPanel.SetActive(true);
        editPanel.SetActive(false);

        addButton.image.color = Color.green;
        ClearColor(buttonEdit);
        
    }
    public void EditButtonActive()
    {
        editPanel.SetActive(true);
        addAnnoPanel.SetActive(false);

        ClearColor(addButton);       
        buttonEdit.image.color = Color.green;
    }

    public void OnSlidARTouch()
    {
        slidARButton.image.color = Color.red;
        ClearColor(rotateButton);
    }
    public void OnRotateTouch()
    {
        rotateButton.image.color = Color.red;
        ClearColor(slidARButton);
    }
    public void OnEditDone()
    {
        ClearColor(rotateButton);
        ClearColor(slidARButton);
		ClearColor (buttonEdit);
		ClearColor (addButton);
        editPanel.SetActive(false);
    }
    public void OnAddAnnoDone()
    {
        addAnnoPanel.SetActive(false);
    }
    public void ArrowPannelOpen()
    {
        ClearAllSubPannel();
        ArrowPannel.SetActive(true);
    }
    public void CArrowPannelOpen()
    {
        ClearAllSubPannel();
        CArrowPannel.SetActive(true);
    }
    public void HalfCirclPannelOpen()
    {
        ClearAllSubPannel();
        HalfCirclePannel.SetActive(true);
    }
    public void ClearAllSubPannel()
    {
        ArrowPannel.SetActive(false);
        CArrowPannel.SetActive(false);
        HalfCirclePannel.SetActive(false);
    }
	public void BackToMenu(){
		SceneManager.LoadScene ("Main");
	}
}

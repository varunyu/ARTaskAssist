using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class eventcontroller2 : MonoBehaviour {

	public GameObject marker;
	public GameObject[] annoPrefab;
	private int annotationtype;
	private string state;    

	private annoScript2 annoScrip;
	private Ray ray;
	private Vector3 grav;
	private GameObject selectedGameobject;

	// Use this for initialization
	void Start () {		
		state = "NONE";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touches.Length >0) {
			Touch touch = Input.GetTouch(0);

			if(!EventSystem.current.IsPointerOverGameObject (touch.fingerId))
			{
				if(touch.phase == TouchPhase.Began) 
				{

					ray = Camera.main.ScreenPointToRay (touch.position);
					Vector3 rayEnd = ray.GetPoint (4);

					if(state.Equals("NONE") || state.Equals ("EDIT"))
					{
						
						foreach(RaycastHit hit  in Physics.RaycastAll(ray) ) 
						{

							if(hit.collider.tag.Equals ("annotation"))
							{
								print (hit.transform.name);

								if (selectedGameobject != null)
								{
									SelectedAnnotationMat(false);
								}

								selectedGameobject = hit.transform.gameObject;
								SelectedAnnotationMat(true);

								break;
							}
						}
					}
					if (state.Equals("ADDANNO"))
					{

						GameObject selectPrefab = null;						                         
						switch (annotationtype)
						{
						case 1:
							selectPrefab = annoPrefab[0];
							break;
						case 2:
							selectPrefab = annoPrefab[1];
							break;
						case 3:
							selectPrefab = annoPrefab[2];
							break;
						case 4:
							selectPrefab = annoPrefab[3];
							break;
						case 5:
							selectPrefab = annoPrefab[4];
							break;
						}

						if (selectPrefab != null){
							CreateAnnotation(selectPrefab,rayEnd);
						}


					}
				}					
			}


		}
	}

	private void CreateAnnotation(GameObject annoPrefab,Vector3 objePos){

		GameObject newAnnotation = Instantiate (annoPrefab,objePos,transform.rotation) as GameObject;
		newAnnotation.transform.parent = marker.transform;
		annoScrip = (annoScript2)newAnnotation.GetComponent (typeof(annoScript2));
		annoScrip.SetInitCam(Camera.main.transform.position);

	}

	private void SelectedAnnotationMat(bool seleted)
	{
		annoScript2 tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		if (seleted)
		{
			tmp.SelectedAnnotation();
		}
		else
		{
			tmp.DeselectedAnnotatin();
		}
	}
	public void SetOrientation(float deg,string axis){

		annoScript2 tmp = (annoScript2)selectedGameobject.GetComponent(typeof(annoScript2));
		tmp.SetOrientation(deg,axis);
	}
}

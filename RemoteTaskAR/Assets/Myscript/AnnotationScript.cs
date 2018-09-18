using UnityEngine;
using System.Collections;

public class AnnotationScript : MonoBehaviour {

	public Vector3 gravity;
	public Vector3 vGra;
	public Vector3 v3Current;
	public bool parallel;
    private int annotype;


	public Vector3 initCamPos;
	public Vector3 initPos;

    public Material mat1;
    public Material mat2;

	public GameObject Axis;

    /*
	private float objScale = 1.0f;
	private Vector3 initScale;
    */

	// Use this for initialization
	void Start () {
		gravity = Input.gyro.gravity;
		//initScale = new Vector3 (1, 1, 1);

		SetRotation ();
	}

	private Vector3 GetGravityVector(){
		Vector3 camAngle = Camera.main.transform.eulerAngles;

		Quaternion rotation = Quaternion.Euler (camAngle);
		Vector3 grav = new Vector3 (Input.acceleration.x,Input.acceleration.y,-Input.acceleration.z);


		Vector3 rotaGrav = rotation * grav;
		return rotaGrav;
	}
	public void SetRotation(){
		

		Vector3 rotaGrav = GetGravityVector ();

	
		if (rotaGrav.y < -0 ) {

			gameObject.transform.rotation = Quaternion.LookRotation (rotaGrav, Vector3.back);


		} else {
			gameObject.transform.rotation = Quaternion.LookRotation(rotaGrav,Vector3.up);
		}
		//Vector3 cEuler = gameObject.transform.eulerAngles;
		if (parallel) {
			gameObject.transform.Rotate (new Vector3 (90, 0, 0));
					
		} else {
			gameObject.transform.Rotate (new Vector3 (0, 0, 0));

		}

	}

	public void Rotate90(bool t){
		if (t) {
			gameObject.transform.Rotate (new Vector3 (90, 0, 0));
		}
	}

	// Update is called once per frame
	void Update () {

		//SetRotation ();
	}

	public void SetState(bool state){
		parallel = state ;
	}

	public void InitCamaraPosition(Vector3 pos){
		initCamPos = pos;
		initPos = gameObject.transform.position;
	}
	public void SetPos(Vector3 p){
		initPos = p;
	}

	public Vector3 GetAnnoPos(){
		return initPos;
	}
	public Vector3 GetInitCamPos(){
		return initCamPos;
	}

	public void SetOrientation(float deg,string Axis){
		/*
		if (parallel) {
			if(Axis.Equals("X"))
			{
				gameObject.transform.Rotate (Vector3.right, deg, Space.Self);
			}
			else if(Axis.Equals("Y"))
			{
				gameObject.transform.Rotate (Vector3.up, deg, Space.Self);

			}
			else if(Axis.Equals("Z"))
			{
				gameObject.transform.Rotate (Vector3.forward, deg, Space.Self);
			}

		} else {
			if(Axis.Equals("X"))
			{
				gameObject.transform.Rotate (Vector3.right, deg, Space.Self);
			}
			else if(Axis.Equals("Y"))
			{
				gameObject.transform.Rotate (Vector3.forward, deg, Space.Self);

			}
			else if(Axis.Equals("Z"))
			{
				gameObject.transform.Rotate (Vector3.up, deg, Space.Self);
			}

		}
		*/
		Vector3 tmp = GetGravityVector ();
		switch (Axis) {

		case "X":
			gameObject.transform.RotateAround (gameObject.transform.position,tmp,deg);
			break;
		/*
		case "Y":
			gameObject.transform.RotateAround(gameObject.transform.position,Camera.main.transform.right,
				deg);
			break;*/
		case "Z":
			gameObject.transform.RotateAround(gameObject.transform.position,Camera.main.transform.forward,
				deg);
			break;
		}			

	}
	public void SetObjectScale(Vector3 scale)
	{
		gameObject.transform.localScale += scale;
	}

    public void SelectedAnnotation()
    {
		
		Axis.SetActive (true);

        ChangeMatColor(mat2);
    }
    public void DeselectedAnnotatin()
    {
		
		Axis.SetActive (false);

        ChangeMatColor(mat1);
    }
    private void ChangeMatColor(Material mat)
    {
        if (annotype > 14)
        {
            Transform c = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0);
            c.GetComponent<MeshRenderer>().material = mat;

        }
        else
        {
            Transform c = gameObject.transform.GetChild(0);
            c.GetComponent<MeshRenderer>().material = mat;
        }
        
    }
    public void SetAnnoType(int t)
    {
        annotype = t;
		Axis = gameObject.transform.GetChild (1).gameObject;

    }
    
}


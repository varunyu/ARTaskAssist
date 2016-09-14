using UnityEngine;
using System.Collections;

public class annoScript2 : MonoBehaviour {

	private int annotype;
	public Material mat1;
	public Material mat2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SelectedAnnotation()
	{
		ChangeMatColor(mat2);
	}
	public void DeselectedAnnotatin()
	{
		ChangeMatColor(mat1);
	}
	private void ChangeMatColor(Material mat)
	{
		if (annotype > 4)
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
	}
}

using UnityEngine;
using System.Collections;

public class DrawCircle : MonoBehaviour {


	public Material lineMat;
	private LineRenderer lr;

	public float ThetaScale = 0.01f;
	public float radius = 0.25f;
	private int Size;
	private float Theta = 0f;


	// Use this for initialization
	void Start () {
		lr = gameObject.AddComponent<LineRenderer>();
		lr.SetColors(Color.red, Color.red);
		lr.SetWidth(0.05F, 0.05F);
		lr.material = lineMat;
		//lr.SetVertexCount(size);
		lr.useWorldSpace = false;


	}
	
	// Update is called once per frame
	void Update () {
		Theta = 0f;
		Size = (int)((1f / ThetaScale) + 1f);
		lr.SetVertexCount(Size); 
		for(int i = 0; i < Size; i++){          
			Theta += (2.0f * Mathf.PI * ThetaScale);         
			float x = radius * Mathf.Cos(Theta);
			float y = radius * Mathf.Sin(Theta);          
			lr.SetPosition(i, new Vector3(x, y, gameObject.transform.position.z));
			
		}
	}
}

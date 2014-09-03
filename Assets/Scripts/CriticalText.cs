using UnityEngine;
using System.Collections;

public class CriticalText : MonoBehaviour {
		
	private float x = 0;
	private float y = 0;
	
	// Use this for initialization
	void Start () {
			
		//	x = Random.Range(0f,0.1f);
		//	y = Random.Range(0f,0.1f);
		//	transform.position += new Vector3(x, y, 0);
				
			x = Random.Range(-0.1f,0.1f);
			y = Random.Range(-0.01f,0.01f);
			transform.Translate (new Vector3(x,0.05f + y, 0));
				
	}
	
	void Update () {
	}
}

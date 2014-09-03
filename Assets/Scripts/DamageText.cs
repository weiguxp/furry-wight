using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {
	
	private float x = 0;
	private float y = 0;
	
	// Use this for initialization
	void Start () {
	
	x = Random.Range(-0.1f,0.1f);
	y = Random.Range(-0.01f,0.01f);
	transform.Translate (new Vector3(x,0.05f + y, 0));

	}
	
	// Update is called once per frame
	void Update () {
		
	transform.position += new Vector3(0, 0.5f*Time.deltaTime, 0);
		
	}
}

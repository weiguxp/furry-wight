using UnityEngine;
using System.Collections;

public class tempscript1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	UILabel c = GetComponent<UILabel>();
	c.text= "hello";
	}
}

using UnityEngine;
using System.Collections;



public class BuyMinion1 : MonoBehaviour {

	public GameObject Minion1CostLabel;
	public GameObject Minion1LevelLabel;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ExecuteBuy (){
		Debug.Log (CookieCore.minion1Level);
		
		//CookieCore.BuyMinion(ref CookieCore.minion1Level, ref CookieCore.minion1Cost, ref CookieCore.minion1CPS, ref Minion1LevelLabel, ref Minion1CostLabel);
	}
}

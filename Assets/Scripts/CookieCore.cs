using UnityEngine;
using System.Collections;

public class CookieCore : MonoBehaviour {

	//Stuff to Save
	private int numberCookies = 0;
	private int comboCoins = 1;
	private int cookieIncome = 0;
	public static float clickPower = 1;
	public static float criticalPower = 1;
	
	//Modified Numbers
	public static float mclickPower = 1;
	
	//Levelup Variables
	private int currentLevel = 1;
	private float playerXP = 0.00000f;
	private float nextLevelXP = 15f;
	private float dNextLevelXP = 10.0000f;
	private float levelProgress = 0;
	public GameObject levelGreenProgress;


	// Minion Upgrades
	// This part is is referred by each script in Minion Buttons
	public static int minion1Cost = 10;
	public static int minion1CPS = 20;
	public static int minion1Level = 1;
	
	private float minion2Cost = 500;
	private float minion2CPS = 20;
	private float minion3Cost = 500;
	private float minion3CPS = 20;
	private float minion4Cost = 500;
	private float minion4CPS = 20;
	private float minion5Cost = 500;
	private float minion5CPS = 20;
	private float minion6Cost = 500;
	private float minion6CPS = 20;
	private float minion7Cost = 500;
	private float minion7CPS = 20;
	
	
	//Player upgrades
	private float critChance = 35;
	private float critCombo = 0;
	
	
	//Cookie Objects
    public GameObject PlusCookie;
	public GameObject PlusCriticalCookie;
	public GameObject PlusMegaCookie;
	public GameObject CookieGlow;
	public GameObject objectCookieButton;
	public GameObject objLabelCookies;
	public GameObject objLabelCPS;
	public GameObject objPlayerGlow;
	public GameObject objPlayerIcon;
	public GameObject PlayerLevelLabel;
	public GameObject CombosCoinsLeft;
	
	//Combo some Crits
	private float comboTime = 0f;
	private float maxCombo = 4;

	// Use this for initialization
	void Start () {
		
		UpdateLevelLabel();
		UpdateComboCoins();
		InvokeRepeating("UpdateCookies",  0, 0.1F);
		InvokeRepeating("SaveGame", 0, 10F);
	}

	//This part is responsible for updating all the UI labels
	//Current used for all parts of the game that has time based considerations
	void UpdateCookies (){
		
		// This determines if the user is currently in combo time and gives a high crit chance
		if (comboTime>0){
		critChance = 75;
		comboTime -= 0.1f;
		}
		else {
		critChance = 35;
		if (CookieGlow != null) NGUITools.SetActive(CookieGlow, false);
		}
		
		
		//Updates Number of Cookies
		UILabel c = objLabelCookies.GetComponent<UILabel>();
		numberCookies = (int)(numberCookies + cookieIncome /10);
		c.text = numberCookies.ToString();
	}

	private void UpdateComboCoins(){
		UILabel cc = CombosCoinsLeft.GetComponent<UILabel>();
		cc.text = comboCoins.ToString();
	}
	
	private void UpdateLevelProgress(){
		levelProgress = playerXP  * 100f / (dNextLevelXP);
		UISprite sprite = levelGreenProgress.GetComponent<UISprite>();
		sprite.fillAmount = levelProgress/100f;
	}
	
	private void UpdateLevelLabel(){
		UILabel e = PlayerLevelLabel.GetComponent<UILabel>();
		e.text = currentLevel.ToString();
	}
	
	private void UpdateCPS(){
		//Updates Cookies per Second
		UILabel d = objLabelCPS.GetComponent<UILabel>();
		d.text = "+ " + cookieIncome.ToString();
	}
	
	public void PlayerLevelCheck (){
		// Checks if the player can level up
		
		if (playerXP > dNextLevelXP){
			//Player levels up
			AnimateLevelUp();
			
			playerXP -= dNextLevelXP;
			currentLevel ++;
			clickPower += currentLevel;
			comboCoins ++;
			
			//Determines xp required to level up.leveling up now includes the Karplus equation which distorts the xp required
			//Calls karplus distort which tells us how much xp is required for next level
			nextLevelXP = nextLevelXP *1.1f;
			dNextLevelXP = nextLevelXP * KarplusDistort(currentLevel);
			UpdateComboCoins();
			UpdateLevelLabel();
		}
	}
	
	//Various functions within the game
	//This function gives comboTime and cookieglow

	private void IssueCameraShake()
	{
		objectCookieButton.animation.Play();
	}

	public void give10ComboTime () {
		if (comboTime <= 0f){
			if (comboCoins - 1 >= 0 ){
				comboCoins --;
				UpdateComboCoins();
				comboTime = 3;
				if (CookieGlow != null) NGUITools.SetActive(CookieGlow, true);
				
			}
		}
	}
	
	private float KarplusDistort(float level){
		return (Mathf.Cos((level)*Mathf.PI/5f) + 4f* Mathf.Pow(Mathf.Cos((level+5f)*(Mathf.PI/5f)),2f)/2f+8.75f)/10f ;
		//Debug.Log(levelDistort.ToString());
	}

	public void SpawnCookies (ref GameObject cookieType, ref float displayText) {
		GameObject TextPopup = NGUITools.AddChild(objectCookieButton, cookieType);
		UILabel c = TextPopup.GetComponent<UILabel>();
		c.text = "+" + displayText.ToString();
	}


	//Click portion
	public void CookieClick (){
		
	//Determines if the cookie click is a crit and gives reward
		if (Random.Range(1F,100F) < critChance) {
			
			if (critCombo < maxCombo){critCombo ++;}
				
			criticalPower = Mathf.Round(clickPower * Mathf.Pow(Random.Range(2f,2.5f),critCombo));
			numberCookies += (int)criticalPower;
	        playerXP ++;
			
			if(comboTime>0){IssueCameraShake();};
		
			// Can spawn Mega Crit Cookies
			if (critCombo == maxCombo){
				SpawnCookies(ref PlusMegaCookie, ref criticalPower);
			}
			else{
				SpawnCookies(ref PlusCriticalCookie, ref criticalPower);
			}
        
		}
        else {
			mclickPower = Random.Range (0.85f,1.15f)*clickPower;
	        numberCookies += (int)mclickPower;
	        mclickPower = Mathf.Round(mclickPower);
			playerXP ++;
	        critCombo = 0;
			SpawnCookies(ref PlusCookie, ref mclickPower);
		}
		PlayerLevelCheck();
		UpdateLevelProgress();
		
	}

	public void BuyMinion(){


			if (numberCookies >= minion1Cost){
				numberCookies -= minion1Cost;
				minion1Cost *= (int)1.2f;
				cookieIncome += minion1CPS;
			}

		}
		

		//Animate the level up
	public void AnimateLevelUp(){
		NGUITools.AddChild(objPlayerIcon, objPlayerGlow);
	}
	
	public void SaveGame(){
		// saves
		PlayerPrefs.SetInt("numberCookies", numberCookies);
		PlayerPrefs.SetInt("comboCoins", comboCoins);
		PlayerPrefs.SetInt("cookieIncome", cookieIncome);
		PlayerPrefs.SetFloat("clickPower", clickPower);
		PlayerPrefs.SetFloat("playerXP", playerXP);
		PlayerPrefs.SetFloat("nextLevelXP", nextLevelXP);
		PlayerPrefs.SetFloat("dNextLevelXP", dNextLevelXP);
		PlayerPrefs.SetInt("currentLevel", currentLevel);
		PlayerPrefs.SetFloat("levelProgress", levelProgress);
		PlayerPrefs.SetFloat("critChance", critChance);
		PlayerPrefs.SetFloat("maxCombo", maxCombo);
		
		//upgrades
		PlayerPrefs.SetFloat("minion1Cost", minion2Cost);
		PlayerPrefs.SetFloat("minion1CPS", minion2CPS);
		PlayerPrefs.SetFloat("minion2Cost", minion2Cost);
		PlayerPrefs.SetFloat("minion2CPS", minion2CPS);
		PlayerPrefs.SetFloat("minion3Cost", minion3Cost);
		PlayerPrefs.SetFloat("minion3CPS", minion3CPS);
		PlayerPrefs.SetFloat("minion4Cost", minion4Cost);
		PlayerPrefs.SetFloat("minion4CPS", minion4CPS);
		PlayerPrefs.SetFloat("minion5Cost", minion5Cost);
		PlayerPrefs.SetFloat("minion5CPS", minion5CPS);
		PlayerPrefs.SetFloat("minion6Cost", minion6Cost);
		PlayerPrefs.SetFloat("minion6CPS", minion6CPS);
		PlayerPrefs.SetFloat("minion7Cost", minion7Cost);
		PlayerPrefs.SetFloat("minion7CPS", minion7CPS);
		
		PlayerPrefs.Save();
	}
	
	public void LoadGame(){
		numberCookies = PlayerPrefs.GetInt("numberCookies");
		comboCoins = PlayerPrefs.GetInt("comboCoins");
		cookieIncome = PlayerPrefs.GetInt("cookieIncome");
		clickPower = PlayerPrefs.GetFloat("clickPower");
		playerXP = PlayerPrefs.GetFloat("playerXP");
		nextLevelXP = PlayerPrefs.GetFloat("nextLevelXP");
		dNextLevelXP = PlayerPrefs.GetFloat("dNextLevelXP");
		currentLevel = PlayerPrefs.GetInt("currentLevel");
		levelProgress = PlayerPrefs.GetFloat("levelProgress");
		critChance = PlayerPrefs.GetFloat("critChance");
		maxCombo = PlayerPrefs.GetFloat("maxCombo");
		
		//Minions
		minion1Cost = PlayerPrefs.GetInt("minion2Cost");
		minion1CPS = PlayerPrefs.GetInt("minion2CPS");
		minion2Cost = PlayerPrefs.GetFloat("minion2Cost");
		minion2CPS = PlayerPrefs.GetFloat("minion2CPS");
		minion3Cost = PlayerPrefs.GetFloat("minion3Cost");
		minion3CPS = PlayerPrefs.GetFloat("minion3CPS");	
		minion4Cost = PlayerPrefs.GetFloat("minion4Cost");
		minion4CPS = PlayerPrefs.GetFloat("minion4CPS");	
		minion5Cost = PlayerPrefs.GetFloat("minion5Cost");
		minion5CPS = PlayerPrefs.GetFloat("minion5CPS");	
		minion6Cost = PlayerPrefs.GetFloat("minion6Cost");
		minion6CPS = PlayerPrefs.GetFloat("minion6CPS");	
		minion7Cost = PlayerPrefs.GetFloat("minion7Cost");
		minion7CPS = PlayerPrefs.GetFloat("minion7CPS");	
	}
	void Update () {
		

	}
	
	
}
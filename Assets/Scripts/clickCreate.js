#pragma strict

static var currencyCookies = 0 ;
static var cookieIncome = 0;
static var clickPower = 1;
static var clickBonus = 0;
static var clickPowerBase = 1;


//Levelup Variables
static var playerXP = 0.00000;
static var nextLevelXP = 10.0000;
static var dNextLevelXP = 10.0000;
static var currentLevel = 1;
private var levelProgress = 0;
private var levelDistort = 2.222;


// Upgrades
var grandmaCost = 10;
static var upgradeGrandma = 0;
private var critChance = 50;
private var critCombo = 0;

// Adds currencyCookies every second
function AddcurrencyCookies() {
	currencyCookies = currencyCookies + cookieIncome/10;
	clickBonus = cookieIncome;
	clickPower = clickPowerBase + clickBonus;
}

//This part adds cookies every 0.1 seconds. 
function Start () {
	InvokeRepeating("AddcurrencyCookies", 0, 0.1);
}

// Updates the GUI counter
function OnGUI() {
	
	GUI.Box(Rect(0,0,200,20), "Cookies " + currencyCookies.ToString());
	GUI.Box(Rect(0,30,200,20), cookieIncome.ToString() + " Cookies per second");
	GUI.Box(Rect(0,60,200,20), clickPower.ToString() + " Click Power");	
	GUI.Box(Rect(0,90,200,20), "Grandmas Cost " + grandmaCost.ToString());	
	GUI.Box(Rect(0,120,200,20), "Level " + currentLevel.ToString() + " Progress " + levelProgress.ToString()+ "%" );		
	GUI.Box(Rect(0,150,200,20), "Currentxp" + playerXP.ToString() + " Next level XP" + dNextLevelXP.ToString());
	GUI.Box(Rect(0,180,200,20), "hello");		 
}



// Functions
function CookieClick () {

// Determines if the player crits. Crits are expressed by critChance (/100) 
		if (Random.Range(1,100) > 50) {
		critCombo = critCombo + 1;
        currencyCookies= currencyCookies + clickPower * Mathf.Pow(2,critCombo);
        playerXP = playerXP + 1;
        }
        else {
        currencyCookies= currencyCookies + clickPower;
        playerXP = playerXP + 1;
        critCombo = 0;
        }
        
// Determines if there is enough XP to level up
	levelProgress = playerXP  * 100 / (nextLevelXP);

	if (playerXP > dNextLevelXP){
	
//Player levels up
	playerXP = playerXP - dNextLevelXP;
	currentLevel = currentLevel + 1;

//Determines xp required to level up.leveling up now includes the Karplus equation which distorts the xp required
	KarplusDistort();
	nextLevelXP = nextLevelXP *1.2;
	dNextLevelXP = nextLevelXP * levelDistort;

	}
        
}
        
        
		

function BuyGrandma () {

// Buys grandma and increases price
	if (currencyCookies - grandmaCost >= 0 ){
        currencyCookies = currencyCookies - grandmaCost;
        grandmaCost = grandmaCost * 1.2;
        cookieIncome = cookieIncome + 10 ;
    }
}

function Update () {

}

function  KarplusDistort(){
levelDistort = (Mathf.Cos((currentLevel)*Mathf.PI/5) + 4 * Mathf.Pow(Mathf.Cos((currentLevel+5)*(Mathf.PI/5)),2)/2+8.75)/10 ;
Debug.Log(levelDistort.ToString());
}
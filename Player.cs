using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    private int money = 1000;
    private int points = 0;
    private int health = 100;
    private int max_health = 100;
    float timer = 0.0f;
    private GameObject buildingSelection; //Variable representing tower selected when building
    private GameObject buildingHighlight;
    private GameObject highlightedCell;
    private GameObject towerSelection; //Variable representing tower selected on the game map
    private Cell[,] Grid;
    bool highlightActive;
    Vector3 mouseWorldPos;

    public Text moneyText;
    public Text healthText;
    myInterface script_interface;
    
    

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f; //Reset timescale on start to make sure the game isn't paused on restarting
        healthText.text = health.ToString();
        buildingSelection = null;
        highlightActive = false;
    }

    void Update(){
//      === Selecting Towers ===        
        if(highlightedCell != null && Input.GetMouseButtonDown(0)){
            int[] gridSpace = Utils.WorldPositionToGrid(highlightedCell.transform.position.x, highlightedCell.transform.position.y);
            Grid = GameObject.Find("TDmap").GetComponent<drawGrid>().getGrid();
            if(Grid[gridSpace[0], gridSpace[1]].getOccupyingGO() != null && Grid[gridSpace[0], gridSpace[1]].getOccupyingGO().tag == "Tower"){
                towerSelection = Grid[gridSpace[0], gridSpace[1]].getOccupyingGO();
                GameObject towerSelectionPanel = GameObject.Find("Tower_Selection_Panel"); //We only act on the child object but search for the parent object because the child is not always active. Parent is an empty GO serving as a way to keep an active reference to the panel
                towerSelectionPanel.transform.GetChild(0).gameObject.SetActive(true);
                //towerSelection.transform.GetChild(0).gameObject.SetActive(true);
                //print("Tower selection is "+towerSelection.name);
            }
            else if(Grid[gridSpace[0], gridSpace[1]].getOccupyingGO() != null && Grid[gridSpace[0], gridSpace[1]].getOccupyingGO().tag == "Obstacle"){
                //TODO: Offer an option to remove the obstacle?
                print("Obstacle");
            }
            else{
                towerSelection = null;
            }
        }

//      === Building Towers ===
        if(buildingSelection != null){
            if(!highlightActive){ //generate the blue "hologram" to show tower placement
                buildingHighlight = new GameObject("Build Highlight");
                buildingHighlight.transform.SetAsLastSibling();
                buildingHighlight.transform.localScale = buildingSelection.transform.GetChild(0).transform.localScale;
                var image = buildingHighlight.AddComponent(typeof(SpriteRenderer));
                image.GetComponent<SpriteRenderer>().sprite = buildingSelection.GetComponent<SpriteRenderer>().sprite;
                image.GetComponent<SpriteRenderer>().color = new Color(85f/255f, 153f/255f, 255f/255f, 255f/255f); //make hologram image blue
                highlightActive = true;
            }
            else{ //If a highlight is active but player clicks on another button, update the highlight
                buildingHighlight.GetComponent<SpriteRenderer>().sprite = buildingSelection.GetComponent<SpriteRenderer>().sprite;
                buildingHighlight.transform.localScale = buildingSelection.transform.GetChild(0).transform.localScale;
            }
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            buildingHighlight.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 1.0f);
            buildingHighlight.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Basic Tower");
            script_interface = buildingSelection.GetComponent<myInterface>();
            Grid = GameObject.Find("TDmap").GetComponent<drawGrid>().getGrid();
            if(highlightedCell != null){
                int[] gridSpace = Utils.WorldPositionToGrid(highlightedCell.transform.position.x, highlightedCell.transform.position.y);
                if(Input.GetMouseButtonDown(0) && Grid[gridSpace[0], gridSpace[1]].getOccupyingGO() == null && !Grid[gridSpace[0], gridSpace[1]].getInEnemyPath()){
                GameObject clone = Instantiate(buildingSelection, new Vector3(highlightedCell.transform.position.x, highlightedCell.transform.position.y, -0.1f), Quaternion.identity);
                Grid[gridSpace[0], gridSpace[1]].setOccupyingGO(clone);
                buildingSelection = null;
                subtractMoney(script_interface.getCost());
                highlightActive = false;
                Destroy(buildingHighlight);
                
                //Instantiate(hologram, new Vector3(mouseWorldPos.x, mouseWorldPos.y, -0.1f), Quaternion.identity);
                }
            }
            
            if(Input.GetMouseButtonDown(1)){
                buildingSelection = null;
                highlightActive = false;
                Destroy(buildingHighlight);
            }
        }

        moneyText.text = money.ToString(); //update the text in the money counter UI element
        timer+=Time.deltaTime;
        if(timer >= 1.0f){
            money += 1;
            timer = 0.0f;
        }

        if(health <= 0){
            gameOver();
        }
    }

    public void pause(){
        Time.timeScale = 0f;
    }

    public void play(){
        Time.timeScale = 1.0f;
    }

    public void Toggle2xSpeed(){
        if(Time.timeScale == 1.0f){
            GameObject.Find("Image_2x").gameObject.GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            Time.timeScale = 2.0f;
        }
        else if(Time.timeScale == 2.0f){
            GameObject.Find("Image_2x").gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Time.timeScale = 1.0f;
        }
    }

    public int getMoney(){
        return money;
    }

    public void setMoney(int val){
        money = val;
    }

    public void addMoney(int val){
        money += val;
    }

    public void subtractMoney(int val){
        money -= val;
    }

    public int getPoints(){
        return points;
    }

    public void setPoints(int val){
        points = val;
    }

    public int getMaxHealth(){
        return max_health; 
    }

    public int getHealth(){
        return health;
    }

    public void setHealth(int hp){
        health = hp;
        healthText.text = health.ToString();
    }

    public void subtractHealth(int hp){
        health -= hp;
        healthText.text = health.ToString();
    }

    public GameObject getBuildingSelection(){
        return buildingSelection;
    }

    public void setBuildingSelection(GameObject go){
        buildingSelection = go;
    }

    public void clearBuildingSelection(){
        buildingSelection = null;
    }

    public GameObject getHighlightedCell(){
        return highlightedCell;
    }

    public void setHighlightedCell(GameObject c){
        highlightedCell = c;
    }

    public GameObject getTowerSelection(){
        return towerSelection;
    }
    
    public void setTowerSelection(GameObject GO){
        towerSelection = GO;
    }

    public void sellTower(){
        int[] gridSpace = Utils.WorldPositionToGrid(towerSelection.transform.position.x, towerSelection.transform.position.y);
        money += towerSelection.GetComponent<myInterface>().getSellValue();
        Grid[gridSpace[0], gridSpace[1]].clearOccupyingGO();
        Object.Destroy(towerSelection);
        towerSelection = null;
    }

    public void upgradeTower(){
        int cost = towerSelection.GetComponent<myInterface>().getUpgradeCost();
        if(money >= cost){
            towerSelection.GetComponent<myInterface>().upgradeTower();
            money -= cost;
        }
    }

    private void drawContextAction(string str){ //function to draw a textbox and have it follow the mouse pointer, to be used when the player is deciding where to build a tower
        GUI.Label(new Rect(mouseWorldPos.x+5, mouseWorldPos.y+5, 100, 20), str);
    }

    /*Pause the game and display the Game Over screen*/
    private void gameOver(){
        Time.timeScale = 0.0f;
        int wave = GameObject.Find("TDmap").GetComponent<EnemySpawn>().getWave();
        GameObject.Find("GameOver").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("GameOver").transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "You survived to Wave "+wave;
        GameObject.Find("GameOver").transform.GetChild(1).gameObject.SetActive(true);
    }

    /*restart the game by re-building the current scene.*/
    public void restartGame(){
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        
    }

    /*Quit the game. When the game has multiple levels this should return to a main menu rather than closing the application*/
    public void quitGame(){
        Application.Quit();
    }
}
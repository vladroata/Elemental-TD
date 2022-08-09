using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infuse : MonoBehaviour
{

    public GameObject player, FireTower, WaterTower, EarthTower, AirTower, FireWaterTower, FireEarthTower, FireAirTower, WaterEarthTower, WaterAirTower, EarthAirTower, text;
    Button FireButton, WaterButton, EarthButton, AirButton;
    GameObject TowerSelection, towerToCreate, clone;
    string element;
    Player playerscript;
    myInterface towerInterface;
    int upgradeLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        playerscript = player.GetComponent<Player>();
        FireButton = transform.GetChild(0).GetComponent<Button>();
        FireButton.onClick.AddListener(Fire_click);
        WaterButton = transform.GetChild(1).GetComponent<Button>();
        WaterButton.onClick.AddListener(Water_click);
        EarthButton = transform.GetChild(2).GetComponent<Button>();
        EarthButton.onClick.AddListener(Earth_click);
        AirButton = transform.GetChild(3).GetComponent<Button>();
        AirButton.onClick.AddListener(Air_click);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerscript.getTowerSelection() != null && playerscript.getTowerSelection().GetComponent<myInterface>().getElement() != null ){
            text.SetActive(true);
            int level = playerscript.getTowerSelection().GetComponent<myInterface>().getUpgradeLevel();
            text.GetComponent<Text>().text = ""+level * 250;
            if(playerscript.getMoney() >= level*250){
                text.GetComponent<Text>().color = Color.blue;
            }
            else{
                text.GetComponent<Text>().color = Color.red;
            }
        }
        else{
            text.SetActive(false);
//            gameObject.GetComponent<Text>().text = "No Selection";
        }
    }

    void Fire_click(){
        TowerSelection = playerscript.getTowerSelection();
        towerInterface = TowerSelection.GetComponent<myInterface>();
        upgradeLevel = towerInterface.getUpgradeLevel();
        if(towerInterface.getElement() != "Fire" && towerInterface.getElement() != null && playerscript.getMoney() >= upgradeLevel*250){
            AddFire(upgradeLevel);
            playerscript.subtractMoney(upgradeLevel*250);
        }
    }
    void Water_click(){
        TowerSelection = playerscript.getTowerSelection();
        towerInterface = TowerSelection.GetComponent<myInterface>();
        upgradeLevel = towerInterface.getUpgradeLevel();
        if(towerInterface.getElement() != "Water" && towerInterface.getElement() != null && playerscript.getMoney() >= upgradeLevel*250){
            AddWater(upgradeLevel);
            playerscript.subtractMoney(upgradeLevel*250);
        }
    }
    void Earth_click(){
        TowerSelection = playerscript.getTowerSelection();
        towerInterface = TowerSelection.GetComponent<myInterface>();
        upgradeLevel = towerInterface.getUpgradeLevel();
        if(towerInterface.getElement() != "Earth" && towerInterface.getElement() != null && playerscript.getMoney() >= upgradeLevel*250){
            AddEarth(upgradeLevel);
            playerscript.subtractMoney(upgradeLevel*250);
        }
    }
    void Air_click(){
        TowerSelection = playerscript.getTowerSelection();
        towerInterface = TowerSelection.GetComponent<myInterface>();
        upgradeLevel = towerInterface.getUpgradeLevel();
        if(towerInterface.getElement() != "Air" && towerInterface.getElement() != null && playerscript.getMoney() >= upgradeLevel*250){
            AddAir(upgradeLevel);
            playerscript.subtractMoney(upgradeLevel*250);
        }
    }


    void AddFire(int upgradeLevel){
        TowerSelection = playerscript.getTowerSelection();
        element = towerInterface.getElement();
        float posX = TowerSelection.transform.position.x;
        float posY = TowerSelection.transform.position.y;
        int[] gridSpace = Utils.WorldPositionToGrid(posX, posY);
        Cell[,] Grid = GameObject.Find("TDmap").GetComponent<drawGrid>().getGrid();
        switch(element){
            case "Water":
                towerToCreate = FireWaterTower;
                break;
            case "Earth":
                towerToCreate = FireEarthTower;
                break;
            case "Air":
                towerToCreate = FireAirTower;
                break;
        }
        Grid[gridSpace[0], gridSpace[1]].clearOccupyingGO();
        Destroy(TowerSelection.gameObject);
        playerscript.setTowerSelection(null);
        clone = Instantiate(towerToCreate, new Vector3(posX, posY, -0.1f), Quaternion.identity);
        Grid[gridSpace[0], gridSpace[1]].setOccupyingGO(clone);
        for(int i = 0; i < upgradeLevel - 1; i++){
            print("upgrade");
            clone.GetComponent<myInterface>().upgradeTower();
        }
    }
    void AddWater(int upgradeLevel){
        TowerSelection = playerscript.getTowerSelection();
        element = towerInterface.getElement();
        float posX = TowerSelection.transform.position.x;
        float posY = TowerSelection.transform.position.y;
        int[] gridSpace = Utils.WorldPositionToGrid(posX, posY);
        Cell[,] Grid = GameObject.Find("TDmap").GetComponent<drawGrid>().getGrid();
        switch(element){
            case "Fire":
                towerToCreate = FireWaterTower;
                break;
            case "Earth":
                towerToCreate = WaterEarthTower;
                break;
            case "Air":
                towerToCreate = WaterAirTower;
                break;
        }
        Grid[gridSpace[0], gridSpace[1]].clearOccupyingGO();
        Destroy(TowerSelection);
        playerscript.setTowerSelection(null);
        clone = Instantiate(towerToCreate, new Vector3(posX, posY, -0.1f), Quaternion.identity);
        Grid[gridSpace[0], gridSpace[1]].setOccupyingGO(clone);
        for(int i = 0; i < upgradeLevel - 1; i++){
            clone.GetComponent<myInterface>().upgradeTower();
        }
    }
    void AddEarth(int upgradeLevel){
        TowerSelection = playerscript.getTowerSelection();
        element = towerInterface.getElement();
        float posX = TowerSelection.transform.position.x;
        float posY = TowerSelection.transform.position.y;
        int[] gridSpace = Utils.WorldPositionToGrid(posX, posY);
        Cell[,] Grid = GameObject.Find("TDmap").GetComponent<drawGrid>().getGrid();
        switch(element){
            case "Fire":
                towerToCreate = FireEarthTower;
                break;
            case "Water":
                towerToCreate = WaterEarthTower;
                break;
            case "Air":
                towerToCreate = EarthAirTower;
                break;
        }
        Grid[gridSpace[0], gridSpace[1]].clearOccupyingGO();
        Destroy(TowerSelection);
        playerscript.setTowerSelection(null);
        clone = Instantiate(towerToCreate, new Vector3(posX, posY, -0.1f), Quaternion.identity);
        Grid[gridSpace[0], gridSpace[1]].setOccupyingGO(clone);
        for(int i = 0; i < upgradeLevel - 1; i++){
            print("upgrade");
            clone.GetComponent<myInterface>().upgradeTower();
        }
    }
    void AddAir(int upgradeLevel){
        TowerSelection = playerscript.getTowerSelection();
        element = towerInterface.getElement();
        float posX = TowerSelection.transform.position.x;
        float posY = TowerSelection.transform.position.y;
        int[] gridSpace = Utils.WorldPositionToGrid(posX, posY);
        Cell[,] Grid = GameObject.Find("TDmap").GetComponent<drawGrid>().getGrid();
        switch(element){
            case "Fire":
                towerToCreate = FireAirTower;
                break;
            case "Earth":
                towerToCreate = EarthAirTower;
                break;
            case "Water":
                towerToCreate = WaterAirTower;
                break;
        }
        Grid[gridSpace[0], gridSpace[1]].clearOccupyingGO();
        Destroy(TowerSelection);
        playerscript.setTowerSelection(null);
        clone = Instantiate(towerToCreate, new Vector3(posX, posY, -0.1f), Quaternion.identity);
        Grid[gridSpace[0], gridSpace[1]].setOccupyingGO(clone);
        for(int i = 0; i < upgradeLevel - 1; i++){
            clone.GetComponent<myInterface>().upgradeTower();
        }
    }
}

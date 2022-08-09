using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower_Selection_Panel : MonoBehaviour
{    
    public GameObject player;
    GameObject towerSelection;
    myInterface towerInterface;

    Text name, level, stats, kills, priority; //Child GameObjects of *this* gameObject
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        name = transform.GetChild(0).gameObject.GetComponent<Text>();
        image = transform.GetChild(1).gameObject.GetComponent<Image>();
        level = transform.GetChild(2).gameObject.GetComponent<Text>();
        stats = transform.GetChild(3).gameObject.GetComponent<Text>();
        kills = transform.GetChild(4).gameObject.GetComponent<Text>();
        priority = transform.GetChild(5).gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        towerSelection = player.GetComponent<Player>().getTowerSelection();
        if(towerSelection == null){
            gameObject.SetActive(false);
        }
        UpdateStatus();
    }

    public void UpdateStatus(){
        /*Update the image and text within the panel to show selected tower's stats*/
        if(towerSelection != null){
            towerInterface = towerSelection.GetComponent<myInterface>();
            name.text = towerInterface.getName();
            image.sprite = towerInterface.getSprite();
            level.text = "Level: "+towerInterface.getUpgradeLevel()+"/5";
            stats.text = "Damage: "+towerInterface.getDamage()+"\n"+"Attack Speed: "+(1/towerInterface.getAttackSpeed()).ToString("0.00")+"/s\n"+"Range: "+towerInterface.getRange();
            kills.text = "Kills: "+towerInterface.getKills();
            priority.text = towerInterface.getPriorityName();
        }
    }

    public void ChangePriorityLeft(){
        int prio = towerInterface.getPriority();
        if(prio > 1){
            towerInterface.setPriority(--prio);
        }
    }

    public void ChangePriorityRight(){
        int prio = towerInterface.getPriority();
        if(prio < 6){
            towerInterface.setPriority(++prio);
        }
    }
}

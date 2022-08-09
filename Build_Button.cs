using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build_Button : MonoBehaviour
{
    public GameObject player;
    public GameObject tower;
    myInterface myinterface;
    Player playerscript;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        myinterface = tower.GetComponent<myInterface>();
        playerscript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick(){
        if(playerscript.getMoney() >= myinterface.getCost()){
            playerscript.setBuildingSelection(tower);
        }
    }
}

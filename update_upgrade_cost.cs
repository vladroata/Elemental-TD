/*Updates the sell button's cost to reflect the sell value of the selected tower*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class update_upgrade_cost : MonoBehaviour
{

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Player>().getTowerSelection() != null){
            int upgradeLevel = player.GetComponent<Player>().getTowerSelection().GetComponent<myInterface>().getUpgradeLevel();
            if(upgradeLevel == 5){
                gameObject.GetComponent<Text>().text = "MAX";
            }
            else{
                int upgrade_cost = player.GetComponent<Player>().getTowerSelection().GetComponent<myInterface>().getUpgradeCost();
                gameObject.GetComponent<Text>().text = upgrade_cost.ToString();
            }
        }
        else{
            gameObject.GetComponent<Text>().text = "No Selection";
        }
    }
}

/*Updates the sell button's cost to reflect the sell value of the selected tower*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class update_sell_cost : MonoBehaviour
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
            int moneyReturned = player.GetComponent<Player>().getTowerSelection().GetComponent<myInterface>().getSellValue();
            gameObject.GetComponent<Text>().text = moneyReturned.ToString();
            
        }
        else{
            gameObject.GetComponent<Text>().text = "No Selection";
        }
    }
}

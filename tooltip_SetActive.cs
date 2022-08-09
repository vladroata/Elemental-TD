using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tooltip_SetActive : MonoBehaviour
{
    public GameObject player, tooltip;
    Player playerscript;
    // Start is called before the first frame update
    void Start()
    {
        playerscript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateTextInfuse(){
        GameObject towerselection = playerscript.getTowerSelection();
        if(towerselection != null){
            tooltip.SetActive(true);
        }
    }
}

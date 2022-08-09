using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight_Tile : MonoBehaviour
{

    GameObject highlight;
    public GameObject player; //maintain a reference to Player object to communicate tile highlight state

    // Start is called before the first frame update
    void Start()
    {
        highlight = this.gameObject.transform.GetChild(0).gameObject;
        player = GameObject.Find("Player"); //The prefab object that this script is attached to doesn't allow for referencing Player through the Inspector.
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Player>().getHealth() <= 0){
            highlight.SetActive(false);
            Destroy(this);
        }
    }

    void OnMouseEnter(){
        highlight.SetActive(true);
        player.GetComponent<Player>().setHighlightedCell(gameObject);
    }

    void OnMouseExit(){
        highlight.SetActive(false);
        player.GetComponent<Player>().setHighlightedCell(null);
    }
}

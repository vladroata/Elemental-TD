using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tooltip_tower_description : MonoBehaviour
{
    Camera cam;
    GameObject text;
    public GameObject player;
    string txt;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        text = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf){
            Vector3 position = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 worldPosition = new Vector3(position.x, position.y, 0f);
            //transform.position = new Vector3(Mathf.Clamp(position.x, min.x, max.x - rect.rect.width), Mathf.Clamp(position.y, min.y, max.y - rect.rect.height), 0f);
            //transform.position = new Vector3(worldPosition.x-canvas.GetComponent<RectTransform>().rect.width/2 + rect.rect.width, worldPosition.y-canvas.GetComponent<RectTransform>().rect.height/2 - rect.rect.height/2, 0f);
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0f);

        }
    }

    public void updateText(string str){
        text.GetComponent<Text>().text = str;
    }

    

    public void updateTextInfuseFire(){
        Debug.Log("AAA");
        Player playerscript = player.GetComponent<Player>();
        GameObject towerselection = playerscript.getTowerSelection();
        if(towerselection != null){
            string element = towerselection.GetComponent<myInterface>().getElement();
            gameObject.SetActive(true);
            switch(element){
                case null:
                    txt = "This tower cannot infuse more elements";
                    break;
                case "Fire":
                    txt = "This element cannot be applied again";
                    break;
                case "Water":
                    txt = "Creates a tower that deals more damage to distant enemies";
                    break;
                case "Earth":
                    txt = "Creates a tower that attacks all nearby enemies";
                    break;
                case "Air":
                    txt = "Creates a tower that attacks faster the longer it spends attacking";
                    break;
            }
            updateText(txt);
        }
    }
    public void updateTextInfuseWater(){
        Player playerscript = player.GetComponent<Player>();
        GameObject towerselection = playerscript.getTowerSelection();
        if(towerselection != null){
            string element = towerselection.GetComponent<myInterface>().getElement();
            text.SetActive(true);
            switch(element){
                case null:
                    txt = "This tower cannot infuse more elements";
                    break;
                case "Water":
                    txt = "This element cannot be applied again";
                    break;
                case "Fire":
                    txt = "Creates a tower that deals more damage to distant enemies";
                    break;
                case "Earth":
                    txt = "Creates a tower that stuns enemies it hits";
                    break;
                case "Air":
                    txt = "Creates a tower that heals your health on kill";
                    break;
            }
            updateText(txt);
        }
    }
    public void updateTextInfuseEarth(){
        Player playerscript = player.GetComponent<Player>();
        GameObject towerselection = playerscript.getTowerSelection();
        if(towerselection != null){
            string element = towerselection.GetComponent<myInterface>().getElement();
            text.SetActive(true);
            switch(element){
                case null:
                    txt = "This tower cannot infuse more elements";
                    break;
                case "Earth":
                    txt = "This element cannot be applied again";
                    break;
                case "Water":
                    txt = "Creates a tower that stuns enemies it hits";
                    break;
                case "Fire":
                    txt = "Creates a tower that attacks all nearby enemies";
                    break;
                case "Air":
                    txt = "Creates a tower that sometimes temporarily reverses enemies' direction on hit";
                    break;
            }
            updateText(txt);
        }
    }
    public void updateTextInfuseAir(){
        Player playerscript = player.GetComponent<Player>();
        GameObject towerselection = playerscript.getTowerSelection();
        if(towerselection != null){
            string element = towerselection.GetComponent<myInterface>().getElement();
            text.SetActive(true);
            switch(element){
                case null:
                    txt = "This tower cannot infuse more elements";
                    break;
                case "Air":
                    txt = "This element cannot be applied again";
                    break;
                case "Water":
                    txt = "Creates a tower that heals your health on kill";
                    break;
                case "Earth":
                    txt = "Creates a tower that sometimes temporarily reverses enemies' direction on hit";
                    break;
                case "Fire":
                    txt = "Creates a tower that attacks faster the longer it spends attacking";
                    break;
            }
            updateText(txt);
        }
    }
}

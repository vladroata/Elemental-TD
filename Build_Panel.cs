using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build_Panel : MonoBehaviour
{
    public GameObject Player;
    public GameObject Fire_Tower;
    public GameObject Water_Tower;
    public GameObject AOE_Tower;
    public GameObject Buff_Tower;
    myInterface script;
    AOE_DMG_Tower AoeScript;
    Buff_Tower BuffScript;
    Text Fire_Tower_Text;
    Text Water_Tower_Text;
    Text AOE_Tower_Text;
    Text Buff_Tower_Text;
    List<Text> textList = new List<Text>();

    // Start is called before the first frame update
    void Start()
    {
        //Set tower cost texts to match actual tower cost. Add the text objects to a list.
        script = Fire_Tower.GetComponent<myInterface>();
        Fire_Tower_Text = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        Fire_Tower_Text.text = script.getCost().ToString();
        script = Water_Tower.GetComponent<myInterface>();
        Water_Tower_Text = this.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
        Water_Tower_Text.text = script.getCost().ToString();
        AoeScript = AOE_Tower.GetComponent<AOE_DMG_Tower>();
        AOE_Tower_Text = this.gameObject.transform.GetChild(2).gameObject.GetComponent<Text>();
        AOE_Tower_Text.text = AoeScript.getCost().ToString();
        BuffScript = Buff_Tower.GetComponent<Buff_Tower>();
        Buff_Tower_Text = this.gameObject.transform.GetChild(3).gameObject.GetComponent<Text>();
        Buff_Tower_Text.text = BuffScript.getCost().ToString();
        textList.Add(Fire_Tower_Text);
        textList.Add(Water_Tower_Text);
        textList.Add(AOE_Tower_Text);
        textList.Add(Buff_Tower_Text);
    }

    // Update is called once per frame
    void Update()
    {
        //Iterate through text objects and update text colors to indicate which towers the player can afford
        foreach(Text t in textList){
            if(Player.GetComponent<Player>().getMoney() >= int.Parse(t.text)){
                t.color = Color.blue;
            }
            else{
                t.color = Color.red;
            }
        }
    }

    void OnEnable(){
        
    }
}

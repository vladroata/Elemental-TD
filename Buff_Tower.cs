using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Tower : MonoBehaviour, myInterface
{
    string name = "Air Tower";
    string element = "Air";
    private int damage = 0;
    private int baseDamage;
    private int cost = 200;
    private bool attackReady;
    private float range = 2.0f;
    private float attackTimer = 2.0f;
    private int kills = 0;
    private int priority;
    private int upgradeLevel = 1;
    private int[] upgrade_damage = {115, 135, 150, 175};
    private float[] upgrade_range = {2.1f, 2.2f, 2.3f, 2.5f};
    private int upgrade_cost = 0;
    private int sellValue = 0;

    float timer = 0.0f;
    float animationTimer = 0.0f;
    int animationFrame = 0;
    List<Collider2D> results = new List<Collider2D>();
    CircleCollider2D col2d;
    SpriteRenderer sr;
    ContactFilter2D contactFilter;
    List<GameObject> targets = new List<GameObject>();
    public Sprite[] sprites;
    public GameObject bullet;
    public GameObject player;

    private readonly Dictionary<ScriptableBuff, TimedBuff> _buffs = new Dictionary<ScriptableBuff, TimedBuff>();
    public ScriptableBuff buff;


    // Start is called before the first frame update
    void Start()
    {
        baseDamage = damage;
        sr = gameObject.GetComponent<SpriteRenderer>();
        col2d = gameObject.GetComponent<CircleCollider2D>();
        col2d.radius = range/gameObject.transform.localScale.x; //divide range by object's scale X variable, scale will affect the collider's size and we want it to match the range
        contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        contactFilter.useLayerMask = true;
        LayerMask mask = LayerMask.GetMask("Ignore Raycast");
        contactFilter.layerMask = mask;
        attackReady = true;
        priority = 1;
        sellValue = cost/2;
        
        upgrade_cost = cost + upgradeLevel*75;
        player = GameObject.Find("Player");
    }

    void Update() {
        if(player.GetComponent<Player>().getTowerSelection() == gameObject){
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else{
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

       /* animationTimer += Time.deltaTime;
        if (animationTimer >= 0.1f) {
            animate();
            animationTimer = 0;
        }*/
        
        // if(targets == null || targets.Count == 0){
        //     targets = getTargets(); 
        //     //print("new target is "+target.GetComponent<enemyInterface>().enemyNum);
        // }  

        targets = getTargets();
        
        if (attackReady == false) {
            timer += Time.deltaTime;
            if (timer >= attackTimer)
            {
                attackReady = true;
                targets = getTargets(); //Update the target right before attacking for more robust target acquisition. (Targets may move, be damaged, etc. between target acquisition and moment of attack leading to them no longer being the best target)
                timer = 0.0f;
            }
        }
        if (targets.Count != 0) {
            if (attackReady) {
                attack(targets);
            }
        }
        
    }

    /*void animate() { //cycle through tower images to "animate" it
        if (animationFrame < sprites.Length - 1)
        {
            sr.sprite = sprites[animationFrame + 1];
            animationFrame++;
        }
        else {
            animationFrame = 0;
        }
    }*/

    void attack(List<GameObject> targets) { //For this tower, the attack() method will dispense buffs to other towers
        foreach(GameObject target in targets){
            ScriptableBuff newbuff = Object.Instantiate(buff); //Instantiate new instances of ScriptableBuff to allow the buff to affect multiple unique targets
            target.GetComponent<myInterface>().AddBuff(buff.InitializeBuff(target));
        }
        attackReady = false; 
    }

    List<GameObject> getTargets() { //For this tower, targets are other towers
        List<GameObject> towers = new List<GameObject>();
        int overlapCount = col2d.OverlapCollider(contactFilter, results);
        if (overlapCount > 0) {
            foreach (Collider2D col in results)
            {
                GameObject obj = col.GetComponent<BoxCollider2D>().gameObject;
                if(obj != this.gameObject){
                    towers.Add(obj);
                }
            }
        }
        return towers;
    }

    public void upgradeTower(){
        if(upgradeLevel < 5){
            upgradeLevel++;
            damage = upgrade_damage[upgradeLevel-2];
            range = upgrade_range[upgradeLevel-2];
            sellValue += upgrade_cost/2;
            upgrade_cost += upgradeLevel * 50;
            col2d.radius = range/gameObject.transform.localScale.x; //update collider circle radius
            transform.GetChild(0).gameObject.SetActive(false); //toggle range ring off/on to re-draw ring
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    float distanceToTarget(GameObject target){ //find distance to the center of the target
        float distance = Vector2.Distance(this.transform.position, target.transform.position);
        return distance;
    }

    public int getCost(){
        return cost;
    }
    
    public void setCost(int x){
        cost = x;
    }

    public float getXpos(){
        return transform.position.x;
    }

    public float getYpos(){
        return transform.position.y;
    }

    public float getRange(){
        return range;
    }

    public int getUpgradeLevel(){
        return upgradeLevel;
    }

    public int getUpgradeCost(){
        return upgrade_cost;
    }

    public int getSellValue(){
        return sellValue;
    }

    public int getKills(){
        return kills;
    }

    public float getAttackSpeed(){
        return attackTimer;
    }

    public int getDamage(){
        return damage;
    }

    public int getBaseDamage(){
        return baseDamage;
    }

    public void setDamage(int x){
        damage = x;
    }

    public Sprite getSprite(){
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public string getName(){
        return name;
    }

    public int getPriority(){
        return priority;
    }

    public void setPriority(int x){
        priority = x;
    }

    public string getElement(){
        return element;
    }

    public string getPriorityName(){
        switch(priority){
            default:
                return "N/A";
                break;
        }
    }

    public void AddBuff(TimedBuff buff)
    {
        if (_buffs.ContainsKey(buff.Buff))
        {
            _buffs[buff.Buff].Activate();
        }
        else
        {
            _buffs.Add(buff.Buff, buff);
            buff.Activate();
        }
    }
}

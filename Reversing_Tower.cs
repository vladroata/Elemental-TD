using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Reversing_Tower : MonoBehaviour, myInterface
{
    string name = "Reversing Tower";
    private int damage = 50;
    private int baseDamage;
    private int cost = 290;
    private bool attackReady;
    private int priority;
    private float range = 1.25f;
    private float attackTimer = 1.3f;
    private int kills = 0;

    private int upgradeLevel = 1;
    private int[] upgrade_damage = {65, 80, 95, 110};
    private float[] upgrade_range = {1.35f, 1.45f, 1.55f, 1.65f};
    private int upgrade_cost = 0; 
    private int sellValue = 0;
    
    float timer = 0.0f;
    float animationTimer = 0.0f;
    int animationFrame = 0;
    List<Collider2D> results = new List<Collider2D>();
    CircleCollider2D col2d;
    SpriteRenderer sr;
    ContactFilter2D contactFilter;
    GameObject target;
    public GameObject bullet;
    public Sprite[] sprites;

    GameObject player;

    private readonly Dictionary<ScriptableBuff, TimedBuff> _buffs = new Dictionary<ScriptableBuff, TimedBuff>();
    public ScriptableBuff reverseDebuff;
    


    // Start is called before the first frame update
    void Start()
    {
        baseDamage = damage;
        sr = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        col2d = gameObject.GetComponent<CircleCollider2D>();
        col2d.radius = range/gameObject.transform.localScale.x;
        contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;
        contactFilter.useLayerMask = true;
        LayerMask mask = LayerMask.GetMask("Enemies");
        contactFilter.layerMask = mask;
        attackReady = true;
        priority = 1;
        sellValue = cost/2;
        upgrade_cost = cost + upgradeLevel*50;
        player = GameObject.Find("Player");
    }

    void Update() {
        if(player.GetComponent<Player>().getTowerSelection() == gameObject){
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else{
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        animationTimer += Time.deltaTime;
        if (animationTimer >= 0.1f) {
            animate();
            animationTimer = 0;
        }
        
        if(target == null || (target != null && (distanceToTarget(target) > range && !col2d.IsTouching(target.GetComponent<BoxCollider2D>())))){
            target = getTarget(priority); 
            //print("new target is "+target.GetComponent<enemyInterface>().enemyNum);
        }
        
        if (attackReady == false) {
            timer += Time.deltaTime;
            if (timer >= attackTimer)
            {
                attackReady = true;
                target = getTarget(priority);
                timer = 0.0f;
            }
        }
        if (target != null) {
            Collider2D tar_Collider = target.GetComponent<BoxCollider2D>(); 
            if (attackReady && (distanceToTarget(target) <= range || col2d.IsTouching(tar_Collider))) {
                attack(target);
            }
        }

        //Tick buffs
        foreach (var buff in _buffs.Values.ToList())
        {
            buff.Tick(Time.deltaTime);
            if (buff.IsFinished)
            {
                _buffs.Remove(buff.Buff);
            }
        }
    }

    void animate() { //cycle through tower images to "animate" it
        if (animationFrame < sprites.Length - 1)
        {
            sr.sprite = sprites[animationFrame + 1];
            animationFrame++;
        }
        else {
            animationFrame = 0;
        }
    }

    void attack(GameObject target) {
        gameObject.InstantiateBullet(bullet, target, transform.position);
        if(target.GetComponent<enemyInterface>().getHealth() <= damage){
            kills++;
        }
        target.GetComponent<enemyInterface>().setHealth(target.GetComponent<enemyInterface>().getHealth()-damage);
        target.GetComponent<enemyInterface>().AddBuff(reverseDebuff.InitializeBuff(target));
        attackReady = false;
    }

    GameObject getTarget(int prio) {
        float xpos = gameObject.transform.position.x;
        float ypos = gameObject.transform.position.y;
        Vector2 position = transform.position;
        
        List<GameObject> enemies = new List<GameObject>();

        int overlapCount = col2d.OverlapCollider(contactFilter, results);
        if (overlapCount > 0) {
            foreach (Collider2D col in results)
            {
                GameObject obj = col.GetComponent<Collider2D>().gameObject;
                enemies.Add(obj);
            }

            if (prio == 1) { //closest
                GameObject closest = null;
                float distance = Mathf.Infinity;
                foreach (GameObject enemy in enemies)
                {
                    float curDistance = Vector2.Distance(this.transform.position, enemy.transform.position);
                    if (curDistance < distance)
                    {
                        closest = enemy;
                        distance = curDistance;
                    }
                }
                return closest;
            }
            else if(prio == 2){ //farthest
                GameObject farthest = null;
                float distance = 0f;
                foreach (GameObject enemy in enemies)
                {
                    float curDistance = Vector2.Distance(this.transform.position, enemy.transform.position);
                    if (curDistance > distance)
                    {
                        farthest = enemy;
                        distance = curDistance;
                    }
                }
                return farthest;
            }
            else if(prio == 3){ //First
                GameObject first = null;
                float distance = Mathf.Infinity;
                foreach (GameObject enemy in enemies)
                {
                    float curDistance = enemy.GetComponent<enemyInterface>().getRemainingDistance();
                    if (curDistance < distance)
                    {
                        first = enemy;
                        distance = curDistance;
                    }
                }
                return first;
            }
            else if(prio == 4){ //Last
                GameObject last = null;
                float distance = 0f;
                foreach (GameObject enemy in enemies)
                {
                    float curDistance = enemy.GetComponent<enemyInterface>().getRemainingDistance();
                    if (curDistance > distance)
                    {
                        last = enemy;
                        distance = curDistance;
                    }
                }
                return last;
            }
            else if(prio == 5){ //Weak
                GameObject weakest = null;
                float health = Mathf.Infinity;
                foreach (GameObject enemy in enemies)
                {
                    float curHealth = enemy.GetComponent<enemyInterface>().getHealth();
                    if (curHealth < health)
                    {
                        weakest = enemy;
                        health = curHealth;
                    }
                }
                return weakest;
            }
            else if(prio == 6){ //Strong
                GameObject strongest = null;
                float health = 0;
                foreach (GameObject enemy in enemies)
                {
                    float curHealth = enemy.GetComponent<enemyInterface>().getHealth();
                    if (curHealth > health)
                    {
                        strongest = enemy;
                        health = curHealth;
                    }
                }
                return strongest;
            }
        }
       
        enemies.Clear();
        results.Clear();
        return null;
    }

// 150 + 0, 150 + 50, 150 + 150, 150 + 300, 150 + 500
// 150, 200, 300, 450, 650
    public void upgradeTower(){
        if(upgradeLevel < 5){
            upgradeLevel++;
            damage = upgrade_damage[upgradeLevel-2];
            baseDamage = upgrade_damage[upgradeLevel-2];
            range = upgrade_range[upgradeLevel-2];
            sellValue += upgrade_cost/2;
            upgrade_cost += upgradeLevel * 50;
            col2d.radius = range; //update collider circle radius
            transform.GetChild(1).gameObject.SetActive(false); //toggle range ring off/on to re-draw ring
            transform.GetChild(1).gameObject.SetActive(true);
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
        return gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
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
        return null;
    }

    public string getPriorityName(){
        switch(priority){
            case 1:
                return "Closest";
                break;
            case 2:
                return "Farthest";
                break;
            case 3:
                return "First";
                break;
            case 4:
                return "Last";
                break;
            case 5: 
                return "Weak";
                break;
            case 6:
                return "Strong";
                break;
            default:
                return "Priority Error";
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

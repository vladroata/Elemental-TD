using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class EnemyScript : MonoBehaviour, enemyInterface
{
    public int max_health;
    [SerializeField]
    private int health;
    [SerializeField]
    private float speed = 1.0f;

    public int numWaypoints;
    public int enemyNum;
    
    float[][] waypoints; //set of x-y coordinates representing destinations for the enemy to reach by walking to them.
    int waypoints_step = 0; //this value represents index in waypoints[] and is incremented when the current destination is reached

    [SerializeField]
    float destinationX;
    [SerializeField]
    float destinationY;

    GameObject Player;
    public GameObject TdMap;

    SpriteRenderer sr;
    public Sprite[] sprites_walk;
    public Sprite[] sprites_die;
    float animationTimer = 0.0f;
    int walk_animationFrame = 0;
    int die_animationFrame = 0;
    bool readyToDestroy = false;
    bool isWalkingBackward = false;
    Collider2D col;
    Component slider;

    private readonly Dictionary<ScriptableBuff, TimedBuff> _buffs = new Dictionary<ScriptableBuff, TimedBuff>();
    

    // Start is called before the first frame update
    void Start()
    {
        TdMap = GameObject.Find("TDmap");
        TdMap.GetComponent<EnemySpawn>().enemiesAlive += 1;
        health = max_health;
        Player = GameObject.Find("Player");
        sr = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        slider = gameObject.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        float[][] temp = GameObject.FindWithTag("Map").GetComponent<EnemySpawn>().getWaypoints();
        int dimension = temp.Length;
        waypoints = new float[dimension][];
        for(int i = 0; i < dimension; i++){
            waypoints[i] = new float[temp[i].Length];
        }
        //setWaypoints(GameObject.FindWithTag("Map").GetComponent<EnemySpawn>().getWaypoints()); 
        copyWaypoints(waypoints, GameObject.FindWithTag("Map").GetComponent<EnemySpawn>().getWaypoints());
        //printWaypoints(static_waypoints);
        //printWaypoints(waypoints);
        destinationX = waypoints[waypoints_step][0];
        destinationY = waypoints[waypoints_step][1];
        setNumWaypoints(waypoints.GetLength(0));
    }

    void Update() {
        if(health < max_health){
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).GetChild(0).GetComponent<Slider>().value = ((float)health/(float)max_health);
        }
        if (health <= 0) {
            //col.enabled = false;
            animationTimer += Time.deltaTime;
            if (animationTimer >= 0.4f) {
                animateDeath();
                animationTimer = 0;
            }
            if(readyToDestroy){
                GameObject.Find("Player").GetComponent<Player>().addMoney(1); //Give money to the player on enemy's death
                Destroy(gameObject);
            }
        }
        else{
            walkWaypoints();
            animationTimer += Time.deltaTime;
            if (animationTimer >= 0.3f) {
                animateWalk();
                animationTimer = 0.0f;
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

    void OnDestroy(){
        TdMap.GetComponent<EnemySpawn>().enemiesAlive -= 1;
    }

    public void walkWaypoints() { //Make the enemy move
        float currentX = gameObject.transform.position.x;
        float currentY = gameObject.transform.position.y;
        if (currentX >= destinationX-0.2 && currentX <= destinationX+0.2 && currentY >= destinationY - 0.2 && currentY <= destinationY + 0.2) { //buffers of 0.2 allow the enemies to start moving in another direction sooner, so they take turns more smoothly
            if(isWalkingBackward && waypoints_step == numWaypoints - 1){
                speed = 0.0f;
            }
            else if (waypoints_step == numWaypoints - 1){ //Enemy has reached the end
                Player.GetComponent<Player>().subtractHealth(10);
                Destroy(gameObject);
            }
            else {
                waypoints_step++; //NOTE: waypoints_step == 1 immediately when the enemy spawns because they are spawned on the first waypoint and are immediately given a new destination to the second waypoint
                destinationX = waypoints[waypoints_step][0];
                destinationY = waypoints[waypoints_step][1];
            }
        }
        if (currentX <= destinationX) {
            transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
            currentX += speed * Time.deltaTime;
        }
        if (currentX >= destinationX) {
            transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
            currentX -= speed * Time.deltaTime;
        }
        if (currentY <= destinationY){
            transform.Translate(new Vector3(0, speed, 0) * Time.deltaTime);
            currentY += speed * Time.deltaTime;
        }
        if (currentY >= destinationY){
            transform.Translate(new Vector3(0, -speed, 0) * Time.deltaTime);
            currentY -= speed * Time.deltaTime;
        }
    }

    public void walkReverse(){ //reverse the enemy's waypoint list and update it's direction
        float[][] reverseWaypoints = reverseArray(waypoints); //NOTE: This reverses the original waypoints array too
        waypoints = reverseWaypoints; 
        //Need to update destinationX and destinationY to set the enemy on the right course
        //Destination index = upperbound - index + 1
        int index = waypoints_step;
        int upperbound = waypoints.GetUpperBound(0);
        print(upperbound);
        int destinationIndex = upperbound - index + 1;
        destinationX = waypoints[destinationIndex][0];
        destinationY = waypoints[destinationIndex][1];
        waypoints_step = destinationIndex;
        isWalkingBackward = true;
        if(speed == 0.0f){
            speed = 1.0f;
        }
    }
    public float[][] reverseArray(float[][] array){ //helper function to reverse the list of waypoints
        float[][] newarray = new float[array.Length][];
        //copyWaypoints[newarray, array];
        for(int i = 0; i<array.Length/2; i++){
            float[] temp = array[array.GetUpperBound(0)-i];
            array[array.GetUpperBound(0)-i] = array[i];
            array[i] = temp;
        }
        return array;
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

    void animateWalk() { //cycle through images to "animate" it
        if (walk_animationFrame == sprites_walk.Length)
        {
            walk_animationFrame = 0;
        }
        sr.sprite = sprites_walk[walk_animationFrame];
        walk_animationFrame++;
    }
    void animateDeath() { //cycle through images to "animate" it
        if (die_animationFrame < sprites_die.Length)
        {
            sr.sprite = sprites_die[die_animationFrame];
            die_animationFrame++;
            if(die_animationFrame == sprites_die.Length){
                readyToDestroy = true;
            }
        }
    }

    public int getHealth() {
        return health;
    }

    public void setHealth(int x) {
        health = x;
    }

    public float getSpeed(){
        return speed;
    }

    public void setSpeed(float x){
        speed = x;
    }

    public int getEnemyNum() {
        return enemyNum;
    }

    public void setEnemyNum(int x) {
        enemyNum = x;
    }

    public void setNumWaypoints(int x) {
        numWaypoints = x;
    }

    public float[][] getWaypoints() {
        return waypoints;
    }
    public void setWaypoints(float[][] newWaypoints) {
        waypoints = newWaypoints;
    }

    public void copyWaypoints(float[][] destination, float[][] source){
        for(int i = 0; i<source.Length; i++){
            for(int j = 0; j<source[i].Length; j++){
                destination[i][j] = source[i][j];
            }
        }
    }

    public void printWaypoints(float[][] waypoints){
        string output = "";
        for (int n = 0; n < waypoints.Length; n++) {
            for (int k = 0; k < waypoints[n].Length; k++) {
                output += ""+waypoints[n][k]+", ";
            }
            output+= " | ";
        }
        print(output);
    }    

    public float getRemainingDistance(){ //Gets the total remaining distance from the enemy's current position to the end of the track
        float currentX = gameObject.transform.position.x;
        float currentY = gameObject.transform.position.y;
        float totalDistance = Math.Abs(destinationX-currentX)+Math.Abs(destinationY-currentY);    //Get distance from current position to next waypoint
        int step = waypoints_step;    //We don't need to increment value of step because enemies go from step 0 to step 1 immediately when they are spawned. Therefore, the value of step is equal to the index of the waypoint the enemy is currently moving to.
        for(int i = step; i<waypoints.GetLength(0)-1; i++){   //Keep adding the distance from the current waypoint to the next, until we run out of waypoints. If this method was called when the enemy was already on the way to the last waypoint, this loop should not run
            totalDistance += Math.Abs(waypoints[i+1][0] - waypoints[i][0]) + Math.Abs(waypoints[i+1][1] - waypoints[i][1]);
        }
        return totalDistance;
    }
}

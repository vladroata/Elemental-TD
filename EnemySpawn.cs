using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemySpawn : MonoBehaviour
{
    
    public GameObject enemy_soldier, enemy_fast, enemy_tank, enemy_flying;
    public GameObject[] enemy_list;
    public GameObject canvas;
    public GameObject switchScene;
    int random; //define a variable to store a random integer, to be used in spawning enemies
    int random_max = 0; //a variable to set an upper-limit to how large the random value can be

    int numEnemies = 10; //number of enemies to spawn this wave
    int enemiesSpawned = 0;
    public int enemiesAlive = 0;
    private float[][] waypoints = null;

    int wave = 1;
    float timer = 0.0f;
    float waveDelayTimer = 0.0f;

    float textHighlightTimer = 0.0f;
    public GameObject wave_counter_text;
    Text text;
    int text_isBeingDrawn = 0;
    Color text_color;
    float text_alpha = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //waypoints = new float[,] { { 5.5f, 3.5f }, { 5.5f, -0.5f }, { -5.5f, -0.5f }, { -5.5f, -3.5f } };
        //waypoints = new float[,] {{-1.5f, 4.5f}, {-1.5f, 1.5f}, {-8.5f, 1.5f}, {-8.5f, 3.5f}, {-6.5f, 3.5f}, {-6.5f, -3.5f}, {-8.5f, -3.5f}, {-8.5f, -1.5f}, {8.5f, -1.5f}, {8.5f, -3.5f}, {6.5f, -3.5f}, {6.5f, 3.5f}, {8.5f, 3.5f}, {8.5f, 1.5f}, {1.5f, 1.5f}, {1.5f, 4.5f}};
        //waypoints = new float[,] { { -8.5f, 4.5f }, { 4.5f, 4.5f }, { 4.5f, -4.5f }, { -3.5f, -4.5f }, {-3.5f, 0.5f} };

        if(switchScene.GetComponent<switchScene>().isUsingRandomMap() == true){
            waypoints = this.GetComponent<drawGrid>().generateRandomPath();
        }
        else{
            waypoints = switchScene.GetComponent<switchScene>().getWaypoints();
        }
        
        //enemy_list = new GameObject[]{enemy_soldier, enemy_fast, enemy_tank, enemy_flying};
        enemy_list = new GameObject[]{enemy_soldier, enemy_fast, enemy_tank, enemy_flying};
        text = wave_counter_text.GetComponent<Text>();
        text_color = new Color(1.0f, 1.0f, 1.0f, text_alpha);
        drawText(wave);
    }

    // Update is called once per frame
    void Update()
    {
        //printWaypoints(waypoints);
//      === Enemy Spawning ===
        timer += Time.deltaTime;
        if (timer >= 1.0f - wave*0.01f) {
            
            if (enemiesSpawned < numEnemies && text_isBeingDrawn == 0) { //keep spawning enemies until the target # is hit
                spawnEnemy(waypoints, enemy_list);
                timer = 0.0f;
            }
        }
        if(enemiesSpawned == numEnemies && enemiesAlive == 0){ //Wave is over, reset variables and start a new wave
            wave++;
            drawText(wave);
            if(wave % 5 == 0){ //every 5 waves, allow a new enemy to be spawned
                if(random_max < enemy_list.Length){
                    random_max++;
                }
            }
            numEnemies += wave*2;
            enemiesSpawned = 0;
        }

//      === Drawing Wave Counter ===
        if(text_isBeingDrawn == 1){ //alpha value of text is scaling up
            text.color = text_color;
            text_color.a += 0.01f;
            if(text.color.a >= 1.0f){
                textHighlightTimer += Time.deltaTime;
                if(textHighlightTimer >= 1.0f){
                    text_isBeingDrawn = 2;
                    textHighlightTimer = 0.0f;
                }
            }
        }
        if(text_isBeingDrawn == 2){ //alpha value of text is scaling down
            text.color = text_color;
            text_color.a -= 0.01f;
            if(text.color.a <= 0.0f){
                text_isBeingDrawn = 0;
                text.enabled = false;
            }
        }
    }

    void drawText(int wave){
        text.text = "Wave "+wave.ToString();
        text.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        text.enabled = true;
        text_isBeingDrawn = 1;
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

    void spawnEnemy(float[][] waypoints, GameObject[] enemylist) {
        random = UnityEngine.Random.Range(0, random_max+1); //Random.Range()'s second parameter is exclusive, so add 1 
        GameObject enemyToSpawn = enemylist[random];
        float[][] copy = new float[waypoints.Length][];
        copyWaypoints(copy, waypoints);
        gameObject.Instantiate(enemyToSpawn, canvas, enemiesSpawned, copy, Quaternion.identity); //use of extension method
        enemiesSpawned++;
    }

    public void copyWaypoints(float[][] destination, float[][] source){
        for(int i = 0; i<source.Length; i++){
            destination[i] = source[i];
        }
    }
    

    public int getWave(){
        return wave;
    }

    public float[][] getWaypoints(){
        return waypoints;
    }

    
}

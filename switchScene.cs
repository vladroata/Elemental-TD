using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour
{
    //Using jagged array to assign a slice (subset) of the array to waypoints
    //They can be mixed with multidimensional arrays as below
    public float[][][] list_of_waypoints = new float[][][]
    {
        new float[][] {new float[] { 5.5f, 3.5f }, new float[] { 5.5f, -0.5f }, new float[] { -5.5f, -0.5f }, new float[] { -5.5f, -3.5f } },
        new float[][] {new float[] {-1.5f, 4.5f}, new float[] {-1.5f, 1.5f}, new float[] {-8.5f, 1.5f}, new float[] {-8.5f, 3.5f}, new float[] {-6.5f, 3.5f}, new float[] {-6.5f, -3.5f}, new float[] {-8.5f, -3.5f}, new float[] {-8.5f, -1.5f}, new float[] {8.5f, -1.5f}, new float[] {8.5f, -3.5f}, new float[] {6.5f, -3.5f}, new float[] {6.5f, 3.5f}, new float[] {8.5f, 3.5f}, new float[] {8.5f, 1.5f}, new float[] {1.5f, 1.5f}, new float[] {1.5f, 4.5f}},
        new float[][] {new float[] {-7.5f, 3.5f}, new float[] {-4.5f, 3.5f}, new float[] {-4.5f, -3.5f}, new float[] {-1.5f, -3.5f}, new float[] {-1.5f, 3.5f}, new float[] {1.5f, 3.5f}, new float[] {1.5f, -3.5f}, new float[] {4.5f, -3.5f}, new float[] {4.5f, 3.5f}, new float[] {7.5f, 3.5f}},
        new float[][] {new float[] {-4.5f, 4.5f}, new float[] {4.5f, 4.5f}, new float[] {4.5f, -4.5f}, new float[] {-4.5f, -4.5f}, new float[] {-4.5f, 2.5f}, new float[] {2.5f, 2.5f}, new float[] {2.5f, -2.5f}, new float[] {-2.5f, -2.5f}, new float[] {-2.5f, 0.5f}, new float[] {0.5f, 0.5f}, new float[] {0.5f, -0.5f}, new float[] {-0.5f, -0.5f}}
    };
    public static float[][] waypoints;
    public static bool usingRandomMap = false;
    public static int size = 5;

    // public static switchScene Instance;

    //void Awake(){
    //    waypoints = list_of_waypoints[2];
   // }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //printWaypoints(waypoints);
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

    public float[][] getWaypoints(){
        return waypoints;
    }

    public bool isUsingRandomMap(){
        return usingRandomMap;
    }

    public void toMapSelection(){
        SceneManager.LoadScene(sceneBuildIndex:1);
    }

    public void loadMap(int x){ //loads a map by passing the array of waypoints to the next scene
        waypoints = list_of_waypoints[x];
        SceneManager.LoadScene(sceneBuildIndex:2);
    }

    public void loadRandomMap(){
        usingRandomMap = true;
        SceneManager.LoadScene(sceneBuildIndex:2);
    }

    public void loadInstructions(){
        SceneManager.LoadScene(sceneBuildIndex:3);
    }

    public void backToMainMenu(){
        SceneManager.LoadScene(sceneBuildIndex:0);
    }

    public void quitGame(){
        Application.Quit();
    }
}

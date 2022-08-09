using UnityEngine;
using UnityEngine.UI;

//Extension methods can be used to add functionality to classes we don't normally have access to, such as Object.
//In this case, we're adding an Instantiate static method to allow us to feed parameters to the prefab objects we are spawning.
public static class ExtensionMethod
{

    public static Object Instantiate(this Object thisObj, Object prefab, GameObject canvas, int enemyNum, float[][] waypoints, Quaternion rotation)
    {
        GameObject enemy = Object.Instantiate(prefab, new Vector3(waypoints[0][0], waypoints[0][1], -0.1f), Quaternion.identity) as GameObject;
        enemy.GetComponent<enemyInterface>().setEnemyNum(enemyNum);
        enemy.GetComponent<enemyInterface>().setWaypoints(waypoints);
        //enemy.transform.SetParent(canvas.transform);
        enemy.SetActive(true);
        return enemy;
    }

    public static Object InstantiateBullet(this Object thisObj, Object prefab, GameObject target, Vector3 position){
        GameObject bullet = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
        bullet.GetComponent<bullet>().setTarget(target);
        return bullet;
    }
}
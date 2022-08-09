using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface enemyInterface{
    void walkWaypoints();
    void walkReverse();
    float[][] reverseArray(float[][] array);
    void AddBuff(TimedBuff buff);
    int getHealth();
    void setHealth(int x);
    float getSpeed();
    void setSpeed(float x);
    int getEnemyNum();
    void setEnemyNum(int x);
    void setNumWaypoints(int x);
    float[][] getWaypoints();
    void setWaypoints(float[][] newWaypoints);
    void copyWaypoints(float[][] destination, float[][] source);
    void printWaypoints(float[][] waypoints);
    float getRemainingDistance();
}

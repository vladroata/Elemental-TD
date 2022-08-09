using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface myInterface{
    int getCost();
    void setCost(int x);
    float getRange();
    float getXpos();
    float getYpos();
    void upgradeTower();
    int getUpgradeCost();
    int getUpgradeLevel();
    int getSellValue();
    int getKills();
    float getAttackSpeed();
    int getDamage();
    int getBaseDamage();
    void setDamage(int x);
    Sprite getSprite();
    string getName();
    int getPriority();
    string getPriorityName();
    void setPriority(int x);
    void AddBuff(TimedBuff buff);
    string getElement();
}

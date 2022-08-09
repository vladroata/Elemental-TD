using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public GameObject gameObject;
    public SpriteRenderer renderer;
    public int X;
    public int Y;

    private GameObject highlight;
    GameObject occupying_GO; //GameObject to represent whatever item is on top of this particular cell. (Ex: tower, rock)
    private bool onEnemyPath;

    public Cell(GameObject prefab, GameObject canvas, Sprite sprite, int x, int y) {
        X = x;
        Y = y;
        gameObject = GameObject.Instantiate(prefab, Utils.GridToWorldPosition(x, y), Quaternion.identity);
        //gameObject.transform.SetParent(canvas.transform);
        renderer = gameObject.GetComponent<SpriteRenderer>();
        gameObject.name = "X: " + x + " Y: " + y;
        renderer.sprite = sprite;
        highlight = gameObject.transform.GetChild(0).gameObject;
    }

    public SpriteRenderer GetTile(){
        return renderer;
    }

    public void UpdateTile(Sprite sprite) {
        renderer.sprite = sprite;
    }

    public bool getInEnemyPath(){
        return onEnemyPath;
    }

    public void setInEnemyPath(bool b){
         onEnemyPath = b;
    }

    public GameObject getOccupyingGO(){
        return occupying_GO;
    }

    public void setOccupyingGO(GameObject GO){
        occupying_GO = GO;
    }

    public void clearOccupyingGO(){
        occupying_GO = null;
    }
}

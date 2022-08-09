using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Theme", menuName="Tiles/Theme")]
public class TileThemeObject : ScriptableObject
{
    public Sprite[] tiles;
}

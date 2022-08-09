using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class drawGrid : MonoBehaviour
{
    public GameObject tilePrefab;
    public TileThemeObject[] themes;
    public Cell[,] Grid;
    public GameObject Tower;
    public GameObject canvas;
    private Camera cam;
    int num_Obstacles; //number of rocks that should be spawned and placed on the grid
    public GameObject rock; 
    public GameObject flag_start;
    public GameObject flag_end;

    public int userID {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        
        cam = Camera.main;
        SetUpGrid();
        DrawEnemyPath();
        DrawObstacles();
    }

    public void Update()
    {

    }

    void OnGUI()
    {
        Vector3 point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;
        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
    }

    /*public GameObject BuildTower(float i, float j) {
        GameObject res = Instantiate(Tower, new Vector3(i, j, -0.1f), Quaternion.identity);
        return res;
    }*/

    public void SetUpGrid()
    {
        //Utils.Vertical = (int)Camera.main.orthographicSize;
        //Utils.Horizontal = Utils.Vertical * (Screen.width / Screen.height);
        Utils.Vertical = switchScene.size; //Set map size to size variable passed from last scene
        Utils.Horizontal = Utils.Vertical*2;
        Utils.Rows = Utils.Vertical * 2;
        Utils.Columns = Utils.Rows * 2;
        Grid = new Cell[Utils.Columns, Utils.Rows];
        num_Obstacles = UnityEngine.Random.Range(Grid.Length/10, Grid.Length/5+1); //We want num_Obstacles to be 10%-20% of the total map size
        for (int i = 0; i < Utils.Columns; i++)
            for (int j = 0; j < Utils.Rows; j++)
                Grid[i, j] = new Cell(tilePrefab, canvas, themes[0].tiles[Utils.GetTile(i, j)], i, j);
    }

    public float[][] generateRandomPath(){ //Generate a random path
        float[][] waypoints = null;
        int random = UnityEngine.Random.Range(0, 4); //Random.Range()'s second parameter is exclusive
        int startingX = 0;
        int startingY = 0;
        int[] allowedDirections = new int[3];
        bool horizontalBlocked = false;
        int steps = 0;
        switch(random){ //Choose an edge of the grid to start from
            case 0:     //left
                startingX = 0;
                startingY = UnityEngine.Random.Range(0, Utils.Rows);
                if(startingY == 0){
                    allowedDirections = new int[]{1, 2};
                }
                else if(startingY == Utils.Rows-1){
                    allowedDirections = new int[]{2, 3};
                }
                else{
                    allowedDirections = new int[]{1, 2, 3};
                }
                break;
            case 1:     //top
                startingY = Utils.Rows-1;
                startingX = UnityEngine.Random.Range(0, Utils.Columns);
                if(startingX == 0){
                    allowedDirections = new int[]{2, 3};
                }
                else if(startingX == Utils.Columns-1){
                    allowedDirections = new int[]{0, 3};
                }
                else{
                    allowedDirections = new int[]{0, 2, 3};
                }
                break;
            case 2:     //right
                startingX = Utils.Columns-1;
                startingY = UnityEngine.Random.Range(0, Utils.Rows);
                if(startingY == 0){
                    allowedDirections = new int[]{0, 1};
                }
                else if(startingY == Utils.Rows-1){
                    allowedDirections = new int[]{0, 3};
                }
                else{
                    allowedDirections = new int[]{0, 1, 3};
                }
                break;
            case 3:     //bottom
                startingY = 0;
                startingX = UnityEngine.Random.Range(0, Utils.Columns);
                if(startingX == 0){
                    allowedDirections = new int[]{1, 2};
                }
                else if(startingX == Utils.Columns-1){
                    allowedDirections = new int[]{2, 3};
                }
                else{
                    allowedDirections = new int[]{0, 1, 2};
                }
                break; 
        }
        
        List<List<int>> waypoints_list = new List<List<int>>();
        waypoints_list.Add(new List<int>());
        waypoints_list[0].Add(startingX);
        waypoints_list[0].Add(startingY);
        steps++;
        pathInDirection(waypoints_list, allowedDirections, 100, startingX, startingY, steps);
        foreach(var x in waypoints_list){
            print(""+x[0]+" "+x[1]);
               
        }
        
        waypoints = new float[waypoints_list.Count][];
        for(int i = 0; i<waypoints_list.Count; i++){
            Vector3 coordinates = Utils.GridToWorldPosition(waypoints_list[i][0], waypoints_list[i][1]);
            waypoints[i] = new float[2];
            waypoints[i][0] = coordinates.x;
            waypoints[i][1] = coordinates.y;
        }
        return waypoints;
    }

    public List<List<int>> pathInDirection(List<List<int>> list, int[] allowedDirections, int threshold, float xposition, float yposition, int steps){
        int random = UnityEngine.Random.Range(0, 100);
        if(random < threshold || steps <= 3){
            int distance = UnityEngine.Random.Range(0, 5) + 3;
            
            int direction = allowedDirections[UnityEngine.Random.Range(0, allowedDirections.Length)];
            switch(direction){
                case 0: //move left
                    if(xposition - distance < 0){ //Make sure we don't try to travel off the grid area
                        distance = (int) (xposition); //Limit the distance we can travel by our distance to the edge
                    }
                    print("distance is "+distance);
                    xposition-=distance;                   
                    if(xposition == 0 && yposition == 0){
                        allowedDirections = new int[]{1};
                    }
                    else if(xposition == 0 && yposition == Utils.Rows-1){
                        allowedDirections = new int[]{3};
                    }
                    else if(xposition == 0){
                        allowedDirections = new int[]{1, 3};
                    }
                    else if(yposition == 0){
                        allowedDirections = new int[]{0, 1};   
                    }
                    else if(yposition == Utils.Rows-1){
                        allowedDirections = new int[]{0, 3};
                    }
                    else{
                        allowedDirections = new int[]{0, 1, 3};
                    }
                    
                    break;
                case 1: //move up
                    if(yposition + distance > Utils.Rows-1){
                        distance = (int) (Utils.Rows-1 - yposition);
                    }
                    print("distance is "+distance);
                    yposition+=distance;
                    if(yposition == Utils.Rows-1 && xposition == 0){
                        allowedDirections = new int[]{2};
                    }
                    else if(yposition == Utils.Rows-1 && xposition == Utils.Columns-1){
                        allowedDirections = new int[]{0};
                    }
                    else if(yposition == Utils.Rows-1){
                        allowedDirections = new int[]{0, 2};
                    }
                    else if(xposition == 0){
                        allowedDirections = new int[]{1, 2};   
                    }
                    else if(xposition == Utils.Columns-1){
                        allowedDirections = new int[]{0, 1};
                    }
                    else{
                        allowedDirections = new int[]{0, 1, 2};
                    }
            
                    break;
                case 2: //move right
                    if(xposition + distance > Utils.Columns-1){
                        distance = (int) (Utils.Columns-1 - xposition);
                    }
                    print("distance is "+distance);
                    xposition+=distance;
                    if(xposition == Utils.Columns-1 && yposition == 0){
                        allowedDirections = new int[]{1};
                    }
                    else if(xposition == Utils.Columns-1 && yposition == Utils.Rows-1){
                        allowedDirections = new int[]{3};
                    }
                    else if(xposition == Utils.Columns-1){
                        allowedDirections = new int[]{1, 3};
                    }
                    else if(yposition == 0){
                        allowedDirections = new int[]{1, 2};   
                    }
                    else if(yposition == Utils.Rows-1){
                        allowedDirections = new int[]{2, 3};
                    }
                    else{
                        allowedDirections = new int[]{1, 2, 3};
                    }
                    break;
                case 3: //move down
                    if(yposition - distance < 0){
                        distance = (int) (yposition);
                    }
                    print("distance is "+distance);
                    yposition-=distance;
                    if(yposition == 0 && xposition == 0){
                        allowedDirections = new int[]{2};
                    }
                    else if(yposition == 0 && xposition == Utils.Columns-1){
                        allowedDirections = new int[]{0};
                    }
                    else if(yposition == 0){
                        allowedDirections = new int[]{0, 2};
                    }
                    else if(xposition == 0){
                        allowedDirections = new int[]{2, 3};   
                    }
                    else if(xposition == Utils.Columns-1){
                        allowedDirections = new int[]{0, 3};
                    }
                    else{
                        allowedDirections = new int[]{0, 2, 3};
                    }
                    break;
            }
            if(distance != 0){ //don't adjust the number of steps or the threshold if distance was 0 because there was no travel
                list.Add(new List<int>());
                list[steps].Add((int)xposition);
                list[steps].Add((int)yposition);
                steps++;
                threshold-=5;
            }
            
            return pathInDirection(list, allowedDirections, threshold, xposition, yposition, steps);
        }
        else{
            return list;
        }
    }

    public void DrawEnemyPath() //"draw" the enemy's path by changing the tile themes of the tiles in the path
    {
        float[][] waypoints = GameObject.FindWithTag("Map").GetComponent<EnemySpawn>().getWaypoints();
        float currentX = waypoints[0][0];
        float currentY = waypoints[0][1];
        float destinationX = waypoints[1][0];
        float destinationY = waypoints[1][1];
        float deltaX = destinationX - currentX;
        float deltaY = destinationY - currentY;

        GameObject startflag = Instantiate(flag_start, new Vector3(currentX, currentY, 0f), Quaternion.identity);
        //startflag.transform.SetParent(canvas.transform);

        for (int i = 0; i < waypoints.GetLength(0); i++) {
            if (deltaX == 0)
            { //vertical
                for (int j = 0; j < Math.Abs(deltaY); j++) {
                    int[] gridSpace = WorldPositionToGrid(currentX, currentY);
                    if (deltaY > 0)
                    { //positive -> go upwards
                        Grid[gridSpace[0], gridSpace[1]].UpdateTile(themes[1].tiles[Utils.GetTile(gridSpace[0], gridSpace[1])]);         
                        currentY++;
                    }
                    else { //negative -> go downwards
                        Grid[gridSpace[0], gridSpace[1]].UpdateTile(themes[1].tiles[Utils.GetTile(gridSpace[0], gridSpace[1])]);
                        currentY--;
                    }
                    Grid[gridSpace[0], gridSpace[1]].setInEnemyPath(true); 
                }
                //catch final tile (because we change the value of currentX or currentY AFTER applying theme)
                int[] grid_Space = WorldPositionToGrid(currentX, currentY);
                Grid[grid_Space[0], grid_Space[1]].UpdateTile(themes[1].tiles[Utils.GetTile(grid_Space[0], grid_Space[1])]);
                Grid[grid_Space[0], grid_Space[1]].setInEnemyPath(true); 
            }
            else { //horizontal
                for (int j = 0; j < Math.Abs(deltaX); j++)
                {
                    int[] gridSpace = WorldPositionToGrid(currentX, currentY);
                    if (deltaX > 0)
                    { //positive -> go right
                        Grid[gridSpace[0], gridSpace[1]].UpdateTile(themes[1].tiles[Utils.GetTile(gridSpace[0], gridSpace[1])]);
                        currentX++;
                    }
                    else
                    { //negative -> go left
                        Grid[gridSpace[0], gridSpace[1]].UpdateTile(themes[1].tiles[Utils.GetTile(gridSpace[0], gridSpace[1])]);
                        currentX--;
                    }
                    
                    Grid[gridSpace[0], gridSpace[1]].setInEnemyPath(true); 
                }
                //catch final tile
                int[] grid_Space = WorldPositionToGrid(currentX, currentY);
                Grid[grid_Space[0], grid_Space[1]].UpdateTile(themes[1].tiles[Utils.GetTile(grid_Space[0], grid_Space[1])]);
                Grid[grid_Space[0], grid_Space[1]].setInEnemyPath(true); 
            }
            currentX = destinationX;
            currentY = destinationY;
            if (i < waypoints.GetLength(0) - 2) //compensate for first two points we started out with
            {
                destinationX = waypoints[i + 2][0];
                destinationY = waypoints[i + 2][1];
            }
            deltaX = destinationX - currentX;
            deltaY = destinationY - currentY;
            if(i == waypoints.GetLength(0)-1){
                GameObject endflag = Instantiate(flag_end, new Vector3(currentX, currentY, 0f), Quaternion.identity);
               //endflag.transform.SetParent(canvas.transform);
            }
        }
    }

    public void DrawObstacles(){
        //Potential idea: Have meteors rain down and smash towers. The towers could be destroyed and rocks spawned in their place.

        //create a list of locations where rocks could be spawned
        List<Cell> cellList = new List<Cell>();
        for(int i = 0; i< Utils.Columns; i++){
            for(int j = 0; j<Utils.Rows; j++){
                if(!(Grid[i, j].getInEnemyPath()) && Grid[i, j].getOccupyingGO() == null){
                    cellList.Add(Grid[i,j]);
                }
            }
        }

        //spawn the obstacles in random locations
        for(int i = 0; i<num_Obstacles; i++){ 
            int r = (UnityEngine.Random.Range(0, cellList.Count)); //specify UnityEngine.Random to avoid ambiguous reference to System.Random
            Vector3 worldpos = Utils.GridToWorldPosition(cellList[r].X, cellList[r].Y);
            GameObject go = BuildObstacle(worldpos.x, worldpos.y);
            cellList[r].setOccupyingGO(go);
            cellList[r].gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1.0f, 0f, 0f, 0.25f);
            cellList.Remove(cellList[r]);
        }
    }

    public GameObject BuildObstacle(float i, float j){
        GameObject res =Instantiate(rock, new Vector3(i, j, -0.1f), Quaternion.identity);
        return res;
    }

    public int[] WorldPositionToGrid(float x, float y) {
        int gridX = (int)(x + Utils.Horizontal - 0.5f);
        int gridY = (int)(y + Utils.Vertical - 0.5f);

        return new int[] { gridX, gridY };
    }

    public void UpdateTileTheme(int index) {
        //currentTheme = index;
        //for (int i = 0; i < Utils.Columns; i++)
        //    for (int j = 0; j < Utils.Rows; j++)
        //        Grid[i, j].UpdateTile(themes[currentTheme].tiles[Utils.GetTile(i, j)]);
    }

    //private void SpawnTile(int x, int y, float value) {
    //    SpriteRenderer sr = Instantiate(tilePrefab, Utils.GridToWorldPosition(x, y), Quaternion.identity).GetComponent<SpriteRenderer>();
    //    sr.name = "x: " + x + " y: " + y;
    //    sr.sprite = sprites[Utils.GetTile(x, y)];
    //}

    public Cell[,] getGrid(){
        return Grid;
    }

}

    

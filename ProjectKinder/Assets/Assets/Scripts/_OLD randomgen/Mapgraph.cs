using UnityEngine;
using System.Collections;

public class Mapgraph : MonoBehaviour
{

    /*
    public prefabs for making gameobjects in a grid.

    1 == ground.
    2 == wall.
    */


    public GameObject ground;
    public GameObject wall;

    public int rows = 0;
    public int cols = 0;
    public GameObject[] tiles;

    public Mapgraph(int[,] grid)
    {
        ground = GameObject.Find("Ground");
        wall = GameObject.Find("Wall");

        rows = grid.GetLength(0);
        cols = grid.GetLength(1);

        // Maak een array waar alle omgevings gameObjecten in worden opgeslagen.
        tiles = new GameObject[grid.Length];

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                switch (grid[r, c])
                {
                    case (0):
                        InstantiateTile(ground, r, c, 0);
                        break;

                    case (1):
                        InstantiateTile(wall, r, c, 5);
                        break;
                }
            }
        }
    }


    /* in deze functie worden de prefabs geÏnstantieerd en op de juiste plaats gezet */
    public void InstantiateTile(GameObject gameObject, int row, int col, int yOffset)
    {
        this.tiles[row + col] = Instantiate(gameObject);

        this.tiles[row + col].transform.position = new Vector3(10 * row, yOffset, 10 * col);
    }
}

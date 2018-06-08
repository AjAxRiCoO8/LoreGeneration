using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightPuzleController : MonoBehaviour {

    public GameObject lightBlock;

    SwitchActiveLightBlock switchActiveBlock;

    private float blockSize = 20;

    //this map will be used to create a grid of doors and LightBlocks. 
    int[,] map = new int[1, 2]
        {
            {1,1}
        };

    // Use this for initialization
    void Start()
    {
        switchActiveBlock = gameObject.GetComponent<SwitchActiveLightBlock>();
        LightsMap(this.map);
    }

    /// <summary>
    /// In this method the grid will handle the map and create objects.
    /// </summary>
    /// <param name="grid">The map</param>
    public void LightsMap(int[,] grid)
    {
        
        int rows = grid.GetLength(1);
        int cols = grid.GetLength(0);

        Debug.Log(rows + "rows");
        Debug.Log(cols + "cols");

        // Maak een array waar alle omgevings gameObjecten in worden opgeslagen.
        //LET OP: dit wordt opgeslagen in het switch handle script.
        switchActiveBlock.lightBlocks = new GameObject[cols, rows];

        for (var c = 0; c < cols; c++)
        {
            for (var r = 0; r < rows; r++)
            {

                switch (grid[c, r])
                {

                    case (1):
                        InstantiateLightBlock(lightBlock, c, r, 1);
                        break;
                }
            }
        }
    }

    /* in deze functie worden de prefabs geÏnstantieerd en op de juiste plaats gezet */
    public void InstantiateLightBlock(GameObject gameObject, int col, int row, int yOffset)
    {
        GameObject clone = Instantiate(gameObject);
        clone.transform.parent = this.transform;

        switchActiveBlock.lightBlocks[col, row] = clone;

        switchActiveBlock.lightBlocks[col, row].transform.localPosition = new Vector3(blockSize * row, yOffset, blockSize * col);

        LightBlockController lightBlockController = clone.GetComponent<LightBlockController>();
        SetLightBlockSides(lightBlockController, col, row);
        lightBlockController.id = col.ToString() + " " + row.ToString();
    }

    public void SetLightBlockSides(LightBlockController lightBlockController, int col, int row)
    {
        if (row > 0)
        {
            lightBlockController.hasSides[(int)LightBlockController.Sides.left] = true;
        }

        if (row < (map.GetLength(1)-1))
        {
            lightBlockController.hasSides[(int)LightBlockController.Sides.Right] = true;
        }

        if (col > 0)
        {
            lightBlockController.hasSides[(int)LightBlockController.Sides.Above] = true;
        }

        if (col < (map.GetLength(0) - 1))
        {
            lightBlockController.hasSides[(int)LightBlockController.Sides.Under] = true;
        }
    }
}

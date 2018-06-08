using UnityEngine;
using System.Collections;

public class SwitchActiveLightBlock : MonoBehaviour {

    public GameObject plateUp;
    public GameObject plateDown;
    public GameObject plateRight;
    public GameObject plateLeft;

    public int startActiveBlockRow;
    public int startActiveBlockCol;

    int activeBlockRow;
    int activeBlockCol;

    public GameObject[,] lightBlocks;
    LightPuzleController lightPuzleController;

    // Use this for initialization
    void Start () {
        activeBlockCol = startActiveBlockRow;
        activeBlockRow = startActiveBlockCol;

        StartCoroutine(LateStart(1));
    }

    void Update()
    {
        MoveActive();
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        lightBlocks[activeBlockCol, activeBlockRow].gameObject.GetComponent<LightBlockController>().active = true;
    }

    public void MoveActive()
    {
        LightBlockController CurrentlightBlockController = lightBlocks[activeBlockCol, activeBlockRow].gameObject.GetComponent<LightBlockController>();    

        if (plateUp.gameObject.GetComponent<PressurePlate>().triggered &&
            CurrentlightBlockController.hasSides[(int)LightBlockController.Sides.Above])
        {
            CurrentlightBlockController.active = false;
            activeBlockCol--;

            LightBlockController newActiveLightBlock = lightBlocks[activeBlockCol, activeBlockRow].gameObject.GetComponent<LightBlockController>();
            newActiveLightBlock.active = true;
        }

        if (plateDown.gameObject.GetComponent<PressurePlate>().triggered &&
            CurrentlightBlockController.hasSides[(int)LightBlockController.Sides.Under])
        {
            CurrentlightBlockController.active = false;
            activeBlockCol++;

            LightBlockController newActiveLightBlock = lightBlocks[activeBlockCol, activeBlockRow].gameObject.GetComponent<LightBlockController>();
            newActiveLightBlock.active = true;
        }

        if (plateRight.gameObject.GetComponent<PressurePlate>().triggered &&
            CurrentlightBlockController.hasSides[(int)LightBlockController.Sides.Right])
        {
            CurrentlightBlockController.active = false;
            activeBlockRow++;

            LightBlockController newActiveLightBlock = lightBlocks[activeBlockCol, activeBlockRow].gameObject.GetComponent<LightBlockController>();
            newActiveLightBlock.active = true;
        }

        if (plateLeft.gameObject.GetComponent<PressurePlate>().triggered &&
            CurrentlightBlockController.hasSides[(int)LightBlockController.Sides.left])
        {
            CurrentlightBlockController.active = false;
            activeBlockRow--;

            LightBlockController newActiveLightBlock = lightBlocks[activeBlockCol, activeBlockRow].gameObject.GetComponent<LightBlockController>();
            newActiveLightBlock.active = true;
        }

    }
}

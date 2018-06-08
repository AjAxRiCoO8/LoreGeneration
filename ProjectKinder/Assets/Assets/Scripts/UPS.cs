using UnityEngine;
using System.Collections;

public class UPS : MonoBehaviour {

    int totalCycles = 0;
    float totalFrames;
    float lowestFPS = 999;
    float highestFPS = 0;

    void Update()
    {
        totalFrames += 1.0f / Time.deltaTime;

        // after 5 sec the program will check the lowest and highest 
        if (Time.time > 5)
        {
            lowestFPS = (1.0f / Time.deltaTime < lowestFPS) ? 1.0f / Time.deltaTime : lowestFPS;
            highestFPS = (1.0f / Time.deltaTime > highestFPS) ? 1.0f / Time.deltaTime : highestFPS;
        }

        totalCycles++;


        Debug.Log("Avg fps: " + totalFrames / totalCycles + ", after " + Time.time + " seconds.");

    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        Rect rect2 = new Rect(0, 20, w, h * 2 / 100);
        Rect rect3 = new Rect(0, 35, w, h * 2 / 100);
        Rect rect4 = new Rect(0, 50, w, h * 2 / 100);

        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);

        float fps = totalFrames / totalCycles;
        string text = "average fps: " + fps;
        string timeElapsed = "time elapsed (in seconds): " + Time.time;
        string lowestFPS = "Lowest fps:" + this.lowestFPS;
        string highestFPS = "Lowest fps:" + this.highestFPS;

        GUI.Label(rect, text, style);
        GUI.Label(rect2, timeElapsed, style);
        GUI.Label(rect3, lowestFPS, style);
        GUI.Label(rect4, highestFPS, style);
    }
}

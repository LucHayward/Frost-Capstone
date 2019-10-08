using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerTest : MonoBehaviour
{
    string[] JoystickNames;
    public string[] AxisNames = {
        "P1_Horizontal",
        "P1_Vertical",
        "P1_Fire",
        "P1_Melee",
        "P1_SpeedBoost",
        "P1_Stun",
        "P1_Heal",
        "P1_DamageBoost",
        //"P1_MouseX",
        //"P1_MouseY",

        "P2_Horizontal",
        "P2_Vertical",
        "P2_Fire",
        "P2_Melee",
        "P2_SpeedBoost",
        "P2_Stun",
        "P2_Heal",
        "P2_DamageBoost",
        "P2_MouseX",
        "P2_MouseY",
        };

    public Text[] axisLabels;
    // Start is called before the first frame update
    void Start()
    {
        JoystickNames = Input.GetJoystickNames();
        foreach(string joystickName in JoystickNames)
        {
            Debug.Log(joystickName);
        }
        for (int i = 0; i < axisLabels.Length; i++)
        {
            Text an = (Text)axisLabels[i];
            an.text = AxisNames[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        axisLabels[0].text = AxisNames[0] + ": " + Input.GetAxis(AxisNames[0]);
        axisLabels[1].text = AxisNames[1] + ": " + Input.GetAxis(AxisNames[1]);
        axisLabels[2].text = AxisNames[2] + ": " + Input.GetButtonDown(AxisNames[2]);
        axisLabels[3].text = AxisNames[3] + ": " + Input.GetButtonDown(AxisNames[3]);
        axisLabels[4].text = AxisNames[4] + ": " + Input.GetButtonDown(AxisNames[4]);
        axisLabels[5].text = AxisNames[5] + ": " + Input.GetButtonDown(AxisNames[5]);
        axisLabels[6].text = AxisNames[6] + ": " + Input.GetButtonDown(AxisNames[6]);
        axisLabels[7].text = AxisNames[7] + ": " + Input.GetButtonDown(AxisNames[7]);

        axisLabels[8].text = AxisNames[8] + ": " + Input.GetAxis(AxisNames[8]);
        axisLabels[9].text = AxisNames[9] + ": " + Input.GetAxisRaw(AxisNames[9]);
        axisLabels[10].text = AxisNames[10] + ": " + Input.GetButtonDown(AxisNames[10]);
        axisLabels[11].text = AxisNames[11] + ": " + Input.GetButtonDown(AxisNames[11]);
        axisLabels[12].text = AxisNames[12] + ": " + Input.GetButtonDown(AxisNames[12]);
        axisLabels[13].text = AxisNames[13] + ": " + Input.GetButtonDown(AxisNames[13]);
        axisLabels[14].text = AxisNames[14] + ": " + Input.GetButtonDown(AxisNames[14]);
        axisLabels[15].text = AxisNames[15] + ": " + Input.GetButtonDown(AxisNames[15]);
        axisLabels[16].text = AxisNames[16] + ": " + Input.GetAxis(AxisNames[16]);
        axisLabels[17].text = AxisNames[17] + ": " + Input.GetAxis(AxisNames[17]);

        Debug.Log(Input.GetButtonDown("P2_Fire"));
    }
}

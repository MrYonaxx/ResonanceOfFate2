using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ControllerConfiguration", menuName = "ControllerConfigurationData", order = 1)]
public class ControllerConfigurationData : ScriptableObject
{

    [SerializeField]
    public string buttonA;
    [SerializeField]
    public string buttonB;
    [SerializeField]
    public string buttonX;
    [SerializeField]
    public string buttonY;

    [SerializeField]
    public string buttonRB;
    [SerializeField]
    public string buttonLB;

    [Title("Axis")]
    [SerializeField]
    public string stickLeftHorizontal;
    [SerializeField]
    public string stickLeftVertical;

    [SerializeField]
    public string stickRightHorizontal;
    [SerializeField]
    public string stickRightVertical;

    [SerializeField]
    public string dpadHorizontal;
    [SerializeField]
    public string dpadVertical;

    [SerializeField]
    public string rightTrigger;
    [SerializeField]
    public string leftTrigger;

    [SerializeField]
    public string start;

    private ValueDropdownList<string> controllerValues = new ValueDropdownList<string>()
    {
        {"ControllerA", "ControllerA" },
        {"ControllerB", "ControllerB" },
        {"ControllerX", "ControllerX" },
        {"ControllerY", "ControllerY" },
    };

    public void SetConfigurationData(ControllerConfigurationData d)
    {
        buttonA = d.buttonA;
        buttonB = d.buttonB;
        buttonX = d.buttonX;
        buttonY = d.buttonY;
        buttonRB = d.buttonRB;
        buttonLB = d.buttonLB;
        stickLeftHorizontal = d.stickLeftHorizontal;
        stickLeftVertical = d.stickLeftVertical;
        stickRightHorizontal = d.stickRightHorizontal;
        stickRightVertical = d.stickRightVertical;
        dpadHorizontal = d.dpadHorizontal;
        dpadVertical = d.dpadVertical;
        rightTrigger = d.rightTrigger;
        leftTrigger = d.leftTrigger;
        start = d.start;
    }


}

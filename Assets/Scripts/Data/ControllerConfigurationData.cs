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
    public string rightTrigger;
    [SerializeField]
    public string leftTrigger;

    private ValueDropdownList<string> controllerValues = new ValueDropdownList<string>()
    {
        {"ControllerA", "ControllerA" },
        {"ControllerB", "ControllerB" },
        {"ControllerX", "ControllerX" },
        {"ControllerY", "ControllerY" },
    };
}

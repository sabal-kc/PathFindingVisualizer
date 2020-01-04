using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Dropdown algoDropdown;
    public TMP_Dropdown connectionDropdown;
    public Toggle stepModeToggle;
    public Button stepButton;
    // Start is called before the first frame update
    void Start() {
        Screen.SetResolution(1600, 900, false);
        algoDropdown.onValueChanged.AddListener(delegate {
            AlgoDropdownValueChanged(algoDropdown);
        });
        connectionDropdown.onValueChanged.AddListener(delegate {
            ConnectionDropdownValueChanged(connectionDropdown);
        });
        stepModeToggle.isOn = false;
        stepModeToggle.onValueChanged.AddListener(delegate {
            ToggleValueChange(stepModeToggle);
        });
        stepButton.onClick.AddListener(StepClick);

    }

    void StepClick() 
    {
        Debug.Log("Clicked");
        StepManager.Instance.StepForward();
    }
    void AlgoDropdownValueChanged(TMP_Dropdown change) {
        Debug.Log(change.value);
        AlgorithmManager.Instance.algorithmType = (AlgorithmManager.AlgoType)change.value;

    }

    void ToggleValueChange(Toggle change) {
        Debug.Log(change.isOn);
        AlgorithmManager.Instance.stepWiseMode = change.isOn;
    }

    void ConnectionDropdownValueChanged(TMP_Dropdown change) {
        Debug.Log(change.value);
        AlgorithmManager.Instance.grid.noOfDirections = change.value == 0 ? Grid.Direction.FOUR : Grid.Direction.EIGHT;
        AlgorithmManager.Instance.grid.CreateConnectors();

    }

}

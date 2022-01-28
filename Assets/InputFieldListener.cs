using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputFieldListener : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private void Awake()
    {
        inputField.onValueChanged.AddListener(delegate(string arg0)
        {
            Debug.Log($"From code changed {arg0}");
        });
        
        inputField.onEndEdit.AddListener(delegate(string arg0)
        {
            Debug.Log($"From code end {arg0}");
        });
    }

    private void OnDestroy()
    {
        inputField.onValueChanged.RemoveAllListeners();
        inputField.onEndEdit.RemoveAllListeners();
    }

    public void TestOnValueChanged()
    {
        Debug.Log($"From changed event {inputField.text}");
    }
    
    public void TestOnValueEnd()
    {
        Debug.Log($"From end event {inputField.text}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class InstructionCanvas : MonoBehaviour
{

    public Text instructionText;
    public Button closeButton;
    public static InstructionCanvas Instance = null;
    // Use this for initialization
    void Start()
    {
        Instance = this;
        //Instance.gameObject.SetActive(false);


    }


    public void SetInstruction(InstructionObject I)
    {
        instructionText.text = I.InstructionText;
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(I.canvasEvent.Invoke);
        closeButton.gameObject.SetActive(false);


    }

    public void SetButtonActive()
    {
        closeButton.gameObject.SetActive(true);


    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public class InstructionObject
{
    public string InstructionText;
    public UnityEvent canvasEvent;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparerMenager : MonoBehaviour
    {
    InputField inputX, inputY, inputZ;
    // Start is called before the first frame update
    void Start()
        {
        inputX = GameObject.Find("InputFieldX").transform.GetComponent<InputField>();
        inputY = GameObject.Find("InputFieldY").transform.GetComponent<InputField>();
        inputZ = GameObject.Find("InputFieldZ").transform.GetComponent<InputField>();
        }

    // Update is called once per frame
    void Update()
        {
        
        }
    public void Aller()
        {
        Debug.Log(inputX.text);
        Debug.Log(inputY.text);
        Debug.Log(inputZ.text);
        }

    }



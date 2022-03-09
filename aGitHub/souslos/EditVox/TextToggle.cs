using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextToggle : MonoBehaviour
    {
    public string textA, textB, textBase;
    public Text text;
    bool rev = false;
    // Start is called before the first frame update
    void Awake()
        {
        text = GetComponent<Text>();

        text.text = textBase + " " + textA;
        text.color = Color.black;

        Toggle();
        }

    public void Toggle()
        {
        rev = !rev;

        if (rev)
            {
            text.text = textBase + " " + textA;
            text.color = Color.black;
            }
        else
            {
            text.text = textBase + " " + textB;
            text.color = Color.green;
            }
        }


    }

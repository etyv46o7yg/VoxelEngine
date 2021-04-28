using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FSP_ind : MonoBehaviour
    {
    public Text textInd;
    List<float> values;

    [Range(0, 300)]
    public int smouze;
    // Start is called before the first frame update
    void Start()
        {
        values = new List<float>();

        if(textInd == null)
            {
            textInd = GetComponent<Text>();
            }
        }

    // Update is called once per frame
    void Update()
        {
        float value = 1.0f / Time.deltaTime;
        float summ = 0.0f;

        if (values.Count < smouze)
            {
            values.Add(value);
            }
        else
            {
            values.RemoveAt(0);
            values.Add(value);
            }

        foreach (var item in values)
            {
            summ += item;
            }
        
        int res = Mathf.RoundToInt( summ/ values.Count );
        textInd.text = res.ToString();
        }
    }

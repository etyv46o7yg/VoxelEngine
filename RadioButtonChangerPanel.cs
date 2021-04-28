using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioButtonChangerPanel : MonoBehaviour
    {
    public Toggle[] toggles;
    public List<GameObject> panels;
    // Start is called before the first frame update
    void Start()
        {
        toggles = transform.GetComponentsInChildren<Toggle>();

        foreach (var item in toggles)
            {
            item.onValueChanged.AddListener( delegate { Check(); } );
            }
        }

    // Update is called once per frame
    void Update()
        {
        
        }

    public void Check()
        {
        int numero = 0;

        for (int i = 0; i < toggles.Length; i++)
            {
            if (toggles[i].isOn)
                {
                numero = i; break;
                }
            }

        ShowPanel(numero);
        }

    void ShowPanel(int _count)
        {
        if (_count >= panels.Count)
            {
            return;
            }

        foreach (var item in panels)
            {
            item.SetActive(false);
            }

        panels[_count].SetActive(true);
        }
    }

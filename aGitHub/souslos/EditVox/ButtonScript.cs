using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
    {
    public Text tex;

    /// <summary>
    /// путь к файлу
    /// </summary>
    string _route;
    // Start is called before the first frame update
    void Start()
        {
        tex = transform.GetComponentInChildren<Text>();
        }

    // Update is called once per frame
    void Update()
        {
        
        }

    public void Clique()
        {
        
        }

    public void SetText(string _str, string _path)
        {
        _route = _path;

        if(tex == null)
            {
            tex = transform.GetComponentInChildren<Text>();
            }

        tex.text = _str;
        }
    }

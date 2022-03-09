using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderElement : MonoBehaviour
    {
    public delegate void Faire(OrderElement _element);
    public event Faire onStartFini;
    // Start is called before the first frame update

    private void Awake()
        {
        
        }

    void Start()
        {
        OrderBeha.instance.Register(this);      
        }

    public void MonStart()
        {
        onStartFini?.Invoke(this);
        }

    public void PostStart()
        {

        }

    // Update is called once per frame
    void Update()
        {
        
        }
    }

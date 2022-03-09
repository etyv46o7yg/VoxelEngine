using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// для задания порядка выполнения методов
/// </summary>
public class OrderBeha : MonoBehaviour
    {
    private List<OrderElement> behaviours;
    public static OrderBeha instance = null;
    // Start is called before the first frame update
    private void Awake()
        {
        if (instance == null)
            {
            instance = this;
            }
        else if (instance == this)
            {
            Destroy(gameObject);
            }

        behaviours = new List<OrderElement>();
        }

    public void Register(OrderElement _meha)
        {
        behaviours.Add(_meha);
        _meha.onStartFini += StartQuelqun;

        _meha.MonStart();
        }

    private void StartQuelqun(OrderElement _element)
        {
        Debug.Log(behaviours.Count);       

        if (behaviours.Count == 0 )
            {
            PostStart();
            }

        behaviours.Remove(_element);
        }

    private void PostStart()
        {
        Debug.Log("пост-старт");

        foreach (var item in behaviours)
            {
            item.PostStart();
            }
        }
// Update is called once per frame
    void Update()
        {
        
        }

    }

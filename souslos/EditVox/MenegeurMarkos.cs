using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenegeurMarkos : MonoBehaviour
    {
    public List<MoveObj> MoveObjs;
    int count = 0;
    public KeyCode keyChanger = KeyCode.RightShift;
    // Start is called before the first frame update
    void Start()
        {
        MoveObjs = this.transform.GetComponentsInChildren<MoveObj>().ToList();
        }

    // Update is called once per frame
    void Update()
        {

        if (Input.GetKeyDown(keyChanger) )
            {
            Debug.Log("измена");
            count++;
            int courrActif = count % MoveObjs.Count;
            SetActif(courrActif);
            }
        }

    void SetActif(int numero)
        {
        foreach (var item in MoveObjs)
            {
            item.estActif = false;
            }

        MoveObjs[numero].estActif = true;
        }
    }

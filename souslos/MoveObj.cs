using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
    {
    public bool estActif = true;
    Transform tr;
        // Start is called before the first frame update
    void Start()
        {
        tr = transform;
        }

        // Update is called once per frame
    void Update()
        {
        if(!estActif)
            {
            return;
            }
        //tr.Translate(Vector3.right * DG);

        if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey("[4]"))
            {           
            tr.Translate(Vector3.right);
            }

        if (Input.GetKey(KeyCode.Keypad6) || Input.GetKey("[6]") )
            {
            tr.Translate(-Vector3.right);
            }

        if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey("[2]"))
            {
            tr.Translate(Vector3.forward);
            }

        if (Input.GetKey(KeyCode.Keypad8) || Input.GetKey("[8]"))
            {
            tr.Translate(-Vector3.forward);
            }

        if (Input.GetKey(KeyCode.Keypad1) || Input.GetKey("[1]"))
            {
            tr.Translate(Vector3.up);
            }

        if (Input.GetKey(KeyCode.Keypad3) || Input.GetKey("[3]"))
            {
            tr.Translate(-Vector3.up);
            }
        }
    }

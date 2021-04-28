using Mathan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MontrerPosCoursor : MonoBehaviour
    {
    public Text textData_3d;
    public Text textData_2d;
    string res = "";
    // Start is called before the first frame update
    void Start()
        {
        
        }

    // Update is called once per frame
    void Update()
        {        
        var val = EditorPrince.instance.render.posMouse3D;
        
        if ( !val.isHaseInfo )
            {
            res = "---";
            textData_3d.color = Color.white;
            textData_2d.text = "---";
            }
        else
            {
            if (val.isCorrectPose)
                {
                res = ( (int3) val.pos3d ).ToString();
                textData_3d.color = Color.black;
                }
            else
                {
                res = "Некорр. поз.";
                textData_3d.color = Color.red;
                }


            textData_2d.text =  ( new Vector2( val.pos2d.x, val.pos2d.y) ).ToString();
            
            }
        
        textData_3d.text = res;
        }
    }

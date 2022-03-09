using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionPaintScript : MonoBehaviour
    {
    public MenedgerRouts menedgerRouts;

    public Toggle [] togglesDir;
    public Toggle [] togglesPlusMinus;
    Vector3 prevVectorDir = Vector3.zero;
    public Vector3 vectorDir;
    public int dirNum = 0, dirPlus = 0;
    public delegate void VecDir(Vector3 _dir);
    public event VecDir VectorAEteChanger;

    void Awake()
        {
        menedgerRouts.directionPaint = this;
        }

    // Start is called before the first frame update
    void Start()
        {       
        foreach (var item in togglesDir)
            {
            item.onValueChanged.AddListener(delegate { CheckDir(); });
            }

        foreach (var item in togglesPlusMinus)
            {
            item.onValueChanged.AddListener(delegate { CheckPlusMinus(); });
            }

        VectorAEteChanger?.Invoke( Vector3.back );
        }

    // Update is called once per frame
    void Update()
        {
        switch (dirNum)
            {
            case 0: prevVectorDir = Vector3.right;   break;
            case 1: prevVectorDir = Vector3.up;      break;
            case 2: prevVectorDir = Vector3.forward; break;
            default: break;
            }

        if (dirPlus == 0)
            {
            vectorDir = -prevVectorDir;
            }
        else
            {
            vectorDir = prevVectorDir;
            }
        }

    void CheckDir()
        {
        
        for (int i = 0; i < togglesDir.Length; i++)
            {
            if (togglesDir [i].isOn)
                {
                dirNum = i; break;
                }
            }

        VectorAEteChanger?.Invoke( GenerteVector(dirNum, dirPlus) );
        }

    void CheckPlusMinus()
        {
        for (int i = 0; i < togglesPlusMinus.Length; i++)
            {
            if (togglesPlusMinus [i].isOn)
                {
                dirPlus = i; break;
                }
            }

        VectorAEteChanger?.Invoke( GenerteVector(dirNum, dirPlus) );
        }

    private Vector3 GenerteVector(int dir, int dirPlus)
        {
        Vector3 vectorDir;

        switch (dir)
            {
            case 0: prevVectorDir = Vector3.right; break;
            case 1: prevVectorDir = Vector3.up; break;
            case 2: prevVectorDir = Vector3.forward; break;
            default: break;
            }

        if (dirPlus == 0)
            {
            vectorDir = -prevVectorDir;
            }
        else
            {
            vectorDir = prevVectorDir;
            }

        return vectorDir;
        }

    }

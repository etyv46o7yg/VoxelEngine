using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experemental : MonoBehaviour
    {
    public Mesh mesh;
    public int xMax, yMax;
    public List<Vector3> transPos;
    public Material mat;

    // Start is called before the first frame update
    void Start()
        {
        transPos = new List<Vector3>();

        for (int x = 0; x < xMax; x++)
            {
            for (int y = 1; y < x; y++)
                {
                transPos.Add(new Vector3(x, y, 0) );
                transPos.Add(new Vector3(x, -y, 0));
                }
            }
        }

    // Update is called once per frame
    void Update()
        {
        foreach (var item in transPos)
            {
            Graphics.DrawMesh(mesh, item, Quaternion.identity, mat, 0 );
            }
        }
    }

using Mathan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imitator : MonoBehaviour
    {
    public List<Particule> Particules;
    public List<int>       RefVoxel;
    // Start is called before the first frame update
    void Start()
        {
        Particules = new List<Particule>();
        RefVoxel   = new List<int>();

        Vector4 colA = new Vector4(0, 0, 0, 0);
        Vector4 colB = new Vector4(1, 1, 1, 1);

        Particules.Add(new Particule(colA, 0));
        Particules.Add(new Particule(colA, 1));
        Particules.Add(new Particule(colB, 2));
        Particules.Add(new Particule(colB, 3));
        Particules.Add(new Particule(colA, 4));
        Particules.Add(new Particule(colA, 5));
        Particules.Add(new Particule(colA, 6));

        RefVoxel.Add(0);
        RefVoxel.Add(1);
        RefVoxel.Add(2);
        RefVoxel.Add(3);
        RefVoxel.Add(4);
        RefVoxel.Add(5);
        RefVoxel.Add(6);

        Show();
        }

    // Update is called once per frame
    void Update()
        {
        if (Input.GetKeyDown(KeyCode.T))
            {
            Debug.Log("тик");

            Move();
            Blitz();

            Show();
            }
        }

    private void Move()
        {
        for (int i = 1; i < Particules.Count - 1; i++)
            {
            if (Particules [RefVoxel [i] - 1].GetColor().w < 0.1f)
                {
                Particules [i].Move(-1);
                }
            }
        }

    void Blitz()
        {
        foreach (var item in Particules)
            {
            if (item.GetColor().w > 0.0f)
                {

                }
            }
        }

    void Show()
        {
        foreach (var item in Particules)
            {
            item.Show();
            }
        }
    }

public class Particule
    {
    int posX;
    Vector4 color;
    float mass;
    int refVoxel;

    public Particule(Vector4 _col, int _pos)
        {
        color = _col;
        posX  = _pos;
        }

    public int GetIntPos(int dim)
        {
        return posX;
        }

    public Vector4 GetColor()
        {
        return color;
        }

    public void SetColor(Vector4 _color)
        {
        color = _color;
        }

    public void Move(int vec)
        {
        posX = posX + vec;
        }

    public void Show()
        {
        string res = "pos = " + posX + " col = " + color.ToString(); 
        Debug.Log(res);
        }
    }

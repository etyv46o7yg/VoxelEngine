using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDiscum : MonoBehaviour
    {
    private string nomDiscum;
    private FileBreauser.FileMonteur fileMonteur;
    private Text text;
    // Start is called before the first frame update
    void Start()
        {
        
        }

    // Update is called once per frame

    public void SetData(string _rout, FileBreauser.FileMonteur _fileMonteur)
        {
        text = GetComponentInChildren<Text>();

        fileMonteur = _fileMonteur;
        nomDiscum = _rout;
        text.text = "  " + nomDiscum;
        }

    public void Cliqus()
        {
        fileMonteur.OuvertDirectory(nomDiscum);
        }
    }

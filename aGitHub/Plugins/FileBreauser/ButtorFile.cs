using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static FileBreauser.DirectoreData;


namespace FileBreauser
    {
    public class ButtorFile : MonoBehaviour
        {
        // Start is called before the first frame update
        public Text text;
        bool estFile;
        FileMonteur fileMonteur;
        public DirectoreData data;
        public Button butt;

        ColorBlock colorButtpnDir, colorButtonFile;

        void Start()
            {
            butt = GetComponent<Button>();
            colorButtpnDir = new ColorBlock();
            colorButtpnDir.normalColor = Color.yellow;
            colorButtpnDir.colorMultiplier = 1.0f;

            colorButtonFile = new ColorBlock();
            colorButtonFile.normalColor = Color.white;
            colorButtonFile.colorMultiplier = 1.0f;
            }

        // Update is called once per frame
        void Update()
            {

            }

        public void SetData(DirectoreData _data, FileMonteur _parau)
            {
            Start();

            data = _data;
            text.text = "  " + data.nom;
            fileMonteur = _parau;


            switch (data.typeEntites)
                {
                case TypeEntites.File: butt.colors = colorButtonFile; break;
                case TypeEntites.Directory: butt.colors = colorButtpnDir; break;
                default: break;
                }
            }

        public void Cliqus()
            {
            fileMonteur.Cliqus(this);
            }
        } 
    }

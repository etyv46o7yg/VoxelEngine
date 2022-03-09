using Assets.souslos.EditVox;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// выбор файла для сохранения и загрузки
/// </summary>
public class SelectFile : MonoBehaviour, IPanelUI
    {
    public GameObject panella;
    public Object     prefab;
    // Start is called before the first frame update
    void Start()
        {
        
        }

    // Update is called once per frame
    void Update()
        {
        
        }


    public void MontrerFilesADirectory(string _dir)
        {
        //var res = Directory.GetFiles(_dir);
        string ResourcesPath = _dir;

        var nomsTex = Directory.GetFiles(ResourcesPath).Select(Path.GetFileNameWithoutExtension); 

        foreach (var item in nomsTex)
            {
            Debug.Log(item);
            ShowItem(item, "sauverTex");               
            }
        }

    public void ShowItem( string _nom, string _path )
        {
        GameObject go = (GameObject) Instantiate(prefab);
        go.transform.parent = panella.transform;
        go.GetComponent<ButtonScript>().SetText(_nom, _path);
            
        }

    public void DelitePanel()
        {
        
        }
    }

using Assets.souslos.EditVox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// класс главного меню
/// </summary>
public class PrinceMenadger : MonoBehaviour
    {
    public static PrinceMenadger instance = null;

    /// <summary>
    /// путь к папке, где лежат файлы сохранения
    /// </summary>
    public string routAFolderFiles;

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

        routAFolderFiles = Application.dataPath;
        }

    // Start is called before the first frame update
    void Start()
        {
        
        
        }

    // Update is called once per frame
    void Update()
        {
        
        }

    public void LoadFile(string _rout)
        {

        }

    /// <summary>
    /// открыть окно
    /// </summary>
    void OuvertFenetre()
        {

        }


    public void Exit()
        {
        Debug.Log("выход");
        Application.Quit();
        }
    }

using Assets.souslos.EditVox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FileBreauser.FileMonteur;

/// <summary>
/// класс главного меню
/// </summary>
public class PrinceMenadger : MonoBehaviour
    {
    public static PrinceMenadger instance = null;
    public MenedgerRouts menedger; 
    public FileBreauser.FileMonteur fileMonteur;
    public readonly string expantionFileFormat = ".tempete";

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
        menedger.princeMenadger = this;
        }

    // Start is called before the first frame update
    void Start()
        {
        fileMonteur.onFileOuvert +=LoadFile;
        fileMonteur.onFileSauver +=FileAEteSauver;
        }

    // Update is called once per frame
    void Update()
        {
        
        }

    public void LoadFile(string _rout)
        {
        Debug.Log("загружаю файл " + _rout);
        menedger.render.LoadMonde(_rout);
        }

    /// <summary>
    /// открыть окно выбора файла
    /// </summary>
    public void OuvertFenetreSelectFilePourLoad()
        {
        if (fileMonteur.regime == RegimeDialogFileBrouser.Noactif)
            {
            fileMonteur.gameObject.SetActive(true);
            fileMonteur.SetFilterExtentions(new List<string> { expantionFileFormat });
            fileMonteur.SetRegime(RegimeDialogFileBrouser.Ouvert, "");
            }
        }

    public void OuvertFenetreSelectFilePourSauver()
        {
        if (fileMonteur.regime == RegimeDialogFileBrouser.Noactif)
            {
            fileMonteur.gameObject.SetActive(true);
            fileMonteur.SetFilterExtentions(new List<string> { expantionFileFormat });
            fileMonteur.SetRegime(RegimeDialogFileBrouser.Sauver, expantionFileFormat);
            }
        }

    void FileAEteSauver(string nom)
        {
        menedger.render.SauverMondeAFile(nom + expantionFileFormat);
        }


    public void Exit()
        {
        Debug.Log("выход");
        Application.Quit();
        }
    }

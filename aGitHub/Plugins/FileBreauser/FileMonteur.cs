using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FileBreauser
    {
    public class FileMonteur : MonoBehaviour
        {
        private string routAFolder = "";
        public GameObject panelFiles, panelDiscum, panelInputName;
        public Object prefabButtonFile, prefabDicsum;
        public InputField fieldCourrentRout, fieldNeauveauFile;

        public delegate void fileData(string _routTout);
        public event fileData onFileOuvert;
        public event fileData onFileSauver;
        List<string> courrFilter;
        string courrExection = "";

        public RegimeDialogFileBrouser regime;

        public enum RegimeDialogFileBrouser
            {
            Noactif = 0,
            Ouvert = 1,
            Sauver = 2
            }
        // Start is called before the first frame update
        void Start()
            {
            OuvertDirectory("F:/");
            Initializer();
            }

        private void Initializer()
            {
            regime = RegimeDialogFileBrouser.Noactif;

            ClearButtDiscum();
            var res = Directory.GetLogicalDrives().ToList();

            foreach (var item in res)
                {
                GameObject butt = Instantiate(prefabDicsum) as GameObject;
                butt.GetComponent<ButtonDiscum>().SetData(item, this);
                butt.transform.SetParent(panelDiscum.transform);
                }
            }

        public void SetRegime(RegimeDialogFileBrouser _regime, string _exectionFile)
            {
            regime = _regime;
            Initializer();

            if (_regime == RegimeDialogFileBrouser.Ouvert)
                {
                panelInputName.SetActive(false);
                }

            if (_regime == RegimeDialogFileBrouser.Sauver)
                {
                panelInputName.SetActive(true);
                }
            }

        public void OuvertDirectory(string _rout)
            {                    
            if(!Directory.Exists(_rout) )
                {
                return;
                }

            ClearButtonius(); 
            
            var directorez = Directory.GetDirectories(_rout).ToList();
            var filiez = Directory.GetFiles(_rout).ToList();

            List<DirectoreData> entites = new List<DirectoreData>();

            foreach (var item in directorez)
                {
                var nom = IncertNomFile(item, _rout);
                entites.Add(new DirectoreData(_rout, nom, DirectoreData.TypeEntites.Directory));
                }

            foreach (var item in filiez)
                {
                var nom = IncertNomFile(item, _rout);
                entites.Add(new DirectoreData(_rout, nom, DirectoreData.TypeEntites.File));
                }

            foreach (var item in entites)
                {             
                bool res = false;

                if (item.typeEntites == DirectoreData.TypeEntites.File)
                    {
                    res = ControlListFilter(courrFilter, item.nom);
                    }

                if ( res || item.typeEntites == DirectoreData.TypeEntites.Directory )
                    {
                    AddEntite(item);
                    }
                
                }

            fieldCourrentRout.text = _rout;
            routAFolder = _rout;
            }

        private bool ControlListFilter(List<string> filer, string file)
            {
            if (filer.Count == 0)
                {
                Debug.Log("нулевой фильтр");
                return true;
                }

            foreach (var item in filer)
                {
                if (ControlFilterExeption(file, item) )
                    {
                    return true;
                    }
                }

            return false;
            }

        /// <summary>
        /// установить разрешенные типы расширений для отображения
        /// </summary>
        /// <param name="_filter"></param>
        public void SetFilterExtentions(List<string> _filter)
            {
            courrFilter = _filter;
            OuvertDirectory(routAFolder);
            }

        private void AddEntite(DirectoreData _data)
            {
            string nom = _data.nom;
            GameObject btnn = Instantiate(prefabButtonFile) as GameObject;
            btnn.transform.SetParent(panelFiles.transform);
            btnn.GetComponent<ButtorFile>().SetData(_data, this);
            }

        private void ClearButtonius()
            {
            List<GameObject> buttuns = new List<GameObject>();
            for (int i = 0; i < panelFiles.transform.childCount; i++)
                {
                buttuns.Add(panelFiles.transform.GetChild(i).gameObject);
                }

            buttuns.ForEach(x => Destroy(x));
            }

        private void ClearButtDiscum()
            {
            List<GameObject> buttuns = new List<GameObject>();
            for (int i = 0; i < panelDiscum.transform.childCount; i++)
                {
                buttuns.Add(panelDiscum.transform.GetChild(i).gameObject);
                }

            buttuns.ForEach(x => Destroy(x));
            }

        public void Cliqus(ButtorFile buttorFile)
            {
            if (buttorFile.data.typeEntites == DirectoreData.TypeEntites.Directory)
                {
                OuvertDirectory(buttorFile.data.rout);
                }

            if (buttorFile.data.typeEntites == DirectoreData.TypeEntites.File)
                {               
                onFileOuvert?.Invoke(buttorFile.data.rout);
                Exit();
                }
            }

        public void BackButton()
            {
            try
                {
                string backRirectopy = GetParuDir(routAFolder);
                OuvertDirectory(backRirectopy);
                }
            catch (System.Exception ex)
                {
                Debug.Log(ex.Message);
                throw;
                }

            }

        public void SauverFile()
            {
            string nom = routAFolder + "/" + fieldNeauveauFile.text;
            onFileSauver?.Invoke(nom);
            }

        public void Exit()
            {
            regime = RegimeDialogFileBrouser.Noactif;
            this.gameObject.SetActive(false);
            }
        //F:/mon_projetes\DirectX-Graphics-Samples-master\DirectX-Graphics-Samples-master\MiniEngine\PropertySheets
        private string GetParuDir(string _path)
            {
            int posSlach = 0;
            string res = "";

            for (int i = _path.Length - 1; i > 0; i--)
                {
                if (_path [i] == '\\' || _path [i] == '/')
                    {
                    posSlach = i;
                    res = _path.Substring(0, posSlach);

                    if (res.Length == 2)
                        {
                        res += "\\";
                        }
                    return res;
                    }
                }

            return res;
            }

        // Update is called once per frame
        void Update()
            {
            if (Input.GetKeyDown(KeyCode.Return ))
                {
                string neouveauRout = fieldCourrentRout.text;
                OuvertDirectory(neouveauRout);
                }
            }

        /// <summary>
        /// проверка наличия разрешения файла
        /// </summary>
        /// <param name="fileNom">имя файла</param>
        /// <param name="_exption">разрешение</param>
        /// <returns>ТРУ есть разрешение совпадает. ФАЛЬШ есть не совпадает</returns>
        public bool ControlFilterExeption(string fileNom, string _exption)
            {
            

            if (_exption.Length > fileNom.Length)
                {
                return false;
                }
            string esptFi = "";

            try
                {
                esptFi = fileNom.Substring(fileNom.Length - _exption.Length, _exption.Length);
                }
            catch (System.Exception)
                {
                return false;
                throw;
                }

            bool res = esptFi.Equals(_exption);
            return res;
            }

        private string IncertNomFile(string _fullMon, string _rout)
            {
            int lentRout = _rout.Length;
            string nom = _fullMon.Substring(lentRout, _fullMon.Length - lentRout);
            return nom;
            }
        }

    public class DirectoreData
        {
        public enum TypeEntites
            {
            File = 0,
            Directory = 1
            }

        public TypeEntites typeEntites;
        public string rout, nom;

        public DirectoreData(string _rout, string _nom, TypeEntites _type)
            {
            rout = _rout + _nom;
            nom = _nom;
            typeEntites = _type;
            }
        } 
    }

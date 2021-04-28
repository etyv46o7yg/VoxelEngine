using Mathan;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using VoxelEngine;

/// <summary>
/// рендер без освещения
/// </summary>
namespace Render
    {
    public class Minimal_Render : MonoBehaviour
        {
        ChaderSysteme chader;
        public RawImage mainEcrane, secondEcrane;
        public RenderTexture RayMap, RayMap_2, mondePosTex, lod_1;
        public Monde monde;
        public Texture2D mapHight, tex_1;

        string [] nomKernel;
        //Dictionary<string, int> dictKernel;

        public RenderTexture rt3d, lumLocTex_1, lumLocTex_2;

        public RenderTexture tempTex { get; private set; }

        // Start is called before the first frame update
        [Range(0, 10.0f)]
        public float k_1, k_2, k_3, k_4, k_5;

        public Vector3 bias_1;
        public CoursorInfo3D posMouse3D;
        public Color ombreColor;

        const int sizeMonde = 256;

        public GameObject mark;

        public bool est_1;

        CliqueImageRender handlerClique;

        Vector4[] colorSave, cartupedMonde;
        ComputeBuffer bufferTextureSave, bufferGeneralInfo;               

        string dataRoutSauveData;
        void Start()
            {               
            Application.targetFrameRate = 30;
            dataRoutSauveData = Application.dataPath + "/monde.sav";            

            monde = new Monde(sizeMonde, sizeMonde, 256);  //невероятно, это работает!
                                                           //monde.GenerateMapFromTexture(mapHight, new Color(1, 1, 1, 0.005f));
            monde.CreerVideMonde(new Color(0, 0, 0, 0));
            monde.CreerTerre(Color.white, 5);
            monde.AddSphere(new Vector3(100, 100, 40), 30, new Color(1, 0, 0, 0.01f));
            monde.AddSphere(new Vector3(180, 100, 50), 30, new Color(1, 0, 1, 0.01f));
            monde.AddBox(new int3(200, 200, 0), new int3(10, 25, 50), Color.white);
            //monde = Monde.LoadMonde("мир_1");

            colorSave     = new Vector4[monde.sizeMonde];
            cartupedMonde = new Vector4[monde.sizeMonde];

            string [] nomKernel = new string [] 
                                            { 
                                            "CSMain", "Prepare", "PaintLocal", "PaintGlobal", 
                                            "BlitzMondeALum", "Lumiere_1", "Clear", 
                                            "Lumiere_3", "BlitzLum_1", "AddBox", "Sauver", "Load",
                                            "PaintColorierGlobal",
                                            "Sauver_Piece", "Load_Piece"
                                            };
            chader = new ChaderSysteme("MinimRenderShader", nomKernel);
            //dictKernel = new Dictionary<string, int>();
            chader.AddDict("Main",     nomKernel [0] );
            chader.AddDict("Prep",     nomKernel [1] );
            chader.AddDict("PaintScr", nomKernel [2] );
            chader.AddDict("PaintWrl", nomKernel [3] );
            chader.AddDict("BlizLum",  nomKernel [4] );
            chader.AddDict("Lum_1",    nomKernel [5] );
            chader.AddDict("Clair",    nomKernel [6] );
            chader.AddDict("Lum_3",    nomKernel [7] );
            chader.AddDict("LOD_1",    nomKernel [8] );
            chader.AddDict("AdBox",    nomKernel [9] );
            chader.AddDict("Save",     nomKernel [10]);
            chader.AddDict("Load",     nomKernel [11]);
            chader.AddDict("PCG",      nomKernel [12]);
            chader.AddDict("SaveP",    nomKernel [13]);
            chader.AddDict("LoadP",    nomKernel [14]);

            RenderTexture mainTex = ChaderSysteme.GetRT(1024, 1024);

            rt3d        = ChaderSysteme.GetRt3D(sizeMonde, sizeMonde, 256);
            tempTex     = ChaderSysteme.GetRt3D(sizeMonde, sizeMonde, 256);
            lumLocTex_1 = ChaderSysteme.GetRt3D(sizeMonde, sizeMonde, 256);
            lumLocTex_2 = ChaderSysteme.GetRt3D(sizeMonde, sizeMonde, 256);
            lod_1       = ChaderSysteme.GetRt3D(sizeMonde, sizeMonde, 256, RenderTextureFormat.ARGBFloat);
            mondePosTex = ChaderSysteme.GetRT(1024, 1024, RenderTextureFormat.ARGBFloat);

            bufferTextureSave = new ComputeBuffer(colorSave.Length, 16);
            bufferTextureSave.SetData(colorSave);
            bufferGeneralInfo = ChaderSysteme.GetComputeBuffer<GeneralInfo>(1);

            chader.SetTexture(mainTex, "Result");
            chader.SetNumeroThredes(32, 32, 1);
            mainEcrane.texture = mainTex;

            chader.SetTexture(RayMap, "RayMap");
            chader.SetTexture(RayMap_2, "RayMap_2");
            chader.SetTexture(monde.texture, "Input");
            chader.SetTexture(rt3d, "mondeTex");
            chader.SetTexture(tempTex, "tempMonde");
            chader.SetTexture(mondePosTex, "mondePosTex");
            chader.SetTexture(lumLocTex_1, "vueMonde");
            chader.SetTexture(lumLocTex_2, "ombre3DCard");
            chader.SetTexture(lod_1, "LOD_1");
            chader.SetTexture(tex_1, "tex_1");
            chader.CreerEtSetVideBuffer<LOD>(4, 32768, "buffLOD");
            chader.SetBufferPourToutKernels(bufferGeneralInfo, "genInfo");
            chader.SetBufferPourToutKernels(bufferTextureSave, "buffSave");

            chader.Dispatch("Prep", 32, 32, 32);
            chader.Dispatch("Clair", 32, 32, 32);

            mainEcrane.GetComponent<CanvasImageClique>().AFinirClique += AEtePaintir;

            handlerClique = mainEcrane.GetComponent<CliqueImageRender>();
            handlerClique.FinirPaint += AEtePaintir_2;
            handlerClique.CommorcerPaint += PaintCommonce;


            AEtePaintir(Vector3.zero, Vector3.zero);

            posMouse3D = new CoursorInfo3D(Vector3.zero);
            posMouse3D.isCorrectPose = false;
            posMouse3D.isHaseInfo    = false;
            }



        // Update is called once per frame
        void Update()
            {
            bias_1 = mark.transform.position;

            chader.shader.SetVector("camPos", this.transform.position);
            chader.shader.SetFloat("k_1", k_1);
            chader.shader.SetFloat("k_2", k_2);
            chader.shader.SetFloat("k_3", k_3);
            chader.shader.SetFloat("k_4", k_4);
            chader.shader.SetFloat("k_5", k_5);
            chader.shader.SetVector("bias_1", bias_1);
            chader.shader.SetVector("ombreColor", ombreColor);


            chader.Dispatch("Lum_1", 32, 32, 20);

            if (est_1)
                {
                chader.Dispatch("Lum_3", 32, 32, 32);
                chader.Dispatch("LOD_1", 32, 32, 32);
                }

            chader.Dispatch("Main");

            if (Input.GetKeyDown(KeyCode.P))
                {
                Debug.Log("схороняю");
                SauverMondeAFile( PrinceMenadger.instance.routAFolderFiles + "/monde.sav" );
                }

            if (Input.GetKeyDown(KeyCode.L))
                {
                Debug.Log("ЗООгружаю");
                LoadMonde( PrinceMenadger.instance.routAFolderFiles + "/monde.sav" );
                }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B) )
                {

                try
                    {
                    //LoadMonde( EditorPrince.instance.undoRedo.Undo() );
                    LoadPieceMonde(EditorPrince.instance.undoRedo_2.Undo() );
                    }
                catch (Exception ex)
                    {
                    Debug.Log("откат, ошибка " + ex.Message);
                    throw;
                    }                                  
                
                }

            if (Input.GetKeyDown(KeyCode.X))
                {
                chader.Dispatch("Clair", 32, 32, 32);
                Debug.Log("копирую буферную текстуру");
                }
                          
            }

        public CoursorInfo3D GetPosMouse3d(Vector3 pos)
            {
            chader.shader.SetVector("targ_0", pos);
            GeneralInfo [] arrData = new GeneralInfo [1];
            bufferGeneralInfo.GetData(arrData);
            Vector4 posMouse = arrData [0].posCoursor * 256.0F;           
            posMouse3D.pos3d = posMouse;
            posMouse3D.isCorrectPose = (posMouse.x > -1);
            return posMouse3D;
            }

        public void ChangeMonde()
            {
            monde.texture.Apply();
            chader.SetTexture(monde.texture, "Input");
            }

        /// <summary>
        /// команда на рисование
        /// </summary>
        /// <param name="_pos">Позиция экрана</param>
        /// <param name="_col">Цвет рисования</param>
        /// <param name="estScreen">Рисованить экранно-глобально</param>
        /// <param name="estAdd">Добавлять-расскрашивать</param>
        public void Draw(Vector2 _pos, Color _col, float _radius, float _durete, bool estScreen, bool estAdd)
            {
            Vector3 pos = new Vector3(_pos.x, _pos.y, 0);
            chader.shader.SetVector("targ_0", pos);
            chader.shader.SetVector("colorPaint", _col);
            chader.shader.SetBool("estAdd", estAdd);
            chader.shader.SetFloat("rad_1", _radius);
            chader.shader.SetFloat("durete", _durete);

            if (estAdd)
                {
                if (estScreen)
                    {
                    chader.Dispatch("PaintScr", 32, 32, 1);
                    }
                else
                    {
                    chader.Dispatch("PaintWrl", 32, 32, 32);
                    } 
                }

            else
                {
                chader.Dispatch("PCG", 32, 32, 32);
                }
            
            }

        public void DrawBox(Vector3 a, Vector3 b, Color _color)
            {
            chader.shader.SetVector("targ_1", a);
            chader.shader.SetVector("targ_2", b);
            chader.shader.SetVector("colorPaint", _color);

            chader.Dispatch("AdBox", 32, 32, 32);

            AEtePaintir(Vector3.zero, Vector3.zero);
            }

        public void LoadTexEtStart(Texture3D tex)
            {

            }

        /// <summary>
        /// получить текстуру
        /// </summary>
        public void SauverMondeAFile(string _rout)
            {
            Vector4[] colors = new Vector4[monde.sizeMonde];           
            chader.Dispatch("Save", 32, 32, 32);
            bufferTextureSave.GetData(colors);
            SauverMonde mondeS = new SauverMonde(colors);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(_rout, FileMode.OpenOrCreate);
            bf.Serialize(fs, mondeS);
            fs.Flush(true);
            fs.Close();

            bufferTextureSave.Dispose();

            _ = colors;
            /*
            chader.Dispatch(dictKernel["Save"], 32, 32, 32);
            bufferTextureSave.GetData(colorSave);
            SauverMonde mondeS = new SauverMonde(colorSave);                       
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(_rout, FileMode.OpenOrCreate);
            bf.Serialize( fs, mondeS );
            fs.Flush(true);
            fs.Close();
            */
            }

        public void LoadMonde(string _rout)
            {
            ComputeBuffer buffer = new ComputeBuffer(colorSave.Length, 16);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(_rout, FileMode.Open);
            SauverMonde mond = bf.Deserialize(fs) as SauverMonde;
            buffer.SetData(mond.GetArrayValue());
            chader.SetBufferPourToutKernels(buffer, "buffSave");
            chader.Dispatch("Load", 32, 32, 32);
            buffer.Dispose();

            fs.Flush(true);
            fs.Close();
            /*
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(_rout, FileMode.Open);

            SauverMonde mond = bf.Deserialize(fs) as SauverMonde;
            bufferTextureSave.SetData(mond.GetArrayValue());
            chader.SetBufferPourToutKernels(bufferTextureSave, "buffSave");
            chader.Dispatch(dictKernel["Load"], 32, 32, 32);
            */
            }

        public void LoadMonde(SauverMonde _monde)
            {
            if (_monde == null)
                {
                return;
                }

            bufferTextureSave.SetData(_monde.GetArrayValue());
            chader.SetBufferPourToutKernels(bufferTextureSave, "buffSave");
            chader.Dispatch("Load", 32, 32, 32);
            }

        public void LoadPieceMonde(PieceDeMonde _monde)
            {
            int3 minPos = _monde.posMin.Clamp(0, 1000);           
            int3 delta = _monde.posMax - _monde.posMin;
            int3 stepKernel = delta / 4 ;
            int3 recirculeDelta = stepKernel * 4;

            chader.shader.SetVector("targ_3", (Vector3) _monde.posMin);
            chader.shader.SetVector("targ_4", (Vector3) stepKernel * 4);
            chader.shader.SetInt("sizeBuffer", _monde.colors.Length);
            var buff = ChaderSysteme.GetComputeBuffer<ColorSavePiece>(_monde.colors.Length );
            buff.SetData(_monde.colors);
            chader.SetBufferPourToutKernels(buff, "buffSaveLoadPiece");
            int countKernN = buff.count / 16 + 1;
            chader.Dispatch("LoadP", countKernN, 1, 1);                    

            buff.Release();
            }

        public SauverMonde GetCartupedMonde()
            {            
            Vector4[] colors;
            try
                {
                colors = new Vector4[monde.sizeMonde];
                }
            catch (Exception ex)
                {
                Debug.Log("Ошибка создания массива Vector4[] colors в методе GetCartupedMonde() " + ex.Message);
                throw;
                }
            
            chader.Dispatch("Save", 32, 32, 32);
            bufferTextureSave.GetData(colors);
            SauverMonde sav = null;
            try
                {
                sav = new SauverMonde(colors);
                }
            catch (Exception ex)
                {
                Debug.Log("попытка создания экз. SauverMonde, ошибка " + ex.Message);
                throw;
                }

            
            return sav;
            }

        public PieceDeMonde GetPieceDeMonde(int3 _minPos, int3 maxPos)
            {
            /*
            для нового распараллеленного метода рисования
            параметр targ_4 - размер всей области, обрабатываемой шейдеров
            т.е. это максимальное значение, которое может принимать id в шейдере
            количество потоков - (x, y, z)
            x = delta.x / 8 + 1
            y = delta.y / 8 + 1
            z = delta.z / 8 + 1
            где 8 - число групп потов на стороне шейдера
            targ_04 должен использоваться для восстановления целочисленного индекса буфера сохранения в шейдере
            */

            int3 minPos = _minPos;

            int3 delta = maxPos - minPos;

            int3 deltaMax = new int3( delta.GetVaxDim(), delta.GetVaxDim(), delta.GetVaxDim() );

            int3 stepKernel = delta / 4;
            int3 recirculeDelta = stepKernel * 4;           
            chader.shader.SetVector("targ_3", (Vector3) minPos);
            chader.shader.SetVector("targ_4", (Vector3)recirculeDelta);
            chader.shader.SetInt("sizeBuffer", recirculeDelta.Mult());           
            var buff = ChaderSysteme.GetComputeBuffer<ColorSavePiece>(recirculeDelta.Mult());
            chader.SetBufferPourToutKernels(buff, "buffSaveLoadPiece");

            int  countKernN = buff.count / 16 +1;

            chader.Dispatch("SaveP", countKernN, 1, 1 );

            ColorSavePiece[] res = new ColorSavePiece[recirculeDelta.Mult()];
            buff.GetData(res);

            Debug.Log("длина буф. = " + buff.count);

            buff.Release();

            PieceDeMonde mond = new PieceDeMonde(res, (Vector3) minPos, (Vector3) maxPos);
            Debug.Log( "размер = " + mond.GetSizeInMegaByte() );
            return mond;
            }

        #region
        /*
        public PieceDeMonde GetPieceDeMonde(int3 _minPos, int3 maxPos)
            {
            int3 minPos = _minPos;

            int3 delta = maxPos - minPos;

            int3 deltaMax = new int3( delta.GetVaxDim(), delta.GetVaxDim(), delta.GetVaxDim() );

            int3 stepKernel = delta / 4;
            int3 recirculeDelta = stepKernel * 4;           
            chader.shader.SetVector("targ_3", (Vector3) minPos);
            chader.shader.SetVector("targ_4", (Vector3)recirculeDelta);
            chader.shader.SetInt("sizeBuffer", recirculeDelta.Mult());           
            var buff = ChaderSysteme.GetComputeBuffer<ColorSavePiece>(recirculeDelta.Mult());
            chader.SetBufferPourToutKernels(buff, "buffSaveLoadPiece");

            int  countKernN = buff.count / 16 +1;

            chader.Dispatch("SaveP", countKernN, 1, 1 );

            ColorSavePiece[] res = new ColorSavePiece[recirculeDelta.Mult()];
            buff.GetData(res);

            Debug.Log("длина буф. = " + buff.count);

            buff.Release();

            PieceDeMonde mond = new PieceDeMonde(res, (Vector3) minPos, (Vector3) maxPos);
            Debug.Log( "размер = " + mond.GetSizeInMegaByte() );
            return mond;
            }
        */
        #endregion


        void AEtePaintir_2(int3 minPos, int3 maxPos)
            {
            //chader.Dispatch(dictKernel ["Clair"], 32, 32, 32);
            EditorPrince.instance.undoRedo_2.AddAction( GetPieceDeMonde( minPos, maxPos) );
            }

        void PaintCommonce()
            {           
            chader.Dispatch("Clair", 32, 32, 32);
            }

        void AEtePaintir(Vector3 minPos, Vector3 maxPos)
            {

            //EditorPrince.instance.undoRedo.AddAction(GetCartupedMonde());
            }

        public class CoursorInfo3D
            {
            public Vector3 pos3d, pos2d;

            /// <summary>
            /// надодится ли курсор над изображением рендера?
            /// </summary>
            public bool isHaseInfo = false;

            /// <summary>
            /// указывает ли курсор на непустой воксель?
            /// </summary>
            public bool isCorrectPose;

            public CoursorInfo3D(Vector3 posMouse)
                {
                this.pos3d = posMouse;
                }
            }
        }

    public struct LOD
        {
        public int i;
        };

    public struct GeneralInfo
        {
        public Vector4 posCoursor;
        };

    [Serializable]
    public struct ColorSave
        {
        public Vector4 _color;
        };

    [Serializable]
    public struct ColorSavePiece
        {
        public Vector4 _color;
        public int _pos;
        };

    /// <summary>
    /// кусек мира с положением и размером
    /// </summary>
    [System.Serializable]
    public class SauverMorseauMonde
        {
        [SerializeField]
        public Vector4[] color;
        public int3 pos;
        public int3 size;

        public SauverMorseauMonde(Vector4[] _colors)
            {
            color = _colors;
            }
        }
    }
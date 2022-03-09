using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mathan;

public class PainteSys : MonoBehaviour
    {
    
    }

public class ChaderSysteme
    {
    public RenderTexture principaleTexture, inputTexture;
    public ComputeShader shader;
    public string[] kernelesNom;
    int[] kernels;
    public int xThr, yThr, zThr;
    Dictionary<String, int> dictKernel;

    public ChaderSysteme(string _NomShader, string[] _nomsKernel, RenderTexture princTexture)
        {       
        shader = (ComputeShader)Resources.Load(_NomShader);
        kernels = new int[_nomsKernel.Length];
        dictKernel = new Dictionary<string, int>();

        if (kernels == null)
            {
            Debug.Log("nullls");
            }

        kernelesNom = _nomsKernel;
        principaleTexture = princTexture;

        for (int i = 0; i < kernels.Length; i++)
            {
            kernels[i] = shader.FindKernel( _nomsKernel[i] );
            }

        SetTexture(principaleTexture, "Result");        
        }

    public void AddDict(string clai, string nom)
        {
        dictKernel.Add(clai, shader.FindKernel(nom));
        }

    public ChaderSysteme(string _NomShader, string[] _nomsKernel)
        {
        shader = (ComputeShader)Resources.Load(_NomShader);
        kernels = new int[_nomsKernel.Length];
        dictKernel = new Dictionary<string, int>();

        if (kernels == null)
            {
            Debug.Log("nullls");
            }

        kernelesNom = _nomsKernel;

        for (int i = 0; i < kernels.Length; i++)
            {
            kernels[i] = shader.FindKernel(_nomsKernel[i]);
            }

        }

    public void SetTexture(RenderTexture _texture, string _nom)
        {

        if (kernels == null)
            {
            Debug.Log("нулевой кернель");
            }

        for (int i = 0; i < kernels.Length; i++)
            {
            shader.SetTexture(kernels[i], _nom, _texture);
            }
        
        }

    public void SetBufferPourToutKernels(ComputeBuffer _buffer, string _nom)
        {
        for (int i = 0; i < kernels.Length; i++)
            {
            shader.SetBuffer(kernels[i], _nom, _buffer);
            }
        }

    public void SetTexture(Texture _texture, string _nom)
        {
        for (int i = 0; i < kernels.Length; i++)
            {
            shader.SetTexture(kernels[i], _nom, _texture);
            }

        }

    public static RenderTexture GetRT(int x, int y, RenderTextureFormat format = RenderTextureFormat.ARGBFloat)
        {
        RenderTexture mainTex;
        mainTex = new RenderTexture(x, y, 24);
        mainTex.enableRandomWrite = true;
        mainTex.format = (RenderTextureFormat)11;
        mainTex.Create();

        return mainTex;
        }

    public static RenderTexture GetRt3D(int x, int y, int z, RenderTextureFormat format = RenderTextureFormat.ARGBFloat)
        {
        RenderTexture rt3d = new RenderTexture(x, y, 0);
        rt3d.volumeDepth = z;
        rt3d.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        rt3d.enableRandomWrite = true;

        rt3d.wrapMode = TextureWrapMode.Clamp;

        rt3d.Create();
        return rt3d;
        }

    public void SetNumeroThredes(int x, int y, int z)
        {
        xThr = x;
        yThr = y;
        zThr = z;
        }

    public void SetInt(string nom, int value)
        {
        shader.SetInt(nom, value);            
        }

    public void SetFloat(string nom, float value)
        {
        shader.SetFloat(nom, value);
        }

    public void Dispatch(string clai)
        {
        shader.Dispatch( dictKernel[clai], xThr, yThr, zThr);
        }

    public void Dispatch(string clai, int x, int y, int z)
        {
        shader.Dispatch( dictKernel[clai] , x, y, z);
        }

    public void Dispatch(int id, int X, int Y, int Z)
        {
        shader.Dispatch(id, X, Y, Z);
        }

    public int GetKernel(string _name)
        {

        for (int i = 0; i < kernels.Length; i++)
            {
            if (kernelesNom[i].Equals(_name))
                {
                return kernels[i];
                }
            }

        return -1;
        }

    public void SetPublicTexture(RenderTexture texture, string _nom)
        {
        for (int i = 0; i < kernels.Length; i++)
            {
            shader.SetTexture(kernels[i], _nom, texture);
            }
        }

    public void SetPrivateTexture(int3 _size, string _name, RenderTextureFormat format = RenderTextureFormat.Default)
        {
        RenderTexture rt = new RenderTexture(_size.x, _size.y, _size.z, format);
        rt.enableRandomWrite = true;
        rt.Create();

        for (int i = 0; i < kernels.Length; i++)
            {
            shader.SetTexture(kernels[i], _name, rt);
            }
        }

    public void ActuallyStart()
        {

        }

    public void CreerEtSetVideBuffer<T>(int sizeElement, int countBuffer, string nom)
        {  
        ComputeBuffer buffer = new ComputeBuffer(countBuffer, sizeElement);
        List<T> inData = new List<T>(countBuffer);
        buffer.SetData(inData.ToArray());
        SetBufferPourToutKernels(buffer, nom);
        }

    public void CreerEtSetVideBuffer<T>(int countBuffer, string nom)
        {
        int sizeElement = 0;
        unsafe
            {
            sizeElement = System.Runtime.InteropServices.Marshal.SizeOf( typeof( T) );

            Debug.Log("размер структуры = " + sizeElement);
            }

        if (sizeElement == 0)
            {
            throw new Exception("Не получилось узнать тип структуры");            
            }

        ComputeBuffer buffer = new ComputeBuffer(countBuffer, sizeElement);
        List<T> inData = new List<T>(countBuffer);
        buffer.SetData(inData.ToArray());
        SetBufferPourToutKernels(buffer, nom);
        }

    /// <summary>
    /// получить пустой буффер
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="countBuffer">число элементов</param>
    /// <returns></returns>
    public static ComputeBuffer GetComputeBuffer<T>(int countBuffer)
        {
        int sizeElement = 0;
        unsafe
            {
            sizeElement = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            }

        if (sizeElement == 0)
            {
            throw new Exception("Не получилось узнать тип структуры");
            }

        ComputeBuffer buffer = new ComputeBuffer(countBuffer, sizeElement);
        List<T> inData = new List<T>(countBuffer);
        buffer.SetData(inData.ToArray());
        return buffer;
        }

    }

    

/// <summary>
/// обработчик текстур при помощи вычислительного шейдера
/// </summary>
public class TextureProcessorAvecCSh
    {

    }
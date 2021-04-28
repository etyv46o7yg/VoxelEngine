using Render;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UndoRedo<T>
    {
    public List<T> listState;
    int courrIteration = 0;
    int maxIteration;       

    public UndoRedo(int _maxIteration)
        {
        listState = new List<T>();
        maxIteration = _maxIteration;
        }

    public float GetSizeMegaByte()
        {
        float res = 0.0f;     

        if ( typeof(T) ==  typeof( PieceDeMonde ) )
            {
            res = 0.0f;
            foreach (var item in listState)
                {
                res += (item as PieceDeMonde).GetSizeInMegaByte();
                }
            }

        else
            {
            throw new Exception("Тип Т не поддерживает размер объекта");
            }

        return res;
        }

    public void AddAction(T action)
        {
        if (action == null)
            {
            Debug.Log("ошибка, null при добавлении действия в систему отката");
            return;
            }

        if (listState.Count <= maxIteration)
            {
            courrIteration++;
            listState.Add(action);
            }

        else
            {
            Debug.Log("переполнение");
            listState.RemoveAt(0);
            listState.Add(action);
            }
       
        
        }

    public void SetLimitIteration(int val)
        {

        if (listState.Count > val)
            {
            listState.RemoveRange(0, listState.Count - val);
            }
       
        }

    public T Undo()
        {

        if (listState.Count > 0)
            {
            var temp = listState.Last();

            if (listState.Count > 0)
                {
                listState.RemoveAt(listState.Count - 1);
                }
            
            return temp;
            }

        else
            {
            throw new Exception("Система отката пуста. Нет предыдущего элемента.");
            }
        }

    public class IntedArray
        {
        public int numero;
        public T obj;

        public IntedArray(int index, T _obj)
            {
            numero = index;
            obj = _obj;
            }
        }
    }

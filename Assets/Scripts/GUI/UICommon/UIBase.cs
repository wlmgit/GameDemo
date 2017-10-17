using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour {

    public delegate void HandleOnPress(int id,bool pressed);
    public HandleOnPress PressHandler;

    protected virtual void OnPress(bool pressed)
    {

    } 

    public void AddListener(int id,HandleOnPress handleOnPress)
    {
        ID = id;
        PressHandler += handleOnPress;
    }
    public void AddListener(HandleOnPress handleOnPress)
    {
        PressHandler += handleOnPress;
    }
    public void RemoveListener(HandleOnPress handleOnPress)
    {
        PressHandler -= handleOnPress;
    }

    public int ID
    {
        get;
        protected set;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectedMove : MonoBehaviour
{
    private Action<float,int> action;
    public Button BtnUp;
    public Button BtnRight;
    public Button BtnDown;
    public Button BtnLeft;

    private void Awake()
    {
        BtnUp.onClick.AddListener(OnClickUp);
        BtnLeft.onClick.AddListener(OnClickLeft);
        BtnDown.onClick.AddListener(OnClickDown);
        BtnRight.onClick.AddListener(OnClickRight);
    }

    private void OnDestroy()
    {
        BtnUp.onClick.RemoveListener(OnClickUp);
        BtnLeft.onClick.RemoveListener(OnClickLeft);
        BtnDown.onClick.RemoveListener(OnClickDown);
        BtnRight.onClick.RemoveListener(OnClickRight);
    }

    public void actionRegister(Action<float,int> act)
    {
        if (act != null)
        {
            action += act;
        }
    }

    public void actionUnRegister(Action<float,int> act)
    {
        if (act != null)
        {
            action += act;
        }
    }

    public void Dispatch(float dir,int type)
    {
        if (action != null)
        {
            action.Invoke(dir,type);
        }
    }

    private void OnClickUp()
    {
        Dispatch(-1,0);
    }
    private void OnClickLeft()
    {
        Dispatch(-1,3);
    }

    private void OnClickRight()
    {
        Dispatch(1,1);
       
    }private void OnClickDown()
    {
        Dispatch(1,2);
    }

    public void SetOnOff(bool active)
    {
        gameObject.SetActive(active);
    }
}
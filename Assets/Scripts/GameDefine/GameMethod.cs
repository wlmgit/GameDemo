using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameDefine{
    public static class GameMethod
    {
       public static Camera GetUICamera
        {
            get
            {
                if (UICamera.currentCamera == null)
                {
                    UICamera.currentCamera = GameUI.Instance.transform.Find("Camera").GetComponent<Camera>();
                }
                return UICamera.currentCamera;
            }
        }
    }
}

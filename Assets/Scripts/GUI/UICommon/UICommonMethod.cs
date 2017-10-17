using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UICommon
{
    public class UICommonMethod
    {
        /// <summary>
        /// 修改透明度
        /// </summary>
        /// <param name="gameobj"></param>
        /// <param name="duration"></param>
        /// <param name="alpha"></param>
        public static void TweenColorBegin(GameObject gameobj,float duration,float alpha)
        {
            UIWidget[] mWidgets= gameobj.GetComponentsInChildren<UIWidget>();
            foreach (UIWidget mWidget in mWidgets)
            {
                TweenColor.Begin(mWidget.gameObject,duration,
                    new Color(mWidget.color.r,mWidget.color.g,mWidget.color.b,alpha));
            }
        }

        public static Vector3 GetWorldSpacePos(Vector2 screenPos)
        {
            Vector3 newPos = Vector3.zero;
            Camera camera = GameDefine.GameMethod.GetUICamera;

            if ( camera != null )
            {
                newPos = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y,-camera.gameObject.transform.position.z));
            }
            return newPos;
        }
    }
}


//虚拟摇杆脚本，需要两个sprite子物体，一个stick，underpan
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UICommon;

public class VirtualStickUI : MonoBehaviour {

    #region variables

    public static VirtualStickUI Instance
    {
        get;
        private set;
    }

    public enum StickState
    {
        ActiveState,
        MoveState,
        InActiveState
    }

    public StickState VirtualStickState
    {
        get;
        private set;
    }
    private Vector3 m_orignalPos = new Vector3();
    private Transform m_point;
    private bool m_canUse = true;
    public float m_adjustRadius = 0.25f;
    #endregion

    #region public function
    #endregion

    #region private function

    void Awake()
    {
        Init();
    }

    void Init()
    {
        Instance = this;
        m_orignalPos = transform.localPosition;
        m_point = transform.Find("stick");
        VirtualStickState = StickState.InActiveState;
    }

    private void OnEnable()
    {
        m_canUse = true;
        SetVisiable(false);
    }

    void SetVisiable( bool visiable )
    {
        if (visiable)
        {
            UICommonMethod.TweenColorBegin(gameObject,3f,1);
        }
        else
        {
            UICommonMethod.TweenColorBegin(gameObject, 3f, 0.5f);
        }
    }
    /// <summary>
    /// 显示遥杆
    /// </summary>
    void ShowStick()
    {
        if (m_canUse == false)
            return;
        Vector2 touchPos = UICamera.currentTouch.pos;
        SetPointPos(touchPos);
        VirtualStickState = StickState.MoveState;
    }
    void SetPointPos(Vector2 pos)
    {
        Vector3 newPos = UICommonMethod.GetWorldSpacePos(pos) + new Vector3(0f,0f,m_point.position.z);
        if (Vector3.Distance(newPos,transform.position) > m_adjustRadius )
        {
            Vector3 direction = newPos - transform.position;
            direction.Normalize();
            direction *= m_adjustRadius;
            newPos = transform.position + direction;
        }
        m_point.position = newPos;
    }
    private void OnDisable()
    {
        VirtualStickState = StickState.InActiveState;
    }
    #endregion
}

using UnityEngine;
using System.Collections;

/// <summary>
/// 震动需要的参数：震动类型，震动周期、偏移周期、震动时间、波峰的最大、最小值
/// </summary>
public class ShakeCamera : MonoBehaviour {

    public enum ShakeType
    {
        horizontal, //水平
        vertical,//垂直
        forward//正朝向
    }

    public ShakeType mShakeType = ShakeType.horizontal;

    public float mShakePeriod = 2; //震动周期
    public float mOffsetPeriod = 0;//偏移周期

    public float mShakeTime = 10.0f;//震动时间

    public float mMaxWave = 5;//最大波峰
    public float mMinWave = 1;//最小波峰

    public bool mIsShake = false;

    public Vector3 mDefaultPos; //初始位置

    //振动方向
    public Vector3 mShakeDir;
    public Transform mCameraTrans;

    private float m_curShakeTime = 0;//总时间

	// Use this for initialization
	void Start () {
	
	}

    public Transform GetCameraTrans()
    {
        if (mCameraTrans == null)
        {
            mCameraTrans = gameObject.transform;
        }
        return mCameraTrans;
    }

    public void CameraShake(ShakeType shakeType, float shakePeriod, float shakeTime,float maxWave,float minWave , float offsetPeriod = 0 )
    {
        if (!mIsShake)
        {
            //确保transform
            if (GetCameraTrans() == null)
                return;
            mShakeType = shakeType;
            mShakePeriod = shakePeriod;
            mShakeTime = shakeTime;
            mMaxWave = maxWave;
            mMinWave = minWave;
            mOffsetPeriod = offsetPeriod;

            mDefaultPos = mCameraTrans.localPosition;

            if (mShakeType == ShakeType.vertical)
            {
                mShakeDir = new Vector3(0, 1, 0);
            }
            else if (mShakeType == ShakeType.forward)
            {
                mShakeDir = mCameraTrans.forward;
            }
            else if ( mShakeType == ShakeType.horizontal )
            {
                Vector3 v1 = new Vector3( 0 , 1 ,0 );
                Vector3 v2 = mCameraTrans.forward;

                mShakeDir = Vector3.Cross(v1,v2);
                mShakeDir.Normalize();
            }

            mIsShake = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (mIsShake)
        {
            float factor = m_curShakeTime / mShakeTime;

            //总周期
            float totalPeriod = mShakePeriod * Mathf.PI;

            //当前的时刻值
            float curWave = mMaxWave - (mMaxWave - mMinWave) * factor;

            //当前的弧度值
            float redValue = mOffsetPeriod * Mathf.PI + totalPeriod * factor;
            float value = curWave * Mathf.Sin(redValue);

            if (mShakeType == ShakeType.vertical)
            {
                mCameraTrans.position = new Vector3(mCameraTrans.localPosition.x, mDefaultPos.y, mCameraTrans.localPosition.z) + mShakeDir * value;
            }
            else
            {
                mCameraTrans.position = mDefaultPos + mShakeDir * value;
            }

            m_curShakeTime += Time.deltaTime;

            //结束震动
            if (m_curShakeTime > mShakeTime)
            {
                mIsShake = false;
                m_curShakeTime = 0;
                //恢复到初始位置
                mCameraTrans.position = mDefaultPos;
            }
        }
	}

#if UNITY_EDITOR
    void OnGUI()
    {
        if (GUI.Button(new Rect( 0 , 10 ,100, 30), "shakeCamera"))
        {
            CameraShake(mShakeType, mShakePeriod,mShakeTime, mMaxWave,mMinWave);
            mDefaultPos = mCameraTrans.localPosition;
            mIsShake = true;
        }
    }
#endif
}

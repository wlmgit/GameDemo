using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour
{

    public static ShowFPS Instance
    {
        private set;
        get;
    }

    void OnEnable()
    {
        Instance = this;
    }

    void OnDisable()
    {
        Instance = null;
    }

    public float m_updateInterval = 0.5f;
    private float m_lastInterval;

    private int m_frames = 0;
    private float m_fps;

    private GUIStyle m_style = new GUIStyle();

    // Use this for initialization
    void Start ()
    {
        Application.targetFrameRate = 300;
        m_lastInterval = Time.realtimeSinceStartup;
        m_frames = 0;

        m_style.fontSize = 12;
        m_style.normal.textColor = new Color( 0 , 255 , 0 , 255);
    }

    void OnGUI()
    {
        GUI.Label( new Rect( 0 ,10 ,100 ,100 ) ,"FPS:" + m_fps.ToString("f2") , m_style);
        Debug.Log(m_fps.ToString("f2"));
    }

    // Update is called once per frame
    void Update ()
    {
        ++m_frames;
        if (Time.realtimeSinceStartup > m_lastInterval + m_updateInterval)
        {
            m_fps = m_frames / ( Time.realtimeSinceStartup - m_lastInterval );
            m_frames = 0;
            m_lastInterval = Time.realtimeSinceStartup;
        }
	}
}

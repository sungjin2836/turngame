using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    public static BattleCamera instance;
    
    [SerializeField] private CinemachineVirtualCamera[] cameras;
    private readonly Dictionary<string, CinemachineVirtualCamera> _virtualCameras = new();
    private CinemachineVirtualCamera _camera;

    private CinemachineVirtualCamera m_Camera
    {
        get => _camera;
        set
        {
            if (_camera != null)
            {
                _camera.m_Priority = 10;
            }

            _camera = value;
            _camera.LookAt = m_Enemy;
            _camera.m_Priority = 100;
        }
    }

    private Transform _player;

    public Transform m_Player
    {
        set
        {
            _player = value;
            m_Camera.Follow = _player ? _player.transform : null;
        }
    }

    private Transform _enemy;

    public Transform m_Enemy
    {
        get => _enemy;
        set
        {
            _enemy = value;
            m_Camera.LookAt = _enemy ? _enemy.transform : null;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        foreach (CinemachineVirtualCamera cam in cameras)
        {
            _virtualCameras.TryAdd(cam.name, cam);
        }

        if (m_Camera == null) m_Camera = cameras[0];
    }

    public void MoveTo(string cam, Transform follow = null, Transform lookAt = null)
    {
        m_Camera = _virtualCameras[cam];
        m_Player = follow;
        m_Enemy = lookAt;
    }
}
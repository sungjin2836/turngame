using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] cameras;
    private static readonly Dictionary<string, CinemachineVirtualCamera> VirtualCameras = new();
    
    private static CinemachineVirtualCamera _camera;

    public static CinemachineVirtualCamera m_Camera
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

    private static Transform _player;

    public static Transform m_Player
    {
        set
        {
            _player = value;
            m_Camera.Follow = _player.transform;
        }
    }

    private static Transform _enemy;

    public static Transform m_Enemy
    {
        get => _enemy;
        set
        {
            _enemy = value;
            m_Camera.LookAt = _enemy.transform;
        }
    }

    private void Awake()
    {
        foreach (var cam in cameras)
        {
            VirtualCameras.Add(cam.name, cam);
        }

        if (m_Camera == null) m_Camera = cameras[0];
    }

    public static void MoveTo(string camera, Transform follow = null, Transform lookAt = null)
    {
        m_Camera = VirtualCameras[camera];
        if (follow) m_Player = follow;
        if (lookAt) m_Enemy = lookAt;
    }
}
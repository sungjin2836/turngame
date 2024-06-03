using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    private const float Duration = 0.2f;

    public Transform startPosition;
    public CinemachineVirtualCamera startCamera;
    public float startAngle = -50;
    public float eachAngle = 4;

    public Enemy[] enemies;

    private readonly Dictionary<Transform, float> _targets = new();

    private CinemachineVirtualCamera _camera;

    public CinemachineVirtualCamera m_Camera
    {
        get => _camera;
        set
        {
            if (_camera != null)
            {
                _camera.m_Priority = 10;
            }

            _camera = value;
            _camera.m_Priority = 100;
            _orbital = m_Camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            
            if (enemies == null) return;

            UpdateEnemyList();

            if (_orbital)
            {
                _orbital.m_XAxis.m_MinValue = _targets[enemies[0].transform];
                _orbital.m_XAxis.m_MaxValue = _targets[enemies[^1].transform];
            }
        }
    }

    private CinemachineOrbitalTransposer _orbital;

    private Transform _player;


    public Transform m_Player
    {
        get => _player;
        set
        {
            _player = value;
            m_Camera.Follow = _player.transform;
        }
    }

    private Transform _enemy;


    public Transform m_Enemy
    {
        get => _enemy;
        set
        {
            _enemy = value;
            m_Camera.LookAt = _enemy.transform;
        }
    }

    private float _lastTime;
    private float _lastPosition;

    private void Awake()
    {
        m_Camera = startCamera;
    }

    private void Start()
    {
        UpdateEnemyList();

        m_Player = startPosition;
        m_Enemy = enemies[0].transform;
    }

    private void Update()
    {
        if (!_orbital)
        {
            m_Enemy = _enemy;
            return;
        }
        _orbital.m_XAxis.Value = Mathf.Lerp(_lastPosition, _targets[m_Enemy], (Time.time - _lastTime) / Duration);

        if (!Input.GetMouseButtonDown(0)) return;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit)) return;
        if (!hit.collider.CompareTag("Enemy")) return;
        m_Enemy = hit.transform;
        _lastPosition = _orbital.m_XAxis.Value;
        _lastTime = Time.time;
    }

    private void UpdateEnemyList()
    {
        _targets.Clear();
        
        for (int i = 0; i < enemies.Length; i++)
        {
            _targets.Add(enemies[i].transform, startAngle + i * eachAngle);
        }
    }
}
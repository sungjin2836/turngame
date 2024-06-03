using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    private const float Duration = 0.2f;

    private Enemy[] _enemies;

    private Dictionary<Transform, float> _targets = new();

    [SerializeField] private CinemachineVirtualCamera _camera;

    private CinemachineOrbitalTransposer _orbital;

    private Transform _Player;

    public Transform m_Player
    {
        get => _Player;
        set
        {
            _Player = value;
            _camera.Follow = _Player.transform;
        }
    }

    private Transform _enemy;

    public Transform m_Enemy
    {
        get => _enemy;
        set
        {
            _enemy = value;
            _camera.LookAt = _enemy.transform;
        }
    }

    private float _lastTime;
    private float _lastPosition;

    private void Awake()
    {
        m_Player = FindObjectOfType<Player>().transform;
        _enemies = FindObjectsOfType<Enemy>();
        _orbital = _camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        for (int i = 0; i < _enemies.Length; i++)
        {
            _targets.Add(_enemies[i].transform, -50 + i * 4);
        }

        _orbital.m_XAxis.m_MinValue = _targets[_enemies[0].transform];
        _orbital.m_XAxis.m_MaxValue = _targets[_enemies[^1].transform];

        m_Enemy = _enemies[0].transform;
    }

    private void Update()
    {
        _orbital.m_XAxis.Value = Mathf.Lerp(_lastPosition, _targets[m_Enemy], (Time.time - _lastTime) / Duration);

        if (!Input.GetMouseButtonDown(0)) return;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit)) return;
        if (!hit.collider.CompareTag("Enemy")) return;
        m_Enemy = hit.transform;
        _lastPosition = _orbital.m_XAxis.Value;
        _lastTime = Time.time;
    }
}
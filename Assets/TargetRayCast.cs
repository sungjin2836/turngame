using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRayCast : MonoBehaviour
{
    [SerializeField]
    float _maxDistance = 10.0f;

    Enemy rayEnemy;
    Color _color = Color.red;
    FieldUIController _controller;
    GameObject _gameObject;

    private void Awake()
    {
        _controller = GetComponent<FieldUIController>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;

        if (Physics.BoxCast(transform.position, transform.lossyScale / 2f, transform.forward, out RaycastHit hit, transform.rotation, _maxDistance))
        {
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);

            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, transform.lossyScale);
            
            if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                if (_controller == null)
                {
                    _controller = GetComponent<FieldUIController>();
                    Debug.Log($"if (_controller == null)이면 실행 {_controller}");
                }

                if (_controller != null)
                {
                    _gameObject = hit.collider.gameObject;
                    rayEnemy = _gameObject.GetComponent<Enemy>();
                    Debug.Log($"적 정보 나오는지 {rayEnemy.charName}");
                    _controller.DetectEnemy(rayEnemy);
                }
            }
            else
            {
                _controller = GetComponent<FieldUIController>();
                _controller.NoDetectEnemy();
            }
            
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * _maxDistance);
        }
    }
}

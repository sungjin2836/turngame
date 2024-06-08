using System;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class DebugBattleCamera : MonoBehaviour
{
    [Header("캐릭터 목록")]
    public Player player;
    public Enemy[] enemies;
    
    [Header("카메라 설정")]
    public BattleCamera cam;
    public CinemachineVirtualCamera normalReady;
    public CinemachineVirtualCamera normalAttack;
    public CinemachineVirtualCamera skillReady;
    public CinemachineVirtualCamera skillUse;

    private PlayerInput _input;
    private int _targetIndex = 0;

    private void Awake()
    {
        _input = gameObject.AddComponent<PlayerInput>();
        _input.OnNormalAttack += NormalReady;
        _input.OnBattleSkill += SkillReady;
    }

    private void OnDestroy()
    {
        _input.OnNormalAttack -= NormalReady;
        _input.OnBattleSkill -= SkillReady;
    }

    private void Update()
    {
        _input.ChangeIndex(enemies, ref _targetIndex);
        cam.LookAt(enemies[_targetIndex].transform);
        
        if (!Input.GetMouseButtonDown(0)) return;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)) return;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (hit.transform == enemies[i].transform)
            {
                _targetIndex = i;
                break;
            }
        }
    }

    private void NormalReady()
    {
        cam.m_Camera = normalReady;
    }

    private void NormalAttack()
    {
        cam.m_Camera = normalAttack;
    }

    private void SkillReady()
    {
        cam.m_Camera = skillReady;
    }
    
    private void SkillUse()
    {
        cam.m_Camera = skillUse;
    }
    
    Rect windowRect = new Rect(10, 10, 150, 100);
    
    private void OnGUI()
    {
        windowRect = GUILayout.Window(0, windowRect, ShowWindow, "BattleCamera Test");
    }

    private void ShowWindow(int windowID)
    {
        if (GUILayout.Button("Normal Ready"))
        {
            NormalReady();
        }
        if (GUILayout.Button("Normal Attack"))
        {
            NormalAttack();
        }
        if (GUILayout.Button("Skill Ready"))
        {
            SkillReady();
        }
        if (GUILayout.Button("Skill Use"))
        {
            SkillUse();
        }
    }
}

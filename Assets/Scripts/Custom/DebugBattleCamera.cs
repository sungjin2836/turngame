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

    private void Awake()
    {
        cam.startCamera = normalReady;
        cam.m_Player = player.transform;
        cam.enemies = enemies;
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

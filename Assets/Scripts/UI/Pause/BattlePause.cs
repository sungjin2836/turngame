using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� �Ͻ����� �� �� ������ �� �޴�

public class BattlePause : Pause
{
    public override void Start()
    {
        base.Start();
    }

    public override void TogglePauseState()
    {
        base.TogglePauseState();
        ToggleTimeStop();
    }
    private void ToggleTimeStop()
    {
        Time.timeScale = isPause ? 0 : 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전투에서 일시정지 할 때 나오는 탭 메뉴

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

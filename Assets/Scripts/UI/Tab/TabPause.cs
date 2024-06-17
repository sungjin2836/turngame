using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPause : Pause
{
    [SerializeField]
    private RectTransform tabContentContainer;

    protected override void Start()
    {
        base.Start();

        TogglePauseCanvas(tabContentContainer);
    }

    protected override void TogglePauseState()
    {
        base.TogglePauseState();
        TogglePauseCanvas(tabContentContainer);
        ToggleTimeStop();
    }

    private void ToggleTimeStop()
    {
        Time.timeScale = isPause ? 0 : 1;
    }
}

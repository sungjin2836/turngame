using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPause : Pause
{
    [SerializeField]
    private RectTransform pauseOutsideBorder;

    protected override void Start()
    {
        base.Start();

        AddToggleEvent(pauseOutsideBorder);
        TogglePauseCanvas(pauseOutsideBorder);
    }

    protected override void TogglePauseState()
    {
        base.TogglePauseState();
        TogglePauseCanvas(pauseOutsideBorder);
        Time.timeScale = isPause ? 0 : 1;
        Cursor.lockState = isPause ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
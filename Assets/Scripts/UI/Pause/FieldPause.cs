using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldPause : Pause
{
    [SerializeField]
    private RectTransform pauseOutsideBorder;

    public override void Start()
    {
        base.Start();

        AddToggleEvent(pauseOutsideBorder);
        TogglePauseCanvas(pauseOutsideBorder);
    }

    protected override void TogglePauseState()
    {
        base.TogglePauseState();
        TogglePauseCanvas(pauseOutsideBorder);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattlePause : Pause
{
    [SerializeField]
    private RectTransform pauseOutsideBorder;
    [SerializeField]
    private RectTransform continueButton;
    [SerializeField]
    private RectTransform exitButton;

    public override void Start()
    {
        base.Start();

        AddToggleEvent(pauseOutsideBorder);
        AddToggleEvent(continueButton);
        AddEvent(exitButton, ExitToFieldScene);
        TogglePauseCanvas(pauseOutsideBorder);
    }

    protected override void TogglePauseState()
    {
        base.TogglePauseState();
        TogglePauseCanvas(pauseOutsideBorder);
        Time.timeScale = isPause ? 0 : 1;
    }

    private void ExitToFieldScene()
    {
        SceneManager.LoadScene(0);
    }
}

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

    protected override void Start()
    {
        base.Start();

        AddToggleEvent(pauseOutsideBorder);
        AddToggleEvent(continueButton);
        AddEvent(exitButton, ExitToFieldScene);
        TogglePauseCanvas(pauseOutsideBorder);

        Cursor.lockState = CursorLockMode.None;
    }

    protected override void TogglePauseState()
    {
        base.TogglePauseState();
        TogglePauseCanvas(pauseOutsideBorder);
        Time.timeScale = isPause ? 0 : 1;
    }

    private void ExitToFieldScene()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("FieldScene");
    }
}

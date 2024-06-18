using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldPause : Pause
{
    [SerializeField]
    private RectTransform pauseOutsideBorder;
    [SerializeField]
    private RectTransform characterListButton;

    protected override void Start()
    {
        base.Start();

        AddToggleEvent(pauseOutsideBorder);
        AddEvent(characterListButton, MoveToCharacterListScene);
        TogglePauseCanvas(pauseOutsideBorder);
    }

    protected override void TogglePauseState()
    {
        base.TogglePauseState();
        TogglePauseCanvas(pauseOutsideBorder);
        Time.timeScale = isPause ? 0 : 1;
        Cursor.lockState = isPause ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void MoveToCharacterListScene()
    {
        SceneManager.LoadScene("CharacterListScene");
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform startClickArea;
    [SerializeField]
    private RectTransform exitButton;

    void Start()
    {
        AddEvent(startClickArea, MoveToFieldScene);
        AddEvent(exitButton, ExitGame);
    }

    private void MoveToFieldScene()
    {
        SceneManager.LoadScene(1);
    }

    private void AddEvent(RectTransform target, Action callback)
    {
        EventTrigger targetTrigger = target.GetComponent<EventTrigger>();

        if (targetTrigger == null)
        {
            Debug.LogError("해당 컴포넌트에는 이벤트 트리거가 등록되지 않았어요.");
            return;
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;

        entry.callback.AddListener((eventData) => callback());
        targetTrigger.triggers.Add(entry);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}

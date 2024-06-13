using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

// ESC키는 변동될 일이 없으므로 Pause 클래스에서만 관리 -> 원작게임에서도 키세팅에서는 출력되지만 변경 불가능하게 막혀있음

public class Pause : MonoBehaviour
{
    static public bool isPause = false;
    [SerializeField]
    private RectTransform pausePanel;
    [SerializeField]
    private RectTransform pauseOutsideBorder;
    [SerializeField]
    private RectTransform closeButton;

    public virtual void Start()
    {
        TogglePauseCanvas();


        AddToggleEvent(pauseOutsideBorder);
        AddToggleEvent(closeButton);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseState();
        }
    }

    public virtual void TogglePauseState()
    {
        isPause = !isPause;
        TogglePauseCanvas();
    }

    private void TogglePauseCanvas()
    {
        if (pausePanel != null)
        {
            pausePanel.gameObject.SetActive(isPause);
            pauseOutsideBorder.gameObject.SetActive(isPause);
        }
    }

        private void AddToggleEvent(RectTransform target)
        {
            EventTrigger targetTrigger = target.GetComponent<EventTrigger>();

            if (targetTrigger == null)
            {
                Debug.LogError("해당 컴포넌트에는 이벤트 트리거가 등록되지 않았어요.");
                return;
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;

            entry.callback.AddListener((eventData) => TogglePauseState());
            targetTrigger.triggers.Add(entry);
        }
}

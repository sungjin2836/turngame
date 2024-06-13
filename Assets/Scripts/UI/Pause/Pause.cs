using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

// ESCŰ�� ������ ���� �����Ƿ� Pause Ŭ���������� ���� -> ���۰��ӿ����� Ű���ÿ����� ��µ����� ���� �Ұ����ϰ� ��������

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
                Debug.LogError("�ش� ������Ʈ���� �̺�Ʈ Ʈ���Ű� ��ϵ��� �ʾҾ��.");
                return;
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;

            entry.callback.AddListener((eventData) => TogglePauseState());
            targetTrigger.triggers.Add(entry);
        }
}

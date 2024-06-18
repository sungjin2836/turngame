using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FieldUIManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform weaknessDisplayPanel;

    static public FieldUIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void DisplayWeakness(ElementType element)
    {
        weaknessDisplayPanel.gameObject.SetActive(true);
        Image weaknessIcon = weaknessDisplayPanel.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI weaknessText = weaknessDisplayPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        weaknessIcon.color = Character.ElementColor(element);
        weaknessText.text = $"약점: {element}!";
    }
}

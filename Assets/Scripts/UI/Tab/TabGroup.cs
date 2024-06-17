using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    List<TabButtonItem> tabButtons = new();
    [Header("버튼 뒷배경")]
    [SerializeField]
    private Sprite tabHover;
    [SerializeField]
    private Sprite tabActive;

    public void Subscribe(TabButtonItem button)
    {
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtonItem button)
    {
        ResetSelection();
        button.background.sprite = tabHover;
    }

    public void OnTabExit(TabButtonItem button)
    {
        ResetSelection();
        button.background.sprite = null;
        button.icon.color = Color.white;
    }

    public void OnTabSelected(TabButtonItem button)
    {
        ResetSelection();
        button.background.sprite = tabActive;
        button.icon.color = Color.black;
    }

    private void ResetSelection()
    {
        foreach (TabButtonItem button in tabButtons)
        {
            button.background.sprite = null;
            button.icon.color = Color.white;
        }
    }
}

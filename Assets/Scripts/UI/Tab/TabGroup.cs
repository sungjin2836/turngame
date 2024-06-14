using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    List<TabButtonItem> tabButtons = new();
    [Header("¹öÆ° µÞ¹è°æ")]
    [SerializeField]
    private Sprite tabIdle;
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
        button.background.sprite = tabIdle;
    }

    public void OnTabSelected(TabButtonItem button)
    {
        ResetSelection();
        button.background.sprite = tabActive;
    }

    private void ResetSelection()
    {
        foreach (TabButtonItem tabButton in tabButtons)
        {
            tabButton.background.sprite = tabIdle;
        }
    }
}

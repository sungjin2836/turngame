using System;
using UnityEngine;

public class DebugCharacterList : MonoBehaviour
{
    private CharacterList _list;
    
    private void Awake()
    {
        _list = GetComponent<CharacterList>();
    }

    private void Start()
    {
        _list.AddCharacter(1001);
        _list.AddCharacter(1002);
        _list.AddCharacter(1003);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Write Current Data"))
        {
            Debug.Log(_list.WriteData());
        }
    }
}
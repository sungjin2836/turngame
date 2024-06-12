using System;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction
{
    NormalAttack,
    BattleSkill,
    Sprint,
    Interact,
    Pause
}

public class PlayerInput : MonoBehaviour
{
    private static Dictionary<KeyAction, KeyCode> _keys = new();

    private KeyCode[] _defaultKeys =
        { KeyCode.Q, KeyCode.E, KeyCode.LeftShift, KeyCode.F, KeyCode.Escape };

    public Action OnNormalAttack;
    public Action OnBattleSkill;

    public bool Sprint => Input.GetKey(_keys[KeyAction.Sprint]) || Input.GetMouseButton(1);
    public bool Interact => Input.GetKeyDown(_keys[KeyAction.Interact]);
    public bool Confirm => Input.GetButtonDown("Submit");
    public bool Cancel => Input.GetButtonDown("Cancel");

    public Vector3 Axis
    {
        get
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            return new Vector3(h, 0, v);
        }
    }

    private void Awake()
    {
        for (int i = 0; i < Enum.GetValues(typeof(KeyAction)).Length; i++)
        {
            _keys.Add((KeyAction)i, _defaultKeys[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(_keys[KeyAction.NormalAttack]))
        {
            OnNormalAttack?.Invoke();
        }

        if (Input.GetKeyDown(_keys[KeyAction.BattleSkill]))
        {
            OnBattleSkill?.Invoke();
        }

        //if (Input.GetKeyDown())
        //{

        //}
    }

    public void ChangeIndex<T>(T[] array, ref int index)
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            index += (int)Axis.x;
        }

        index = Mathf.Clamp(index, 0, array.Length - 1);
    }

    public void ChangeKey(KeyAction key)
    {
        Event currentEvent = Event.current;
        if (currentEvent.isKey) _keys[key] = currentEvent.keyCode;
    }
}
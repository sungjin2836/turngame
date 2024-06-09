using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(150)]
public class FieldUIController : MonoBehaviour
{
    public Player player;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        
        text = GetComponent<Text>();
        text.text = player.charName;
    }
}

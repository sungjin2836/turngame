using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float speed = 5f;

    public GameObject player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        Vector3 moveVector = new Vector3(dir.x * speed * Time.deltaTime, (dir.y+3) * speed * Time.deltaTime, 0.0f);

        this.transform.Translate(moveVector);
    }
}

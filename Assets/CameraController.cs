using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;

    void Start()
    {
        offset = new Vector3(0, 2, -5);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Vector3 dir = player.transform.position - this.transform.position;
        //Vector3 moveVector = new Vector3(dir.x * speed * Time.deltaTime, (dir.y+3) * speed * Time.deltaTime, 0.0f);

        transform.position =player.transform.position + player.transform.rotation * offset;

        transform.LookAt(player.transform.position + Vector3.up * 1f);
    }
}

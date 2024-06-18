using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    float speed = 5f;
    float fastSpeed = 10f;
    bool isRun;
    Rigidbody rbody;
    Rotate rotate;
    [SerializeField]
    private GameObject targetRange;

    public UnityEngine.UI.Button runButton;

    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    Vector3 moveVelocity;

    [SerializeField]
    private GameObject swordEffect;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        targetRange.GetComponent<BoxCollider>().enabled = false;
        targetRange.GetComponent<MeshRenderer>().enabled = false;
        isRun = false;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (Pause.isPause || GoInTurnGame.isSceneMove)
        {
            return;
        }

        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xMove, 0, zMove).normalized;
        if (isRun)
        {
            moveVelocity = transform.TransformDirection(moveDirection) * fastSpeed;
        }
        else
        {
            moveVelocity = transform.TransformDirection(moveDirection) * speed;
        }
        rbody.velocity = new Vector3(moveVelocity.x, rbody.velocity.y, moveVelocity.z);

        //마우스 회전
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 회전각 제한

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f); // 카메라 회전만 위아래 설정

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnFieldNormalAttack();
            CreateSwordEffect();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse1))
        {
            OnRunButtonClick();
        }
    }

    public void OnFieldNormalAttack()
    {
        //targetRange.SetActive(true);
        targetRange.GetComponent<BoxCollider>().enabled = true;
        targetRange.GetComponent<MeshRenderer>().enabled = true;
        Invoke("TargetActive", 1f);
    }

    private void TargetActive()
    {
        //targetRange.SetActive(false);
        targetRange.GetComponent<BoxCollider>().enabled = false;
        targetRange.GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnRunButtonClick()
    {
        isRun = !isRun;
        ColorBlock colorBlock = runButton.colors;
        if (isRun)
        {
            colorBlock.normalColor = Color.gray;
            colorBlock.selectedColor = Color.gray;
        }
        else
        {
            colorBlock.normalColor = Color.white;
            colorBlock.selectedColor = Color.white;
        }
        runButton.colors = colorBlock;
    }

    private void CreateSwordEffect()
    {
        GameObject effect = Instantiate(swordEffect);
        effect.transform.position = gameObject.transform.position + transform.forward;
    }

    
}

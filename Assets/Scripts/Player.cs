using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float currentSpeed = 5.0f;
    private float normalSpeed;
    [SerializeField] private GameObject _camera;
    public bool canMove = true;


    private float _cameraHeight;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        normalSpeed = currentSpeed;

    }
    
    private void Update()
    {
        if (!canMove)
            return;

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            _cameraHeight = _camera.transform.position.y;
            Vector3 CurPos = _camera.transform.position;
            _camera.transform.position = new Vector3(CurPos.x, _cameraHeight, CurPos.z);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            Vector3 CurPos = _camera.transform.position;
            _camera.transform.position = new Vector3(CurPos.x, CurPos.y, CurPos.z);
            currentSpeed = normalSpeed;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
            return;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 moveDirection = new Vector3(horizontal,0, vertical) * currentSpeed * Time.deltaTime;
        transform.Translate(moveDirection);
    }
}

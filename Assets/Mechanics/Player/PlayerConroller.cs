using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerConroller : MonoBehaviour
{
    [Header("Movment")]
    [SerializeField]
    float moveSpeed = 15f;
    [SerializeField]
    bool rotateMovment = false;

    [Header("Gun")]
    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    float cooldown = 0.25f;

    [Header("Camera")]
    [SerializeField]
    Texture2D cursorTexture;
    new Camera camera;

    Health health;
    PlayerInputActions inputActions;

    CharacterController controller;
    private void Awake()
    {
        health = GetComponent<Health>();
        health.OnDeath += HandlerDeath;

        inputActions = new PlayerInputActions();

        controller = GetComponent<CharacterController>();

        camera = Camera.main;

    }


    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2), CursorMode.Auto);
        inputActions.Player.Attack.started += Shooting;
    }

    void Update()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 movmentVector = new Vector3(inputVector.x, 0, inputVector.y);
                
        movmentVector = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * movmentVector;
        controller.Move(movmentVector * (moveSpeed * Time.deltaTime));
       
        if (!rotateMovment)
        {   
            MouseRotate();
        }
        else
        {
            MovmentRotate(movmentVector);
        }
        
    }

    void MovmentRotate(Vector3 movmentVector)
    {
        if (movmentVector.magnitude == 0)
            return;
        var rotation = Quaternion.LookRotation(movmentVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 15f);
    }

    void MouseRotate()
    {
        Ray cameraRay = camera.ScreenPointToRay(inputActions.Player.Pointer.ReadValue<Vector2>());

        if (Physics.Raycast(cameraRay, out RaycastHit raycastHit, 300f))
        {
            var target = raycastHit.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }
    }

    float curCooldown = 0;
    public void Shooting(InputAction.CallbackContext callback)
    {
        if (Time.time > curCooldown)
        {
            curCooldown = Time.time + cooldown;
            Instantiate(projectile, spawnPoint.transform.position, transform.localRotation);
        }
    }

    void HandlerDeath()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Medicine":
                if (health.CanAddHealth(25))
                {
                    health.AddHealth(25);
                    Destroy(other.gameObject);
                }
                return;
            case "Projectile":
                Projectile projectile = other.GetComponent<Projectile>();
                health.AddHealth(-projectile.damage/10);
                return;

            case "Weapon":
                Weapon weapon = other.GetComponent<Weapon>();
                health.AddHealth(-weapon.Damage / 10);
                return;
        }
    }

}


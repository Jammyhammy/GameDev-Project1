using UnityEngine;
using System.Collections;

// Handles all of the FPS controls for Luke.

[RequireComponent(typeof(CharacterController))]
public class FPSControl : MonoBehaviour
{
    public float speed = 6.0f;
    public float animSpeed = 2.0f;
    public float sprintSpeed = 10.0f;
    public bool canSprint = true;
    public float sprintJumpSpeed = 8.0f;
    public float normSpeed = 6.0f;
    public float crouchSpeed = 6.0f;
    public float crouchDeltaHeight = 0.5f;
    public float jumpSpeed = 8.0f;
    public float normJumpSpeed = 8.0f;
    public float gravity = 9.0f;

    public GameObject mainCamera;
    public GameObject weapon;
    public bool crouching = false;
    public bool stopCrouching = false;
    public float standardCamHeight;
    public float crouchingCamHeight;
    public Vector3 moveDirection = Vector3.zero;
    public bool grounded = false;
    public bool walking = false;
    public bool isAttacking = false;
    public AudioSource meleeSound;
    public AudioSource laserSound;
    public GameObject laserPrefab;
    public GameObject lightSaberBlade;

    public PlayerCharacter player;

    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        speed = normSpeed;
        mainCamera = Camera.main.gameObject;
        crouching = false;
        standardCamHeight = mainCamera.transform.localPosition.y;
        crouchingCamHeight = standardCamHeight - crouchDeltaHeight;
        player = GetComponent<PlayerCharacter>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(mainCamera.transform.localPosition.y > standardCamHeight) {
            var mainCameraPos = new Vector3(mainCamera.transform.localPosition.x, standardCamHeight, mainCamera.transform.localPosition.z);
            mainCamera.transform.localPosition = mainCameraPos;
        } else if(mainCamera.transform.localPosition.y < crouchingCamHeight) {
            var mainCameraPos = new Vector3(mainCamera.transform.localPosition.x, crouchingCamHeight, mainCamera.transform.localPosition.z);
            mainCamera.transform.localPosition = mainCameraPos;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl)) {
            if(crouching) {
                stopCrouching=true;
                NormalSpeed();
                return;
            }

            if(!crouching)
                Crouch();
        }
        if(Input.GetKey(KeyCode.LeftShift)) {
            speed = sprintSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            NormalSpeed(); 
        }
        if(crouching){
            if(mainCamera.transform.localPosition.y > crouchingCamHeight){
                var changeHeight = crouchDeltaHeight*Time.deltaTime*8;
                if(mainCamera.transform.localPosition.y - changeHeight < crouchingCamHeight){
                    var mainCameraPos = new Vector3(mainCamera.transform.localPosition.x, changeHeight, mainCamera.transform.localPosition.z);
                    mainCamera.transform.localPosition = mainCameraPos;
                } else {
                    var mainCameraPos = new Vector3(mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y - changeHeight, mainCamera.transform.localPosition.z);
                    mainCamera.transform.localPosition = mainCameraPos;
                }
            }
        } else {
            var changeHeight = crouchDeltaHeight*Time.deltaTime*8;
            if(mainCamera.transform.localPosition.y < standardCamHeight){
                if(mainCamera.transform.localPosition.y + changeHeight > standardCamHeight){
                    var mainCameraPos = new Vector3(mainCamera.transform.localPosition.x, changeHeight, mainCamera.transform.localPosition.z);
                    mainCamera.transform.localPosition = mainCameraPos;
                } else {
                    var mainCameraPos = new Vector3(mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y + changeHeight, mainCamera.transform.localPosition.z);
                    mainCamera.transform.localPosition = mainCameraPos;
                }
            }
        }
        if(player.lightSaberDepleted) {
            lightSaberBlade.SetActive(false);
        }

        if(!player.lightSaberDepleted) {
            lightSaberBlade.SetActive(true);
        }        

        if((Input.GetMouseButton(0) || Input.GetKey(KeyCode.F)) && !player.lightSaberDepleted) {
            Attack();
        }

        if((Input.GetMouseButton(1) || Input.GetKey(KeyCode.G)) && !player.lightSaberDepleted) {
            LaserAttack();
        }        

        if(isAttacking) {
            weapon.transform.localRotation *= Quaternion.AngleAxis(-5, Vector3.left);
        }
    }

    public void Attack(){
        if(!isAttacking)
        {
            meleeSound.Play();
            isAttacking = true;
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 3f)) {
                if(hit.collider.gameObject.CompareTag("Enemy")) {
                    hit.collider.gameObject.GetComponent<EnemyCharacter>().TakeDamage(100);
                }
            }
            StartCoroutine(StopAttack());
        }
    }

    public void LaserAttack(){
        if(!isAttacking)
        {
            laserSound.Play();
            Instantiate(laserPrefab, transform.position + transform.forward * 1.0f, transform.rotation);
            isAttacking = true;
            StartCoroutine(StopAttack());
        }
    }    

    IEnumerator StopAttack() {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        weapon.transform.localRotation = Quaternion.Euler(-60, -30, 0);
    }

    void FixedUpdate()
    {
        if(grounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            
            if (Input.GetButton ("Jump")) {
                moveDirection.y = jumpSpeed;
                if(crouching){
                    stopCrouching = true;
                    NormalSpeed();
                }
            }            
        }
        moveDirection.y -= gravity * Time.deltaTime;
        var controller = GetComponent<CharacterController>();
        var flags = controller.Move(moveDirection * Time.deltaTime);	
        grounded = (flags & CollisionFlags.CollidedBelow) != 0;
        
        if((Mathf.Abs(moveDirection.x) > 0) && grounded || (Mathf.Abs(moveDirection.z) > 0 && grounded)){
            if(!walking){
                walking = true;
            }
        } else if(walking){
            walking = false;
        }
    }

    void Aiming() {
        speed = animSpeed;
    }

    void Crouch() {
        speed = crouchSpeed;
        var mainCameraPos = new Vector3(mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y - crouchDeltaHeight, mainCamera.transform.localPosition.z);
        mainCamera.transform.localPosition = mainCameraPos;
        weapon.transform.localPosition = new Vector3(weapon.transform.localPosition.x, weapon.transform.localPosition.y - crouchDeltaHeight, weapon.transform.localPosition.z);
        this.GetComponent<CharacterController>().height -= crouchDeltaHeight;
        this.GetComponent<CharacterController>().center -= new Vector3(0,crouchDeltaHeight/2, 0);
        crouching = true;        
    }

    void NormalSpeed() {
        if(stopCrouching){
            crouching = false;
            var mainCameraPos = new Vector3(mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y + crouchDeltaHeight, mainCamera.transform.localPosition.z);
            mainCamera.transform.localPosition = mainCameraPos;
            weapon.transform.localPosition = new Vector3(weapon.transform.localPosition.x, weapon.transform.localPosition.y + crouchDeltaHeight, weapon.transform.localPosition.z);
            //weaponCamera.transform.position.y += crouchDeltaHeight;
            this.GetComponent<CharacterController>().height += crouchDeltaHeight;
            this.GetComponent<CharacterController>().center += new Vector3(0,crouchDeltaHeight/2, 0);
            stopCrouching = false;		
        } else if(crouching){
            speed = crouchSpeed;
            return;
        }
            speed = normSpeed;
            jumpSpeed = normJumpSpeed;
    }
    void Sprinting() {
        if(crouching){
            crouching = false;
            var mainCameraPos = new Vector3(mainCamera.transform.localPosition.x, mainCamera.transform.localPosition.y + crouchDeltaHeight, mainCamera.transform.localPosition.z);
            mainCamera.transform.localPosition = mainCameraPos;
            weapon.transform.localPosition = new Vector3(weapon.transform.localPosition.x, weapon.transform.localPosition.y + crouchDeltaHeight, weapon.transform.localPosition.z);
            this.GetComponent<CharacterController>().height += crouchDeltaHeight;
            this.GetComponent<CharacterController>().center += new Vector3(0,crouchDeltaHeight/2, 0);

        }
        if(canSprint){
            speed = sprintSpeed;
            jumpSpeed = sprintJumpSpeed;
        }
    }    

}
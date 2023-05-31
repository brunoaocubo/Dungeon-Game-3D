using System.Collections;
using UnityEngine;
using GlobalConstants;

public enum PlayerState 
{
    Walking,
    Attacking,
    Dodging
}

public class PlayerController : MonoBehaviour
{
    #region VARIAVEIS...
    [Header("REFERENCES")]
    [SerializeField] private Inputs inputs;
    [SerializeField] private GameObject hitParticle;

    [Header("STATUS")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float swordDamage;

    [Header("Sound_Fx")]
    [SerializeField] private GameObject walkSound;
    [SerializeField] private GameObject evasionSound;
    [SerializeField] private AudioSource slashSound;
    [SerializeField] private AudioSource slashAirSound;



    private PlayerAnimation _playerAnimation;
    private Health _playerHealth;
    private Rigidbody _playerRigdbody;
    private Vector2 _inputVector;
    private float _timeAnimationDodge = 0.6f;
    private float _timeCooldownAttack = 1f;
    private bool _isWalking;
    private bool _isDodge = false;
    #endregion

    #region STARTS, UPDATES...

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _playerRigdbody = GetComponent<Rigidbody>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerHealth = GetComponentInChildren<Health>();
    }

    void Start()
    {
        walkSound.SetActive(false);
        evasionSound.SetActive(false);

        _inputVector = inputs.GetMovementVectorNormalized();
        inputs.OnInteractAction += Inputs_OnInteractAction;
        inputs.OnAttackAction += Inputs_OnAttackAction;
        inputs.OnDodgeAction += Inputs_OnDodgeAction;

        StartCoroutine(InstancePlayerAnimation());
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation();
    }

    private void Update()
    {
        if(_playerAnimation != null) 
        {
            _playerAnimation.SetWalkAnimation(_isWalking);
            _playerAnimation.SetDodgeAnimation(_isDodge);
        }

        if (_playerHealth.DamageTaken) 
        {
            _playerAnimation.PlayGetHitAnimation();
        }

        if(_isDodge)
        {
            evasionSound.SetActive(true);
        }
        else 
        {
            evasionSound.SetActive(false);   
        }
    }
    #endregion

    #region EVENTS
    private void Inputs_OnInteractAction(object sender, System.EventArgs e)
    {

    }

    private void Inputs_OnAttackAction(object sender, System.EventArgs e)
    {
        StartCoroutine(Attack());
    }

    private void Inputs_OnDodgeAction(object sender, System.EventArgs e)
    {     
        if (!_isDodge)
        {
            _isDodge = true;
            _playerRigdbody.AddForce(transform.forward * dodgeForce, ForceMode.Impulse);
            StartCoroutine(CalculatorCooldown(_isDodge, _timeAnimationDodge));
        }
    }
    #endregion

    #region METHODS
    private void PlayerMovement() 
    {
        _inputVector = inputs.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(_inputVector.x, 0, _inputVector.y);
        moveDir = _playerRigdbody.rotation * moveDir;
        _playerRigdbody.MovePosition(_playerRigdbody.position + moveDir * moveSpeed * Time.fixedDeltaTime);      
        _isWalking = (moveDir != Vector3.zero);

        if (_isWalking && _isDodge == false) 
        {
            walkSound.SetActive(true);
            
        }
        else 
        {
            walkSound.SetActive(false);
        }
    }

    private void PlayerRotation() 
    {
        Vector3 rotate = new Vector3(0, _inputVector.x, 0) * rotateSpeed * 100f;
        _playerRigdbody.MoveRotation(_playerRigdbody.rotation * Quaternion.Euler(rotate * Time.fixedDeltaTime));
    }

    private void AttackComplement() 
    {
        Collider[] getEnemies = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider collider in getEnemies) 
        {
            if(collider != null)
            {
                if (collider.gameObject.layer == Constants.OBJECT_DESTRUCTABLE)
                {
                    slashSound.Play();
                    Destroy(collider.gameObject);
                }
                else if (collider.gameObject.layer == Constants.ENEMY)
                {
                    slashSound.Play();
                    collider.GetComponent<Enemy>().TakeDamage(collider.transform.TransformPoint(Vector3.up), swordDamage);
                }
                else 
                {
                    slashAirSound.Play();
                }
            }
        }
    }
    #endregion

    #region COROUTINES

    IEnumerator CalculatorCooldown(bool x, float timeAnimation)
    {
        
        yield return new WaitForSeconds(timeAnimation); 
        if (x == _isDodge) { _isDodge = false; }
    }

    IEnumerator Attack()
    {       
        _playerAnimation.PlayAttackAnimation();
        AttackComplement();

        yield return new WaitForSeconds(_timeCooldownAttack);
    }

    IEnumerator Die() 
    {
        _playerAnimation.PlayDieAnimation();
        //animator.SetTrigger(Constants.DIE);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator InstancePlayerAnimation()
    {
        yield return new WaitForEndOfFrame();
        _playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }
    #endregion

    #region FUNCTIONS
    public bool IsWalking()
    {
        return _isWalking;
    }

    public bool IsDodge() 
    { 
        return _isDodge;  
    }
    #endregion
}

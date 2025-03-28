using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    private float _speed, _accelerationLerpInterpolator;
    [SerializeField] private float _speedMax = 3f, _accelerationFactor = 0.02f, _decelerationFactor = 0.01f, _rotationSpeed = 0.5f;
    private bool _isAccelerating;
    [SerializeField] private AnimationCurve _accelerationCurve;

    private bool _hasBoost = false;
    [SerializeField] private float _boostSpeed = 6f;
    [SerializeField] private float _boostDuration = 2f;
    [SerializeField] private Image _boostIcon;

    private bool _isSlowed = false;
    [SerializeField] private float _slowSpeed = 1.5f;
    private float _defaultSpeedMax;

    private bool _isGiant = false;
    [SerializeField] private Transform _carTransform;

    private bool _hasGiantItem = false;
    private float _giantSizeMultiplier;
    private float _giantDuration;
    [SerializeField] private Image _giantItemIcon;

    private bool _hasBanana = false;
    [SerializeField] private GameObject bananaPrefab;
    [SerializeField] private Transform dropPosition;
    [SerializeField] private Image _bananaIcon;

    private bool _canJump = false;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private Image _jumpItemIcon;

    private void Start()
    {
        _defaultSpeedMax = _speedMax;
        if (_giantItemIcon != null) _giantItemIcon.enabled = false;
        if (_jumpItemIcon != null) _jumpItemIcon.enabled = false; 
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.eulerAngles += Vector3.down * _rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.eulerAngles += Vector3.up * _rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isAccelerating = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isAccelerating = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (_hasBoost)
            {
                StartCoroutine(UseBoost());
            }
            else if (_hasGiantItem)
            {
                ActivateGiantMode(_giantSizeMultiplier, _giantDuration);
                _hasGiantItem = false;
                if (_giantItemIcon != null) _giantItemIcon.enabled = false;
            }
            else if (_hasBanana)
            {
                DropBanana();
                _bananaIcon.enabled = false;
            }
            else if (_canJump)
            {
                Jump();
                _canJump = false;
                if (_jumpItemIcon != null)
                {
                    _jumpItemIcon.enabled = false; 
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isAccelerating)
        {
            _accelerationLerpInterpolator += _accelerationFactor;
        }
        else
        {
            _accelerationLerpInterpolator -= _decelerationFactor;
        }

        _accelerationLerpInterpolator = Mathf.Clamp01(_accelerationLerpInterpolator);
        _speed = _accelerationCurve.Evaluate(_accelerationLerpInterpolator) * _speedMax;

        if (_speed > 0.01f)
        {
            _rb.MovePosition(transform.position + transform.forward * _speed * Time.fixedDeltaTime);
        }
    }

    public void ReceiveBoost()
    {
        _hasBoost = true;
        if (_boostIcon != null) _boostIcon.enabled = true;
    }

    private IEnumerator UseBoost()
    {
        if (_boostIcon != null) _boostIcon.enabled = false;

        float originalSpeed = _speedMax;
        _speedMax = _boostSpeed;

        yield return new WaitForSeconds(_boostDuration);

        _hasBoost = false;
        _speedMax = _isSlowed ? _slowSpeed : _defaultSpeedMax;
    }

    public void ApplyTemporaryBoost(float speed, float duration)
    {
        StartCoroutine(TemporaryBoostCoroutine(speed, duration));
    }

    private IEnumerator TemporaryBoostCoroutine(float speed, float duration)
    {
        float originalSpeed = _speedMax;
        _speedMax = speed;

        yield return new WaitForSeconds(duration);

        _speedMax = _isSlowed ? _slowSpeed : _defaultSpeedMax;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlowZone"))
        {
            _isSlowed = true;
            _speedMax = _slowSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlowZone"))
        {
            _isSlowed = false;
            _speedMax = _hasBoost ? _boostSpeed : _defaultSpeedMax;
        }
    }

    public void ReceiveGiantItem(float sizeMultiplier, float duration)
    {
        _hasGiantItem = true;
        _giantSizeMultiplier = sizeMultiplier;
        _giantDuration = duration;
        if (_giantItemIcon != null) _giantItemIcon.enabled = true;
    }

    public void ActivateGiantMode(float sizeMultiplier, float duration)
    {
        StartCoroutine(ActivateGiantModeCoroutine(sizeMultiplier, duration));
    }

    private IEnumerator ActivateGiantModeCoroutine(float sizeMultiplier, float duration)
    {
        if (_isGiant) yield break;

        _isGiant = true;
        _carTransform.localScale *= sizeMultiplier;

        yield return new WaitForSeconds(duration);

        _carTransform.localScale /= sizeMultiplier;
        _isGiant = false;
    }

    public void ReceiveBanana()
    {
        _hasBanana = true;
        if (_bananaIcon != null)
        {
            _bananaIcon.enabled = true;
        }
    }

    private void DropBanana()
    {
        if (bananaPrefab != null && dropPosition != null)
        {
            Instantiate(bananaPrefab, dropPosition.position, Quaternion.identity);
            _hasBanana = false;
        }
    }

    public void EnableJump()
    {
        _canJump = true;
        if (_jumpItemIcon != null)
        {
            _jumpItemIcon.enabled = true; 
        }
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class ObjectController : MonoBehaviour
{
    [SerializeField] private Transform rightPosition;
    [Header("Rotation")]
    [SerializeField] private bool enableHorizontalRotation = false;
    [SerializeField] private bool enableVerticalRotation = false;
    [SerializeField] private float rotationSpeed = 3.0f;
    [SerializeField] private float rotationOffset = 3.0f;
    [Header("Movements")]
    [SerializeField] private bool enableMovements = false;
    [SerializeField] private float movementSpeed = 3.0f;
    [SerializeField] private float movementOffset = 0.1f;

    private GameManager _gameManager;
    private Rigidbody _rigidbody;
    private Vector3 _rotationHorizontalVelocity;
    private Vector3 _rotationVerticalVelocity;
    private Vector3 _movementVelocity;
    public bool IsGameActive { get; set; } = true;
    private bool _isGameVictory = false;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rotationHorizontalVelocity = new Vector3(0, rotationSpeed, 0);
        _rotationVerticalVelocity = new Vector3(rotationSpeed, 0, 0);
        _movementVelocity = new Vector3(movementSpeed, 0, 0);
    }

    private void Update()
    {
        if (!IsGameActive) return;
        if (_isGameVictory)
        {
            IsGameActive = false;
            _gameManager.UnlockLevel(SceneManager.GetActiveScene().buildIndex);
            _gameManager.LoadLevel(0);
        }
        else
        {
            _isGameVictory = true;

            if (enableHorizontalRotation || enableVerticalRotation)
            {
                if (!CheckRotation())
                {
                    _isGameVictory = false;
                }
            }

            if (enableMovements)
            {
                if (!CheckMovement())
                {
                    _isGameVictory = false;
                }
            }

            if (!Input.GetMouseButton(0))
            {
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.velocity = Vector3.zero;
                return;
            }
            if (enableVerticalRotation && Input.GetKey((KeyCode.LeftControl)))
            {
                _rigidbody.angularVelocity = _rotationVerticalVelocity * Input.GetAxis("Mouse X");
            }
            else if (enableMovements && Input.GetKey((KeyCode.LeftShift)))
            {
                _rigidbody.velocity = _movementVelocity * Input.GetAxis("Mouse X");
            }
            else if (enableHorizontalRotation)
            {
                _rigidbody.angularVelocity = _rotationHorizontalVelocity * Input.GetAxis("Mouse X");
            }
        }
    }

    private bool CheckRotation()
    {
        float angle = Quaternion.Angle(transform.rotation, rightPosition.rotation);
        if (angle < rotationOffset) {
            return true;
        }
        return false;
    }

    private bool CheckMovement()
    {
        if (Mathf.Abs(Vector3.Distance(rightPosition.position, 
                transform.position
            )) < movementOffset) {
            return true;
        }
        return false;
    }
}

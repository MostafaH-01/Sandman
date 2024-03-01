using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField]
    private float speed = 17;
    [SerializeField]
    private InputScript inputScript;

    private Vector3 _movement;
    private Rigidbody _rb;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        _movement = new Vector3(inputScript.Movement.x, 0, inputScript.Movement.y);

        _rb.velocity = _movement * speed * Time.deltaTime;
    }

    public void StartConvertNightmare()
    {

    }
    public void CheckNightmareSuccess()
    {

    }
}

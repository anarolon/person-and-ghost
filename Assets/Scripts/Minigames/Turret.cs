using System.Collections;
using System.Collections.Generic;
using PersonAndGhost.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

public class Turret : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer lineRenderer;
    [SerializeField] private LayerMask _spiritLayer;
    [SerializeField] private float _aimSpeed = 2.0f;
    private bool _shoot;
    private float _aimDir;
    
    public void OnShoot(InputAction.CallbackContext context) {
        _shoot = context.action.triggered;
    }

    public void OnAim(InputAction.CallbackContext context) {
        _aimDir = context.ReadValue<float>();
    }

    private void Start() {
        GameObject lineObject = GameObject.FindGameObjectWithTag("Shot"); 
        if (lineObject) {
            lineRenderer = lineObject.GetComponent<LineRenderer>();
        } else {
            // dummy for tests
            lineRenderer = GameObject.Instantiate((GameObject) Resources.Load(Utility.LINEPREFABPATH)).GetComponent<LineRenderer>();
        }
        
    }

    private void Update()
    {
        if(_shoot) {
            StartCoroutine(Shoot());
        }

        gameObject.transform.Rotate(0, 0, _aimDir * Time.deltaTime * _aimSpeed * Mathf.Rad2Deg);
    }

    private IEnumerator Shoot() 
    {
        RaycastHit2D  hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, Mathf.Infinity, _spiritLayer);
        _shoot = false;
        
        if (hitInfo) 
        {
            Spirit spirit = hitInfo.transform.GetComponent<Spirit>();
            if(spirit != null) 
            {
                
                Debug.Log("Hit!");
                spirit.Hit();
            }

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        } else {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 50f);
        }

        lineRenderer.enabled = true;
        // wait one frame approx.
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
    }
}

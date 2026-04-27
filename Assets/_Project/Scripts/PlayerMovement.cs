using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Gun data")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Movement data")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float rotSpeed;

    private float v;
    private float h;

    [Header ("Aim data")]
    [SerializeField] private LayerMask whatIsAimMask;
    [SerializeField] private Transform aimTransform;

    [Header ("Tower data")]
    [SerializeField] private Transform towerTransform;
    [SerializeField] private float towerRotationSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }
    private void Update()
    {
        CheckInputs();

        UpdateAim();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyBodyRotation();
        ApplyTowerRotation();
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = gunPoint.forward * bulletSpeed;

        Destroy(bullet, 7f);

    }

    private void CheckInputs()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        if (v < 0)
        {
            h = -Input.GetAxis("Horizontal");
        }
    }
    
    private void ApplyTowerRotation()
    {
        Vector3 dir = aimTransform.position - towerTransform.position;
        dir.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        towerTransform.rotation = Quaternion.RotateTowards(towerTransform.rotation, targetRotation, towerRotationSpeed);
    }

    private void ApplyBodyRotation()
    {
        transform.Rotate(0, h * rotSpeed, 0);
    }

    private void ApplyMovement()
    {
        Vector3 movement = transform.forward * _moveSpeed * v;
        rb.velocity = movement;
    }

    private void UpdateAim()
    {
        Ray aim = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if(Physics.Raycast(aim, out hit, Mathf.Infinity,whatIsAimMask))
        {
            float fixedY = aimTransform.position.y;
            aimTransform.position = new Vector3(hit.point.x, fixedY, hit.point.z);
        }
    }
}

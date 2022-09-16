using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] private float speed;
    public Vector3 direction;

    private bool isOverlapping = false;
    private Collider blockCollider;
    private RaycastHit hit;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask baseBlocks;
    // Start is called before the first frame update
    void Start()
    {
        blockCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed);
        if(transform.position.x > 3 ||
            transform.position.x < - 3||
            transform.position.z > 3 ||
            transform.position.z < -3)
        {
            direction = -direction;
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    //Check if there has been a hit yet
    //    if (isOverlapping)
    //    {
    //        speed = 0;
    //        Gizmos.color = Color.green;
    //        //Draw a Ray forward from GameObject toward the hit
    //        Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);
    //        //Draw a cube that extends to where the hit exists
    //        Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, blockCollider.bounds.extents * 2);
    //    }
    //    //If there hasn't been a hit yet, draw the ray at the maximum distance
    //    else
    //    {
    //        Gizmos.color = Color.red;
    //        //Draw a Ray forward from GameObject toward the maximum distance
    //        Gizmos.DrawRay(transform.position, Vector3.down * blockCollider.bounds.extents.y);
    //        //Draw a cube at the maximum distance
    //        Gizmos.DrawWireCube(transform.position + Vector3.down * blockCollider.bounds.extents.y, blockCollider.bounds.extents * 2);
    //    }
    //}
    public bool IsOverlapping()
    {
        speed = 0;
        isOverlapping = Physics.BoxCast(blockCollider.bounds.center, blockCollider.bounds.extents, Vector3.down, out hit, transform.rotation, distance, baseBlocks);
        if (isOverlapping) 
        {
            Debug.Log("YAY");
            return true; 
        }
        Debug.Log("NAY");
        return false;
    }


}

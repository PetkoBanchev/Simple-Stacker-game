using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] private float speed;
    public Vector3 direction;
    private bool hasPassedCentre = false;

    private bool isOverlapping = false;
    private Collider blockCollider;
    private RaycastHit hit;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask baseBlocks;
    // Start is called before the first frame update
    void Start()
    {
        hasPassedCentre = false;
        blockCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed);
        
        if((transform.position.x > 3 ||
            transform.position.x < -3 ||
            transform.position.z > 3 ||
            transform.position.z < -3) &&
            hasPassedCentre)
        {
            direction = -direction;
            hasPassedCentre = false;
        }
        
        if (!hasPassedCentre && Physics.Raycast(blockCollider.bounds.center, Vector3.down, 1f, baseBlocks))
        {
            hasPassedCentre = true;
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
            CutTheBlock();
            //Debug.Log("YAY");
            return true; 
        }
       // Debug.Log("NAY");
        return false;
    }

    private void CutTheBlock()
    {
        //z axis
        if (transform.position.x == hit.transform.position.x)
        {
            float distance = Mathf.Abs((hit.transform.position - transform.position).z);
            transform.localScale = new Vector3(transform.localScale.x,
                                               transform.localScale.y,
                                               transform.localScale.z - distance);
            if (transform.position.z > hit.transform.position.z)
            {
                distance *= -1;
            }
            transform.position = new Vector3(transform.position.x,
                                               transform.position.y,
                                               transform.position.z + distance / 2);

            CreateOverhangBlock(new Vector3(transform.position.x,
                                            transform.position.y,
                                            transform.position.z - distance * 1.5f),
                                new Vector3(transform.localScale.x,
                                            transform.localScale.y,
                                            distance));
        }
        //x axis
        if (transform.position.z == hit.transform.position.z)
        {
            float distance = Mathf.Abs((hit.transform.position - transform.position).x);
            transform.localScale = new Vector3(transform.localScale.x - distance,
                                               transform.localScale.y,
                                               transform.localScale.z);
            if (transform.position.x > hit.transform.position.x)
            {
                distance *= -1;
            }
            transform.position = new Vector3(transform.position.x + distance / 2,
                                               transform.position.y,
                                               transform.position.z);

            CreateOverhangBlock(new Vector3(transform.position.x - distance*1.5f,
                                                transform.position.y,
                                                transform.position.z),
                                    new Vector3(distance,
                                                transform.localScale.y,
                                                transform.localScale.z));
        }


    }

    private void CreateOverhangBlock(Vector3 pos, Vector3 size)
    {
        Material cloneMaterial = new Material(Shader.Find("Standard"));

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = size;
        cube.transform.position = pos;
        cube.transform.GetComponent<MeshRenderer>().material = cloneMaterial;
        cube.transform.GetComponent<MeshRenderer>().material.SetColor("_Color",transform.GetComponent<MeshRenderer>().material.GetColor("_Color"));
        cube.AddComponent<Rigidbody>();
        Destroy(cube, 3f);
    }

}

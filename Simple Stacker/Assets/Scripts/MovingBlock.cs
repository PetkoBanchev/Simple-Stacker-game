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

    public bool IsOverlapping()
    {
        speed = 0;
        isOverlapping = Physics.BoxCast(blockCollider.bounds.center, blockCollider.bounds.extents, Vector3.down, out hit, transform.rotation, distance, baseBlocks);
        if (isOverlapping) 
        {
            CutTheBlock();
            return true; 
        }
        return false;
    }

    private void CutTheBlock()
    {
        Vector3 positionOnStop = transform.position; //Caching the stop position so the two new blocks have the same starting positions
        //z axis
        if (positionOnStop.x == hit.transform.position.x)
        {
            float distance = Mathf.Abs((hit.transform.position - positionOnStop).z);
            Vector3 direction = -Vector3.forward;

            transform.localScale = new Vector3(transform.localScale.x,
                                               transform.localScale.y,
                                               transform.localScale.z - distance);

            if (positionOnStop.z > hit.transform.position.z)
            {
                distance *= -1;
                direction *= -1;
            }

            transform.position = new Vector3(positionOnStop.x,
                                             positionOnStop.y,
                                             positionOnStop.z + distance / 2);

            CreateOverhangBlock(new Vector3(positionOnStop.x,
                                            positionOnStop.y,
                                            positionOnStop.z),
                                new Vector3(transform.localScale.x,
                                            transform.localScale.y,
                                            distance),
                                direction);
        }
        //x axis
        if (positionOnStop.z == hit.transform.position.z)
        {
            float distance = Mathf.Abs((hit.transform.position - positionOnStop).x);
            Vector3 direction = -Vector3.right;

            transform.localScale = new Vector3(transform.localScale.x - distance,
                                               transform.localScale.y,
                                               transform.localScale.z);

            if (positionOnStop.x > hit.transform.position.x)
            {
                distance *= -1;
                direction *= -1;
            }

            transform.position = new Vector3(positionOnStop.x + distance / 2,
                                               positionOnStop.y,
                                               positionOnStop.z);

            CreateOverhangBlock(new Vector3(positionOnStop.x,
                                            positionOnStop.y,
                                            positionOnStop.z),
                                new Vector3(distance,
                                            transform.localScale.y,
                                            transform.localScale.z),
                                direction);
        }


    }

    private void CreateOverhangBlock(Vector3 pos, Vector3 size, Vector3 dir)
    {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = size;
        cube.transform.position = pos;
        cube.transform.SetParent(transform);

        if(dir == Vector3.right)
        {
            cube.transform.localPosition = new Vector3(cube.transform.localPosition.x + 0.5f, cube.transform.localPosition.y, cube.transform.localPosition.z);
        }
        else if( dir == -Vector3.right)
        {
            cube.transform.localPosition = new Vector3(cube.transform.localPosition.x - 0.5f, cube.transform.localPosition.y, cube.transform.localPosition.z);
        }
        else if(dir == Vector3.forward)
        {
            cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, cube.transform.localPosition.y, cube.transform.localPosition.z + 0.5f);
        }                                                                                                                                             
        else if(dir == -Vector3.forward)                                                                                                              
        {                                                                                                                                             
            cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, cube.transform.localPosition.y, cube.transform.localPosition.z - 0.5f);
        }

        cube.AddComponent<Rigidbody>();
        cube.transform.GetComponent<Rigidbody>().AddForce(dir * 100);
        cube.transform.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        cube.transform.GetComponent<MeshRenderer>().material.SetColor("_Color", transform.GetComponent<MeshRenderer>().material.GetColor("_Color"));
        Destroy(cube, 3f);
    }

}

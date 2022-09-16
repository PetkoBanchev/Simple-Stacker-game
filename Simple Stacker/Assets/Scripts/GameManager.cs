using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Score = 0;
    [SerializeField] private Vector3[] spawnPosition = new Vector3[4];
    [SerializeField] private Vector3[] directions = new Vector3[4];
    [SerializeField] private GameObject blockPrefab;
    private GameObject currentBlock;
    private GameObject previousBlock;
    private int posCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        SpawnBlock();
        previousBlock = transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (currentBlock.GetComponent<MovingBlock>().IsOverlapping())
            {
                previousBlock = currentBlock;
                previousBlock.transform.SetParent(transform);
                previousBlock.gameObject.layer = LayerMask.NameToLayer("BaseBlocks");
                //previousBlock.gameObject.AddComponent<Rigidbody>();
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.252f, transform.position.z);
                StartCoroutine(Delay());
            }
        }
    }

    private void SpawnBlock()
    {
        currentBlock = Instantiate(blockPrefab, spawnPosition[posCounter], Quaternion.identity);
        currentBlock.GetComponent<MovingBlock>().direction = directions[posCounter];
        posCounter++;
        if (posCounter > 3) { posCounter = 0; }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.25f);
        SpawnBlock();
    }
}

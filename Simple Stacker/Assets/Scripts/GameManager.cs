using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private Vector3[] spawnPosition = new Vector3[4];
    [SerializeField] private Vector3[] directions = new Vector3[4];
    [SerializeField] private GameObject blockPrefab;
    private GameObject currentBlock;
    private GameObject previousBlock;
    private int posCounter = 0;

    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        previousBlock = transform.gameObject;
        SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (currentBlock.GetComponent<MovingBlock>().IsOverlapping())
            {
                GetComponent<AudioSource>().Play();
                score++;
                ScoreManager.Instance.AddPoint();
                previousBlock = currentBlock;
                previousBlock.transform.SetParent(transform);
                previousBlock.gameObject.layer = LayerMask.NameToLayer("BaseBlocks");
                target = new Vector3(transform.position.x, transform.position.y - 0.252f, transform.position.z);
                StartCoroutine(Delay());
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 1);
    }

    private void SpawnBlock()
    {
        
        currentBlock = Instantiate(blockPrefab);
        currentBlock.transform.localScale = previousBlock.transform.lossyScale;
        currentBlock.transform.position = previousBlock.transform.position;
        currentBlock.transform.position += spawnPosition[posCounter];
        currentBlock.GetComponent<MovingBlock>().direction = directions[posCounter];
        currentBlock.name = score + "";
        currentBlock.transform.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((score/50f) % 1f, 1f, 1f)); //change color to create a rainbow effect
        
        posCounter++;
        if (posCounter > 3) { posCounter = 0; }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.25f);
        SpawnBlock();
    }

    public void ResetGame()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        ScoreManager.Instance.ResetScoreText();
    }
}

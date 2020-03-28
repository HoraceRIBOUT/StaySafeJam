using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PopulateTree : MonoBehaviour
{
    public bool createTree = false;
    public bool deleteAllTrees = false;
    public List<GameObject> allTrees = new List<GameObject>();
    public Transform folderTree; 

    public int numberOfCase = 100;
    [Range(0,1)]
    public float chanceOfHavingATree = 0.01f;
    public GameObject treeToSpawn;


    // Start is called before the first frame update
    void Start()
    {
        if (!Application.isPlaying)
            return;

        populate();
    }

    public void Update()
    {


        if (!Application.isPlaying) {
            if (createTree)
                populate();
            if (deleteAllTrees)
                DeleteAllTree();
        }
    }

    private Vector3 minL = new Vector3(0,0,43);
    private Vector3 maxL = new Vector3(88, 0, 0);
    private Vector3 minR = new Vector3(-88, 0, 0);
    private Vector3 maxR = new Vector3(0, 0, -43);
    public Vector2 minMaxHeight = new Vector2(0.4f, 0.4f);
    public Vector2 minMaxLarger = new Vector2(0.4f, 0.4f);

    public void populate()
    {
        DeleteAllTree();
        createTree = false;
        float randomSeed = Random.Range(-0.34765f, 0.9425f) * 100f;

        float lerpStep = 1f / (float)numberOfCase;

        for (float lerpX = 0; lerpX< 1; lerpX+=lerpStep)
        {
            for (float lerpY = 0; lerpY < 1; lerpY += lerpStep)
            {
                if (Mathf.PerlinNoise((randomSeed + Time.time) * 10 * lerpX, (randomSeed + Time.time) * lerpY) < chanceOfHavingATree)
                {

                    Vector3 position = Vector3.Lerp(Vector3.Lerp(minL, maxL, lerpX), Vector3.Lerp(minR, maxR, lerpX), lerpY);
                    Vector3 randomValue = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                    GameObject tree = Instantiate(treeToSpawn, position + randomValue, Quaternion.identity, folderTree);
                    tree.transform.GetChild(0).localScale = new Vector3( 
                        Random.Range(minMaxHeight.x, minMaxHeight.y),
                        Random.Range(minMaxLarger.x, minMaxLarger.y),
                        0.4f);

                    allTrees.Add(tree);
                }
            }
        }



    }

    public void DeleteAllTree()
    {
        deleteAllTrees = false;
        foreach (GameObject g0 in allTrees)
        {
            DestroyImmediate(g0);
        }
        allTrees.Clear();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTrees : MonoBehaviour
{
    public GameObject tree;
    public int numTrees;

    public int area;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numTrees; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-area, area), 1, Random.Range(-area, area));
            Instantiate(tree, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

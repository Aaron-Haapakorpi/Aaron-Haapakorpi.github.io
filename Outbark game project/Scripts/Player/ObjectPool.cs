using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generic t instead to hold the scripts?
/// <summary>
/// basic object pooling for gameobjects, used for bullets, maybe should make bullet object pool to hold bullet scripts to avoid getcomponent
/// </summary>
public class ObjectPool:MonoBehaviour
{
    private Queue<GameObject> queue = new Queue<GameObject>();

    public void Initialize(int count, GameObject g)
    {
        for(int i=0; i< count; i++)
        {
            var b = Instantiate(g);
            b.transform.position = Vector3.zero;
            b.SetActive(false);
            queue.Enqueue(b.gameObject);
        }
    }
    public GameObject GetNext()
    {
        var g = queue.Dequeue();
        queue.Enqueue(g);
        return g;
    }

    public void AddObject(GameObject g)
    {
        queue.Enqueue(g);
    }

    public void Empty()
    {
        queue.Clear();
    }

    public int GetTotalActive()
    {
        int active = 0;
        foreach(var v in queue)
        {
            if (v.activeSelf == true) active++;
        }
        return active;
    }
}

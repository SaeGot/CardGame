using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLaneGame : MonoBehaviour
{
    public GameObject card;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        for (int n = 0; n < 3; n++)
        {
            GameObject temp = Instantiate(card, transform.position, transform.rotation, canvas.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

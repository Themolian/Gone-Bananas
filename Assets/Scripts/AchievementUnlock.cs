using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUnlock : MonoBehaviour
{

    public Vector3 startPos;
    public bool moveUp;
    public bool moveDown;

    void Start()
    {
        startPos = this.transform.position;
        this.transform.position = Vector3.Lerp(startPos, new Vector3(0, 36f, 0),200f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

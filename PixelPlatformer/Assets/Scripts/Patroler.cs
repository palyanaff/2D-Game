using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroler : Entity
{
    public float speed;

    public int positionOfPatrol;
    public float stoppingDistance;

    public Transform point;
    Transform player;

    bool movingRight;
    bool chill = true;
    bool angry;
    bool goBack = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && !angry)
        {
            chill = true;
            angry = false;
        }

        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            angry = true;
            chill = false;
            goBack = false;
        }

        if (Vector2.Distance(transform/*point for area agr*/.position, player.position) > stoppingDistance)
        {
            chill = true;
            goBack = true;
            angry = false;
        }

        if (chill == true)
        {
            Chill();
        }
        if (angry == true)
        {
            Angry();
        }
        if (goBack == true)
        {
            GoBack();
        }
    }

    void Chill()
    {
        if (transform.position.x > point.position.x + positionOfPatrol) 
        {
            movingRight = false;
        }
        else if (transform.position.x < point.position.x - positionOfPatrol) 
        {
            movingRight = true;
        }

        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
    }

    void Angry()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, (speed + 2) * Time.deltaTime);
    }

    void GoBack()
    {
        //transform.position = Vector2.MoveTowards(transform.position, point.position, speed  * Time.deltaTime);
        Chill();
    }
}

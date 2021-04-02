using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : NetworkBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    private GameManager gameManager;
    private int goalLayerMask;

    public override void NetworkStart()
    {
        gameManager = FindObjectOfType<GameManager>();
        goalLayerMask = LayerMask.GetMask("Goal");
    }

    public void Launch()
    {
        if (!IsOwner)
            return;
        
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
        StartCoroutine(nameof(LaunchCoroutine));
    }

    private IEnumerator LaunchCoroutine()
    {
        yield return new WaitForSeconds(1);
        
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(speed * x, speed * y);
    }
}

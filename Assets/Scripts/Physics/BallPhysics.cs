﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour {
    [Header("Adjust Ball Speed here")]
    public float speedY = 10;
    public float speedX = 10;

    [Header("Respawn Settings")]
    public float startDelay = 2.0f;
    public float respawnDelay = 1.5f;

    [Header("Extra Settings")]
    public float ballBounceVolume = 0.1f;
    public float glassVolume = 1f;

    Rigidbody2D rigidBody;

    public GameObject paddle;
    public GameObject levelController;

    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        Kick();
    }
    void Update()
    {
        CheckingOutOfBOund();
    }
    public void Kick()
    {
        StartCoroutine(WaitThenKick(startDelay));

    }
    IEnumerator WaitThenKick(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        rigidBody.velocity = new Vector2(0f, -1 * speedY);
    }
    public void Bounce()
    {
        if(SpawnBlocks.blocksHit % 10 == 0)
            SoundController.Play((int)SFX.CarExplodeGlass, glassVolume);
        else
            SoundController.Play((int)SFX.BallBounce, ballBounceVolume);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * -1);
    }
    public void Bounce(float radian)
    {
        SoundController.Play((int)SFX.BallBounce, ballBounceVolume);
        rigidBody.velocity = new Vector2(speedX * Mathf.Cos(radian), rigidBody.velocity.y * -1);
    }
    public void BounceDown()
    {
        SoundController.Play((int)SFX.BallBounce, ballBounceVolume);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x , Mathf.Abs(rigidBody.velocity.y) * -1f);
    }
    public void LeftSideBounce()
    {
        SoundController.Play((int)SFX.BallBounce, ballBounceVolume);
        rigidBody.velocity = new Vector2(Mathf.Abs(rigidBody.velocity.x), rigidBody.velocity.y);
    }
    public void RightSideBounce()
    {
        SoundController.Play((int)SFX.BallBounce, ballBounceVolume);
        rigidBody.velocity = new Vector2(Mathf.Abs(rigidBody.velocity.x) * -1f, rigidBody.velocity.y);
    }
    public void RespawnBall()
    {
        rigidBody.velocity = new Vector2(0.0f, 0.0f);
        transform.position = new Vector2(paddle.transform.position.x, paddle.transform.position.y + 2f);
        StartCoroutine(WaitThenKick(respawnDelay));
    }
    void CheckingOutOfBOund()
    {
        if(transform.position.y < -8f && !LevelController.isGameOver)
        {
            if(levelController.GetComponent<LevelController>().Life <= 0)
            {
                levelController.GetComponent<LevelController>().GameOver();
            }
            else
            {
                levelController.GetComponent<LevelController>().LoseLife(1);
                levelController.GetComponent<LevelController>().SetLifeText();
                RespawnBall();
            }

        }
        
    }
    
}

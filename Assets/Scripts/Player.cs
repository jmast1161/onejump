using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action<Player> PlayerDied;
    public event Action<Player> LevelComplete;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    private bool facingRight = true;
    private bool jumping = false;
    private bool fullJump = true;
    
    [SerializeField] private AudioSource walkingAudioSource;
    [SerializeField] private AudioSource jumpAudioSource;
    [SerializeField] private AudioSource deathAudioSource;
    [SerializeField] private AudioSource levelCompleteAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FlipSprite(float moveDirection)
    {
        if(moveDirection == 0)
        {
            return;
        }

        var isNowFacingRight = moveDirection > 0;
        if (facingRight != isNowFacingRight)
        {
            var localScale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
            facingRight = isNowFacingRight;
        }
    }

    public void MovePlayer()
    {
        var moveDirection = Input.GetAxis("Horizontal");
        FlipSprite(moveDirection);
        animator.SetFloat("Speed", Math.Abs(moveDirection));

        if(moveDirection == 0)
        {
            animator.StopPlayback();
            walkingAudioSource.Stop();
        }
        else if(!walkingAudioSource.isPlaying && !jumping)
        {
            walkingAudioSource.Play();
        }

        rigidBody2D.velocity = new Vector2(moveDirection * moveSpeed, rigidBody2D.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space) && !jumping)
        {
            var jumpHeight = fullJump ? 7 : 0.5f;
            rigidBody2D.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            jumping = true;
            animator.SetBool("Jumping", true);
            jumpAudioSource.Play();
            walkingAudioSource.Stop();
            fullJump = false;
        }
    }

    public void StopPlayer() =>
        animator.SetFloat("Speed", 0);

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Platform")
        {
            jumping = false;
            animator.SetBool("Jumping", false);
        }
        else if(other.gameObject.tag == "Spikes")
        {
            PlayerDied?.Invoke(this);
            deathAudioSource.Play();
            walkingAudioSource.Stop();
        }
        else if(other.gameObject.tag == "Flag")
        {
            LevelComplete?.Invoke(this);
            walkingAudioSource.Stop();
            levelCompleteAudioSource.Play();
        }
    }
}

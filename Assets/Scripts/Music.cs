using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{     
    [SerializeField] private AudioSource audioSource;
     private void Awake()
     {
         DontDestroyOnLoad(transform.gameObject);
     }
 
     public void PlayMusic()
     {
         if (audioSource.isPlaying) 
         {
            return;
         }
         
         audioSource.Play();
     }
 
     public void StopMusic()
     {
         audioSource.Stop();
     }
}

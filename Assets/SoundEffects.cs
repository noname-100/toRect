using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip splitSound;
    public AudioClip snapSound;
    public AudioClip mergeSound;
    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        splitSound = (AudioClip) Resources.Load("soundEffect/split");
        mergeSound = (AudioClip) Resources.Load("soundEffect/merge");
        snapSound = (AudioClip) Resources.Load("soundEffect/snap");   
    }

    // Update is called once per frame
    public void playSplit(){
        audioSource.PlayOneShot(splitSound, 1f);
    }
    public void playMerge(){
        audioSource.PlayOneShot(mergeSound, 1f);
    }
    public void playSnap(){
        audioSource.PlayOneShot(snapSound, 1f);
    }
}

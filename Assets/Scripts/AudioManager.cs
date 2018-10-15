using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public AudioMixerSnapshot peaceful;
    public AudioMixerSnapshot inCombat;
    public AudioClip[] transitions; //ramdomly select which audio sting is played so not the same sound everytime
    public AudioSource transitionsSource; //transition sting sfx
    public float bpm = 120; //helps with fading in time with the music tempo

    private float m_TransitionIn; 
    private float m_TransitionOut;
    private float m_QuarterNote; //length of a quarter note at 120bpm
    //_player = GameObject.Find("Player").GetComponent<Transform>();

    // Use this for initialization
    void Start()
    {
        m_QuarterNote = 60 / bpm; //length of a quarter note in milliseconds for calculating transition times
        m_TransitionIn = m_QuarterNote * 4; //fade in over the length of one quarternote (225 ms)
        m_TransitionOut = m_QuarterNote * 32; //long slow fade out
    }

    void PlayTransitionIn()
    {
            inCombat.TransitionTo(m_TransitionIn);
            PlayTransition();
    }

    void PlayTransitionOut()
    {
            peaceful.TransitionTo(m_TransitionOut);
    }

    private void Update()
    {
        if (SceneMgr.Instance.AnimalsAreAggressive)
        {
            PlayTransitionIn();
        }
        else {
            PlayTransitionOut();
        }
    }

    void PlayTransition()
    {
        int randClip = Random.Range(0, transitions.Length);
        //print(randClip); [] Come back to this when this array is not zero
        //transitionsSource.clip = transitions[randClip];
        transitionsSource.Play();
    }

}
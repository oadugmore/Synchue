using UnityEngine;

public class AudioLooper : MonoBehaviour
{
    public AudioClip clip;
    public float loopOffset;
    public bool playOnAwake;
    public float defaultWarmUp = 1f;
    private double nextPlayTime;
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private bool isPlaying;
    private bool flip;

    void Start()
    {
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource1.clip = clip;
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.clip = clip;
        if (playOnAwake)
        {
            PlayLooping(defaultWarmUp);
        }
    }

    public void PlayLooping(float warmUp)
    {
        nextPlayTime = AudioSettings.dspTime + warmUp;
        isPlaying = true;
        ScheduleClip1();
    }

    void Update()
    {
        if (isPlaying)
        {
            if (flip && !audioSource1.isPlaying)
            {
                ScheduleClip1(loopOffset);
                flip = false;

            }
            else if (!flip && !audioSource2.isPlaying)
            {
                ScheduleClip2(loopOffset);
                flip = true;
            }
        }
    }

    void ScheduleClip1(float startTime = 0f)
    {
        audioSource1.PlayScheduled(nextPlayTime);
        Debug.Log("Scheduled clip 1 to play at " + nextPlayTime);
        audioSource1.time = startTime;
        nextPlayTime += clip.length - startTime;
    }

    void ScheduleClip2(float startTime = 0f)
    {
        audioSource2.PlayScheduled(nextPlayTime);
        Debug.Log("Scheduled clip 2 to play at " + nextPlayTime);
        audioSource2.time = startTime;
        nextPlayTime += clip.length - startTime;
    }
}

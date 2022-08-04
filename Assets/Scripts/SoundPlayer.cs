using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource smashSoundObject;
    [SerializeField] private AudioClip[] backgroundClips;
    private AudioSource backgroundMusic;

    private void Start()
    {
        backgroundMusic = gameObject.GetComponent<AudioSource>();
    }

    public void PlayBackgroundMusic()
    {
        int index = Random.Range(0, backgroundClips.Length);
        backgroundMusic.clip = backgroundClips[index];
        backgroundMusic.Play();
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    public void PlaySmashSound()
    {
        smashSoundObject.GetComponent<AudioSource>().Play();
    }

}

using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform origin, float volume)
    {
        var audioSource = Instantiate(soundFXObject, origin);

        audioSource.transform.localPosition = Vector3.zero;

        audioSource.volume = volume;

        audioSource.clip = audioClip;

        audioSource.Play();

        var length = audioClip.length;

        Destroy(audioSource.gameObject, length);
    }

    public void StartPlayingLoopSound(AudioClip audioClip, Transform origin, float volume) => 
        StartCoroutine(PlaySoundAtLoop(audioClip, origin, volume));

    public void StopPlayingLoopSound() =>
        StopCoroutine("PlaySoundAtLoop");

    private IEnumerator PlaySoundAtLoop(AudioClip audioClip, Transform origin, float volume)
    {
        var delay = new WaitForSeconds(audioClip.length);

        PlaySoundFXClip(audioClip, origin, volume);

        yield return delay;
    }
}

using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundFXObject;
    private Coroutine cur;
    public GameObject curLoopObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform origin, float volume)
    {
        var audioSource = Instantiate(soundFXObject, origin);

        DontDestroyOnLoad(audioSource);

        audioSource.transform.localPosition = Vector3.zero;

        audioSource.volume = volume;

        audioSource.clip = audioClip;

        audioSource.Play();

        curLoopObj = audioSource.gameObject;

        var length = audioSource.clip.length;

        Destroy(audioSource.gameObject, length);
    }

    public void StartPlayingLoopSound(AudioClip audioClip, Transform origin, float volume)
    {
        cur = StartCoroutine(PlaySoundAtLoop(audioClip, origin, volume));
    }

    public void StopPlayingLoopSound()
    {
        if (cur is not null)
        {
            StopCoroutine(cur);
            Destroy(curLoopObj);
        }
    }

    private IEnumerator PlaySoundAtLoop(AudioClip audioClip, Transform origin, float volume)
    {
        while (true)
        {
            PlaySoundFXClip(audioClip, origin, volume);

            yield return new WaitForSeconds(audioClip.length);
        }
    }

    private IEnumerator PlaySoundAtLoopWithDestr(AudioClip audioClip, Transform origin, float volume)
    {
        while (true)
        {
            PlaySoundFXClip(audioClip, origin, volume);

            yield return new WaitForSeconds(audioClip.length);
        }
    }

    public void StartPlayingLoopSoundWithDestr(AudioClip audioClip, Transform origin, float volume)
    {
        cur = StartCoroutine(PlaySoundAtLoopWithDestr(audioClip, origin, volume));
    }

    public void PlaySoundFXClipWithDestr(AudioClip audioClip, Transform origin, float volume)
    {
        var audioSource = Instantiate(soundFXObject, origin);

        audioSource.transform.localPosition = Vector3.zero;

        audioSource.volume = volume;

        audioSource.clip = audioClip;

        audioSource.Play();

        curLoopObj = audioSource.gameObject;

        var length = audioSource.clip.length;

        Destroy(audioSource.gameObject, length);
    }

    public void ChangeVolume(float volume)
    {
        if (curLoopObj != default)
            curLoopObj.GetComponent<AudioSource>().volume = volume;
    }
}

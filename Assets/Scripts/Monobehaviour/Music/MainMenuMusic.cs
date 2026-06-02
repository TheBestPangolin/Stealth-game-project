using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{

    private bool IsPlaying = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying)
        {
            var instance = SoundManager.instance;
            
            if (instance is not null)
            {
                instance.StopPlayingLoopSound();
                instance.StartPlayingLoopSound(Resources.Load<AudioClip>("Music/main"), transform.parent, Player_container.MasterVolume * Player_container.MusicVolume);
                IsPlaying = true;
            }
        }
    }
}

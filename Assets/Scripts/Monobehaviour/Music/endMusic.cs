using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class endMusic : MonoBehaviour
{
    private bool IsPlaying = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
                instance.StartPlayingLoopSound(Resources.Load<AudioClip>("Music/Ending"), transform.parent, Player_container.MasterVolume * Player_container.MusicVolume);
                IsPlaying = true;
            }
        }
    }
}

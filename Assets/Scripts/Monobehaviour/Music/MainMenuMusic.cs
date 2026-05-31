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
                instance.StartPlayingLoopSound(Resources.Load<AudioClip>("Music/main"), transform.parent, 1);
                IsPlaying = true;
            }
        }
    }
}

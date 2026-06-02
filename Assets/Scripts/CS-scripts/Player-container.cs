using UnityEngine;

public static class Player_container
{
    public static Vector3 CurrentRespawn;
    public static int NPCCounter = 0;

    public static float MasterVolume = 1.0f;
    public static float SoundVolume = 1.0f;
    public static float MusicVolume = 1.0f;

    public static int[] InstrumentCount = new int[3];

    public static int cardsCounter = 0;

    public static void SetDefault()
    {
        CurrentRespawn = default;
        NPCCounter = 0;

        InstrumentCount = new int[3];

        cardsCounter = 0;
    }
}

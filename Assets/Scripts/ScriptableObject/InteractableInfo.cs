using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "InteractableInfo", menuName = "Scriptable Objects/InteractableInfo")]
public class InteractableInfo : ScriptableObject
{
    /// <summary>
    /// 0 = Подбираемое;
    /// 1 = Живой НПС;
    /// 2 = Полумёртвый НПС;
    /// 3 = Переход;
    /// 4 = Рубильник
    /// 5 = переход на сцену
    /// 6 = ключ
    /// </summary>
    public int Type;

    public string PathToDialogFile;

    /// <summary>
    /// 0 = Stone;
    /// 1 = Smoke;
    /// 2 = EMP;
    /// </summary>
    public int[] PickableInstruments = new int[3];

    public Vector2 TeleportLocation;

    public string SceneName;
}

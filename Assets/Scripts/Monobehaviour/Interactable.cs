using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    [SerializeField] public Door[] Doors;
    [SerializeField] private InteractableInfo Info;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Info.Type == 4 || Info.Type == 6)
            foreach (var door in Doors)
                door.NumbOfSwitches++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerScript>();

            var hint = "";

            switch (Info.Type)
            {
                case 0:
                    player.Interact = () =>
                    {
                        player.PickUp(Info.PickableInstruments);
                        Destroy(transform.parent.gameObject);
                    };
                    hint = "[E] - Подобрать";
                    break;
                case 1:
                    player.Interact = () =>
                    {
                        DialogWindow.ReadFileDialogs(Info.PathToDialogFile);
                        player.PickUp(Info.PickableInstruments);
                        Destroy(GetComponent<BoxCollider2D>());
                    };
                    hint = "[E] - Поговорить";
                    break;
                case 2:
                    player.Interact = () =>
                    {
                        player.PickUp(Info.PickableInstruments);
                        Player_container.NPCCounter++;
                        Destroy(GetComponent<BoxCollider2D>());
                    };
                    hint = "[E] - Помочь";
                    break;
                case 3:
                    player.Interact = () =>
                    {
                        player.ChangePosition(Info.TeleportLocation);
                        Destroy(gameObject);
                    };
                    hint = "[E] - Перейти";
                    break;
                case 4:
                    player.Interact = () =>
                    {
                        foreach (var door in Doors)
                            door.ActuatedSwitches++;
                        
                    };
                    hint = "[E] - Переключить рубильник";
                    break;
                case 5:
                    player.Interact = () =>
                    {
                        SceneManager.LoadScene(Info.SceneName);
                    };
                    hint = "[E] - Перейти";
                    break;
                case 6:
                    player.Interact = () =>
                    {
                        foreach (var door in Doors)
                            door.ActuatedSwitches++;
                        Player_container.cardsCounter++;
                        Destroy(transform.parent.gameObject);
                    };
                    hint = "[E] - Подобрать";
                    break;
                case 7:
                    player.Interact = () =>
                    {
                        SceneManager.LoadScene(Info.SceneName);
                    };
                    hint = "[E] - Спасти человечество";
                    break;
            }
            
            TextHint.DisplayHint?.Invoke(hint);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerScript>();
            player.Interact = null;
            TextHint.DisableHint?.Invoke();
        }
    }
}

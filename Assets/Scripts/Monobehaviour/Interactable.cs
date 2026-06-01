using UnityEngine;

public class Interactable : MonoBehaviour
{

    [SerializeField] private InteractableInfo Info;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Info.Type == 4)
            foreach (var door in Info.Doors)
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
                        Destroy(gameObject);
                    };
                    hint = "[E] - Подобрать";
                    break;
                case 1:
                    player.Interact = () =>
                    {
                        DialogWindow.ReadFileDialogs(Info.PathToDialogFile);
                    };
                    hint = "[E] - Поговорить";
                    break;
                case 2:
                    player.Interact = () =>
                    {
                        player.PickUp(Info.PickableInstruments);
                        Destroy(gameObject);
                    };
                    hint = "[E] - Помочь";
                    break;
                case 3:
                    player.Interact = () =>
                    {
                        player.ChangePosition(Info.TeleportLocation);
                    };
                    hint = "[E] - Перейти";
                    break;
                case 4:
                    player.Interact = () =>
                    {
                        foreach (var door in Info.Doors)
                            door.ActuatedSwitches++;
                    };
                    hint = "[E] - Переключить рубильник";
                    break;
            }
            
            TextHint.DisplayHint?.Invoke(hint);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TextHint.DisableHint?.Invoke();
        }
    }
}

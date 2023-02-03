using UnityEngine;

public class shopDisplayManager : MonoBehaviour, IInteractable
{
    public GameObject shopMenu;

    void Start()
    {
        shopMenu.SetActive(false);
    }


    public void openShopMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        shopMenu.SetActive(true);
        //Time.timeScale 0f;
    }

    public void closeShopMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        shopMenu.SetActive(false);
        //Time.timeScale 1f;
    }

    public void Interact(GameObject host)
    {
        openShopMenu();
    }
}

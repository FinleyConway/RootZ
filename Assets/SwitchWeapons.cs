using UnityEngine;

public class SwitchWeapons : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] _fakeWeapon;
    [SerializeField] private GameObject[] _weapons;

    private bool _isAxe = true;

    public void Interact(GameObject host)
    {
        _isAxe = !_isAxe;

        if (_isAxe)
        {
            EquipAxe();
        }
        else
        {
            EquipShovel();
        }
    }

    private void EquipAxe()
    {

    }

    private void EquipShovel()
    {

    }
}

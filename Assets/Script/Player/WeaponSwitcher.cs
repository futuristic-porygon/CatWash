using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int selectedWeapon = 0;
    AudioManager audioManager;

    GameManager gm;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // Buscar GameManager correctamente
        gm = GameManager.Instance;

        if (gm == null)
            gm = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            NextWeapon();

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            PreviousWeapon();

        if (previousSelectedWeapon != selectedWeapon)
            SelectWeapon();
    }

    void NextWeapon()
    {
        int totalW = transform.childCount;

        do
        {
            selectedWeapon++;
            if (selectedWeapon >= totalW)
                selectedWeapon = 0;

        } while (!WeaponUnlocked(selectedWeapon));

        audioManager.PlaySFX(audioManager.weaponChage);
    }

    void PreviousWeapon()
    {
        int totalW = transform.childCount;

        do
        {
            selectedWeapon--;
            if (selectedWeapon < 0)
                selectedWeapon = totalW - 1;

        } while (!WeaponUnlocked(selectedWeapon));

        audioManager.PlaySFX(audioManager.weaponChage);
    }

    bool WeaponUnlocked(int index)
    {
        // Arma 0 siempre desbloqueada
        if (index == 0) return true;

        // Arma 1 ? arma2
        if (index == 1) return gm.arma2Desbloqueada;

        // Arma 2 ? arma3
        if (index == 2) return gm.arma3Desbloqueada;

        return true;
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }
}

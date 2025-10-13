using UnityEngine;
using UnityEngine.UI;

public class FuelDisplay : MonoBehaviour
{
    [SerializeField] private Image _fuelBarImage;

    public void SetFuelLevel(float currentFuel, float maxFuel)
    {
        if (_fuelBarImage != null)
            _fuelBarImage.fillAmount = Mathf.Clamp01(currentFuel / maxFuel);
    }
}
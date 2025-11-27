using UnityEngine;

public class arrow : MonoBehaviour
{
    public float rotateSpeed = 100f;
    private float currentRotation = 0f;

    void Update()
    {
        // ci¹g³y obrót, ale zaokr¹glany do najbli¿szych 90°
        currentRotation += rotateSpeed * Time.deltaTime;
        currentRotation %= 360;

        float snapped = Mathf.Round(currentRotation / 90f) * 90f;
        transform.rotation = Quaternion.Euler(0, snapped, 0);
            
    }
}

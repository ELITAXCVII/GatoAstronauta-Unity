using UnityEngine;

// Sigue al jugador de lado a lado. En altura se queda fija (se ve desde el suelo
// hasta las plataformas más altas) y solo sube si el jugador salta muy alto.
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 6f;
    public Vector3 offset = new Vector3(2f, 1f, -10f);

    public float fixedY    = 1f;    // altura normal de la cámara (nunca baja de aquí)
    public float viewSize  = 4.5f;  // mitad de la altura visible (antes 3)
    public float topMargin = 1.2f;  // espacio mínimo entre el jugador y el borde de arriba

    float minX = float.NegativeInfinity, maxX = float.PositiveInfinity;

    void Start()
    {
        var cam = GetComponent<Camera>();
        if (cam) cam.orthographicSize = viewSize;

        // No dejar que la cámara muestre más allá de los bordes del suelo
        var ground = GameObject.Find("Ground");
        if (cam && ground)
        {
            var b = ground.GetComponent<Renderer>().bounds;
            float halfW = viewSize * cam.aspect;
            minX = b.min.x + halfW;
            maxX = b.max.x - halfW;
        }

        if (target)
            transform.position = Desired();
    }

    void LateUpdate()
    {
        if (!target) return;
        transform.position = Vector3.Lerp(transform.position, Desired(), smoothSpeed * Time.deltaTime);
    }

    Vector3 Desired()
    {
        float x = Mathf.Clamp(target.position.x + offset.x, minX, maxX);
        // Solo sube si el jugador se acerca al borde de arriba; nunca baja de fixedY
        float y = Mathf.Max(fixedY, target.position.y + topMargin - viewSize);
        return new Vector3(x, y, -10f);
    }
}

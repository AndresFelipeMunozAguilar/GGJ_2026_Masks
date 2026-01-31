using UnityEngine;

public class Movement : MonoBehaviour
{

    public enum MovementType
    {
        Horizontal,
        Vertical,
        Circular
    }

    [SerializeField]
    private MovementType movementType = MovementType.Horizontal;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float radius = 3f;

    private float angle = 0f;

    private Vector3 startPosition;

    public void Start()
    {
        startPosition = transform.position;
        SetRandomMovementType();
    }

    public void Update()
    {
        switch (movementType)
        {
            case MovementType.Horizontal:
                MoveHorizontal();
                break;
            case MovementType.Vertical:
                MoveVertical();
                break;
            case MovementType.Circular:
                MoveCircular();
                break;
        }
    }

    public void MoveHorizontal()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * speed) * radius;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public void MoveVertical()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * radius;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void MoveCircular()
    {
        angle += speed * Time.deltaTime;
        float x = startPosition.x + Mathf.Cos(angle) * radius;
        float y = startPosition.y + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void SetMovementType(int type)
    {
        if (type >= 0 && type < System.Enum.GetNames(typeof(MovementType)).Length)
        {
            movementType = (MovementType)type;
        }
        else
        {
            Debug.LogError("Invalid movement type index: " + type);
        }
    }

    public void SetRandomMovementType()
    {
        int randomIndex = Random.Range(0, System.Enum.GetNames(typeof(MovementType)).Length);
        movementType = (MovementType)randomIndex;
    }

}
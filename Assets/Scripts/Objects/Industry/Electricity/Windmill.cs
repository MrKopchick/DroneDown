using UnityEngine;

public class Windmill : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.4f;
    private GameObject rotor;

    void Start(){
        rotor = gameObject.transform.GetChild(0).gameObject;
        rotor.transform.Rotate(0f, 0f, Random.Range(0f, 180f));
    }

    void FixedUpdate()
    {
        rotor.transform.Rotate(0f, 0f, rotationSpeed);
    }
}

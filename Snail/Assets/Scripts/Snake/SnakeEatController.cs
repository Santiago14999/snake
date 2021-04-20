using UnityEngine;

public class SnakeEatController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Food>(out var food))
        {
            Destroy(food.gameObject);
            FoodSpawner.Instance.SpawnFood();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFood : MonoBehaviour
{
    [SerializeField] GameObject _food;
    Collider2D _collider;
    private void Start()
    {
        _collider = GetComponent<Collider2D>();

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Animal" || other.gameObject.tag == "Idle Animal")
            StartCoroutine("CollectFoodCooldown");
    }

    IEnumerator CollectFoodCooldown()
    {
        _food.SetActive(false);
        _collider.enabled = false;
        yield return new WaitForSeconds(5);

        _food.SetActive(true);
        _collider.enabled = true;
    }
}

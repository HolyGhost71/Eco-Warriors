using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Other items will inherit from this and override the collection behaviour
public class CollectableItem : MonoBehaviour
{
    public AudioClip sfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {      
            Collect();
            GameManager.Instance.PlayAudio(sfx);
            gameObject.SetActive(false);
        }
    }

    protected virtual void Collect()
    {
        Debug.Log("No child class");
    }

    public IEnumerator AnimateItem()
    {
        GameObject player = GameObject.Find("Player");

        // Stops the player being able to collect the item until the animation is done
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<CharacterSwitcher>().GetAnimal().GetComponent<CapsuleCollider2D>());

        // Rises slowly out the ground over 1 second / 100 movements
        for (int i = 0; i < 100; i++)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.01f);
            yield return new WaitForSeconds(0.01f);
        }

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<CharacterSwitcher>().GetAnimal().GetComponent<CapsuleCollider2D>(), false);
    }

}

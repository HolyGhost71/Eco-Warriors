using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] GameObject[] animals;
    [SerializeField] Animal[] animalStats;
    private int counter = 0;

    [SerializeField] GameObject transformationSmoke;

    private GameObject activeAnimal;
    public GameObject GetAnimal() { return activeAnimal; }

    [SerializeField] LayerMask dontSwitchLayers;

    private void Start()
    {
        activeAnimal =  animals[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            float nextAnimalHeight = animals[(counter + 1) % animals.Length].transform.localScale.y;
            GameObject previousAnimal = animals[counter % animals.Length];

            // Stops the player from being able to transform when the gap is too small i.e switching from the frog when in a small space
            RaycastHit2D hit = Physics2D.Raycast(previousAnimal.transform.position, Vector2.up, 100, dontSwitchLayers);

            if (hit.collider != null)
            {
                float distanceToHit = Vector2.Distance(previousAnimal.transform.position, hit.point);

                if (distanceToHit < nextAnimalHeight / 4 && animals[(counter + 1) % animals.Length].GetComponent<CapsuleCollider2D>().size.y > previousAnimal.GetComponent<CapsuleCollider2D>().size.y) { return;  }
            }

            // Switches the animal to the next one in the order
            previousAnimal = animals[counter % animals.Length];
            previousAnimal.SetActive(false);
            counter++;

            activeAnimal = animals[counter % animals.Length];

            // Offsets the position of the new animal such that it is placed above the ground
            float yOffset = 0;
            if (activeAnimal.transform.localScale.y > previousAnimal.transform.localScale.y)
            {
                yOffset = activeAnimal.transform.localScale.y / 2 - previousAnimal.transform.localScale.y/2;
            }

            activeAnimal.transform.position = new Vector2(previousAnimal.transform.position.x, previousAnimal.transform.position.y + yOffset);
            activeAnimal.SetActive(true);

            // Creates an effect for the transformation
            CreateSmoke();
        }
    }

    private void CreateSmoke()
    {
        // Creates and then destroys the smoke in the posiition
        GameObject smoke = Instantiate(transformationSmoke, activeAnimal.transform.position, Quaternion.identity);
        Destroy(smoke, 0.3f);
    }
}

using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject player;  // Player
    public float interactDistance = 3f;  // Distance at which the player can interact
    private bool isNear = false;  // Is the player near the object?
    private bool isLookingAt = false;  // Is the player looking at the object?

    public BlockRaceGame blockRaceGame;  // Reference to the block race game

    private void Update()
    {
        // Check if the player is within range
        float distance = Vector3.Distance(transform.position, player.transform.position);
        isNear = distance <= interactDistance;

        // Perform a Raycast from the player's camera position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the Raycast hits this object
            isLookingAt = hit.collider != null && hit.collider.gameObject == gameObject;
        }
        else
        {
            isLookingAt = false;
        }

        // If the player is near and looking at the object, allow interaction
        if (isNear && isLookingAt && Input.GetKeyDown(KeyCode.E))
        {
            StartBlockRaceGame();
        }
    }

    void OnGUI()
    {
        // Display text only when the player is near and looking at the object
        if (isNear && isLookingAt)
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Press 'E' to start the Rat Race");
        }
    }

    // Method to start the game
    void StartBlockRaceGame()
    {
        if (blockRaceGame != null)
        {
            // Start the game
            blockRaceGame.StartRace(0); // Default bet on rat 1, can be adjusted
        }
    }
}
xd
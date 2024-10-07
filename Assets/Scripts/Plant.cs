using UnityEngine;

public class Plant
{
    public int currentDay;
    public int maxGrowthStage;
    public int currentGrowthStage;
    public Vector3Int position;

    private SpriteRenderer spriteRenderer;
    private Sprite[] growthStages; // Array of sprites for each growth stage

    public Plant(Vector3Int position, Sprite[] growthSprites)
    {
        this.position = position;

        // Check if growthSprites is null or empty
        if (growthSprites == null || growthSprites.Length == 0)
        {
            Debug.LogError("Growth sprites array is null or empty. Cannot create plant.");
            return; // Prevent further execution
        }

        this.growthStages = growthSprites;
        this.maxGrowthStage = growthSprites.Length - 1;
        this.currentGrowthStage = 0;
        this.currentDay = 0;

        // Get or set up a SpriteRenderer to display the plant sprite
        GameObject plantObject = new GameObject("Plant");
        plantObject.transform.position = position;
        spriteRenderer = plantObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = growthStages[currentGrowthStage];
    }

    public void Grow(int dayCount)
    {
        // Assuming growth happens every few days, for example every 5 days
        int daysPerStage = 1;

        if (dayCount >= currentDay + daysPerStage && currentGrowthStage < maxGrowthStage)
        {
            currentGrowthStage++;
            currentDay = dayCount; // Update the currentDay to the new dayCount
            spriteRenderer.sprite = growthStages[currentGrowthStage]; // Change the sprite to match the growth stage
        }
    }

}

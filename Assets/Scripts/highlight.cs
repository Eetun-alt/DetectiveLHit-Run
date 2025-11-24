using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    public Sprite normalSprite;       // Normaalikuva
    public Sprite highlightSprite;    // Highlight-kuva

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = normalSprite;  // Varmista että alkaa normaalina
    }

    void OnMouseEnter()
    {
        sr.sprite = highlightSprite; // Vaihda highlight
    }

    void OnMouseExit()
    {
        sr.sprite = normalSprite; // Takaisin normaaliksi
    }
}
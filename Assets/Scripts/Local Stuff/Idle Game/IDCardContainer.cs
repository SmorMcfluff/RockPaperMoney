using UnityEngine;

public class IDCardContainer : MonoBehaviour
{
    public static IDCardContainer Instance;

    [SerializeField] private RectTransform idCardContainer;
    public IDCard IDCard;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        RescaleIDCard();
    }

    private void RescaleIDCard()
    {
        var canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        var canvasWidth = canvasRect.rect.width;
        var canvasHeight = canvasRect.rect.height;

        var cardWidth = 384f;
        var cardHeight = 216f;

        var scaleFactor = canvasWidth / cardWidth;

        var scaledHeight = cardHeight * scaleFactor;
        
        if (scaledHeight > canvasHeight)
        {
            scaleFactor = canvasHeight / cardHeight;
        }

        idCardContainer.localScale = new Vector3(scaleFactor, scaleFactor, 1);
    }
}
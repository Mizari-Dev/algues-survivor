using UnityEngine;
using UnityEngine.UI;
public class SpriteElement : UIElement
{
    [SerializeField] private Image image;
    [SerializeField] private SpriteState state;
    public override void OnDown()
    {
        if (image == null)
            return;
        image.sprite = state.pressedSprite;
    }

    public override void OnEnter()
    {
        if (image == null)
            return;
        image.sprite = state.highlightedSprite;
    }

    public override void OnExit()
    {
        if (image == null)
            return;
        image.sprite = state.selectedSprite;
    }

    public override void OnUp()
    {
        if (image == null)
            return;
        image.sprite = state.selectedSprite;
    }
}

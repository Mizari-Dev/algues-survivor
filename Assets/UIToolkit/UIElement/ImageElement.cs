using UnityEngine;
using UnityEngine.UI;
public class ImageElement : UIElement
{
    [SerializeField] private Image image;
    [SerializeField] private ColorBlock colors = ColorBlock.defaultColorBlock;
    public override void OnDown()
    {
        if (image == null)
            return;
        image.color = colors.selectedColor;
    }

    public override void OnEnter()
    {
        if (image == null)
            return;
        image.color = colors.highlightedColor;
    }

    public override void OnExit()
    {
        if (image == null)
            return;
        image.color = colors.normalColor;
    }

    public override void OnUp()
    {
        if (image == null)
            return;
        image.color = colors.normalColor;
    }
}

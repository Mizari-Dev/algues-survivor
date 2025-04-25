using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextElement : UIElement
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private ColorBlock colors = ColorBlock.defaultColorBlock;
    public override void OnDown()
    {
        if (text == null)
            return;
        text.color = colors.selectedColor;
    }

    public override void OnEnter()
    {
        if (text == null)
            return;
        text.color = colors.highlightedColor;
    }

    public override void OnExit()
    {
        if (text == null)
            return;
        text.color = colors.normalColor;
    }

    public override void OnUp()
    {
        if (text == null)
            return;
        text.color = colors.normalColor;
    }
}

using System;

[Serializable]
public abstract class UIElement
{
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnDown();
    public abstract void OnUp();
}

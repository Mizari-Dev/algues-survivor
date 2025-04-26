public class DancingItem : UIPanelManager
{
    protected void Start()
    {
        Events._danceState += Dance;   
    }

    protected void OnDestroy()
    {
        Events._danceState -= Dance;
    }
    protected void Dance(bool state)
    {
        if (state)
            ShowInstant();
        else
            HideInstant();
    }
}

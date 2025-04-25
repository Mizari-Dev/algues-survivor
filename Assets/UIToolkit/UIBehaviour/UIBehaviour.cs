using System;
using UnityEngine;

[Serializable]
public abstract class UIBehaviour
{
    [SerializeField] protected RelativePositionStartType transistionType;
    public void DoBehaviour(UIView view)
    {
        switch (transistionType)
        {
            case RelativePositionStartType.FromStart:
                DoFromStart(view); 
                break;
            case RelativePositionStartType.FromCurrent:
                DoFromCurrent(view); 
                break;
            case RelativePositionStartType.ToCustom:
                DoToCustom(view); 
                break;
        }
    }    
    public void DoBehaviourInstant(UIView view)
    {
        switch (transistionType)
        {
            case RelativePositionStartType.FromStart:
                DoFromStartInstant(view); 
                break;
            case RelativePositionStartType.FromCurrent:
                DoFromCurrentInstant(view); 
                break;
            case RelativePositionStartType.ToCustom:
                DoToCustomInstant(view); 
                break;
        }
    }
    protected abstract void DoFromStart(UIView view);
    protected abstract void DoFromCurrent(UIView view);
    protected abstract void DoToCustom(UIView view);
    protected abstract void DoFromStartInstant(UIView view);
    protected abstract void DoFromCurrentInstant(UIView view);
    protected abstract void DoToCustomInstant(UIView view);
}

using UnityEngine;

public abstract class UIView : MonoBehaviour
{
    public virtual void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}

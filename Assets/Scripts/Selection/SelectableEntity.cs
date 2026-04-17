using UnityEngine;

public class SelectableEntity : MonoBehaviour
{
    public bool IsSelected {get; private set;}

    public void Select()
    {
        IsSelected = true;
        Debug.Log($"{gameObject.name} selecionado");
    }

    public void Deselect()
    {
        IsSelected = false;
        Debug.Log($"{gameObject.name} desselecionado");
    }
}

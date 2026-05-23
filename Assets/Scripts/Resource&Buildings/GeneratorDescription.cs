using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Generetor", menuName = "Scriptable Objects/Generetor")]
public class GeneretorDescription : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    [TextArea] public string description;
}

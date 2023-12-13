using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitPNG : MonoBehaviour
{
    public Sprite Club;
    public Sprite Heart;
    public Sprite Diamond;
    public Sprite Spade;
    public Sprite TransParent;

    [SerializeField] SpriteRenderer thisspriterender;

    public void SetSprite(Sprite _sprite)
    {
        thisspriterender.sprite = _sprite;
    }
}

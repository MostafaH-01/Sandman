using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GetSpriteFromAtlas : MonoBehaviour
{
    [SerializeField] SpriteAtlas atlas;
    [SerializeField] string spriteName;

    private void Start()
    {
        GetComponent<Image>().sprite = atlas.GetSprite(spriteName);
    }
}

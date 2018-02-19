using UnityEngine;
using UnityEngine.UI;

public class ArrayTexture : MonoBehaviour
{

    [SerializeField]
    Sprite[] sprites;

    [SerializeField]
    Image image;

    public void ChangeTexture(int id)
    {
        if(id > sprites.Length)
        {
            //Debug.LogError("sprite is out of range");
        }
        else if(id < 0)
        {
            image.sprite = sprites[0];
        }
        else
        {
            image.sprite = sprites[id];
        }
    }

    public void SetActive(bool f)
    {
        gameObject.SetActive(f);
    }

}

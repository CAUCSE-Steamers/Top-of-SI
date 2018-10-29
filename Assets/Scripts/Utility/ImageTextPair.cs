using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class ImageTextPair
    {
        [SerializeField]
        private Image imageComponent;
        [SerializeField]
        private Text textComponent;

        public Image ImageComponent
        {
            get
            {
                return imageComponent;
            }
        }

        public Text TextComponent
        {
            get
            {
                return textComponent;
            }
        }

        public void SetActiveState(bool newState)
        {
            imageComponent.gameObject.SetActive(newState);
            textComponent.gameObject.SetActive(newState);
        }

        public void SetText(string text)
        {
            TextComponent.text = text;
        }

        public void SetSprite(Sprite sprite)
        {
            ImageComponent.sprite = sprite;
        }
    }
}

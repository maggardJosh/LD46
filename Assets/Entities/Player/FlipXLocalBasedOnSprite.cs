using System;
using UnityEngine;

namespace Entities.Player
{
    public class FlipXLocalBasedOnSprite : MonoBehaviour
    {
        public SpriteRenderer sprite;

        private float xValue = .4f;

        private void Start()
        {
            xValue = transform.localPosition.x;
        }

        // Update is called once per frame
        void Update()
        {
            float xPos = xValue * (sprite.flipX ? -1 : 1);
            var pos = transform.localPosition;
            pos.x = xPos;
            transform.localPosition = pos;
        }
    }
}
using UnityEngine;

namespace Entities.Player
{
    public class FlipXLocalBasedOnSprite : MonoBehaviour
    {
        public SpriteRenderer sprite;

        public float xValue = .4f;

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
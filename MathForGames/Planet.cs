using System;
using System.Collections.Generic;
using System.Text;

namespace MathForGames
{
    class Planet : Actor
    {
        private float _rotate;
        private Sprite _sprite;
        public Planet(float x, float y, float radius, float rotation, string image = " ", float scaleX = 1, float scaleY = 1) : base(x, y, radius)
        {
            _rotate = rotation;
            SetScale(scaleX, scaleY);
            _sprite = new Sprite(image);
        }
        public override void Update(float deltaTime)
        {
            Rotate(_rotate);
            base.Update(deltaTime);
        }
        public override void Draw()
        {
            _sprite.Draw(_globalTransform);
            base.Draw();
        }
    }
}

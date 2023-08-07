namespace SpriterAnimation;

public partial class Spriter
{
    private class SpatialInfo
    {
        public float x = 0;
        public float y = 0;
        public float angle = 0;
        public float scaleX = 1;
        public float scaleY = 1;
        public float a = 1;
        public int spin = 1;

        void unmapFromParent(SpatialInfo parentInfo)
        {
            angle += parentInfo.angle;
            scaleX *= parentInfo.scaleX;
            scaleY *= parentInfo.scaleY;
            a *= parentInfo.a;

            if (x != 0 || y != 0)
            {
                var preMultX = x * parentInfo.scaleX;
                var preMultY = y * parentInfo.scaleY;
                var s = MathF.Sin(ToRadians(parentInfo.angle));
                var c = MathF.Cos(ToRadians(parentInfo.angle));
                x = preMultX * c - preMultY * s;
                y = preMultX * s + preMultY * c;
                x += parentInfo.x;
                y += parentInfo.y;
            }
            else
            {
                // Mandatory optimization for future features           
                x = parentInfo.x;
                y = parentInfo.y;
            }
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace BrickBreaker
{
    public class Ball
    {
        Boolean angle1, angle2, travelDown, travelRight;
        public int x, y, size;
        public double speed, xSpeed, ySpeed;
        public Color colour;

        SoundPlayer paddleCollide = new SoundPlayer(Properties.Resources.paddleCollision);
        SoundPlayer otherCollide = new SoundPlayer(Properties.Resources.brickCollision);
        SoundPlayer dead = new SoundPlayer(Properties.Resources.deathSound);

        public static Random rand = new Random();

        public Ball(int _x, int _y, double _speed, int _ballSize)
        {
            x = _x;
            y = _y;
            speed = _speed;
            size = _ballSize;
        }

        public void Move()
        {
            if (angle1)
            {
                xSpeed = speed * 1.25;
                ySpeed = speed * 0.75;
            }
            else if (angle2)
            {
                xSpeed = speed * 1.5;
                ySpeed = speed * 0.5;
            }
            else
            {
                xSpeed = speed;
                ySpeed = speed;
            }

            if (travelRight)
            {
                x += Convert.ToInt16(xSpeed);
            }
            else
            {
                x -= Convert.ToInt16(xSpeed);
            }

            if (travelDown)
            {
                y += Convert.ToInt16(ySpeed);
            }
            else
            {
                y -= Convert.ToInt16(ySpeed);
            }
            
        }

        public bool BlockCollision(Block b)
        {
            Rectangle blockBotRec = new Rectangle(b.x, (b.y + (b.height / 2)), b.width, b.height);
            Rectangle blockTopRec = new Rectangle(b.x, b.y, b.width, b.height / 2);
            Rectangle blockLeftRec = new Rectangle(b.x, b.y + (b.height / 4), b.width / 2, b.height / 2);
            Rectangle blockRightRec = new Rectangle(b.x + (b.width / 2), b.y + (b.height / 4), b.width / 2, b.height / 2);

            Rectangle ballRec = new Rectangle(x, y, size, size);

            if (ballRec.IntersectsWith(blockTopRec) && ySpeed > 0)
            {
                travelDown = false;
                otherCollide.Play();
            }
            else if (ballRec.IntersectsWith(blockBotRec) && ySpeed < 0)
            {
                travelDown = true;
                otherCollide.Play();
            }
            else if (ballRec.IntersectsWith(blockLeftRec) && xSpeed > 0)
            {
                travelRight = false;
                otherCollide.Play();
            }
            else if (ballRec.IntersectsWith(blockRightRec) && xSpeed < 0)
            {
                travelRight = true;
                otherCollide.Play();
            }

            if (blockBotRec.IntersectsWith(ballRec) || blockTopRec.IntersectsWith(ballRec) || blockLeftRec.IntersectsWith(ballRec) || blockRightRec.IntersectsWith(ballRec))
            {
                otherCollide.Play();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PaddleCollision(Paddle p, bool pMovingLeft, bool pMovingRight)
        {
            int midX = x + (size / 2), segment = p.width / 6;

            Rectangle ballRec = new Rectangle(x, y, size, size);
            Rectangle paddleRec = new Rectangle(p.x, p.y, p.width, p.height);

            Rectangle paddleRec1 = new Rectangle(p.x, p.y, segment, p.height);
            Rectangle paddleRec2 = new Rectangle(p.x + segment, p.y, segment, p.height);
            Rectangle paddleRec3 = new Rectangle(p.x + (segment * 2), p.y, segment, p.height);
            Rectangle paddleRec4 = new Rectangle(p.x + (segment * 3), p.y, segment, p.height);
            Rectangle paddleRec5 = new Rectangle(p.x + (segment * 4), p.y, segment, p.height);
            Rectangle paddleRec6 = new Rectangle(p.x + (segment * 5), p.y, segment, p.height);

            if (ballRec.IntersectsWith(paddleRec) && ySpeed > 0)
            {
                paddleCollide.Play();
                travelDown = false;

                if (ballRec.IntersectsWith(paddleRec1))
                {
                    angle1 = false;
                    angle2 = true;
                }
                else if (ballRec.IntersectsWith(paddleRec2))
                {
                    angle1 = true;
                    angle2 = false;
                }
                else if (ballRec.IntersectsWith(paddleRec3))
                {
                    angle1 = false;
                    angle2 = false;
                }
                else if (ballRec.IntersectsWith(paddleRec4))
                {
                    angle1 = false;
                    angle2 = false;
                }
                else if (ballRec.IntersectsWith(paddleRec5))
                {
                    angle1 = true;
                    angle2 = false;
                }
                else if (ballRec.IntersectsWith(paddleRec6))
                {
                    angle1 = false;
                    angle2 = true;
                }

                    if (pMovingLeft)
                    travelRight = false;
                else if (pMovingRight)
                    travelRight = true;
            }
        }

        public void WallCollision(UserControl UC)
        {
            // Collision with left wall
            if (x <= 0)
            {
                otherCollide.Play();
                if (xSpeed <= 0)
                {
                    travelRight = true;
                }
            }

            // Collision with right wall
            if (x >= (UC.Width - size))
            {
                otherCollide.Play();
                if (xSpeed >= 0)
                {
                    travelRight = false;
                }
            }

            // Collision with top wall
            if (y <= 41)
            {
                otherCollide.Play();
                if (ySpeed <= 0)
                {
                    travelDown = true;
                }
            }
        }

        public bool BottomCollision(UserControl UC)
        {
            Boolean didCollide = false;

            if (y >= UC.Height)            {
                
                didCollide = true;
            }

            return didCollide;
        }

    }
}

using System;
using Raylib_cs;

namespace Vinter_Projekt
{
    class Program
    {
        static int screenW = 800;
        static int screenH = 600;

 
        static void Main(string[] args)
        {
            //Texture2D spaceship = Raylib.LoadTexture(@"Resurser/spaceship.png");
            
            Random generator = new Random();
            bool hasShot = false;

            //Slumpar fiendens storlek
            int enemySize = generator.Next(15, 35);

            //Slumpar fiendens position i x-led
            int enemyX = generator.Next(enemySize, screenW - enemySize * 2);

            //Slumpar fiendens hastighet
            int enemySpeed = generator.Next(2, 6);
            int enemyY = 0;

            //Create player
            Rectangle player = new Rectangle(screenW / 2, screenH / 2, 25, 25);

            Raylib.InitWindow(screenW, screenH, "Space Shooter 76");
            Raylib.SetTargetFPS(60);

            // Animationsloopen
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.LIGHTGRAY);

                

                //Create enemy
                Rectangle enemy = new Rectangle(enemyX, enemyY, enemySize, enemySize);

                //Move player
                Move(ref player.x, ref player.y);

                if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
                {
                    hasShot = true;
                    Bullet.TurnOnBulletColor();
                }

                if (hasShot)
                {
                    float bulletPosY = Bullet.Shoot(player.x, player.y);

                    if (bulletPosY < -15)
                    {
                        hasShot = false;
                    }
                    else if (Raylib.CheckCollisionRecs(enemy, Bullet.bulletRectangle))
                    {
                        enemySize = 0;
                        Bullet.BulletHit();
                    }
                }

                Raylib.DrawRectangleRec(player, Color.BLUE);
                PlayerCollision(ref player);

                //Draw enemy
                Raylib.DrawRectangleRec(enemy, Color.BLACK);

                //Move enemies
                enemyY += enemySpeed;

                Raylib.EndDrawing();
            }
        }

        static void Move(ref float posX, ref float posY)
        {
            int pullForce = 3;
            int speed = 6;

            //Om man åker uppåt
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
            {
                posY -= speed;
            }
            //Åker automatiskt neråt
            else
            {
                posY += pullForce;
            }

            //Om man åker neråt
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
            {
                posY += speed / 2;
            }

            //Åker åt höger
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                posX += speed;
            }
            //Åker åt vänster
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                posX -= speed;
            }
        }

        static void PlayerCollision(ref Rectangle player)
        {
            if (player.y <= 0)
            {
                player.y = 0;
            }
            else if (player.y + 25 >= screenH)
            {
                player.y = screenH - 25;
            }
            if (player.x <= 0)
            {
                player.x = 0;
            }
            else if (player.x + 25 >= screenW)
            {
                player.x = screenW - 25;
            }
        }
    }

    class Bullet
    {
        static bool check = false;

        static int bulletWidth = 10;
        static int bulletHeight = 15;

        static Color bulletColor = Color.GOLD;

        public static Rectangle bulletRectangle = new Rectangle(0, 0, bulletWidth, bulletHeight);

        public static float Shoot(float playerPosX, float playerPosY)
        {
            if (bulletRectangle.y > -bulletHeight-1)
            {
                int bulletSpeed = 10;

                if (!check)
                {
                    //Sätter skottets position lika med spelarens position
                    bulletRectangle.x = playerPosX + bulletWidth / 2;
                    bulletRectangle.y = playerPosY;
                    check = true;
                }

                //Ritar skottet
                Raylib.DrawRectangleRec(bulletRectangle, bulletColor);

                //Flyttar skottet 
                bulletRectangle.y -= bulletSpeed;
            }
            else
            {
                //Återställer skottets position när skottet hamnar utanför skärmen
                bulletRectangle.x = playerPosX + bulletWidth / 2;
                bulletRectangle.y = playerPosY;
            }

            return bulletRectangle.y;
        }
    
        public static void ResetBulletPosition(int playerPosX, int playerPosY)
        {
            //Återställer skottets position
            bulletRectangle.x = playerPosX + bulletWidth / 2;
            bulletRectangle.y = playerPosY;
        }

        public static void BulletHit()
        {
            //Stänger av skottets färg
            bulletColor = Color.BLANK;

            //flyttar skottet till slutet så att spelaren kan skjuta igen
            bulletRectangle.y = -15;
        }
        public static void TurnOnBulletColor()
        {
            bulletColor = Color.GOLD;
        }
    }
}
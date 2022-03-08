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
            Random generator = new Random();
            bool hasShot = false;

            //Spelarens position
            int posX = 200;
            int posY = 300;

            //Slumpar fiendens storlek
            int enemySize = generator.Next(15, 35);

            //Slumpar fiendens position i x-led
            int enemyX = generator.Next(enemySize, screenW - enemySize * 2);

            //Slumpar fiendens hastighet
            int enemySpeed = generator.Next(2, 6);
            int enemyY = 0;

            //Texture2D spaceship = Raylib.LoadTexture(@"Resurser/spaceship.png");

            Raylib.InitWindow(screenW, screenH, "Space Shooter 76");
            Raylib.SetTargetFPS(60);

            // Animationsloopen
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.LIGHTGRAY);

                //Create player
                Rectangle player = new Rectangle(posX, posY, 25, 25);
                
                //Create enemy
                Rectangle enemy = new Rectangle(enemyX, enemyY, enemySize, enemySize);


                //Move player
                Move(ref posX, ref posY);

                if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
                {
                    hasShot = true;
                }

                if (hasShot)
                {
                    float bulletPosY = Bullet.Shoot(posX, posY);

                    if (bulletPosY < 1 || Raylib.CheckCollisionRecs(player, enemy))
                    {
                        hasShot = false;
                    }
                }


                //Raylib.DrawTexture(spaceship, 200, 300, Color.BLACK);
                Raylib.DrawRectangleRec(player, Color.BLUE);

                //Draw enemy
                Raylib.DrawRectangleRec(enemy, Color.BLACK);

                //Move enemies
                enemyY += enemySpeed;

                Raylib.EndDrawing();
            }
        }

        static void Move(ref int posX, ref int posY)
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
    }

    class Bullet
    {
        static int bulletY = 0;
        static int bulletX = 0;

        static bool check = false;

        public static float Shoot(int posX, int posY)
        {
            if (bulletY > 0)
            {
                int bulletWidth = 10;
                int bulletHeight = 15;
                int bulletSpeed = 10;

                if (!check)
                {
                    //Sätter skottets position lika med spelarens position
                    bulletX = posX;
                    bulletY = posY;
                    check = true;
                }

                //Ritar skottet och flyttar det
                Raylib.DrawRectangle(bulletX + bulletWidth / 2, bulletY, bulletWidth, bulletHeight, Color.GOLD);
                bulletY -= bulletSpeed;
            }
            else
            {
                bulletX = posX;
                bulletY = posY;
            }

            return bulletY;
        }
    }
}
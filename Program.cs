using System;
using Raylib_cs;

namespace Vinter_Projekt
{
    class Program
    {
        static bool hasShot = false;
        static Rectangle bullet = new Rectangle();

        static void Main(string[] args)
        {
            const int fönsterB = 800;
            const int fönsterH = 600;

            int posX = 200;
            int posY = 300;

            Random generator = new Random();

            int enemySize = generator.Next(15, 35); ;
            int enemyX = generator.Next(enemySize, fönsterB - enemySize * 2);
            int enemySpeed = generator.Next(2, 6);
            int enemyY = 0;

            //Texture2D spaceship = Raylib.LoadTexture(@"Resurser/spaceship.png");

            Raylib.InitWindow(fönsterB, fönsterH, "Flappy Bird");
            Raylib.SetTargetFPS(60);

            // Animationsloopen
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.LIGHTGRAY);

                Shoot(posX, posY, enemySize);

                Raylib.DrawRectangle(posX, posY, 25, 25, Color.BLUE);

                //Raylib.DrawTexture(spaceship, 200, 300, Color.BLACK);

                enemyY += enemySpeed;

                Drive(ref posX, ref posY);

                Raylib.DrawRectangle(enemyX, enemyY, enemySize, enemySize, Color.BLACK);

                Raylib.EndDrawing();
            }
        }

        static void Drive(ref int posX, ref int posY)
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

        static void Shoot(int posX, int posY, int enemySize)
        {
            int bulletSpeed = 10;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                hasShot = true;
            }

            if (hasShot)
            {
                Raylib.DrawRectangleRec(bullet, Color.DARKPURPLE);
                bullet.y -= bulletSpeed;
            }
            else
            {
                bullet.x = posX + enemySize / 4;
                bullet.y = posY;
                bullet.width = 10;
                bullet.height = 15;
            }
        }
    }
}
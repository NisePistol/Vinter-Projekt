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
            float time = 30;
            int points = 0;

            //Skapar variabler till fienden
            int enemyX = 0;
            int enemyY = 0;
            int enemySize = 0;
            int enemySpeed = 0;

            //Sätter fiendens position
            ResetEnemy(ref enemyX, ref enemyY, ref enemySize, ref enemySpeed);

            //Create player
            Rectangle player = new Rectangle(screenW / 2, screenH / 2, 25, 25);

            Raylib.InitWindow(screenW, screenH, "Space Shooter 76");
            Raylib.SetTargetFPS(60);

            // Animationsloopen
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                Raylib.ClearBackground(Color.LIGHTGRAY);

                //Scen 1
                if (time > 0)
                {
                    //Ritar tiden som text och minskar den varje sekund
                    Raylib.DrawText($"{(int)time}", 350, 100, 75, Color.RED);
                    time -= Raylib.GetFrameTime();

                    //Skapar fiende
                    Rectangle enemy = new Rectangle(enemyX, enemyY, enemySize, enemySize);

                    //Flyttar spelare
                    Move(ref player.x, ref player.y);

                    //Om klickar space så skjuter man
                    if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
                    {
                        hasShot = true;
                    }

                    if (hasShot)
                    {
                        //Kör skjut funktionen från bullet klassen
                        float bulletPosY = Bullet.Shoot(player.x, player.y);

                        //Om skottet åker utanför skärmen så skjuter man inte längre
                        if (bulletPosY < -15)
                        {
                            hasShot = false;
                        }

                        //Om skottet träffar en fiende
                        else if (Raylib.CheckCollisionRecs(enemy, Bullet.bulletRectangle))
                        {
                            //Återställer skottet
                            Bullet.ResetBullet();

                            //Man får ett poäng
                            points++;

                            //Återställer fiendens position och värden
                            ResetEnemy(ref enemyX, ref enemyY, ref enemySize, ref enemySpeed);
                        }
                    }

                    //Ritar spelaren
                    Raylib.DrawRectangleRec(player, Color.BLUE);

                    //Kollar kollisionen för spelaren
                    PlayerCollision(ref player);

                    //Ritar fienden
                    Raylib.DrawRectangleRec(enemy, Color.BLACK);

                    //Om fienden åker utanför skärmen kommer en ny
                    if (enemyY + enemySize > screenH)
                    {
                        ResetEnemy(ref enemyX, ref enemyY, ref enemySize, ref enemySpeed);
                    }

                    //Flytta fiende
                    enemyY += enemySpeed;
                }

                //Scen 2
                else
                {
                    //Skriver slut text
                    Raylib.DrawText("GAME OVER", 185, 100, 75, Color.RED);
                    Raylib.DrawText($"SCORE: {points}", 230, 250, 75, Color.PURPLE);
                }

                Raylib.EndDrawing();
            }
        }

        static void ResetEnemy (ref int enemyX, ref int enemyY, ref int enemySize, ref int enemySpeed) 
        {
            Random generator = new Random();
            enemyY = 0;

            //Slumpar fiendens x position
            enemyX = generator.Next(enemySize, screenW - enemySize * 2);

            //Slumpar fiendens storlek
            enemySize = generator.Next(15, 35);

            //Slumpar fiendens hastighet
            enemySpeed = generator.Next(3, 6);
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
            //Gör så att spelaren inte kan åka utanför skärmen
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
            if (bulletRectangle.y > -bulletHeight - 1)
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

        public static void ResetBullet()
        {
            //flyttar skottet till slutet så att spelaren kan skjuta igen
            bulletRectangle.y = -15;
        }
    }
}
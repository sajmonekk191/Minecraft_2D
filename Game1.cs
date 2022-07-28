﻿using System;
using System.Drawing;
using System.Windows.Forms;
using test_RPG.Essentials;
using test_RPG.Essentials.GameObjects;

namespace test_RPG
{
    public partial class Game1 : Form
    {
        Player player;
        Food food;
        Gate gate;
        private int speed = 5;
        private Timer GameTimer = new Timer();
        private Timer timeleft = new Timer();
        private Random random = new Random();
        private bool foodeaten = true;
        public Game1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            player = new Player(this);
            food = new Food(this);
            gate = new Gate(this);
            player.Spawn();
            GameTimer.Enabled = true;
            timeleft.Enabled = true;
            GameTimer.Interval = 100;
            GameTimer.Interval = hodnoty.FPS;
            GameTimer.Tick += new EventHandler(Updater);
            timeleft.Tick += new EventHandler(timeleftevent);
            timebar.Value = 100;
            levellbl.Text = "Level: " + hodnoty.level;
            scorelbl.Text = "Score: " + hodnoty.score;
        }
        private void Updater(Object Object, EventArgs EventArgs)
        {
            Barrier();
            if (Imports.isUpPressed())
            {
                player.Top -= speed;
                player.Image = Properties.Resources.Player_up;
            }
            if (Imports.isDownPressed())
            {
                player.Top += speed;
                player.Image = Properties.Resources.Player_down;
            }
            if (Imports.isLeftPressed())
            {
                player.Left -= speed;
                player.Image = Properties.Resources.Player_left;
            }
            if (Imports.isRightPressed())
            {
                player.Left += speed;
                player.Image = Properties.Resources.Player_right;
            }
            if (foodeaten)
            {
                food.Spawn();
                food.Location = new Point(random.Next(0, 600), random.Next(0, 600));
                foodeaten = false;
                Gatepercent();
            }
            foreach (Control i in this.Controls)
            { 
                if (i is PictureBox && i.Tag == "food")
                {
                    if (player.Bounds.IntersectsWith(i.Bounds))
                    {
                        foodeaten = true;
                        hodnoty.score++;
                        this.Controls.Remove(i);
                        scorelbl.Text = "Score: " + hodnoty.score.ToString();
                        try { timebar.Value += 10; }
                        catch { timebar.Value = 100; }
                    }
                }
                if (i is PictureBox && i.Tag == "gate")
                {
                    if (player.Bounds.IntersectsWith(i.Bounds))
                    {
                        hodnoty.level++;
                        RenderLVL(hodnoty.level);
                        this.Controls.Remove(i);
                    }
                }
            }
        }
        private void timeleftevent(Object myObject, EventArgs myEventArgs)
        {
            if (hodnoty.score <= 10) ScoreCalculator(1);
            if (hodnoty.score >= 10 && hodnoty.score <= 20) ScoreCalculator(2);
            if (hodnoty.score >= 20 && hodnoty.score <= 30) ScoreCalculator(3);
        }
        private void GameOver()
        {
            GameTimer.Enabled = false;
            timeleft.Enabled = false;
            MessageBox.Show("Game Over!\nYour Score: " + hodnoty.score, "Game", MessageBoxButtons.OK);
            Environment.Exit(0);
        }
        private void ScoreCalculator(int value)
        {
            try { timebar.Value -= value; }
            catch { GameOver(); }
        }
        private void Gatepercent()
        {
            int value = 0;
            if (hodnoty.score > 13)
            {
                value = random.Next(0, 10);
            }
            if (hodnoty.score > 20)
            {
                value = random.Next(5, 10);
            }
            if (hodnoty.score > 25)
            {
                value = random.Next(7, 10);
            }
            if (value > 8)
            {
                gate.Spawn();
                if (hodnoty.level == 2) gate.Image = Properties.Resources.Final_Gate;
                gate.Location = new Point(random.Next(0, 600), random.Next(0, 600));
                gate.Visible = true;
            }
        }
        private void RenderLVL(int room)
        {
            switch (room)
            {
                case 2:
                    timebar.Value = 100;
                    levellbl.Text = "Level: " + hodnoty.level.ToString();
                    scorelbl.Text = "Score: " + hodnoty.score.ToString();
                    this.BackColor = Color.MediumBlue;
                    break;
                case 3:
                    Game2 level3 = new Game2();
                    level3.Show();
                    level3.BringToFront();
                    GameTimer.Stop();
                    timeleft.Stop();
                    this.Hide();
                    break;
            }
        }
        private void Barrier()
        {
            if (player.Location.X < 5) player.Location = new Point(player.Location.X + 10, player.Location.Y);
            if (player.Location.X > 930) player.Location = new Point(player.Location.X - 10, player.Location.Y);
            if (player.Location.Y < 5) player.Location = new Point(player.Location.X, player.Location.Y + 10);
            if (player.Location.Y > 585) player.Location = new Point(player.Location.X, player.Location.Y - 10);
        }
    }
}

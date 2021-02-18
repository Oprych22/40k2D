using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _40k2d
{
    public partial class Form1 : Form
    {

        bool mousePressed, multipress, shootPress;
        float x, y;
        readonly List<Player> players = new List<Player>();
        readonly List<Wall> walls = new List<Wall>();
        Unit activeUnit = null;
        float currentMouseX, currentMouseY;
        Wall wall;
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            players.Add(new Player(50, 50, Color.Blue));
            players.Add(new Player(300, 300, Color.Red));
          //  walls.Add(new Wall() { x1 = 100, x2 = 150, y1 = 130, y2 = 150 });

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            walls.ForEach(w => w.Paint(e));
            players.ForEach(p => p.units.PaintUnits(e));
            if (shootPress && activeUnit != null)
                activeUnit.PaintShoot(e);
            players.ForEach(p => p.units.PaintClosest(e,currentMouseX,currentMouseY));
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
                multipress = true;

            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                shootPress = true;
            mousePressed = true;
            x = e.X;
            y = e.Y;
            activeUnit = GetUnit(e.X, e.Y);
            if (activeUnit != null)
                activeUnit.active = true;
            else
            {
                wall = new Wall() { x1 = Convert.ToInt32(x), y1 = Convert.ToInt32(y) };
            }

        }

        private Unit GetUnit(float x, float y)
        {
            Unit unit;
            foreach (Player p in players)
            {
                unit = p.units.Collision(x, y);
                if (unit != null)
                    return unit;
            }
            return null;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mousePressed = false;
            multipress = false;
            shootPress = false;
            if (activeUnit != null)
            {
                activeUnit.active = false;
                activeUnit.shooting = false;
            }
            else
            {
                wall.y2 = e.Y;
                wall.x2 = e.X;
                wall.active = true;
                walls.Add(wall);
            }
            Refresh();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            currentMouseX = e.X;
            currentMouseY = e.Y;
            if (mousePressed && activeUnit != null)
            {
                if (e.X != x || e.Y != y)
                {
                    if (multipress)
                        activeUnit.owner.units.MoveAllUnits(e.X - x, e.Y - y);
                    else if (shootPress)
                    {
                        Unit shotUnit = GetUnit(e.X, e.Y);
                        foreach(Wall w in walls)
                        {
                            if (activeUnit.LineHit(w.x1, w.x2, w.y1, w.y2, e.X,e.Y))
                            {
                                activeUnit.lineHit = true;
                                break;
                            }
                            else
                                activeUnit.lineHit = false;

                        }
                        if (shotUnit != null && shotUnit.owner != activeUnit.owner)
                            activeUnit.ShootAt(shotUnit.xPos+ (shotUnit.size/2), shotUnit.yPos + (shotUnit.size / 2));
                        else
                            activeUnit.ShootAt(e.X, e.Y);
                    }
                    else
                        activeUnit.Move(e.X - x, e.Y - y);
                    x = e.X;
                    y = e.Y;
                }
            }

            Refresh();
        }
    }

 
   
}

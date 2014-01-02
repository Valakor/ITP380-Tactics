using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace itp380
{
    public partial class LevelEditor : Form
    {

        enum Brush { hWall, vWall, spawn, king, erase }
        Brush currentBrush;
        List<EditorTile> tiles;
        UI.UIMainMenu menu;

        public LevelEditor(UI.UIMainMenu mm)
        {
            menu = mm;
            currentBrush = Brush.erase;
            InitializeComponent();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewMapDialog newDialog = new NewMapDialog(this);
            newDialog.ShowDialog();
        }

        private void exportXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialogue = new SaveFileDialog();
            dialogue.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            if (dialogue.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(dialogue.FileName);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(EditorTile));
                for (int i = 0; i < tiles.Count; i++)
                {
                    writer.Serialize(file, tiles[i]);
                }
                file.Close();
                this.Close();
            }
        }

        private void LevelEditor_Load(object sender, EventArgs e)
        {

        }

        public void AddItem(object sender, EventArgs e)
        {
            if (currentBrush == Brush.erase)
            {
                ((Button)sender).BackColor = Color.White;
                int xGrid = (((Button)sender).Left - 20) / 20;
                int yGrid = (((Button)sender).Top - 40) / 20;
                ChangeTypeOfTile(xGrid, yGrid, "none");
            }
            else if (currentBrush == Brush.hWall)
            {
                ((Button)sender).BackColor = Color.Black;
                int xGrid = (((Button)sender).Left - 20) / 20;
                int yGrid = (((Button)sender).Top - 40) / 20;
                ChangeTypeOfTile(xGrid, yGrid, "hWall");
            }
            else if (currentBrush == Brush.vWall)
            {
                ((Button)sender).BackColor = Color.Black;
                int xGrid = (((Button)sender).Left - 20) / 20;
                int yGrid = (((Button)sender).Top - 40) / 20;
                ChangeTypeOfTile(xGrid, yGrid, "vWall");
            }
            else if (currentBrush == Brush.spawn)
            {
                ((Button)sender).BackColor = Color.Magenta;
                int xGrid = (((Button)sender).Left - 20) / 20;
                int yGrid = (((Button)sender).Top - 40) / 20;
                ChangeTypeOfTile(xGrid, yGrid, "spawn");
            }
            else if (currentBrush == Brush.king)
            {
                ((Button)sender).BackColor = Color.Green;
                int xGrid = (((Button)sender).Left - 20) / 20;
                int yGrid = (((Button)sender).Top - 40) / 20;
                ChangeTypeOfTile(xGrid, yGrid, "king");
            }
        }

        public void ChangeTypeOfTile(int x, int y, String t)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].xGrid == x && tiles[i].yGrid == y)
                {
                    tiles[i].type = t;
                }
            }
        }

        public void createNewMapOfSize(int rows, int columns)
        {
            tiles = new List<EditorTile>(rows * columns);
            int width = 20;
            int height = 20;
            int x_start = 20;
            int y_start = 40;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    EditorTile newTile = new EditorTile();
                    newTile.setType("none");
                    newTile.setX(j);
                    newTile.setY(i);
                    tiles.Add(newTile);
                    Button newButton = new Button();
                    newButton.Top = y_start + j * 20;
                    newButton.Left = x_start + i * 20;
                    newButton.Width = width;
                    newButton.Height = height;
                    newButton.BackColor = Color.White;
                    newButton.Click += AddItem;
                    this.Controls.Add(newButton);
                }
            }
        }

        private void BrushCombo_Click(object sender, EventArgs e)
        {
            
        }

        private void BrushCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(((ToolStripComboBox)sender).ComboBox.GetItemText(sender).Contains("Erase"))
            {
                currentBrush = Brush.erase;
            }
            else if (((ToolStripComboBox)sender).ComboBox.GetItemText(sender).Contains("Spawn"))
            {
                currentBrush = Brush.spawn;
            }
            else if (((ToolStripComboBox)sender).ComboBox.GetItemText(sender).Contains("Horizontal Wall"))
            {
                currentBrush = Brush.hWall;
            }
            else if (((ToolStripComboBox)sender).ComboBox.GetItemText(sender).Contains("Vertical Wall"))
            {
                currentBrush = Brush.vWall;
            }
            else if (((ToolStripComboBox)sender).ComboBox.GetItemText(sender).Contains("King"))
            {
                currentBrush = Brush.king;
            }
        }

        private void loadLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            if (open.ShowDialog() == DialogResult.OK)
            {
                menu.loadLevel(open.FileName);
                this.Close();
            }
        }


    }

    public class EditorTile
    {
        public int xGrid;
        public int yGrid;
        public String type;
        public EditorTile()
        {
            xGrid = -1;
            yGrid = -1;
            type = "";
        }
        public void setX(int x)
        {
            xGrid = x;
        }
        public void setY(int y)
        {
            yGrid = y;
        }
        public void setType(String t)
        {
            type = t;
        }
    }
}

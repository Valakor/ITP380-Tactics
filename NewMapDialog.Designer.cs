namespace itp380
{
    partial class NewMapDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.ColumnInput = new System.Windows.Forms.TextBox();
            this.RowInput = new System.Windows.Forms.TextBox();
            this.RowLabel = new System.Windows.Forms.Label();
            this.ColumnLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(145, 127);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ColumnInput
            // 
            this.ColumnInput.Location = new System.Drawing.Point(220, 40);
            this.ColumnInput.Name = "ColumnInput";
            this.ColumnInput.Size = new System.Drawing.Size(100, 20);
            this.ColumnInput.TabIndex = 1;
            this.ColumnInput.TextChanged += new System.EventHandler(this.ColumnInput_TextChanged);
            // 
            // RowInput
            // 
            this.RowInput.Location = new System.Drawing.Point(20, 40);
            this.RowInput.Name = "RowInput";
            this.RowInput.Size = new System.Drawing.Size(100, 20);
            this.RowInput.TabIndex = 2;
            this.RowInput.TextChanged += new System.EventHandler(this.RowInput_TextChanged);
            // 
            // RowLabel
            // 
            this.RowLabel.AutoSize = true;
            this.RowLabel.Location = new System.Drawing.Point(17, 24);
            this.RowLabel.Name = "RowLabel";
            this.RowLabel.Size = new System.Drawing.Size(71, 13);
            this.RowLabel.TabIndex = 3;
            this.RowLabel.Text = "Rows of Tiles";
            // 
            // ColumnLabel
            // 
            this.ColumnLabel.AutoSize = true;
            this.ColumnLabel.Location = new System.Drawing.Point(220, 21);
            this.ColumnLabel.Name = "ColumnLabel";
            this.ColumnLabel.Size = new System.Drawing.Size(84, 13);
            this.ColumnLabel.TabIndex = 4;
            this.ColumnLabel.Text = "Columns of Tiles";
            // 
            // NewMapDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 162);
            this.Controls.Add(this.ColumnLabel);
            this.Controls.Add(this.RowLabel);
            this.Controls.Add(this.RowInput);
            this.Controls.Add(this.ColumnInput);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewMapDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NewMapDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox ColumnInput;
        private System.Windows.Forms.TextBox RowInput;
        private System.Windows.Forms.Label RowLabel;
        private System.Windows.Forms.Label ColumnLabel;
    }
}
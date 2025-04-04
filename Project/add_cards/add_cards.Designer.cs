namespace add_cards
{
    partial class add_cards
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
            this.add_cards_DB = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // add_cards_DB
            // 
            this.add_cards_DB.Font = new System.Drawing.Font("TH Sarabun New", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.add_cards_DB.Location = new System.Drawing.Point(473, 337);
            this.add_cards_DB.Name = "add_cards_DB";
            this.add_cards_DB.Size = new System.Drawing.Size(272, 75);
            this.add_cards_DB.TabIndex = 0;
            this.add_cards_DB.Text = "เพิ่มข้อมูลของไพ่ใหม่อีกครั้ง";
            this.add_cards_DB.UseVisualStyleBackColor = true;
            this.add_cards_DB.Click += new System.EventHandler(this.add_cards_DB_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(640, 418);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(8, 8);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // add_cards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 460);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.add_cards_DB);
            this.Name = "add_cards";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button add_cards_DB;
        private System.Windows.Forms.Button button1;
    }
}


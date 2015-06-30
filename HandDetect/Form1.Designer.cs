namespace Bitirme
{
    partial class Form1
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
            this.video_box = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.combo_dev = new System.Windows.Forms.ComboBox();
            this.otsu_pcbx = new System.Windows.Forms.PictureBox();
            this.gri_pcx = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.video_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.otsu_pcbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gri_pcx)).BeginInit();
            this.SuspendLayout();
            // 
            // video_box
            // 
            this.video_box.Location = new System.Drawing.Point(36, 22);
            this.video_box.Name = "video_box";
            this.video_box.Size = new System.Drawing.Size(290, 266);
            this.video_box.TabIndex = 0;
            this.video_box.TabStop = false;
            this.video_box.Click += new System.EventHandler(this.video_box_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(36, 370);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Kamerayı Başlat";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(138, 370);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Kamerayı Durdur";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // combo_dev
            // 
            this.combo_dev.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_dev.FormattingEnabled = true;
            this.combo_dev.Location = new System.Drawing.Point(36, 325);
            this.combo_dev.Name = "combo_dev";
            this.combo_dev.Size = new System.Drawing.Size(198, 21);
            this.combo_dev.TabIndex = 3;
            // 
            // otsu_pcbx
            // 
            this.otsu_pcbx.Location = new System.Drawing.Point(379, 22);
            this.otsu_pcbx.Name = "otsu_pcbx";
            this.otsu_pcbx.Size = new System.Drawing.Size(290, 266);
            this.otsu_pcbx.TabIndex = 4;
            this.otsu_pcbx.TabStop = false;
            this.otsu_pcbx.Click += new System.EventHandler(this.otsu_pcbx_Click);
            // 
            // gri_pcx
            // 
            this.gri_pcx.Location = new System.Drawing.Point(379, 316);
            this.gri_pcx.Name = "gri_pcx";
            this.gri_pcx.Size = new System.Drawing.Size(290, 266);
            this.gri_pcx.TabIndex = 5;
            this.gri_pcx.TabStop = false;
            this.gri_pcx.Click += new System.EventHandler(this.gri_pcx_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(685, 399);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "İşlenmiş Görüntü";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(675, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Otsu Seviye";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(36, 451);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "İzlemeye Başla";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(138, 451);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "Gönder";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 594);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gri_pcx);
            this.Controls.Add(this.otsu_pcbx);
            this.Controls.Add(this.combo_dev);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.video_box);
            this.Name = "Form1";
            this.Text = "Görüntü İşleme El İzleme";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.video_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.otsu_pcbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gri_pcx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox video_box;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox combo_dev;
        private System.Windows.Forms.PictureBox otsu_pcbx;
        private System.Windows.Forms.PictureBox gri_pcx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}


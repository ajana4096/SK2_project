﻿using System.Drawing;
using System.Windows.Forms;

namespace Draughts
{
    partial class Settings
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            //             
            this.textBox1.Location = new System.Drawing.Point(231, 144);
            this.textBox1.Name = "textBox1";
            this.textBox1.BorderStyle = BorderStyle.None;
            this.textBox1.BackColor = System.Drawing.Color.SaddleBrown;
            this.textBox1.ForeColor = System.Drawing.Color.GhostWhite;
            this.textBox1.Size = new System.Drawing.Size(156, 26);
            this.textBox1.Font = new Font("Arial", 14, FontStyle.Bold);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(231, 194);
            this.textBox2.Name = "textBox2";
            this.textBox2.BorderStyle = BorderStyle.None;
            this.textBox2.BackColor = System.Drawing.Color.SaddleBrown;
            this.textBox2.ForeColor = System.Drawing.Color.GhostWhite;
            this.textBox2.Size = new System.Drawing.Size(156, 26);
            this.textBox2.Font = new Font("Arial", 14, FontStyle.Bold);
            this.textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(231, 243);
            this.textBox3.Name = "textBox3";
            this.textBox3.BorderStyle = BorderStyle.None;
            this.textBox3.BackColor = System.Drawing.Color.SaddleBrown;
            this.textBox3.ForeColor = System.Drawing.Color.GhostWhite;
            this.textBox3.Size = new System.Drawing.Size(156, 26);
            this.textBox3.Font = new Font("Arial", 14, FontStyle.Bold);
            this.textBox3.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(231, 99);
            this.button1.Name = "button1";
            this.button1.TabStop = false;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.BackColor = System.Drawing.Color.SaddleBrown;
            this.button1.ForeColor = System.Drawing.Color.GhostWhite;
            this.button1.Size = new System.Drawing.Size(156, 26);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = Color.Transparent;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.Font = new Font("Arial", 14, FontStyle.Bold);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tryb gry";
            // 
            // label2
            // 
            this.label2.BackColor = Color.Transparent;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.Font = new Font("Arial", 14, FontStyle.Bold);
            this.label2.TabIndex = 5;
            this.label2.Text = "Adres serwera";
            // 
            // label3
            // 
            this.label3.BackColor = Color.Transparent;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.Font = new Font("Arial", 14, FontStyle.Bold);
            this.label3.TabIndex = 6;
            this.label3.Text = "Tryb gry";
            // 
            // label4
            // 
            this.label4.BackColor = Color.Transparent;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(72, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Tryb gry";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
namespace Enerefsys
{
    partial class CoolTower
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
            this.coolTower_l = new System.Windows.Forms.Label();
            this.coolTower_l1 = new System.Windows.Forms.Label();
            this.coolTower_cb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.coolTower_tb = new System.Windows.Forms.TextBox();
            this.coolTower_tb1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // coolTower_l
            // 
            this.coolTower_l.AutoSize = true;
            this.coolTower_l.Location = new System.Drawing.Point(12, 28);
            this.coolTower_l.Name = "coolTower_l";
            this.coolTower_l.Size = new System.Drawing.Size(41, 12);
            this.coolTower_l.TabIndex = 0;
            this.coolTower_l.Text = "冷却塔";
            // 
            // coolTower_l1
            // 
            this.coolTower_l1.AutoSize = true;
            this.coolTower_l1.Location = new System.Drawing.Point(12, 69);
            this.coolTower_l1.Name = "coolTower_l1";
            this.coolTower_l1.Size = new System.Drawing.Size(47, 12);
            this.coolTower_l1.TabIndex = 2;
            this.coolTower_l1.Text = "冷却塔1";
            // 
            // coolTower_cb
            // 
            this.coolTower_cb.FormattingEnabled = true;
            this.coolTower_cb.Location = new System.Drawing.Point(196, 66);
            this.coolTower_cb.Name = "coolTower_cb";
            this.coolTower_cb.Size = new System.Drawing.Size(102, 20);
            this.coolTower_cb.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "变频否";
            // 
            // coolTower_tb
            // 
            this.coolTower_tb.Location = new System.Drawing.Point(59, 25);
            this.coolTower_tb.Name = "coolTower_tb";
            this.coolTower_tb.Size = new System.Drawing.Size(100, 21);
            this.coolTower_tb.TabIndex = 10;
            // 
            // coolTower_tb1
            // 
            this.coolTower_tb1.Location = new System.Drawing.Point(59, 65);
            this.coolTower_tb1.Name = "coolTower_tb1";
            this.coolTower_tb1.Size = new System.Drawing.Size(100, 21);
            this.coolTower_tb1.TabIndex = 11;
            this.coolTower_tb1.Text = "流量";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(165, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "台";
            // 
            // CoolTower
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 329);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.coolTower_tb1);
            this.Controls.Add(this.coolTower_tb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.coolTower_cb);
            this.Controls.Add(this.coolTower_l1);
            this.Controls.Add(this.coolTower_l);
            this.Name = "CoolTower";
            this.Text = "冷却塔";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label coolTower_l;
        private System.Windows.Forms.Label coolTower_l1;
        private System.Windows.Forms.ComboBox coolTower_cb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox coolTower_tb;
        private System.Windows.Forms.TextBox coolTower_tb1;
        private System.Windows.Forms.Label label5;
    }
}
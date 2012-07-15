namespace WindowsFormsApplication1
{
    partial class SystemRealTime
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Temperature = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Load = new System.Windows.Forms.TextBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOptimizeResult = new System.Windows.Forms.Button();
            this.btnCoolTower = new System.Windows.Forms.Button();
            this.btnPump = new System.Windows.Forms.Button();
            this.btnEngine = new System.Windows.Forms.Button();
            this.btnEnvironmemt = new System.Windows.Forms.Button();
            this.textBox_Message = new System.Windows.Forms.TextBox();
            this.pictureBox_Result = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Result)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "室外温度";
            // 
            // textBox_Temperature
            // 
            this.textBox_Temperature.Location = new System.Drawing.Point(71, 63);
            this.textBox_Temperature.Name = "textBox_Temperature";
            this.textBox_Temperature.Size = new System.Drawing.Size(115, 21);
            this.textBox_Temperature.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "系统负荷";
            // 
            // textBox_Load
            // 
            this.textBox_Load.Location = new System.Drawing.Point(261, 64);
            this.textBox_Load.Name = "textBox_Load";
            this.textBox_Load.Size = new System.Drawing.Size(131, 21);
            this.textBox_Load.TabIndex = 3;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(660, 63);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 8;
            this.btnShow.Text = "计算";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOptimizeResult);
            this.groupBox1.Controls.Add(this.btnCoolTower);
            this.groupBox1.Controls.Add(this.btnPump);
            this.groupBox1.Controls.Add(this.btnEngine);
            this.groupBox1.Controls.Add(this.btnEnvironmemt);
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(831, 43);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置";
            // 
            // btnOptimizeResult
            // 
            this.btnOptimizeResult.Location = new System.Drawing.Point(424, 14);
            this.btnOptimizeResult.Name = "btnOptimizeResult";
            this.btnOptimizeResult.Size = new System.Drawing.Size(75, 23);
            this.btnOptimizeResult.TabIndex = 4;
            this.btnOptimizeResult.Text = "优化结果";
            this.btnOptimizeResult.UseVisualStyleBackColor = true;
            this.btnOptimizeResult.Click += new System.EventHandler(this.btnOptimizeResult_Click);
            // 
            // btnCoolTower
            // 
            this.btnCoolTower.Location = new System.Drawing.Point(329, 14);
            this.btnCoolTower.Name = "btnCoolTower";
            this.btnCoolTower.Size = new System.Drawing.Size(75, 23);
            this.btnCoolTower.TabIndex = 3;
            this.btnCoolTower.Text = "冷却塔配置";
            this.btnCoolTower.UseVisualStyleBackColor = true;
            this.btnCoolTower.Click += new System.EventHandler(this.btnCoolTower_Click);
            // 
            // btnPump
            // 
            this.btnPump.Location = new System.Drawing.Point(234, 14);
            this.btnPump.Name = "btnPump";
            this.btnPump.Size = new System.Drawing.Size(75, 23);
            this.btnPump.TabIndex = 2;
            this.btnPump.Text = "水泵配置";
            this.btnPump.UseVisualStyleBackColor = true;
            this.btnPump.Click += new System.EventHandler(this.btnPump_Click);
            // 
            // btnEngine
            // 
            this.btnEngine.Location = new System.Drawing.Point(144, 14);
            this.btnEngine.Name = "btnEngine";
            this.btnEngine.Size = new System.Drawing.Size(75, 23);
            this.btnEngine.TabIndex = 1;
            this.btnEngine.Text = "冷冻机配置";
            this.btnEngine.UseVisualStyleBackColor = true;
            this.btnEngine.Click += new System.EventHandler(this.btnEngine_Click);
            // 
            // btnEnvironmemt
            // 
            this.btnEnvironmemt.Location = new System.Drawing.Point(53, 14);
            this.btnEnvironmemt.Name = "btnEnvironmemt";
            this.btnEnvironmemt.Size = new System.Drawing.Size(75, 23);
            this.btnEnvironmemt.TabIndex = 0;
            this.btnEnvironmemt.Text = "环境配置";
            this.btnEnvironmemt.UseVisualStyleBackColor = true;
            this.btnEnvironmemt.Click += new System.EventHandler(this.btnEnvironmemt_Click);
            // 
            // textBox_Message
            // 
            this.textBox_Message.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox_Message.Font = new System.Drawing.Font("YouYuan", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Message.ForeColor = System.Drawing.Color.Black;
            this.textBox_Message.Location = new System.Drawing.Point(15, 115);
            this.textBox_Message.Multiline = true;
            this.textBox_Message.Name = "textBox_Message";
            this.textBox_Message.Size = new System.Drawing.Size(720, 186);
            this.textBox_Message.TabIndex = 10;
            // 
            // pictureBox_Result
            // 
            this.pictureBox_Result.Location = new System.Drawing.Point(14, 323);
            this.pictureBox_Result.Name = "pictureBox_Result";
            this.pictureBox_Result.Size = new System.Drawing.Size(721, 194);
            this.pictureBox_Result.TabIndex = 7;
            this.pictureBox_Result.TabStop = false;
            // 
            // SystemRealTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 529);
            this.Controls.Add(this.textBox_Message);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.pictureBox_Result);
            this.Controls.Add(this.textBox_Load);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_Temperature);
            this.Controls.Add(this.label1);
            this.Name = "SystemRealTime";
            this.Text = "SystemRealTime";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Result)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Temperature;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Load;
        private System.Windows.Forms.PictureBox pictureBox_Result;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCoolTower;
        private System.Windows.Forms.Button btnPump;
        private System.Windows.Forms.Button btnEngine;
        private System.Windows.Forms.Button btnEnvironmemt;
        private System.Windows.Forms.TextBox textBox_Message;
        private System.Windows.Forms.Button btnOptimizeResult;
    }
}
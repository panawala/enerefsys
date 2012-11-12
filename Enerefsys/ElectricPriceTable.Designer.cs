namespace Enerefsys
{
    partial class ElectricPriceTable
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSub = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxKvalue = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Month = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Day = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DryTemperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WetTemperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RealLoad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EnterTemperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ElectronicPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StandardLoadID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.btnSub);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxKvalue);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.btnImport);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(829, 501);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设计日逐时电价表";
            // 
            // btnSub
            // 
            this.btnSub.Location = new System.Drawing.Point(123, 20);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(75, 23);
            this.btnSub.TabIndex = 11;
            this.btnSub.Text = "确定";
            this.btnSub.UseVisualStyleBackColor = true;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "K值=";
            // 
            // textBoxKvalue
            // 
            this.textBoxKvalue.Location = new System.Drawing.Point(44, 22);
            this.textBoxKvalue.Name = "textBoxKvalue";
            this.textBoxKvalue.Size = new System.Drawing.Size(73, 21);
            this.textBoxKvalue.TabIndex = 9;
            this.textBoxKvalue.Text = "3.8";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(717, 20);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "取消";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(621, 20);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "导入Excel";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Month,
            this.Day,
            this.Hour,
            this.DryTemperature,
            this.WetTemperature,
            this.RealLoad,
            this.EnterTemperature,
            this.ElectronicPrice,
            this.StandardLoadID});
            this.dataGridView1.Location = new System.Drawing.Point(6, 60);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(814, 435);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // Month
            // 
            this.Month.DataPropertyName = "Month";
            this.Month.HeaderText = "月";
            this.Month.Name = "Month";
            this.Month.ReadOnly = true;
            // 
            // Day
            // 
            this.Day.DataPropertyName = "Day";
            this.Day.HeaderText = "日";
            this.Day.Name = "Day";
            this.Day.ReadOnly = true;
            // 
            // Hour
            // 
            this.Hour.DataPropertyName = "Hour";
            this.Hour.HeaderText = "时";
            this.Hour.Name = "Hour";
            this.Hour.ReadOnly = true;
            // 
            // DryTemperature
            // 
            this.DryTemperature.DataPropertyName = "DryTemperature";
            this.DryTemperature.HeaderText = "干球温度（℃）";
            this.DryTemperature.Name = "DryTemperature";
            this.DryTemperature.ReadOnly = true;
            // 
            // WetTemperature
            // 
            this.WetTemperature.DataPropertyName = "WetTemperature";
            this.WetTemperature.HeaderText = "湿球温度（℃）";
            this.WetTemperature.Name = "WetTemperature";
            this.WetTemperature.ReadOnly = true;
            // 
            // RealLoad
            // 
            this.RealLoad.DataPropertyName = "Load";
            this.RealLoad.HeaderText = "冷负荷(KW)";
            this.RealLoad.Name = "RealLoad";
            this.RealLoad.ReadOnly = true;
            // 
            // EnterTemperature
            // 
            this.EnterTemperature.DataPropertyName = "EnterTemperature";
            this.EnterTemperature.HeaderText = "冷却水进口温度（℃）";
            this.EnterTemperature.Name = "EnterTemperature";
            this.EnterTemperature.ReadOnly = true;
            // 
            // ElectronicPrice
            // 
            this.ElectronicPrice.DataPropertyName = "ElectronicPrice";
            this.ElectronicPrice.HeaderText = "电价（元/KW）";
            this.ElectronicPrice.Name = "ElectronicPrice";
            this.ElectronicPrice.ReadOnly = true;
            // 
            // StandardLoadID
            // 
            this.StandardLoadID.DataPropertyName = "StandardLoadID";
            this.StandardLoadID.HeaderText = "StandardLoadID";
            this.StandardLoadID.Name = "StandardLoadID";
            this.StandardLoadID.ReadOnly = true;
            this.StandardLoadID.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(228, 20);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(316, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(551, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 13;
            // 
            // ElectricPriceTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 501);
            this.Controls.Add(this.groupBox1);
            this.Name = "ElectricPriceTable";
            this.Text = "逐时负荷";
            this.Load += new System.EventHandler(this.ElectricPriceTable_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Month;
        private System.Windows.Forms.DataGridViewTextBoxColumn Day;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hour;
        private System.Windows.Forms.DataGridViewTextBoxColumn DryTemperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn WetTemperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn RealLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn EnterTemperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn ElectronicPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn StandardLoadID;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxKvalue;
        private System.Windows.Forms.Button btnSub;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;

    }
}
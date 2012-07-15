namespace WindowsFormsApplication1
{
    partial class Freezer
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
            this.freezerMachine_Label = new System.Windows.Forms.Label();
            this.freezerNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBox10 = new System.Windows.Forms.ComboBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox11 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBox12 = new System.Windows.Forms.ComboBox();
            this.freezer_Panel = new System.Windows.Forms.Panel();
            this.is_Frequency_Conversion_Label = new System.Windows.Forms.Label();
            this.performance_Data_Label = new System.Windows.Forms.Label();
            this.model_Label = new System.Windows.Forms.Label();
            this.brand_Label = new System.Windows.Forms.Label();
            this.cooling_Capacity_Label = new System.Windows.Forms.Label();
            this.type_Label = new System.Windows.Forms.Label();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // freezerMachine_Label
            // 
            this.freezerMachine_Label.AutoSize = true;
            this.freezerMachine_Label.Location = new System.Drawing.Point(32, 32);
            this.freezerMachine_Label.Name = "freezerMachine_Label";
            this.freezerMachine_Label.Size = new System.Drawing.Size(41, 12);
            this.freezerMachine_Label.TabIndex = 0;
            this.freezerMachine_Label.Text = "冷冻机";
            // 
            // freezerNum
            // 
            this.freezerNum.Location = new System.Drawing.Point(126, 29);
            this.freezerNum.Name = "freezerNum";
            this.freezerNum.Size = new System.Drawing.Size(100, 21);
            this.freezerNum.TabIndex = 1;
            this.freezerNum.TextChanged += new System.EventHandler(this.freezerNum_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "台";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(-4, 215);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(936, 316);
            this.dataGridView1.TabIndex = 30;
            // 
            // comboBox10
            // 
            this.comboBox10.FormattingEnabled = true;
            this.comboBox10.Location = new System.Drawing.Point(132, 537);
            this.comboBox10.Name = "comboBox10";
            this.comboBox10.Size = new System.Drawing.Size(121, 20);
            this.comboBox10.TabIndex = 31;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(34, 539);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(72, 16);
            this.checkBox4.TabIndex = 32;
            this.checkBox4.Text = "免费板换";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(60, 585);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 12);
            this.label12.TabIndex = 33;
            this.label12.Text = "板换1";
            // 
            // comboBox11
            // 
            this.comboBox11.FormattingEnabled = true;
            this.comboBox11.Location = new System.Drawing.Point(132, 582);
            this.comboBox11.Name = "comboBox11";
            this.comboBox11.Size = new System.Drawing.Size(121, 20);
            this.comboBox11.TabIndex = 34;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(330, 540);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 35;
            this.label13.Text = "板换投运条件";
            // 
            // comboBox12
            // 
            this.comboBox12.FormattingEnabled = true;
            this.comboBox12.Location = new System.Drawing.Point(430, 537);
            this.comboBox12.Name = "comboBox12";
            this.comboBox12.Size = new System.Drawing.Size(186, 20);
            this.comboBox12.TabIndex = 36;
            // 
            // freezer_Panel
            // 
            this.freezer_Panel.AutoScroll = true;
            this.freezer_Panel.Location = new System.Drawing.Point(-4, 80);
            this.freezer_Panel.Name = "freezer_Panel";
            this.freezer_Panel.Size = new System.Drawing.Size(936, 129);
            this.freezer_Panel.TabIndex = 37;
            // 
            // is_Frequency_Conversion_Label
            // 
            this.is_Frequency_Conversion_Label.AutoSize = true;
            this.is_Frequency_Conversion_Label.Location = new System.Drawing.Point(646, 65);
            this.is_Frequency_Conversion_Label.Name = "is_Frequency_Conversion_Label";
            this.is_Frequency_Conversion_Label.Size = new System.Drawing.Size(41, 12);
            this.is_Frequency_Conversion_Label.TabIndex = 48;
            this.is_Frequency_Conversion_Label.Text = "变频否";
            // 
            // performance_Data_Label
            // 
            this.performance_Data_Label.AutoSize = true;
            this.performance_Data_Label.Location = new System.Drawing.Point(780, 65);
            this.performance_Data_Label.Name = "performance_Data_Label";
            this.performance_Data_Label.Size = new System.Drawing.Size(53, 12);
            this.performance_Data_Label.TabIndex = 44;
            this.performance_Data_Label.Text = "性能数据";
            // 
            // model_Label
            // 
            this.model_Label.AutoSize = true;
            this.model_Label.Location = new System.Drawing.Point(530, 65);
            this.model_Label.Name = "model_Label";
            this.model_Label.Size = new System.Drawing.Size(29, 12);
            this.model_Label.TabIndex = 43;
            this.model_Label.Text = "型号";
            // 
            // brand_Label
            // 
            this.brand_Label.AutoSize = true;
            this.brand_Label.Location = new System.Drawing.Point(393, 65);
            this.brand_Label.Name = "brand_Label";
            this.brand_Label.Size = new System.Drawing.Size(29, 12);
            this.brand_Label.TabIndex = 42;
            this.brand_Label.Text = "品牌";
            // 
            // cooling_Capacity_Label
            // 
            this.cooling_Capacity_Label.AutoSize = true;
            this.cooling_Capacity_Label.Location = new System.Drawing.Point(268, 65);
            this.cooling_Capacity_Label.Name = "cooling_Capacity_Label";
            this.cooling_Capacity_Label.Size = new System.Drawing.Size(29, 12);
            this.cooling_Capacity_Label.TabIndex = 41;
            this.cooling_Capacity_Label.Text = "冷量";
            // 
            // type_Label
            // 
            this.type_Label.AutoSize = true;
            this.type_Label.Location = new System.Drawing.Point(155, 65);
            this.type_Label.Name = "type_Label";
            this.type_Label.Size = new System.Drawing.Size(29, 12);
            this.type_Label.TabIndex = 40;
            this.type_Label.Text = "类型";
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(847, 20);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 49;
            this.btn_ok.Text = "确定";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(308, 20);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(99, 23);
            this.btnLoadData.TabIndex = 50;
            this.btnLoadData.Text = "载入新的数据";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(450, 20);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(347, 23);
            this.progressBar1.TabIndex = 51;
            // 
            // Freezer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 630);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnLoadData);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.performance_Data_Label);
            this.Controls.Add(this.is_Frequency_Conversion_Label);
            this.Controls.Add(this.freezer_Panel);
            this.Controls.Add(this.comboBox12);
            this.Controls.Add(this.model_Label);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.brand_Label);
            this.Controls.Add(this.comboBox11);
            this.Controls.Add(this.cooling_Capacity_Label);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.type_Label);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.comboBox10);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.freezerNum);
            this.Controls.Add(this.freezerMachine_Label);
            this.Name = "Freezer";
            this.Text = "冷冻机";
            this.TextChanged += new System.EventHandler(this.freezerNum_TextChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label freezerMachine_Label;
        private System.Windows.Forms.TextBox freezerNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox comboBox10;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBox11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBox12;
        private System.Windows.Forms.Panel freezer_Panel;
        private System.Windows.Forms.Label is_Frequency_Conversion_Label;
        private System.Windows.Forms.Label performance_Data_Label;
        private System.Windows.Forms.Label model_Label;
        private System.Windows.Forms.Label brand_Label;
        private System.Windows.Forms.Label cooling_Capacity_Label;
        private System.Windows.Forms.Label type_Label;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
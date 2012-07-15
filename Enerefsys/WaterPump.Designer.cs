namespace Enerefsys
{
    partial class WaterPump
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
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.brand_Label = new System.Windows.Forms.Label();
            this.flow_Label = new System.Windows.Forms.Label();
            this.lift_Label = new System.Windows.Forms.Label();
            this.power_Label = new System.Windows.Forms.Label();
            this.model_Label = new System.Windows.Forms.Label();
            this.is_Frequency_Conversion_Label = new System.Windows.Forms.Label();
            this.performance_Data_Label = new System.Windows.Forms.Label();
            this.freezingNum = new System.Windows.Forms.TextBox();
            this.coolingNum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.coolingPanel = new System.Windows.Forms.Panel();
            this.freezingPanel = new System.Windows.Forms.Panel();
            this.type_comboBox = new System.Windows.Forms.Label();
            this.comboBox_PumpType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "冰冻水泵";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "冷却水泵";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(345, 50);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(173, 20);
            this.comboBox2.TabIndex = 3;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(345, 84);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(173, 20);
            this.comboBox4.TabIndex = 5;
            // 
            // brand_Label
            // 
            this.brand_Label.AutoSize = true;
            this.brand_Label.Location = new System.Drawing.Point(103, 119);
            this.brand_Label.Name = "brand_Label";
            this.brand_Label.Size = new System.Drawing.Size(29, 12);
            this.brand_Label.TabIndex = 6;
            this.brand_Label.Text = "品牌";
            // 
            // flow_Label
            // 
            this.flow_Label.AutoSize = true;
            this.flow_Label.Location = new System.Drawing.Point(194, 119);
            this.flow_Label.Name = "flow_Label";
            this.flow_Label.Size = new System.Drawing.Size(29, 12);
            this.flow_Label.TabIndex = 7;
            this.flow_Label.Text = "流量";
            // 
            // lift_Label
            // 
            this.lift_Label.AutoSize = true;
            this.lift_Label.Location = new System.Drawing.Point(288, 120);
            this.lift_Label.Name = "lift_Label";
            this.lift_Label.Size = new System.Drawing.Size(29, 12);
            this.lift_Label.TabIndex = 8;
            this.lift_Label.Text = "扬程";
            // 
            // power_Label
            // 
            this.power_Label.AutoSize = true;
            this.power_Label.Location = new System.Drawing.Point(382, 120);
            this.power_Label.Name = "power_Label";
            this.power_Label.Size = new System.Drawing.Size(29, 12);
            this.power_Label.TabIndex = 9;
            this.power_Label.Text = "功率";
            // 
            // model_Label
            // 
            this.model_Label.AutoSize = true;
            this.model_Label.Location = new System.Drawing.Point(476, 120);
            this.model_Label.Name = "model_Label";
            this.model_Label.Size = new System.Drawing.Size(29, 12);
            this.model_Label.TabIndex = 10;
            this.model_Label.Text = "型号";
            // 
            // is_Frequency_Conversion_Label
            // 
            this.is_Frequency_Conversion_Label.AutoSize = true;
            this.is_Frequency_Conversion_Label.Location = new System.Drawing.Point(650, 119);
            this.is_Frequency_Conversion_Label.Name = "is_Frequency_Conversion_Label";
            this.is_Frequency_Conversion_Label.Size = new System.Drawing.Size(41, 12);
            this.is_Frequency_Conversion_Label.TabIndex = 12;
            this.is_Frequency_Conversion_Label.Text = "变频否";
            // 
            // performance_Data_Label
            // 
            this.performance_Data_Label.AutoSize = true;
            this.performance_Data_Label.Location = new System.Drawing.Point(717, 119);
            this.performance_Data_Label.Name = "performance_Data_Label";
            this.performance_Data_Label.Size = new System.Drawing.Size(53, 12);
            this.performance_Data_Label.TabIndex = 13;
            this.performance_Data_Label.Text = "性能数据";
            // 
            // freezingNum
            // 
            this.freezingNum.Location = new System.Drawing.Point(137, 50);
            this.freezingNum.Name = "freezingNum";
            this.freezingNum.Size = new System.Drawing.Size(157, 21);
            this.freezingNum.TabIndex = 68;
            this.freezingNum.TextChanged += new System.EventHandler(this.freezingNum_TextChanged);
            // 
            // coolingNum
            // 
            this.coolingNum.Location = new System.Drawing.Point(137, 84);
            this.coolingNum.Name = "coolingNum";
            this.coolingNum.Size = new System.Drawing.Size(157, 21);
            this.coolingNum.TabIndex = 69;
            this.coolingNum.TextChanged += new System.EventHandler(this.coolingNum_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(300, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 70;
            this.label8.Text = "台";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(300, 87);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 12);
            this.label17.TabIndex = 71;
            this.label17.Text = "台";
            // 
            // coolingPanel
            // 
            this.coolingPanel.AutoScroll = true;
            this.coolingPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.coolingPanel.Location = new System.Drawing.Point(2, 252);
            this.coolingPanel.Name = "coolingPanel";
            this.coolingPanel.Size = new System.Drawing.Size(801, 142);
            this.coolingPanel.TabIndex = 72;
            // 
            // freezingPanel
            // 
            this.freezingPanel.AutoScroll = true;
            this.freezingPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.freezingPanel.Location = new System.Drawing.Point(2, 135);
            this.freezingPanel.Name = "freezingPanel";
            this.freezingPanel.Size = new System.Drawing.Size(801, 111);
            this.freezingPanel.TabIndex = 73;
            // 
            // type_comboBox
            // 
            this.type_comboBox.AutoSize = true;
            this.type_comboBox.Location = new System.Drawing.Point(572, 119);
            this.type_comboBox.Name = "type_comboBox";
            this.type_comboBox.Size = new System.Drawing.Size(29, 12);
            this.type_comboBox.TabIndex = 74;
            this.type_comboBox.Text = "类型";
            // 
            // comboBox_PumpType
            // 
            this.comboBox_PumpType.FormattingEnabled = true;
            this.comboBox_PumpType.Location = new System.Drawing.Point(574, 15);
            this.comboBox_PumpType.Name = "comboBox_PumpType";
            this.comboBox_PumpType.Size = new System.Drawing.Size(121, 20);
            this.comboBox_PumpType.TabIndex = 75;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(493, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 76;
            this.label3.Text = "水泵类型：";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(719, 12);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 77;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(27, 12);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(92, 23);
            this.btnLoadData.TabIndex = 78;
            this.btnLoadData.Text = "载入新的数据";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(137, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(339, 23);
            this.progressBar1.TabIndex = 79;
            // 
            // WaterPump
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 397);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnLoadData);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_PumpType);
            this.Controls.Add(this.type_comboBox);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.coolingNum);
            this.Controls.Add(this.freezingNum);
            this.Controls.Add(this.performance_Data_Label);
            this.Controls.Add(this.is_Frequency_Conversion_Label);
            this.Controls.Add(this.model_Label);
            this.Controls.Add(this.power_Label);
            this.Controls.Add(this.lift_Label);
            this.Controls.Add(this.flow_Label);
            this.Controls.Add(this.brand_Label);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.coolingPanel);
            this.Controls.Add(this.freezingPanel);
            this.Name = "WaterPump";
            this.Text = "水泵";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label brand_Label;
        private System.Windows.Forms.Label flow_Label;
        private System.Windows.Forms.Label lift_Label;
        private System.Windows.Forms.Label power_Label;
        private System.Windows.Forms.Label model_Label;
        private System.Windows.Forms.Label is_Frequency_Conversion_Label;
        private System.Windows.Forms.Label performance_Data_Label;
        private System.Windows.Forms.TextBox freezingNum;
        private System.Windows.Forms.TextBox coolingNum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel coolingPanel;
        private System.Windows.Forms.Panel freezingPanel;
        private System.Windows.Forms.Label type_comboBox;
        private System.Windows.Forms.ComboBox comboBox_PumpType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
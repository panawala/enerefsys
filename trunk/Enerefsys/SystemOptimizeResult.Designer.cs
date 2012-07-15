namespace Enerefsys
{
    partial class SystemOptimizeResult
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
            this.rowUnitView1 = new MultiHeaderDataGridView.RowUnitView();
            this.Day = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Temperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EngineType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EngineValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EngineLoadRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EnginePower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Flow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SystemMinPower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.rowUnitView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("YouYuan", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(179, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(467, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enerefsys系统全年逐时优化结果";
            // 
            // rowUnitView1
            // 
            this.rowUnitView1.AllowUserToAddRows = false;
            this.rowUnitView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rowUnitView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rowUnitView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Day,
            this.Time,
            this.Temperature,
            this.EngineType,
            this.EngineValue,
            this.EngineLoadRatio,
            this.EnginePower,
            this.Flow,
            this.SystemMinPower,
            this.Id});
            this.rowUnitView1.Location = new System.Drawing.Point(0, 66);
            this.rowUnitView1.Name = "rowUnitView1";
            this.rowUnitView1.ReadOnly = true;
            this.rowUnitView1.RowHeadersVisible = false;
            this.rowUnitView1.RowTemplate.Height = 23;
            this.rowUnitView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.rowUnitView1.Size = new System.Drawing.Size(924, 400);
            this.rowUnitView1.TabIndex = 1;
            // 
            // Day
            // 
            this.Day.DataPropertyName = "Day";
            this.Day.HeaderText = "日期";
            this.Day.Name = "Day";
            this.Day.ReadOnly = true;
            // 
            // Time
            // 
            this.Time.DataPropertyName = "Time";
            this.Time.HeaderText = "时间";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // Temperature
            // 
            this.Temperature.DataPropertyName = "Temperature";
            this.Temperature.HeaderText = "温度";
            this.Temperature.Name = "Temperature";
            this.Temperature.ReadOnly = true;
            // 
            // EngineType
            // 
            this.EngineType.DataPropertyName = "EngineType";
            this.EngineType.HeaderText = "冷冻机类型";
            this.EngineType.Name = "EngineType";
            this.EngineType.ReadOnly = true;
            // 
            // EngineValue
            // 
            this.EngineValue.DataPropertyName = "EngineValue";
            this.EngineValue.HeaderText = "冷量";
            this.EngineValue.Name = "EngineValue";
            this.EngineValue.ReadOnly = true;
            // 
            // EngineLoadRatio
            // 
            this.EngineLoadRatio.DataPropertyName = "EngineLoadRatio";
            this.EngineLoadRatio.HeaderText = "负荷率";
            this.EngineLoadRatio.Name = "EngineLoadRatio";
            this.EngineLoadRatio.ReadOnly = true;
            // 
            // EnginePower
            // 
            this.EnginePower.DataPropertyName = "EnginePower";
            this.EnginePower.HeaderText = "冷冻机功率";
            this.EnginePower.Name = "EnginePower";
            this.EnginePower.ReadOnly = true;
            // 
            // Flow
            // 
            this.Flow.DataPropertyName = "Flow";
            this.Flow.HeaderText = "流量";
            this.Flow.Name = "Flow";
            this.Flow.ReadOnly = true;
            // 
            // SystemMinPower
            // 
            this.SystemMinPower.DataPropertyName = "SystemMinPower";
            this.SystemMinPower.HeaderText = "系统最低功率";
            this.SystemMinPower.Name = "SystemMinPower";
            this.SystemMinPower.ReadOnly = true;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // SystemOptimizeResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 465);
            this.Controls.Add(this.rowUnitView1);
            this.Controls.Add(this.label1);
            this.Name = "SystemOptimizeResult";
            this.Text = "SystemOptimizeResult";
            this.Load += new System.EventHandler(this.SystemOptimizeResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rowUnitView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private MultiHeaderDataGridView.RowUnitView rowUnitView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Day;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Temperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn EngineType;
        private System.Windows.Forms.DataGridViewTextBoxColumn EngineValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn EngineLoadRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn EnginePower;
        private System.Windows.Forms.DataGridViewTextBoxColumn Flow;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemMinPower;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
    }
}
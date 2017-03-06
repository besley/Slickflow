namespace Slickflow.Winform
{
    partial class MOrderUserCaseForm
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
            this.btnPrintOrder = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnQC = new System.Windows.Forms.Button();
            this.btnMade = new System.Windows.Forms.Button();
            this.btnPrintLogistics = new System.Windows.Forms.Button();
            this.btnWeight = new System.Windows.Forms.Button();
            this.chkStock = new System.Windows.Forms.CheckBox();
            this.chkWeight = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(28, 34);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(404, 385);
            this.textBox1.TabIndex = 0;
            // 
            // btnPrintOrder
            // 
            this.btnPrintOrder.Location = new System.Drawing.Point(457, 106);
            this.btnPrintOrder.Name = "btnPrintOrder";
            this.btnPrintOrder.Size = new System.Drawing.Size(104, 23);
            this.btnPrintOrder.TabIndex = 1;
            this.btnPrintOrder.Text = "Print Order";
            this.btnPrintOrder.UseVisualStyleBackColor = true;
            this.btnPrintOrder.Click += new System.EventHandler(this.btnPrintOrder_Click);
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(457, 160);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(104, 23);
            this.btnOutput.TabIndex = 2;
            this.btnOutput.Text = "Output";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnQC
            // 
            this.btnQC.Location = new System.Drawing.Point(457, 274);
            this.btnQC.Name = "btnQC";
            this.btnQC.Size = new System.Drawing.Size(104, 23);
            this.btnQC.TabIndex = 4;
            this.btnQC.Text = "QC";
            this.btnQC.UseVisualStyleBackColor = true;
            this.btnQC.Click += new System.EventHandler(this.btnQC_Click);
            // 
            // btnMade
            // 
            this.btnMade.Enabled = false;
            this.btnMade.Location = new System.Drawing.Point(457, 220);
            this.btnMade.Name = "btnMade";
            this.btnMade.Size = new System.Drawing.Size(104, 23);
            this.btnMade.TabIndex = 3;
            this.btnMade.Text = "Made";
            this.btnMade.UseVisualStyleBackColor = true;
            this.btnMade.Click += new System.EventHandler(this.btnMade_Click);
            // 
            // btnPrintLogistics
            // 
            this.btnPrintLogistics.Location = new System.Drawing.Point(457, 384);
            this.btnPrintLogistics.Name = "btnPrintLogistics";
            this.btnPrintLogistics.Size = new System.Drawing.Size(104, 23);
            this.btnPrintLogistics.TabIndex = 6;
            this.btnPrintLogistics.Text = "Print Logistics";
            this.btnPrintLogistics.UseVisualStyleBackColor = true;
            this.btnPrintLogistics.Click += new System.EventHandler(this.btnPrintLogistics_Click);
            // 
            // btnWeight
            // 
            this.btnWeight.Location = new System.Drawing.Point(457, 330);
            this.btnWeight.Name = "btnWeight";
            this.btnWeight.Size = new System.Drawing.Size(104, 23);
            this.btnWeight.TabIndex = 5;
            this.btnWeight.Text = "Weight";
            this.btnWeight.UseVisualStyleBackColor = true;
            this.btnWeight.Click += new System.EventHandler(this.btnWeight_Click);
            // 
            // chkStock
            // 
            this.chkStock.AutoSize = true;
            this.chkStock.Location = new System.Drawing.Point(457, 41);
            this.chkStock.Name = "chkStock";
            this.chkStock.Size = new System.Drawing.Size(78, 16);
            this.chkStock.TabIndex = 7;
            this.chkStock.Text = "Use Stock";
            this.chkStock.UseVisualStyleBackColor = true;
            // 
            // chkWeight
            // 
            this.chkWeight.AutoSize = true;
            this.chkWeight.Location = new System.Drawing.Point(457, 63);
            this.chkWeight.Name = "chkWeight";
            this.chkWeight.Size = new System.Drawing.Size(72, 16);
            this.chkWeight.TabIndex = 8;
            this.chkWeight.Text = "Weighted";
            this.chkWeight.UseVisualStyleBackColor = true;
            // 
            // MOrderUserCaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 439);
            this.Controls.Add(this.chkWeight);
            this.Controls.Add(this.chkStock);
            this.Controls.Add(this.btnPrintLogistics);
            this.Controls.Add(this.btnWeight);
            this.Controls.Add(this.btnQC);
            this.Controls.Add(this.btnMade);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.btnPrintOrder);
            this.Controls.Add(this.textBox1);
            this.Name = "MOrderUserCaseForm";
            this.Text = "MOrderForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnPrintOrder;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnQC;
        private System.Windows.Forms.Button btnMade;
        private System.Windows.Forms.Button btnPrintLogistics;
        private System.Windows.Forms.Button btnWeight;
        private System.Windows.Forms.CheckBox chkStock;
        private System.Windows.Forms.CheckBox chkWeight;
    }
}
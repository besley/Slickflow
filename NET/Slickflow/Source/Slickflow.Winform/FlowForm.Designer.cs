namespace Slickflow.Winform
{
    partial class FlowForm
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
            this.btnRun = new System.Windows.Forms.Button();
            this.btnStartup = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnWithdraw = new System.Windows.Forms.Button();
            this.btnSendback = new System.Windows.Forms.Button();
            this.btnReverse = new System.Windows.Forms.Button();
            this.btnJump = new System.Windows.Forms.Button();
            this.btnBackward = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(526, 46);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(116, 36);
            this.btnRun.TabIndex = 5;
            this.btnRun.Text = "流程运行Run Process";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStartup
            // 
            this.btnStartup.Location = new System.Drawing.Point(526, 4);
            this.btnStartup.Name = "btnStartup";
            this.btnStartup.Size = new System.Drawing.Size(116, 36);
            this.btnStartup.TabIndex = 4;
            this.btnStartup.Text = "启动流程Startup Process";
            this.btnStartup.UseVisualStyleBackColor = true;
            this.btnStartup.Click += new System.EventHandler(this.btnStartup_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(556, 130);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(326, 313);
            this.textBox1.TabIndex = 3;
            // 
            // btnWithdraw
            // 
            this.btnWithdraw.Location = new System.Drawing.Point(648, 88);
            this.btnWithdraw.Name = "btnWithdraw";
            this.btnWithdraw.Size = new System.Drawing.Size(116, 36);
            this.btnWithdraw.TabIndex = 6;
            this.btnWithdraw.Text = "撤回到上一节点(pull)";
            this.btnWithdraw.UseVisualStyleBackColor = true;
            this.btnWithdraw.Click += new System.EventHandler(this.btnWithdraw_Click);
            // 
            // btnSendback
            // 
            this.btnSendback.Location = new System.Drawing.Point(770, 88);
            this.btnSendback.Name = "btnSendback";
            this.btnSendback.Size = new System.Drawing.Size(116, 36);
            this.btnSendback.TabIndex = 7;
            this.btnSendback.Text = "退回到上一节点(push)";
            this.btnSendback.UseVisualStyleBackColor = true;
            this.btnSendback.Click += new System.EventHandler(this.btnSendback_Click);
            // 
            // btnReverse
            // 
            this.btnReverse.Location = new System.Drawing.Point(526, 88);
            this.btnReverse.Name = "btnReverse";
            this.btnReverse.Size = new System.Drawing.Size(116, 36);
            this.btnReverse.TabIndex = 8;
            this.btnReverse.Text = "流程结束返回";
            this.btnReverse.UseVisualStyleBackColor = true;
            this.btnReverse.Click += new System.EventHandler(this.btnReverse_Click);
            // 
            // btnJump
            // 
            this.btnJump.Location = new System.Drawing.Point(770, 4);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(116, 36);
            this.btnJump.TabIndex = 9;
            this.btnJump.Text = "跳转到指定的任务节点(forward)";
            this.btnJump.UseVisualStyleBackColor = true;
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point(770, 46);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(116, 36);
            this.btnBackward.TabIndex = 10;
            this.btnBackward.Text = "跳转到指定的任务节点(backward)";
            this.btnBackward.UseVisualStyleBackColor = true;
            this.btnBackward.Click += new System.EventHandler(this.btnBackward_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(648, 4);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(116, 36);
            this.btnSkip.TabIndex = 11;
            this.btnSkip.Text = "跳转到指定的任务节点(Skip)";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "5C5041FC-AB7F-46C0-85A5-6250C3AEA375"});
            this.comboBox1.Location = new System.Drawing.Point(86, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(216, 20);
            this.comboBox1.TabIndex = 12;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(86, 43);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(216, 21);
            this.textBox2.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "实例编号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "流程编码";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 130);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(538, 313);
            this.dataGridView1.TabIndex = 16;
            // 
            // FlowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 455);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnBackward);
            this.Controls.Add(this.btnJump);
            this.Controls.Add(this.btnReverse);
            this.Controls.Add(this.btnSendback);
            this.Controls.Add(this.btnWithdraw);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnStartup);
            this.Controls.Add(this.textBox1);
            this.Name = "FlowForm";
            this.Text = "FlowForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStartup;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnWithdraw;
        private System.Windows.Forms.Button btnSendback;
        private System.Windows.Forms.Button btnReverse;
        private System.Windows.Forms.Button btnJump;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}
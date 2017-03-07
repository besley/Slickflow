namespace Slickflow.Winform
{
    partial class FormOfficeInandSplitandJoin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOfficeInandSplitandJoin));
            this.tBoxResult = new System.Windows.Forms.TextBox();
            this.btn开始 = new System.Windows.Forms.Button();
            this.btn仓库签字 = new System.Windows.Forms.Button();
            this.cBoxSurplus = new System.Windows.Forms.ComboBox();
            this.btn总经理签字 = new System.Windows.Forms.Button();
            this.btn财务部签字 = new System.Windows.Forms.Button();
            this.btn综合部签字 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tBoxResult
            // 
            this.tBoxResult.Location = new System.Drawing.Point(12, 90);
            this.tBoxResult.Multiline = true;
            this.tBoxResult.Name = "tBoxResult";
            this.tBoxResult.Size = new System.Drawing.Size(255, 162);
            this.tBoxResult.TabIndex = 2;
            // 
            // btn开始
            // 
            this.btn开始.Location = new System.Drawing.Point(348, 14);
            this.btn开始.Name = "btn开始";
            this.btn开始.Size = new System.Drawing.Size(75, 23);
            this.btn开始.TabIndex = 3;
            this.btn开始.Text = "开始";
            this.btn开始.UseVisualStyleBackColor = true;
            this.btn开始.Click += new System.EventHandler(this.btn开始_Click);
            // 
            // btn仓库签字
            // 
            this.btn仓库签字.Location = new System.Drawing.Point(348, 69);
            this.btn仓库签字.Name = "btn仓库签字";
            this.btn仓库签字.Size = new System.Drawing.Size(75, 23);
            this.btn仓库签字.TabIndex = 4;
            this.btn仓库签字.Text = "仓库签字";
            this.btn仓库签字.UseVisualStyleBackColor = true;
            this.btn仓库签字.Click += new System.EventHandler(this.btn仓库签字_Click);
            // 
            // cBoxSurplus
            // 
            this.cBoxSurplus.FormattingEnabled = true;
            this.cBoxSurplus.Items.AddRange(new object[] {
            "超量",
            "正常"});
            this.cBoxSurplus.Location = new System.Drawing.Point(362, 43);
            this.cBoxSurplus.Name = "cBoxSurplus";
            this.cBoxSurplus.Size = new System.Drawing.Size(61, 20);
            this.cBoxSurplus.TabIndex = 6;
            this.cBoxSurplus.Text = "超量";
            // 
            // btn总经理签字
            // 
            this.btn总经理签字.Location = new System.Drawing.Point(348, 229);
            this.btn总经理签字.Name = "btn总经理签字";
            this.btn总经理签字.Size = new System.Drawing.Size(75, 23);
            this.btn总经理签字.TabIndex = 9;
            this.btn总经理签字.Text = "总经理签字";
            this.btn总经理签字.UseVisualStyleBackColor = true;
            this.btn总经理签字.Click += new System.EventHandler(this.btn总经理签字_Click);
            // 
            // btn财务部签字
            // 
            this.btn财务部签字.Location = new System.Drawing.Point(418, 153);
            this.btn财务部签字.Name = "btn财务部签字";
            this.btn财务部签字.Size = new System.Drawing.Size(75, 23);
            this.btn财务部签字.TabIndex = 7;
            this.btn财务部签字.Text = "财务部签字";
            this.btn财务部签字.UseVisualStyleBackColor = true;
            this.btn财务部签字.Click += new System.EventHandler(this.btn财务部签字_Click);
            // 
            // btn综合部签字
            // 
            this.btn综合部签字.Location = new System.Drawing.Point(285, 153);
            this.btn综合部签字.Name = "btn综合部签字";
            this.btn综合部签字.Size = new System.Drawing.Size(75, 23);
            this.btn综合部签字.TabIndex = 8;
            this.btn综合部签字.Text = "综合部签字";
            this.btn综合部签字.UseVisualStyleBackColor = true;
            this.btn综合部签字.Click += new System.EventHandler(this.btn综合部签字_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 60);
            this.label1.TabIndex = 10;
            this.label1.Text = "AndSplit :（并行分支）\r\n所有连线都满足条件，才可以执行后续节点；\r\n\r\nAndJoin :（并行合并）\r\n所有连线汇集后，才可以执行后续节点。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "surplus != \"正常\"";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(418, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "surplus == \"超量\"";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(303, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Surplus=";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(360, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "AndSplit";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(360, 214);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "AndJoin";
            // 
            // FormOfficeInandSplitandJoin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(524, 261);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn总经理签字);
            this.Controls.Add(this.btn财务部签字);
            this.Controls.Add(this.btn综合部签字);
            this.Controls.Add(this.cBoxSurplus);
            this.Controls.Add(this.btn仓库签字);
            this.Controls.Add(this.btn开始);
            this.Controls.Add(this.tBoxResult);
            this.Name = "FormOfficeInandSplitandJoin";
            this.Text = "企业办公用品申领的流程";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tBoxResult;
        private System.Windows.Forms.Button btn开始;
        private System.Windows.Forms.Button btn仓库签字;
        private System.Windows.Forms.ComboBox cBoxSurplus;
        private System.Windows.Forms.Button btn总经理签字;
        private System.Windows.Forms.Button btn财务部签字;
        private System.Windows.Forms.Button btn综合部签字;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
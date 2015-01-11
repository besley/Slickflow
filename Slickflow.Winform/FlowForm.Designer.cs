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
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(404, 68);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(163, 36);
            this.btnRun.TabIndex = 5;
            this.btnRun.Text = "Run Process";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStartup
            // 
            this.btnStartup.Location = new System.Drawing.Point(404, 19);
            this.btnStartup.Name = "btnStartup";
            this.btnStartup.Size = new System.Drawing.Size(163, 32);
            this.btnStartup.TabIndex = 4;
            this.btnStartup.Text = "Startup Process";
            this.btnStartup.UseVisualStyleBackColor = true;
            this.btnStartup.Click += new System.EventHandler(this.btnStartup_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(29, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(360, 430);
            this.textBox1.TabIndex = 3;
            // 
            // btnWithdraw
            // 
            this.btnWithdraw.Location = new System.Drawing.Point(404, 301);
            this.btnWithdraw.Name = "btnWithdraw";
            this.btnWithdraw.Size = new System.Drawing.Size(163, 36);
            this.btnWithdraw.TabIndex = 6;
            this.btnWithdraw.Text = "Withdraw Process(pull)";
            this.btnWithdraw.UseVisualStyleBackColor = true;
            this.btnWithdraw.Click += new System.EventHandler(this.btnWithdraw_Click);
            // 
            // btnSendback
            // 
            this.btnSendback.Location = new System.Drawing.Point(404, 360);
            this.btnSendback.Name = "btnSendback";
            this.btnSendback.Size = new System.Drawing.Size(163, 36);
            this.btnSendback.TabIndex = 7;
            this.btnSendback.Text = "SendBack Process(push)";
            this.btnSendback.UseVisualStyleBackColor = true;
            this.btnSendback.Click += new System.EventHandler(this.btnSendback_Click);
            // 
            // btnReverse
            // 
            this.btnReverse.Location = new System.Drawing.Point(404, 413);
            this.btnReverse.Name = "btnReverse";
            this.btnReverse.Size = new System.Drawing.Size(163, 36);
            this.btnReverse.TabIndex = 8;
            this.btnReverse.Text = "Reverse Process";
            this.btnReverse.UseVisualStyleBackColor = true;
            this.btnReverse.Click += new System.EventHandler(this.btnReverse_Click);
            // 
            // btnJump
            // 
            this.btnJump.Location = new System.Drawing.Point(404, 177);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(163, 37);
            this.btnJump.TabIndex = 9;
            this.btnJump.Text = "Jump Process(forward)";
            this.btnJump.UseVisualStyleBackColor = true;
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point(404, 240);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(163, 38);
            this.btnBackward.TabIndex = 10;
            this.btnBackward.Text = "Jump Process(backward)";
            this.btnBackward.UseVisualStyleBackColor = true;
            this.btnBackward.Click += new System.EventHandler(this.btnBackward_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(404, 121);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(163, 36);
            this.btnSkip.TabIndex = 11;
            this.btnSkip.Text = "Jump Process(Skip)";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // FlowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 487);
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
    }
}
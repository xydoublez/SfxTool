namespace SfxProductTimer
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cbRules = new System.Windows.Forms.ComboBox();
            this.rbResult = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.webSocketServer1 = new GhostWinterCaptureImage.WebSocketServer();
            this.SuspendLayout();
            // 
            // cbRules
            // 
            this.cbRules.FormattingEnabled = true;
            this.cbRules.Location = new System.Drawing.Point(53, 46);
            this.cbRules.Name = "cbRules";
            this.cbRules.Size = new System.Drawing.Size(121, 20);
            this.cbRules.TabIndex = 0;
            // 
            // rbResult
            // 
            this.rbResult.Location = new System.Drawing.Point(342, 34);
            this.rbResult.Name = "rbResult";
            this.rbResult.Size = new System.Drawing.Size(378, 404);
            this.rbResult.TabIndex = 1;
            this.rbResult.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(234, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "开始检测";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // webSocketServer1
            // 
            this.webSocketServer1.Location = new System.Drawing.Point(209, 397);
            this.webSocketServer1.Name = "webSocketServer1";
            this.webSocketServer1.Size = new System.Drawing.Size(75, 23);
            this.webSocketServer1.TabIndex = 3;
            this.webSocketServer1.Text = "webSocketServer1";
            this.webSocketServer1.OnReceiveMsg += new GhostWinterCaptureImage.WebSocketServer.MessageHander(this.webSocketServer1_OnReceiveMsg);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.webSocketServer1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rbResult);
            this.Controls.Add(this.cbRules);
            this.Name = "Form1";
            this.Text = "众阳产品业务流程耗时检测";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbRules;
        private System.Windows.Forms.RichTextBox rbResult;
        private System.Windows.Forms.Button button1;
        private GhostWinterCaptureImage.WebSocketServer webSocketServer1;
    }
}


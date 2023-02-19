namespace ChatClient
{
    partial class ChatRoom
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
            this.listBox_msg = new System.Windows.Forms.ListBox();
            this.tbx_msg = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.listBox_user = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox_msg
            // 
            this.listBox_msg.FormattingEnabled = true;
            this.listBox_msg.ItemHeight = 15;
            this.listBox_msg.Location = new System.Drawing.Point(12, 12);
            this.listBox_msg.Name = "listBox_msg";
            this.listBox_msg.Size = new System.Drawing.Size(240, 364);
            this.listBox_msg.TabIndex = 0;
            // 
            // tbx_msg
            // 
            this.tbx_msg.Location = new System.Drawing.Point(12, 382);
            this.tbx_msg.Name = "tbx_msg";
            this.tbx_msg.Size = new System.Drawing.Size(240, 23);
            this.tbx_msg.TabIndex = 1;
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(270, 382);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 2;
            this.btn_send.Text = "전송하기";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // listBox_user
            // 
            this.listBox_user.FormattingEnabled = true;
            this.listBox_user.ItemHeight = 15;
            this.listBox_user.Location = new System.Drawing.Point(270, 12);
            this.listBox_user.Name = "listBox_user";
            this.listBox_user.Size = new System.Drawing.Size(120, 94);
            this.listBox_user.TabIndex = 3;
            // 
            // ChatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 450);
            this.Controls.Add(this.listBox_user);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.tbx_msg);
            this.Controls.Add(this.listBox_msg);
            this.Name = "ChatRoom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ChatRoom";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox listBox_msg;
        private TextBox tbx_msg;
        private Button btn_send;
        private ListBox listBox_user;
    }
}
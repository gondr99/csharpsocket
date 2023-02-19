namespace ChatClient;

partial class RoomList
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
        this.listBox_roomList = new System.Windows.Forms.ListBox();
        this.btn_createRoom = new System.Windows.Forms.Button();
        this.tbx_roomName = new System.Windows.Forms.TextBox();
        this.btn_entrance = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // listBox_roomList
        // 
        this.listBox_roomList.FormattingEnabled = true;
        this.listBox_roomList.ItemHeight = 15;
        this.listBox_roomList.Location = new System.Drawing.Point(12, 12);
        this.listBox_roomList.Name = "listBox_roomList";
        this.listBox_roomList.Size = new System.Drawing.Size(120, 199);
        this.listBox_roomList.TabIndex = 0;
        // 
        // btn_createRoom
        // 
        this.btn_createRoom.Location = new System.Drawing.Point(138, 82);
        this.btn_createRoom.Name = "btn_createRoom";
        this.btn_createRoom.Size = new System.Drawing.Size(75, 23);
        this.btn_createRoom.TabIndex = 2;
        this.btn_createRoom.Text = "방만들기";
        this.btn_createRoom.UseVisualStyleBackColor = true;
        this.btn_createRoom.Click += new System.EventHandler(this.btn_createRoom_Click);
        // 
        // tbx_roomName
        // 
        this.tbx_roomName.Location = new System.Drawing.Point(138, 53);
        this.tbx_roomName.Name = "tbx_roomName";
        this.tbx_roomName.Size = new System.Drawing.Size(100, 23);
        this.tbx_roomName.TabIndex = 3;
        // 
        // btn_entrance
        // 
        this.btn_entrance.Location = new System.Drawing.Point(12, 217);
        this.btn_entrance.Name = "btn_entrance";
        this.btn_entrance.Size = new System.Drawing.Size(120, 23);
        this.btn_entrance.TabIndex = 4;
        this.btn_entrance.Text = "입장하기";
        this.btn_entrance.UseVisualStyleBackColor = true;
        this.btn_entrance.Click += new System.EventHandler(this.btn_entrance_Click);
        // 
        // RoomList
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(259, 253);
        this.Controls.Add(this.btn_entrance);
        this.Controls.Add(this.tbx_roomName);
        this.Controls.Add(this.btn_createRoom);
        this.Controls.Add(this.listBox_roomList);
        this.Name = "RoomList";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "방목록";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private ListBox listBox_roomList;
    private Button btn_createRoom;
    private TextBox tbx_roomName;
    private Button btn_entrance;
}

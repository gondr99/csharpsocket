namespace ChatClient;

public partial class ChatRoom : Form
{
    public ChatRoom()
    {
        InitializeComponent();
        listBox_user.Items.Add(NetworkManager.Instance.NickName);
    }

    // 메시지 전송하기
    private void btn_send_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(tbx_msg.Text) )
        {
            return;
        }
        listBox_msg.Items.Add(tbx_msg.Text);
        tbx_msg.Text = null;
        ScrollToDown();
    }

    // 맨 아래로 스크롤 내리기
    private void ScrollToDown()
    {
        listBox_msg.SelectedIndex = listBox_msg.Items.Count - 1;
        listBox_msg.SelectedIndex = -1;
    }
}

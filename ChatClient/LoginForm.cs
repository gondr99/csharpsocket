using Core;
using System.Net.Sockets;

namespace ChatClient
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            NetworkManager.Instance.LoginResponsed += LoginResponsed;
            FormClosing += (sender, evt) =>
            {
                NetworkManager.Instance.LoginResponsed -= LoginResponsed;
            };
        }

        private async void btn_login_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tbx_id.Text) || string.IsNullOrEmpty(tbx_nickname.Text))
            {
                MessageBox.Show("�ʼ����� �Է��ϼ���");
                return;
            }

            await NetworkManager.Instance.ConnectAsync();

            LoginRequestPacket packet = new LoginRequestPacket(tbx_id.Text, tbx_nickname.Text);

            await NetworkManager.Instance.Socket.SendAsync(packet.Serialize(), SocketFlags.None);

            
        }
        
        public void LoginResponsed(object? sender, EventArgs evt)
        {
            LoginResponsePacket packet = (LoginResponsePacket)evt;
            if(packet.Code == 200)
            {
                NetworkManager.Instance.Id = tbx_id.Text;
                NetworkManager.Instance.NickName = tbx_nickname.Text;

                //�븮��Ʈ �����ֱ�
                IAsyncResult ar = null;
                // java�� runAfter�ϰ� �Ȱ���. ���� �����忡�� �۾� ����.
                BeginInvoke(() =>
                {
                    RoomList roomList = new RoomList();
                    roomList.ShowDialog();
                    EndInvoke(ar);
                });

                
            }
            else
            {
                //��¸� �ݾƾ� ������ Dispose�� ���� �����Ű�°� ���� �� �ִ�.
                NetworkManager.Instance.Socket.Shutdown(SocketShutdown.Send); //������� 0����Ʈ ������
                //�����ʿ��� ���� ������ ���� �ȴ�.(ClientSocket.dispose�� ȣ���)

                MessageBox.Show("���� ����");
            }
        }
    }
}
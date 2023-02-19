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
                MessageBox.Show("필수값을 입력하세요");
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

                //룸리스트 보여주기
                IAsyncResult ar = null;
                // java은 runAfter하고 똑같아. 메인 쓰레드에게 작업 전달.
                BeginInvoke(() =>
                {
                    RoomList roomList = new RoomList();
                    roomList.ShowDialog();
                    EndInvoke(ar);
                });

                
            }
            else
            {
                //출력만 닫아야 서버가 Dispose를 통해 종료시키는걸 받을 수 있다.
                NetworkManager.Instance.Socket.Shutdown(SocketShutdown.Send); //보내기로 0바이트 보내면
                //서버쪽에서 먼저 소켓을 끊게 된다.(ClientSocket.dispose가 호출됨)

                MessageBox.Show("연결 실패");
            }
        }
    }
}
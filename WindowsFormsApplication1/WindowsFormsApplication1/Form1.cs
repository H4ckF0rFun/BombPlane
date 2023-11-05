using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")] 
        public static extern bool AllocConsole();

        public Form1()
        {
            InitializeComponent();
            AllocConsole();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string nickname = this.nickname.Text;
            string password = this.password.Text;
            string serveraddr = this.serveraddr.Text;
            string session = this.session.Text;

            dynamic response = null;

            if (nickname.Length == 0 ||
                password.Length == 0 ||
                serveraddr.Length == 0 || 
                session.Length == 0)
            {
                string message = "Please fill in the information completely.";
                string title = "Tips";
                MessageBox.Show(message, title);
                return;
            }

            try
            {
                string url = "http://" + serveraddr + "/api/joingame";

                //get game session

                string payload = JsonConvert.SerializeObject(new
                {
                    session = session,
                    nickname = nickname,
                    password = password
                });

                HttpClient client = new HttpClient();
                HttpResponseMessage res = await client.PostAsync(url, new StringContent(payload));

                string StatuCode = res.StatusCode.ToString();
                if (StatuCode != "OK")
                {
                    string message = "Please fill in the information completely.";
                    string title = "Tips";
                    MessageBox.Show(message, title);
                    return;
                }

                string json = await res.Content.ReadAsStringAsync();
                response = JObject.Parse(json);

                json = response.ToString();

                if (response["result"] != 0)
                {
                    string message = "Join game failed!";
                    string title = "Error";
                    MessageBox.Show(message, title);
                    return;
                }

            }
            catch(HttpRequestException)
            {
                string message = "Coultn't connect to server!";
                string title = "Error";
                MessageBox.Show(message, title);
                return;
                 
            }
      
            string game_session = response["session"];
            State form2 = new State(serveraddr,game_session, nickname, password,1);
            form2.ShowDialog();
            return;
        }

        //Create Game.
        private async void button2_Click(object sender, EventArgs e)
        {
            //State form2 = new State();
            //form2.ShowDialog();

            string nickname = this.nickname.Text;
            string password = this.password.Text;
            string serveraddr = this.serveraddr.Text;

            if (nickname.Length == 0 ||
                password.Length == 0 ||
                serveraddr.Length == 0)
            {
                string message = "Please fill in the information completely.";
                string title = "Tips";
                MessageBox.Show(message, title);
                return;
            }


            string url = "http://" + serveraddr + "/api/creategame";

            //get game session

            string payload = JsonConvert.SerializeObject(new 
            {
                nickname = nickname,
                password = password
            });

            HttpClient client = new HttpClient();
            HttpResponseMessage res;
            try
            {
                res = await client.PostAsync(url, new StringContent(payload));
            }
            catch(System.Net.Http.HttpRequestException)
            {
                string message = "Could not connect server!";
                string title = "Tips";
                MessageBox.Show(message, title);
                return;
            }
           
            string StatuCode = res.StatusCode.ToString();
            if (StatuCode != "OK")
            {
                string message = "Please fill in the information completely.";
                string title = "Tips";
                MessageBox.Show(message, title);
                return;
            }

            string json = await res.Content.ReadAsStringAsync();
            dynamic response = JObject.Parse(json);

            json = response.ToString();

            if(response["result"] != 0)
            {
                string message = "Create game failed!";
                string title = "Error";
                MessageBox.Show(message, title);
                return;
            }

            string game_session = response["session"];
            State form2 = new State(serveraddr,game_session, nickname,password);
            form2.ShowDialog();
            return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.nickname.Text = "Player";
            this.password.Text = Guid.NewGuid().ToString();
            this.serveraddr.Text = "127.0.0.1:7777";
            //this.serveraddr.Text = "49.235.129.40:54321";
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}

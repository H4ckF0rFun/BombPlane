using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    
    public partial class State : Form
    {
        const int BLOCK_WIDTH = 40;


        private int AIProcessed;
        private int last_hover_x;
        private int last_hover_y;
        private int last_rotate;
        private int last_turn;

        private bool drawing;

        private int put_or_remove;
        private int left_plane;
        private string session;
        private string nickname;
        private string password;
        private int who;
        private int inTimer;
        private string ai;
        //
        const int SET_HEAD_FLAG = 1;
        const int SET_BODY_FLAG = 2;
        const int REMOVE_FLAG = 3;
        const int ATTACK = 4;

        
        private int guess_operation;


        private int last_state;
        /***********************************************************/
        const int PLANE_HEAD = 2;
        const int PLANE_BODY = 1;
        const int PLANE_NONE = 0;

        private int [][][] Planes;
        private int[][] board_value;
        private string server_addr;

        private Button[][] board_me;
        private Button[][] play_board;

        private System.Windows.Forms.Timer timer;
        
        // (rotate << 16) | (HEAD)
        public State(string serveraddr,string _session,string _nickname,string _password,int who = 0)
        {
            InitializeComponent();

            this.last_turn = -1;

            this.AIProcessed = 0;
            this.drawing = false;
            this.ai = "";
            this.nickname = _nickname;
            this.session = _session;
            this.password = _password;
            this.who = who;
            this.inTimer = 0;

            this.server_addr = serveraddr;
            //
            this.guess_operation = ATTACK;
            this.last_state = 0;
            this.last_hover_x = -1;
            this.last_hover_y = -1;
            this.last_rotate = -1;
            this.put_or_remove = 0;
            this.left_plane = 3;

            board_value = new int[10][];
            
            for (int y = 0; y < 10; y++)
            {
                board_value[y] = new int[10];
                for (int x = 0; x < 10; x++)
                {
                    board_value[y][x] = PLANE_NONE;
                }
            }

            Planes = new int[4][][];

            Planes[0] = new int[4][];
            Planes[0][0] = new int[5] { PLANE_NONE, PLANE_NONE, PLANE_HEAD, PLANE_NONE, PLANE_NONE };
            Planes[0][1] = new int[5] { PLANE_BODY, PLANE_BODY, PLANE_BODY, PLANE_BODY, PLANE_BODY };
            Planes[0][2] = new int[5] { PLANE_NONE, PLANE_NONE, PLANE_BODY, PLANE_NONE, PLANE_NONE };
            Planes[0][3] = new int[5] { PLANE_NONE, PLANE_BODY, PLANE_BODY, PLANE_BODY, PLANE_NONE };

            Planes[1] = new int[5][];
            Planes[1][0] = new int[4] { PLANE_NONE, PLANE_NONE, PLANE_BODY, PLANE_NONE };
            Planes[1][1] = new int[4] { PLANE_BODY, PLANE_NONE, PLANE_BODY, PLANE_NONE };
            Planes[1][2] = new int[4] { PLANE_BODY, PLANE_BODY, PLANE_BODY, PLANE_HEAD };
            Planes[1][3] = new int[4] { PLANE_BODY, PLANE_NONE, PLANE_BODY, PLANE_NONE };
            Planes[1][4] = new int[4] { PLANE_NONE, PLANE_NONE, PLANE_BODY, PLANE_NONE };



            Planes[2] = new int[4][];
            Planes[2][0] = new int[5] { PLANE_NONE, PLANE_BODY, PLANE_BODY, PLANE_BODY, PLANE_NONE };
            Planes[2][1] = new int[5] { PLANE_NONE, PLANE_NONE, PLANE_BODY, PLANE_NONE, PLANE_NONE };
            Planes[2][2] = new int[5] { PLANE_BODY, PLANE_BODY, PLANE_BODY, PLANE_BODY, PLANE_BODY };
            Planes[2][3] = new int[5] { PLANE_NONE, PLANE_NONE, PLANE_HEAD, PLANE_NONE, PLANE_NONE };



            Planes[3] = new int[5][];
            Planes[3][0] = new int[4] { PLANE_NONE, PLANE_BODY, PLANE_NONE, PLANE_NONE };
            Planes[3][1] = new int[4] { PLANE_NONE, PLANE_BODY, PLANE_NONE, PLANE_BODY };
            Planes[3][2] = new int[4] { PLANE_HEAD, PLANE_BODY, PLANE_BODY, PLANE_BODY };
            Planes[3][3] = new int[4] { PLANE_NONE, PLANE_BODY, PLANE_NONE, PLANE_BODY };
            Planes[3][4] = new int[4] { PLANE_NONE, PLANE_BODY, PLANE_NONE, PLANE_NONE };

        }

        private int[][] calc_valid(int plane_x, int plane_y, int plane_idx)
        {
            int[][] valid = new int[10][];

            for (int i = 0; i < 10; i++)
            {
                valid[i] = new int[10];
                for (int j = 0; j < 10; j++)
                {
                    if (this.board_value[i][j] == PLANE_NONE)
                        valid[i][j] = 1;
                }
            }

            int height = Planes[plane_idx].Length;
            int width = Planes[plane_idx][0].Length;

            for (int dy = 0; dy < height; dy++)
            {
                for (int dx = 0; dx < width; dx++)
                {
                    int _x = dx + plane_x;
                    int _y = dy + plane_y;

                    if(_x <0 || _x >= 10 ||  _y < 0 || _y >= 10)
                        continue;
              

                    if (valid[_y][_x] == 1 && 
                        Planes[plane_idx][dy][dx] != 0)
                    {
                        valid[_y][_x] = -1;
                    }
                }
            }
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (valid[i][j] == -1)
                        valid[i][j] = 1;
                    else
                        valid[i][j] = 0;

            return valid;
        }

        private void UpdateMyBoard(int last_x,int last_y,int last_r,
            int cur_x,int cur_y,int cur_r)
        {
            //当前有效 ^ 上一次有效 = 重叠部分
            //删掉上一次，保留重叠部分.
            //
            int [][] cur_valid  = calc_valid(cur_x, cur_y, cur_r);
            int[][] last_valid = calc_valid(last_x, last_y, last_r);


            //
            int valid_cnt = 0;
            for(int y = 0;y < 10;y++)
            {
                for(int x = 0;x < 10;x++)
                {
                    if(last_valid[y][x]!= 0 && cur_valid[y][x] == 0)
                    {
                        this.board_me[y][x].BackColor = Color.LightGray;
                    }
                    else if (cur_valid[y][x] != 0)
                    {
                        valid_cnt++;
                    }
                }
            }

            for(int y = 0;y < 10;y++)
            {
                for(int x = 0;x < 10;x ++)
                {
                    if(cur_valid[y][x] != 0)
                    {
                        this.board_me[y][x].BackColor = 
                            valid_cnt == 10 ? Color.LightBlue : Color.Red;
                    }
                }
            }

        }
        private void OnSelectPlanePos(object sender, EventArgs e)
        {
            if(this.last_rotate == -1 || this.drawing == true)      //plane is placed.
            {
                return;
            }
            
            Button btn = (Button)sender;
            int y = btn.Top / BLOCK_WIDTH;
            int x = btn.Left / BLOCK_WIDTH;
            int[] coor = GetPlaneCoorByHeadCoor(x, y, this.last_rotate);

            x = coor[0];
            y = coor[1];

            if (x == last_hover_x && y == last_hover_y)
            {
                return;
            }

            UpdateMyBoard(last_hover_x, last_hover_y, last_rotate, x, y, last_rotate);

            //
            last_hover_x = x;
            last_hover_y = y;
            return;
        }

        private async void on_left_click(object sender, MouseEventArgs e)
        {
            if (this.put_or_remove == 0)
            {
                if (this.last_rotate != -1 && last_hover_x != -1 && last_hover_y != -1)      //plane is placed.
                {
                    //redraw.
                    this.drawing = true;

                    //check place is valid?
                    int[][] valid = calc_valid(last_hover_x, last_hover_y, last_rotate % 4);
                    int valid_cnt = 0;
                    for (int y = 0; y < 10;y++ )
                    {
                        for(int x = 0;x < 10;x ++)
                        {
                            if (valid[y][x] == 1)
                                valid_cnt++;
                        }
                    }
                    //invalid place.
                    if (valid_cnt != 10)
                    {
                        this.drawing = false;
                        return;
                    }
                        

                    /*
                     *  x : last_hover_x
                     *  y : last_hover_y
                     
                        idx
                    */
                    //

                    try
                    {
                        string url = "http://" + this.server_addr + "/api/putplane";
                        string payload = JsonConvert.SerializeObject(new
                        {
                            session = session,
                            nickname = nickname,
                            password = password,
                            idx = last_rotate,
                            x = last_hover_x,
                            y = last_hover_y
                        });

                        HttpClient client = new HttpClient();
                        HttpResponseMessage res = await client.PostAsync(url, new StringContent(payload));

                        string StatuCode = res.StatusCode.ToString();
                        if (StatuCode != "OK")
                        {
                            return;
                        }

                        string json = await res.Content.ReadAsStringAsync();
                        dynamic response = JObject.Parse(json);

                        int result = response.result;

                        if (result != 0)
                        {
                            this.drawing = false;
                            return;
                        }
                            

                    }
                    catch (HttpRequestException)
                    {
                        string message = "Couldn't connect to server!";
                        string title = "Tips";
                        MessageBox.Show(message, title);
                        this.drawing = false;
                        return;
                    }

                    //place plane to board.
                    int height = Planes[last_rotate % 4].Length;
                    int width = Planes[last_rotate % 4][0].Length;

                    for (int dy = 0; dy < height; dy++)
                    {
                        for(int dx = 0;dx < width;dx++)
                        {
                            int _x = last_hover_x + dx;
                            int _y = last_hover_y + dy;


                            if (Planes[last_rotate % 4][dy][dx] != PLANE_NONE)
                            {
                                this.board_value[_y][_x] = Planes[last_rotate % 4][dy][dx];
                                if(Planes[last_rotate % 4][dy][dx] == PLANE_HEAD)
                                {
                                    this.board_value[_y][_x] |= this.last_rotate << 16;
                                }
                            }

                            if(this.board_value[_y][_x] != PLANE_NONE)
                                this.board_me[_y][_x].BackColor = Color.Blue;

                            if ((this.board_value[_y][_x] & 0xffff) == PLANE_HEAD)
                            {
                                this.board_me[_y][_x].Text = "☆";
                                this.board_me[_y][_x].ForeColor = Color.White;
                            }
                        }
                    }
                    last_rotate = -1;
                    last_hover_x = -1;
                    last_hover_y = -1;

                    this.left_plane -= 1;
                    this.drawing = false;
                    return;
                }
            }
            else 
            {
                Button btn = (Button)sender;
                int x = btn.Left / BLOCK_WIDTH;
                int y = btn.Top / BLOCK_WIDTH;

                //no plane to remove.
                if(this.left_plane >= 3)
                {
                    return;
                }

                if((this.board_value[y][x] & 0xffff) != PLANE_HEAD)
                {
                    //select is not a plane head.
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Are you sure to remove this plane?", 
                    "Tips", 
                    MessageBoxButtons.OKCancel);
                
                if (result != DialogResult.OK)
                {
                    return;
                }

                //Remove Plane.
                this.board_me[y][x].Text = "";
                int plane_idx = this.board_value[y][x] >> 16;
                int height = Planes[plane_idx].Length;
                int width = Planes[plane_idx][0].Length;

                int []coor = GetPlaneCoorByHeadCoor(x, y, plane_idx);
                x = coor[0];
                y = coor[1];

                try
                {
                    string url = "http://" + this.server_addr + "/api/removeplane";
                    string payload = JsonConvert.SerializeObject(new
                    {
                        session = session,
                        nickname = nickname,
                        password = password,
                        idx = plane_idx,
                        x = x,
                        y = y
                    });

                    HttpClient client = new HttpClient();
                    HttpResponseMessage res = await client.PostAsync(url, new StringContent(payload));

                    string StatuCode = res.StatusCode.ToString();
                    if (StatuCode != "OK")
                    {
                        return;
                    }

                    string json = await res.Content.ReadAsStringAsync();
                    dynamic response = JObject.Parse(json);

                    int code = response.result;

                    if (code != 0)
                        return;

                }
                catch (HttpRequestException)
                {
                    string message = "Couldn't connect to server!";
                    string title = "Tips";
                    MessageBox.Show(message, title);
                    return;
                }


                for (int dy = 0; dy < height;dy++ )
                {
                    for (int dx = 0; dx < width; dx++)
                    {
                        if(Planes[plane_idx][dy][dx] != PLANE_NONE)
                        {
                            int _x = x + dx;
                            int _y = y + dy;
                            this.board_value[_y][_x] = 0;
                            this.board_me[_y][_x].BackColor = Color.LightGray;
                        }
                    }
                }

                this.left_plane += 1;
            }
        }

        private int[] GetPlaneCoorByHeadCoor(int x,int y,int idx)
        {
            switch (idx)
            {
                case 0:
                    x -= 2;
                    break;
                case 2:
                    x -= 2;
                    y -= 3;
                    break;

                case 1:
                    x -= 3;
                    y -= 2;
                    break;
                case 3:
                    y -= 2;
                    break;
            }
            return new int [2] {x,y};
        }


        private void on_right_click(object sender, MouseEventArgs e)
        {
            if(this.put_or_remove == 0)
            {
                Button btn = (Button)sender;
                int y = btn.Top / BLOCK_WIDTH;
                int x = btn.Left / BLOCK_WIDTH;
                int idx = (this.last_rotate + 1) % 4;
                int[] coor;

                coor = GetPlaneCoorByHeadCoor(x, y, idx);

                x = coor[0];
                y = coor[1];

                UpdateMyBoard(last_hover_x, last_hover_y, last_rotate, x, y, idx);

                last_rotate = idx;
                last_hover_x = x;
                last_hover_y = y;
            }
            else
            {
                //remove.
                //do noting.
            }
        }

        private void OnMyBoardClicked(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Left:
                    this.on_left_click(sender, e);
                    break;

                case MouseButtons.Right:
                    this.on_right_click(sender, e);
                    break;
            }
        }
        
        private void updateState(int newState)
        {
            if (newState == 1)
            {
                this.wait_tips.Visible = false;
                this.panel1.Visible = true;
                this.Plane.Visible = true;
                this.Remove.Visible = true;
                this.Confirm.Visible = true;

                this.guess_board.Visible = true;
            }
            else if (newState == 2)
            {
                this.label2.Visible = false;
                this.guess_board.BackColor = this.panel1.BackColor;
                //
                play_board = new Button[10][];
                for (int y = 0; y < 10; y++)
                {
                    play_board[y] = new Button[10];
                    for (int x = 0; x < 10; x++)
                    {
                        play_board[y][x] = new Button();

                        play_board[y][x].Parent = this.guess_board;
                        play_board[y][x].BackColor = Color.LightGray;
                        play_board[y][x].Left = x * BLOCK_WIDTH;
                        play_board[y][x].Top = y * BLOCK_WIDTH;

                        play_board[y][x].Width = BLOCK_WIDTH;
                        play_board[y][x].Height = BLOCK_WIDTH;

                        play_board[y][x].Show();
                        play_board[y][x].Click += this.OnPlayBoardClicked;
                    }
                }

                //显示按钮.
                this.Attack.Visible = true;
                this.BodyFlag.Visible = true;
                this.HeadFlag.Visible = true;
                this.RmFlag.Visible = true;
                this.LoadAI.Visible = true;

                //
                this.Attack.Enabled = false;
            }
            else if (newState == 3)

            {
                //游戏结束.
                this.Attack.Enabled = false;
                this.BodyFlag.Enabled = false;
                this.HeadFlag.Enabled = false;
                this.RmFlag.Enabled = false;
                
            }
        }

        private void OnPlayBoardClicked(object sender, EventArgs e)
        {
            switch(this.guess_operation)
            {
                case SET_BODY_FLAG:
                    SetBodyFlag(sender, e);
                    break;

                case SET_HEAD_FLAG:
                    SetHeadFlag(sender, e);
                    break;
                  
                case ATTACK:
                    AttackPlane(sender, e);
                    break;

                case REMOVE_FLAG:
                    RemoveFlag(sender, e);
                    break;
            }
        }


        //AI打飞机
        private async void AttackByAI(int x,int y)
        {
            if (x < 0 || x >= 10 || y < 0 || y >= 10)
            {
                return;
            }

            Button btn = (Button)play_board[y][x];

            string url = "http://" + this.server_addr + "/api/attack";
            string payload = JsonConvert.SerializeObject(new
            {
                session = session,
                nickname = nickname,
                password = password,
                x = x,
                y = y
            });

            HttpClient client = new HttpClient();
            HttpResponseMessage res;
            dynamic response = null;
            try
            {
                res = await client.PostAsync(url, new StringContent(payload));
                string StatuCode = res.StatusCode.ToString();
                if (StatuCode != "OK")
                {
                    return;
                }

                string json = await res.Content.ReadAsStringAsync();
                response = JObject.Parse(json);
            }
            catch (HttpRequestException)
            {
                return;
            }

            int result = response.result;
            int value = response.value;
            
            if (result != 0)
            {
                string err = response.error;
                MessageBox.Show(err, "Error");
                return;
            }

            switch (value)
            {
                case 0:
                    if (btn.BackColor != Color.White)
                    {
                        btn.BackColor = Color.White;
                        btn.Text = "";
                    }
                    break;
                case 1:
                    if (btn.BackColor != Color.Red && btn.BackColor != Color.Orange)
                    {
                        btn.BackColor = Color.Orange;
                        btn.Text = "";
                    }
                    break;
                case 2:
                    if (btn.BackColor != Color.Red)
                    {
                        btn.BackColor = Color.Red;
                        btn.Text = "";
                    }
                    break;
            }
            Console.WriteLine("ok");
        }

        //手动打飞机
        private async void AttackPlane(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int x = btn.Left / BLOCK_WIDTH;
            int y = btn.Top / BLOCK_WIDTH;

            string url = "http://" + this.server_addr + "/api/attack";
            string payload = JsonConvert.SerializeObject(new
            {
                session = session,
                nickname = nickname,
                password = password,
                x = x,
                y = y
            });

            HttpClient client = new HttpClient();
            HttpResponseMessage res;
            dynamic response = null;

            try
            {

                res = await client.PostAsync(url, new StringContent(payload));

                string StatuCode = res.StatusCode.ToString();
                if (StatuCode != "OK")
                {
                    return;
                }

                string json = await res.Content.ReadAsStringAsync();
                response = JObject.Parse(json);
            }
            catch (HttpRequestException)
            {
                return;
            }

            //
            int code = response.result;
            if (code != 0)
            {
                string err = response.error;
                string title = "Error";
                MessageBox.Show(err, title);
                return;
            }

            int value = response.value;

            switch(value)
            {
                case 0:
                    if (btn.BackColor != Color.White)
                    {
                        btn.BackColor = Color.White;
                        btn.Text = "";
                    }
                        
                    break;
                case 1:
                    if (btn.BackColor != Color.Red && btn.BackColor != Color.Orange)
                    {
                        btn.BackColor = Color.Orange;
                        btn.Text = "";
                    }
                        
                    break;
                case 2:
                    if(btn.BackColor != Color.Red)
                    {
                        btn.BackColor = Color.Red;
                        btn.Text = "";
                    }
                    break;
            }
        }

        //飞机标记. 飞机头标记.
        private void SetBodyFlag(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int x = btn.Left / BLOCK_WIDTH;
            int y = btn.Top / BLOCK_WIDTH;
            btn.Text = "⚪";
            btn.ForeColor = Color.Brown;
        }

        //飞机标记. 飞机头标记.
        private void SetHeadFlag(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int x = btn.Left / BLOCK_WIDTH;
            int y = btn.Top / BLOCK_WIDTH;

            btn.Text = "☆";
            btn.ForeColor = Color.Brown;
        }

        private void RemoveFlag(object sender,EventArgs e)
        {
            Button btn = (Button)sender;

            int x = btn.Left / BLOCK_WIDTH;
            int y = btn.Top / BLOCK_WIDTH;
            btn.Text = "";
        }

        private string getPlayBoard()
        {
            string board = "";
            for (int i = 0; i < 100; i++)
            {
                if (this.play_board[i / 10][i % 10].BackColor == Color.White)
                    board += " ";
                else if (this.play_board[i / 10][i % 10].BackColor == Color.Red)
                    board += "H";
                else if (this.play_board[i / 10][i % 10].BackColor == Color.Orange)
                    board += "B";
                else
                    board += "U";
            }
            return board;
        }

        //处理打飞机阶段接收到的数据包。
        private void OnGuessQueryInfo(
            int state,
            string player_0,
            string player_1,
            int p0_planes,
            int p1_planes,
            int turn,
            int win,
            int[] pos,
            string p0_left_time,
            string p1_left_time
            )
        {

            //更新剩余时间.
            if (this.who == 0)
            {
                Player_0_alive_planes.Text = p0_planes.ToString();
                Player_1_alive_planes.Text = p1_planes.ToString();

                my_left_time.Text = p0_left_time.ToString() + "S";
                your_left_time.Text = p1_left_time.ToString() + "S";
            }
            else
            {
                Player_0_alive_planes.Text = p1_planes.ToString();
                Player_1_alive_planes.Text = p0_planes.ToString();
                my_left_time.Text = p1_left_time.ToString() + "S";
                your_left_time.Text = p0_left_time.ToString() + "S";
            }

            if(state == 2)
            {
                if(this.last_turn != turn)
                {
                    //update data.
                    this.Attack.Enabled = false;
                    AIProcessed = 0;
                }

                this.last_turn = turn;

                if((turn & 1) == this.who)
                {
                    if (this.Attack.Enabled == false)
                    {
                        this.Attack.Enabled = true;
                        Console.WriteLine(getPlayBoard());                        
                    }

                    if (AIProcessed == 0 && this.ai.Length != 0)
                    {
                        AIProcessed = 1;
                        //只调用一次
                        Console.WriteLine("begin\n");
                        int[] coor = getSolution(getPlayBoard(), this.ai);
                        if (coor[0] != -1 && coor[1] != -1)
                        {
                            AttackByAI(coor[0], coor[1]);
                        };
                        Console.WriteLine("End\n");
                    }
                }
            }

            //更新自己被击中的飞机.
            for (int i = 0; i < pos.Length; i++)
            {
                int idx = pos[i];
                int x = idx % 10;
                int y = idx / 10;
                int plane_idx = 0;
                int[] coor;

                if (x < 0 || x >= 10 || y < 0 || y >= 10)
                    continue;

                switch (board_value[y][x])
                {
                    case 0:
                        if (board_me[y][x].BackColor != Color.White)
                            board_me[y][x].BackColor = Color.White;
                        break;
                    case 1:
                        if (board_me[y][x].BackColor != Color.Orange &&
                            board_me[y][x].BackColor != Color.Red)
                            board_me[y][x].BackColor = Color.Orange;
                        break;
                    default:
                        //被打头了.
                        //将整个飞机都变成红色.
                        plane_idx = board_value[y][x] >> 16;
                            
                        //以及染成红色了.
                        if (board_me[y][x].BackColor == Color.Red)
                            break;

                        board_me[y][x].BackColor = Color.Red;

                        coor = GetPlaneCoorByHeadCoor(x, y, plane_idx);
                        x = coor[0];
                        y = coor[1];

                        int width = Planes[plane_idx][0].Length;
                        int height = Planes[plane_idx].Length;

                        for (int dx = 0; dx < width; dx++)
                        {
                            for (int dy = 0; dy < height; dy++)
                            {
                                if (Planes[plane_idx][dy][dx] != PLANE_NONE)
                                {
                                    board_me[y + dy][x + dx].BackColor = Color.Red;
                                }
                            }
                        }
                        break;
                }
                
            }

            if (state != 3)
                return;

            timer.Stop();
            if (this.who != win)
            {
                string message = "You lose!";
                string title = "Game Over";
                MessageBox.Show(message, title);
            }
            else
            {
                string message = "You win!";
                string title = "Game Over";
                MessageBox.Show(message, title);
            }
        }

        //处理放置飞机状态接收到的数据包.
        private void OnPutPlaneQueryInfo(
            string player_0,
            string player_1,
            int p0_planes,
            int p1_planes,
            int turn,
            int win,
            int[] pos,
            string p0_left_time,
            string p1_left_time
            )
        {
            //双方都加入了.
            if (this.who == 0)      //who ==0 ,create by me.
            {
                this.my_nickname.Text = player_0;
                this.your_nickname.Text = player_1;
                this.my_left_time.Text = p0_left_time.ToString() + "S";
                this.your_left_time.Text = p1_left_time.ToString() + "S";

                //I am player_0
                if (p0_planes != -1)
                    Player_0_alive_planes.Text = p0_planes.ToString();
                else
                    Player_0_alive_planes.Text = "Confirmed";

                if (p1_planes != -1)
                    Player_1_alive_planes.Text = p1_planes.ToString();
                else
                    Player_1_alive_planes.Text = "Confirmed";

            }
            else
            {
                this.my_nickname.Text = player_1;
                this.your_nickname.Text = player_0;
                this.my_left_time.Text = p1_left_time.ToString() + "S";
                this.your_left_time.Text = p0_left_time.ToString() + "S";

                //I am player_1
                if (p1_planes != -1)
                    Player_0_alive_planes.Text = p1_planes.ToString();
                else
                    Player_0_alive_planes.Text = "Confirmed";

                if (p0_planes != -1)
                    Player_1_alive_planes.Text = p0_planes.ToString();
                else
                    Player_1_alive_planes.Text = "Confirmed";
            }
        }


        private async void OnTimer(object sender, EventArgs e)
        {

            if (Interlocked.Exchange(ref inTimer, 1) == 1)
            {
                return;
            }

            string url = "http://" + this.server_addr + "/api/queryinfo";

            string payload = JsonConvert.SerializeObject(new
            {
                session = session,
                nickname = nickname,
                password = password
            });

            HttpClient client = new HttpClient();
            HttpResponseMessage res;
            dynamic response = null;

            try
            {
                res = await client.PostAsync(url, new StringContent(payload));

                string StatuCode = res.StatusCode.ToString();
                if (StatuCode != "OK")
                {
                    Interlocked.Exchange(ref inTimer, 0);
                    return;
                }

                string json = await res.Content.ReadAsStringAsync();
                response = JObject.Parse(json);
            }
            catch (HttpRequestException)
            {
                Interlocked.Exchange(ref inTimer, 0);
                return;
            }

            int state = response.state;
            string player_0 = response["player_0"].ToString();
            string player_1 = response["player_1"].ToString();
            int p0_planes = response.player_0_plane;
            int p1_planes = response.player_1_plane;
            int turn = response.turn;
            int win = response.win;
            int[] pos = response.attack_pos.ToObject<int[]>();
            string p0_left_time = response.p0_left_time;
            string p1_left_time = response.p1_left_time;

            if(p0_left_time == "-1")
                p0_left_time = "0";

            if(p1_left_time == "-1")
                p1_left_time = "0";

            switch (state)
            {
                    //等待对方加入游戏.

                case 0:
                    this.last_state = state;
                    break;

                case  1:        //放置飞机
                    if (this.last_state != state)
                    {
                        updateState(state);
                    }
                    OnPutPlaneQueryInfo(
                        player_0, 
                        player_1, 
                        p0_planes, 
                        p1_planes, 
                        turn, 
                        win, 
                        pos, 
                        p0_left_time, 
                        p1_left_time);
                    
                    this.last_state = state;
                    break;

                case 2:     //游戏进行中.
                case 3:
                    if (this.last_state != state)
                            updateState(state);

                        OnGuessQueryInfo(state,
                            player_0, 
                            player_1, 
                            p0_planes, 
                            p1_planes, 
                            turn, win, 
                            pos, 
                            p0_left_time, 
                            p1_left_time);
                        this.last_state = state;
                    break;
            }
            Interlocked.Exchange(ref inTimer, 0);
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            //
            this.GameSession.Text = this.session;
            this.my_nickname.Text = this.nickname;

            this.panel1.Visible = false;
            this.Plane.Visible = false;
            this.Remove.Visible = false;
            this.Confirm.Visible = false;

            this.wait_tips.Parent = this.Player1Group;
            //set timer
            this.timer = new System.Windows.Forms.Timer();
            this.timer.Tick += OnTimer;
            this.timer.Interval = 1000;
            this.timer.Start();

            //create game board.
            board_me = new Button [10][];
            for (int y = 0; y < 10; y++)
            {
                board_me[y] = new Button[10];
                for (int x = 0; x < 10; x++)
                {
                    board_me[y][x] = new Button();

                    board_me[y][x].Parent = this.panel1;
                    board_me[y][x].BackColor = Color.LightGray;
                    board_me[y][x].Left = x * BLOCK_WIDTH;
                    board_me[y][x].Top = y * BLOCK_WIDTH;

                    board_me[y][x].Width = BLOCK_WIDTH;
                    board_me[y][x].Height = BLOCK_WIDTH;

                    board_me[y][x].Show();
                    board_me[y][x].MouseHover += new System.EventHandler(this.OnSelectPlanePos);
                    board_me[y][x].MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMyBoardClicked);
                }
            }
            this.panel1.Width = 10 * BLOCK_WIDTH;
            this.panel1.Height = 10 * BLOCK_WIDTH;

        }

        //放置飞机按钮
        private void Plane_Click(object sender, EventArgs e)
        {
            if (this.left_plane == 0 || this.drawing == true)
                return;


            this.last_hover_x = -1;
            this.last_hover_y = -1;
            this.last_rotate = 0;
            this.put_or_remove = 0;
        }

        //删除飞机按钮
        private void Remove_Click(object sender, EventArgs e)
        {
            this.put_or_remove = 1;
        }

        //确认按钮
        private async void Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://" + this.server_addr + "/api/confirm";

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
                    return;
                }

                string json = await res.Content.ReadAsStringAsync();
                dynamic response = JObject.Parse(json);

                int result = response.result;
                string err = response.error;

                if (result != 0)
                {
                    string title = "Error";
                    MessageBox.Show(err, title);
                    return;
                }
                Confirm.Enabled = false;
                Plane.Enabled = false;
                Remove.Enabled = false;
            }
            catch (Exception)
            {
                //do nothing...
            }
        }
        private void Attack_Click(object sender, EventArgs e)
        {
            this.guess_operation = ATTACK;
        }
        private void BodyFlag_Click(object sender, EventArgs e)
        {
            this.guess_operation = SET_BODY_FLAG;
        }
        private void HeadFlag_Click(object sender, EventArgs e)
        {
            this.guess_operation = SET_HEAD_FLAG;
        }
        private void RemoveFlag_Click(object sender, EventArgs e)
        {
            this.guess_operation = REMOVE_FLAG;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.ai.Length != 0)
            {
                this.ai = "";
                this.LoadAI.Text = "LoadAI";
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                
                this.ai = openFileDialog.FileName;
                this.LoadAI.Text = "UnloadAI";
                this.AIProcessed = 0;
            }
        } 


        /*
         *      : 空白
         * H    : 飞机头
         * B    : body
         * U    : 未知的
         */
        private int [] getSolution(string board,string aiProgram)
        {
            int[] ans = new int[2];
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = aiProgram;
                process.StartInfo.Arguments = "";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();

                process.StandardInput.Write(board.ToCharArray());
                string ans_x = process.StandardOutput.ReadLine();
                string ans_y = process.StandardOutput.ReadLine();

                ans[0] = Int32.Parse(ans_x);
                ans[1] = Int32.Parse(ans_y);

                Console.WriteLine("Policy -> X:" + ans[0].ToString() + "  Y:" + ans[1].ToString());

            }
            catch(Exception)
            {
                ans[0] = -1;
                ans[1] = -1;
            }
            return ans;
        }
    }
}

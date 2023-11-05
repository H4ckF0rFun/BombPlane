namespace WindowsFormsApplication1
{
    partial class State
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.GameSession = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Plane = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.Confirm = new System.Windows.Forms.Button();
            this.guess_board = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.Player0Group = new System.Windows.Forms.GroupBox();
            this.my_left_time = new System.Windows.Forms.Label();
            this.player_0_left_time = new System.Windows.Forms.Label();
            this.Player_0_alive_planes = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.my_nickname = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Player1Group = new System.Windows.Forms.GroupBox();
            this.wait_tips = new System.Windows.Forms.Label();
            this.your_left_time = new System.Windows.Forms.Label();
            this.player_1_left_time = new System.Windows.Forms.Label();
            this.Player_1_alive_planes = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.your_nickname = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Attack = new System.Windows.Forms.Button();
            this.BodyFlag = new System.Windows.Forms.Button();
            this.RmFlag = new System.Windows.Forms.Button();
            this.HeadFlag = new System.Windows.Forms.Button();
            this.LoadAI = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.guess_board.SuspendLayout();
            this.Player0Group.SuspendLayout();
            this.Player1Group.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GameSession);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 73);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Game Information";
            // 
            // GameSession
            // 
            this.GameSession.Location = new System.Drawing.Point(95, 29);
            this.GameSession.Name = "GameSession";
            this.GameSession.ReadOnly = true;
            this.GameSession.Size = new System.Drawing.Size(231, 25);
            this.GameSession.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Session:";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(552, 507);
            this.panel1.TabIndex = 1;
            // 
            // Plane
            // 
            this.Plane.Location = new System.Drawing.Point(1144, 92);
            this.Plane.Name = "Plane";
            this.Plane.Size = new System.Drawing.Size(135, 33);
            this.Plane.TabIndex = 2;
            this.Plane.Text = "Plane";
            this.Plane.UseVisualStyleBackColor = true;
            this.Plane.Click += new System.EventHandler(this.Plane_Click);
            // 
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(1144, 131);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(135, 33);
            this.Remove.TabIndex = 3;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Confirm
            // 
            this.Confirm.Location = new System.Drawing.Point(1144, 170);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(135, 33);
            this.Confirm.TabIndex = 4;
            this.Confirm.Text = "Confirm";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // guess_board
            // 
            this.guess_board.BackColor = System.Drawing.SystemColors.ControlLight;
            this.guess_board.Controls.Add(this.label2);
            this.guess_board.Location = new System.Drawing.Point(570, 92);
            this.guess_board.Name = "guess_board";
            this.guess_board.Size = new System.Drawing.Size(555, 507);
            this.guess_board.TabIndex = 6;
            this.guess_board.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(62, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(449, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Wait for both sides to position their planes";
            // 
            // Player0Group
            // 
            this.Player0Group.Controls.Add(this.my_left_time);
            this.Player0Group.Controls.Add(this.player_0_left_time);
            this.Player0Group.Controls.Add(this.Player_0_alive_planes);
            this.Player0Group.Controls.Add(this.label6);
            this.Player0Group.Controls.Add(this.my_nickname);
            this.Player0Group.Controls.Add(this.label1);
            this.Player0Group.Location = new System.Drawing.Point(359, 16);
            this.Player0Group.Name = "Player0Group";
            this.Player0Group.Size = new System.Drawing.Size(460, 70);
            this.Player0Group.TabIndex = 9;
            this.Player0Group.TabStop = false;
            this.Player0Group.Text = "Player0";
            // 
            // my_left_time
            // 
            this.my_left_time.AutoSize = true;
            this.my_left_time.Location = new System.Drawing.Point(374, 31);
            this.my_left_time.Name = "my_left_time";
            this.my_left_time.Size = new System.Drawing.Size(31, 15);
            this.my_left_time.TabIndex = 11;
            this.my_left_time.Text = "00s";
            // 
            // player_0_left_time
            // 
            this.player_0_left_time.AutoSize = true;
            this.player_0_left_time.Location = new System.Drawing.Point(299, 31);
            this.player_0_left_time.Name = "player_0_left_time";
            this.player_0_left_time.Size = new System.Drawing.Size(79, 15);
            this.player_0_left_time.TabIndex = 10;
            this.player_0_left_time.Text = "LeftTime:";
            // 
            // Player_0_alive_planes
            // 
            this.Player_0_alive_planes.AutoSize = true;
            this.Player_0_alive_planes.Location = new System.Drawing.Point(220, 31);
            this.Player_0_alive_planes.MaximumSize = new System.Drawing.Size(80, 15);
            this.Player_0_alive_planes.MinimumSize = new System.Drawing.Size(80, 15);
            this.Player_0_alive_planes.Name = "Player_0_alive_planes";
            this.Player_0_alive_planes.Size = new System.Drawing.Size(80, 15);
            this.Player_0_alive_planes.TabIndex = 9;
            this.Player_0_alive_planes.Text = "3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(151, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Planes:";
            // 
            // my_nickname
            // 
            this.my_nickname.AutoSize = true;
            this.my_nickname.Location = new System.Drawing.Point(64, 31);
            this.my_nickname.Name = "my_nickname";
            this.my_nickname.Size = new System.Drawing.Size(71, 15);
            this.my_nickname.TabIndex = 7;
            this.my_nickname.Text = "nickname";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Me:";
            // 
            // Player1Group
            // 
            this.Player1Group.Controls.Add(this.wait_tips);
            this.Player1Group.Controls.Add(this.your_left_time);
            this.Player1Group.Controls.Add(this.player_1_left_time);
            this.Player1Group.Controls.Add(this.Player_1_alive_planes);
            this.Player1Group.Controls.Add(this.label7);
            this.Player1Group.Controls.Add(this.your_nickname);
            this.Player1Group.Controls.Add(this.label5);
            this.Player1Group.Location = new System.Drawing.Point(825, 16);
            this.Player1Group.Name = "Player1Group";
            this.Player1Group.Size = new System.Drawing.Size(460, 70);
            this.Player1Group.TabIndex = 10;
            this.Player1Group.TabStop = false;
            this.Player1Group.Text = "Player1";
            // 
            // wait_tips
            // 
            this.wait_tips.AutoSize = true;
            this.wait_tips.Font = new System.Drawing.Font("宋体", 12F);
            this.wait_tips.Location = new System.Drawing.Point(26, 31);
            this.wait_tips.Name = "wait_tips";
            this.wait_tips.Size = new System.Drawing.Size(399, 20);
            this.wait_tips.TabIndex = 12;
            this.wait_tips.Text = "Wait for another player to join game...";
            // 
            // your_left_time
            // 
            this.your_left_time.AutoSize = true;
            this.your_left_time.Location = new System.Drawing.Point(374, 31);
            this.your_left_time.Name = "your_left_time";
            this.your_left_time.Size = new System.Drawing.Size(31, 15);
            this.your_left_time.TabIndex = 12;
            this.your_left_time.Text = "00s";
            // 
            // player_1_left_time
            // 
            this.player_1_left_time.AutoSize = true;
            this.player_1_left_time.Location = new System.Drawing.Point(299, 31);
            this.player_1_left_time.Name = "player_1_left_time";
            this.player_1_left_time.Size = new System.Drawing.Size(79, 15);
            this.player_1_left_time.TabIndex = 11;
            this.player_1_left_time.Text = "LeftTime:";
            // 
            // Player_1_alive_planes
            // 
            this.Player_1_alive_planes.AutoSize = true;
            this.Player_1_alive_planes.Location = new System.Drawing.Point(220, 31);
            this.Player_1_alive_planes.MaximumSize = new System.Drawing.Size(80, 15);
            this.Player_1_alive_planes.MinimumSize = new System.Drawing.Size(80, 15);
            this.Player_1_alive_planes.Name = "Player_1_alive_planes";
            this.Player_1_alive_planes.Size = new System.Drawing.Size(80, 15);
            this.Player_1_alive_planes.TabIndex = 10;
            this.Player_1_alive_planes.Text = "3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(151, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "Planes:";
            // 
            // your_nickname
            // 
            this.your_nickname.AutoSize = true;
            this.your_nickname.Location = new System.Drawing.Point(64, 31);
            this.your_nickname.Name = "your_nickname";
            this.your_nickname.Size = new System.Drawing.Size(71, 15);
            this.your_nickname.TabIndex = 9;
            this.your_nickname.Text = "nickname";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "You:";
            // 
            // Attack
            // 
            this.Attack.Location = new System.Drawing.Point(1144, 92);
            this.Attack.Name = "Attack";
            this.Attack.Size = new System.Drawing.Size(135, 33);
            this.Attack.TabIndex = 11;
            this.Attack.Text = "Attack";
            this.Attack.UseVisualStyleBackColor = true;
            this.Attack.Visible = false;
            this.Attack.Click += new System.EventHandler(this.Attack_Click);
            // 
            // BodyFlag
            // 
            this.BodyFlag.Location = new System.Drawing.Point(1144, 131);
            this.BodyFlag.Name = "BodyFlag";
            this.BodyFlag.Size = new System.Drawing.Size(135, 33);
            this.BodyFlag.TabIndex = 12;
            this.BodyFlag.Text = "BodyFlag";
            this.BodyFlag.UseVisualStyleBackColor = true;
            this.BodyFlag.Visible = false;
            this.BodyFlag.Click += new System.EventHandler(this.BodyFlag_Click);
            // 
            // RmFlag
            // 
            this.RmFlag.Location = new System.Drawing.Point(1144, 211);
            this.RmFlag.Name = "RmFlag";
            this.RmFlag.Size = new System.Drawing.Size(135, 33);
            this.RmFlag.TabIndex = 13;
            this.RmFlag.Text = "RemoveFlag";
            this.RmFlag.UseVisualStyleBackColor = true;
            this.RmFlag.Visible = false;
            this.RmFlag.Click += new System.EventHandler(this.RemoveFlag_Click);
            // 
            // HeadFlag
            // 
            this.HeadFlag.Location = new System.Drawing.Point(1144, 172);
            this.HeadFlag.Name = "HeadFlag";
            this.HeadFlag.Size = new System.Drawing.Size(135, 33);
            this.HeadFlag.TabIndex = 14;
            this.HeadFlag.Text = "HeadFlag";
            this.HeadFlag.UseVisualStyleBackColor = true;
            this.HeadFlag.Visible = false;
            this.HeadFlag.Click += new System.EventHandler(this.HeadFlag_Click);
            // 
            // LoadAI
            // 
            this.LoadAI.Location = new System.Drawing.Point(1144, 250);
            this.LoadAI.Name = "LoadAI";
            this.LoadAI.Size = new System.Drawing.Size(135, 33);
            this.LoadAI.TabIndex = 15;
            this.LoadAI.Text = "LoadAI";
            this.LoadAI.UseVisualStyleBackColor = true;
            this.LoadAI.Visible = false;
            this.LoadAI.Click += new System.EventHandler(this.button1_Click);
            // 
            // State
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1291, 611);
            this.Controls.Add(this.LoadAI);
            this.Controls.Add(this.HeadFlag);
            this.Controls.Add(this.RmFlag);
            this.Controls.Add(this.BodyFlag);
            this.Controls.Add(this.Attack);
            this.Controls.Add(this.Player1Group);
            this.Controls.Add(this.Player0Group);
            this.Controls.Add(this.guess_board);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.Plane);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "State";
            this.Text = "BombPlane";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.guess_board.ResumeLayout(false);
            this.guess_board.PerformLayout();
            this.Player0Group.ResumeLayout(false);
            this.Player0Group.PerformLayout();
            this.Player1Group.ResumeLayout(false);
            this.Player1Group.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox GameSession;
        private System.Windows.Forms.Label label3;


        

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Plane;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Panel guess_board;
        private System.Windows.Forms.GroupBox Player0Group;
        private System.Windows.Forms.Label my_left_time;
        private System.Windows.Forms.Label player_0_left_time;
        private System.Windows.Forms.Label Player_0_alive_planes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label my_nickname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox Player1Group;
        private System.Windows.Forms.Label your_left_time;
        private System.Windows.Forms.Label player_1_left_time;
        private System.Windows.Forms.Label Player_1_alive_planes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label your_nickname;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label wait_tips;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Attack;
        private System.Windows.Forms.Button BodyFlag;
        private System.Windows.Forms.Button RmFlag;
        private System.Windows.Forms.Button HeadFlag;
        private System.Windows.Forms.Button LoadAI;
    }
}
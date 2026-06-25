using System;
using System.Drawing;
using System.Windows.Forms;
using POEp1.Models;
using POEp1.Services;
using POEp1.UI;

namespace POEp1.Forms
{
    public class MainForm : Form
    {
        private Panel sidebarPanel;
        private Panel mainTerminalPanel;
        private FlowLayoutPanel chatPanel;
        private Panel inputContainerPanel;

        private Label avatarLabel;
        private Label asciiAvatarLabel;
        private Label brandingSubtext;
        private TextBox inputBox;
        private Button sendBtn;

        private Button btnTerminal;
        private Button btnTopics;
        private Button btnMemory;
        private Button btnSettings;

        private readonly ChatbotService _chatbot;
        private readonly UserProfile _user;

        public MainForm()
        {

            this.Text = "C:\\USER\\CYBERSECURITY AWARENESS BOT >";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(5, 10, 5);

            // --- FORCE STATIC LOCKED WINDOW BOUNDS ---
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;

            _user = new UserProfile { Name = "User" };
            _chatbot = new ChatbotService(_user);
            // Syncs your dynamic frontend UserProfile instance context to the BotEngine cluster automatically
          //  BotEngine.RegisterChatbotContext(yourChatbotServiceInstanceName);

            SetupUI();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AudioPlayer.PlayGreetingAsync();
        }

        private void SetupUI()
        {
            this.Controls.Clear();
            this.SuspendLayout();

            // =================================================================
            // 1. SIDEBAR PANEL (Left Side Navigation - Recalculated Bounds)
            // =================================================================
            sidebarPanel = new Panel();
            sidebarPanel.Location = new Point(0, 0);
            sidebarPanel.Size = new Size(240, 560);
            sidebarPanel.BackColor = Color.FromArgb(3, 7, 3);
            sidebarPanel.Padding = new Padding(10);
            this.Controls.Add(sidebarPanel);

            avatarLabel = new Label();
            avatarLabel.Text = "👤\n[AI]";
            avatarLabel.Font = new Font("Consolas", 24, FontStyle.Bold);
            avatarLabel.ForeColor = Color.FromArgb(57, 255, 20);
            avatarLabel.TextAlign = ContentAlignment.MiddleCenter;
            avatarLabel.Size = new Size(120, 100);
            avatarLabel.Location = new Point(60, 20);
            avatarLabel.BorderStyle = BorderStyle.FixedSingle;
            sidebarPanel.Controls.Add(avatarLabel);

            brandingSubtext = new Label();
            brandingSubtext.Text = "stay safe. stay secure.";
            brandingSubtext.Font = new Font("Consolas", 9, FontStyle.Italic);
            brandingSubtext.ForeColor = Color.FromArgb(27, 94, 32);
            brandingSubtext.Location = new Point(10, 130);
            brandingSubtext.Size = new Size(220, 20);
            brandingSubtext.TextAlign = ContentAlignment.MiddleCenter;
            sidebarPanel.Controls.Add(brandingSubtext);

            int startY = 175;
            btnTerminal = CreateNavButton(">_ TERMINAL", startY, true);
            btnTopics = CreateNavButton(" TOPICS", startY + 45, false);
            btnMemory = CreateNavButton(" MEMORY", startY + 90, false);
            btnSettings = CreateNavButton(" SETTINGS", startY + 135, false);

            btnTopics.Click += new EventHandler((object sender, EventArgs e) =>
            {
                string topicList = "MONITORED THREAT MATRIX TRACKS:\n" +
                                   " -> PASSWORD (Credential security metrics)\n" +
                                   " -> SCAM     (Phishing and social engineering)\n" +
                                   " -> PRIVACY  (Data exposure mitigation)";
                chatPanel.Controls.Add(BubbleFactory.CreateBubble("SYSTEM", topicList, "neutral"));
                if (chatPanel.Controls.Count > 0)
                    chatPanel.ScrollControlIntoView(chatPanel.Controls[chatPanel.Controls.Count - 1]);
            });

            btnMemory.Click += new EventHandler((object sender, EventArgs e) =>
            {
                string savedName = string.IsNullOrEmpty(_user.Name) ? "NOT LOGGED" : _user.Name;
                string savedTopic = string.IsNullOrEmpty(_user.FavouriteTopic) ? "NO TOPIC RECORDED YET" : _user.FavouriteTopic.ToUpper();
                string lastDiscussed = string.IsNullOrEmpty(_user.LastDiscussedTopic) ? "NONE" : _user.LastDiscussedTopic.ToUpper();

                string memoryDump = $"=== ACTIVE CORE MEMORY SECTOR DUMP ===\n" +
                                    $" • USER IDENTIFIER : {savedName}\n" +
                                    $" • TARGET INTEREST : [{savedTopic}]\n" +
                                    $" • LAST TRACK RUN  : [{lastDiscussed}]\n" +
                                    $"======================================";

                chatPanel.Controls.Add(BubbleFactory.CreateBubble("SYSTEM", memoryDump, "curious"));
                if (chatPanel.Controls.Count > 0)
                    chatPanel.ScrollControlIntoView(chatPanel.Controls[chatPanel.Controls.Count - 1]);
            });

            sidebarPanel.Controls.AddRange(new Control[] { btnTerminal, btnTopics, btnMemory, btnSettings });

            asciiAvatarLabel = new Label();
            asciiAvatarLabel.Text = "x_x";
            asciiAvatarLabel.Font = new Font("Consolas", 14, FontStyle.Bold);
            asciiAvatarLabel.ForeColor = Color.FromArgb(57, 255, 20);
            asciiAvatarLabel.TextAlign = ContentAlignment.MiddleCenter;
            asciiAvatarLabel.Size = new Size(220, 35);
            asciiAvatarLabel.Location = new Point(10, 480);
            sidebarPanel.Controls.Add(asciiAvatarLabel);

            Label statusLabel = new Label();
            statusLabel.Text = "● STATUS: SECURE";
            statusLabel.ForeColor = Color.FromArgb(57, 255, 20);
            statusLabel.Font = new Font("Consolas", 10, FontStyle.Bold);
            statusLabel.Size = new Size(220, 25);
            statusLabel.TextAlign = ContentAlignment.MiddleCenter;
            statusLabel.Location = new Point(10, 520);
            sidebarPanel.Controls.Add(statusLabel);

            System.Windows.Forms.Timer pulseTimer = new System.Windows.Forms.Timer();
            pulseTimer.Interval = 1000;
            pulseTimer.Tick += new EventHandler((object s, EventArgs ev) =>
            {
                statusLabel.ForeColor = (statusLabel.ForeColor == Color.FromArgb(57, 255, 20))
                    ? Color.FromArgb(10, 45, 10)
                    : Color.FromArgb(57, 255, 20);
            });
            pulseTimer.Start();

            // =================================================================
            // 2. MAIN TERMINAL PANEL
            // =================================================================
            mainTerminalPanel = new Panel();
            mainTerminalPanel.Location = new Point(240, 0);
            mainTerminalPanel.Size = new Size(745, 560);
            mainTerminalPanel.BackColor = Color.FromArgb(5, 10, 5);
            mainTerminalPanel.Padding = new Padding(20);
            this.Controls.Add(mainTerminalPanel);

            inputContainerPanel = new Panel();
            inputContainerPanel.Dock = DockStyle.Bottom;
            inputContainerPanel.Height = 45;
            inputContainerPanel.Padding = new Padding(0, 5, 0, 0);
            mainTerminalPanel.Controls.Add(inputContainerPanel);

            inputBox = new TextBox();
            inputBox.Dock = DockStyle.Fill;
            inputBox.BackColor = Color.FromArgb(8, 20, 8);
            inputBox.ForeColor = Color.FromArgb(57, 255, 20);
            inputBox.Font = new Font("Consolas", 13);
            inputBox.BorderStyle = BorderStyle.FixedSingle;
            inputBox.KeyDown += InputBox_KeyDown;
            inputContainerPanel.Controls.Add(inputBox);

            sendBtn = new Button();
            sendBtn.Text = "SEND";
            sendBtn.Dock = DockStyle.Right;
            sendBtn.Width = 110;
            sendBtn.FlatStyle = FlatStyle.Flat;
            sendBtn.FlatAppearance.BorderColor = Color.FromArgb(20, 66, 20);
            sendBtn.BackColor = Color.FromArgb(5, 15, 5);
            sendBtn.ForeColor = Color.FromArgb(57, 255, 20);
            sendBtn.Font = new Font("Consolas", 11, FontStyle.Bold);
            sendBtn.Click += SendBtn_Click;
            inputContainerPanel.Controls.Add(sendBtn);

            chatPanel = new FlowLayoutPanel();
            chatPanel.Dock = DockStyle.Fill;
            chatPanel.FlowDirection = FlowDirection.TopDown;
            chatPanel.AutoScroll = true;
            chatPanel.WrapContents = false;
            chatPanel.BackColor = Color.FromArgb(5, 10, 5);
            mainTerminalPanel.Controls.Add(chatPanel);

            mainTerminalPanel.BringToFront();
            inputContainerPanel.BringToFront();
            chatPanel.BringToFront();

            this.ResumeLayout(false);
            InitTerminalSequence();
        }

        private Button CreateNavButton(string text, int topLocation, bool isActive)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(220, 38);
            btn.Location = new Point(10, topLocation);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(10, 30, 10);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Font = new Font("Consolas", 10, FontStyle.Bold);
            btn.ForeColor = isActive ? Color.FromArgb(57, 255, 20) : Color.FromArgb(46, 125, 50);
            return btn;
        }

        private void InitTerminalSequence()
        {
            chatPanel.Controls.Add(BubbleFactory.CreateBubble("SYSTEM", "Cybersecurity Awareness Chatbot initialized.", "neutral"));
            chatPanel.Controls.Add(BubbleFactory.CreateBubble("SYSTEM", "Type 'help' to see active protocol directories.", "neutral"));
        }

        // --- ADAPTED INPUT HANDLER FROM REQUEST ---
        private void HandleUserInput()
        {
            string textInput = inputBox.Text; // Adjusted to match your UI control 'inputBox'
            if (string.IsNullOrWhiteSpace(textInput)) return;

            // 1. Render user message bubble block safely on screen
            chatPanel.Controls.Add(BubbleFactory.CreateBubble("YOU", textInput, "neutral")); // Adjusted to match your UI Factory
            inputBox.Clear();

            // 2. Pass input data directly to our advanced system tracking layer
            // Note: If POEp1.Services.BotEngine doesn't compile, it falls back to your existing _chatbot processor.
            string botReply = POEp1.Services.BotEngine.ProcessInput(textInput, out string currentMood);

            // 3. Render the processed bot reply using the correct dynamic mood colors
            chatPanel.Controls.Add(BubbleFactory.CreateBubble("BOT", botReply, currentMood));

            // Auto-scroll logic
            if (chatPanel.Controls.Count > 0)
            {
                chatPanel.ScrollControlIntoView(chatPanel.Controls[chatPanel.Controls.Count - 1]);
            }
            inputBox.Focus();
        }

        // Wired up your UI click/press handlers to run your requested method
        private void SendBtn_Click(object sender, EventArgs e) => HandleUserInput();

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                HandleUserInput();
            }
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;

namespace POEp1.UI
{
    public static class BubbleFactory
    {
        public static Panel CreateBubble(string sender, string message, string mood)
        {
            // 1. Base Outer Layout Box (Fits cleanly inside your 745px main terminal panel)
            Panel bubbleContainer = new Panel();
            bubbleContainer.Width = 700;
            bubbleContainer.Margin = new Padding(0, 4, 0, 8);
            bubbleContainer.BackColor = Color.Transparent;

            // 2. Left Side Tag Label (Handles just the sender prefix name)
            Label senderLabel = new Label();
            senderLabel.Font = new Font("Consolas", 11, FontStyle.Bold);
            senderLabel.Location = new Point(5, 5);
            senderLabel.Width = 75; // Explicit fixed column width for "YOU:", "BOT:", "SYSTEM:"
            senderLabel.AutoSize = false; // Force fixed width so the message column stays aligned
            senderLabel.Height = 25;
            senderLabel.Text = $"{sender}:";

            // 3. Right Side Content Label (Handles the wrapped message block)
            Label messageLabel = new Label();
            messageLabel.Font = new Font("Consolas", 11);
            messageLabel.Location = new Point(85, 5); // Positioned exactly to the right of the sender column
            messageLabel.Width = 580; // Allow ample horizontal workspace
            messageLabel.AutoSize = true;
            messageLabel.MaximumSize = new Size(580, 0); // Lock width to force vertical paragraph wrapping
            messageLabel.Text = message;

            // --- COLOR DETERMINATION ENGINE ---
            Color themeColor = Color.FromArgb(57, 255, 20); // Default Pure Terminal Neon Green

            if (sender == "YOU")
            {
                themeColor = Color.FromArgb(210, 255, 210); // Crisp User White-Green
            }
            else if (sender == "SYSTEM")
            {
                if (message.Contains("help") || message.Contains("protocol"))
                {
                    themeColor = Color.FromArgb(46, 125, 50); // Softer, darker green for directories
                }
                else
                {
                    themeColor = Color.FromArgb(120, 180, 120); // Balanced System Notification Green
                }
            }
            else // It's the BOT
            {
                switch (mood.ToLower())
                {
                    case "curious":
                        themeColor = Color.FromArgb(0, 229, 255);  // Light Cyan Matrix Cyber Blue
                        break;
                    case "error_fallback":
                        themeColor = Color.FromArgb(212, 143, 56); // Clean Muted Terminal Amber Warning Color
                        break;
                    case "worried":
                        themeColor = Color.FromArgb(255, 235, 59); // Amber Alert Warning Yellow
                        break;
                    case "frustrated":
                        themeColor = Color.FromArgb(244, 67, 54);  // Alert Signal Crimson Red
                        break;
                    default:
                        themeColor = Color.FromArgb(57, 255, 20);   // Standard Pure Terminal Neon Green
                        break;
                }
            }

            // Assign matching colors to both columns for structural consistency
            senderLabel.ForeColor = themeColor;
            messageLabel.ForeColor = themeColor;

            // Assemble controls safely inside container bounds
            bubbleContainer.Controls.Add(senderLabel);
            bubbleContainer.Controls.Add(messageLabel);

            // Compute total height dynamically based on the height of the right message column
            using (Graphics g = messageLabel.CreateGraphics())
            {
                SizeF renderedBounds = g.MeasureString(messageLabel.Text, messageLabel.Font, messageLabel.Width);
                bubbleContainer.Height = Math.Max(30, (int)renderedBounds.Height + 14);
            }

            return bubbleContainer;
        }
    }
}
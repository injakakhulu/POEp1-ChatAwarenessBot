using System;
using System.Drawing;
using System.Windows.Forms;

namespace POEp1.UI
{
    public static class BubbleFactory
    {
        public static Panel CreateBubble(string sender, string message, string mood)
        {
            Panel bubbleContainer = new Panel();
            bubbleContainer.Width = 680;
            bubbleContainer.Margin = new Padding(0, 4, 0, 8);
            bubbleContainer.BackColor = Color.Transparent;

            Label outputLabel = new Label();
            outputLabel.Font = new Font("Consolas", 11);
            outputLabel.Location = new Point(5, 5);
            outputLabel.Width = 650;
            outputLabel.AutoSize = true;

            // Apply different design languages based on the active user profile track state
            if (sender == "YOU")
            {
                outputLabel.ForeColor = Color.FromArgb(210, 255, 210);
                outputLabel.Text = $"YOU: {message}";
            }
            else if (sender == "SYSTEM")
            {
                // FIX 1: If it's a help menu or protocol text, use the softer memory/topics green
                if (message.Contains("help") || message.Contains("protocol"))
                {
                    outputLabel.ForeColor = Color.FromArgb(120, 180, 120); // Softer, darker green
                }
                else
                {
                    outputLabel.ForeColor = Color.FromArgb(120, 180, 120); // Default relaxed system green
                }

                outputLabel.Text = $"SYSTEM: {message}";
            }
            else
            {
                // FIX 2: Intercept the "I don't understand" error message before it gets colored bright green
                if (message.Contains("don't understand") || message.Contains("Please focus on inquiry"))
                {
                    // Muted terminal amber warning color (much easier on the eyes than bright neon)
                    outputLabel.ForeColor = Color.FromArgb(212, 143, 56);
                }
                else
                {
                    // Highlight text color tracks to react to detected user sentiments
                    switch (mood.ToLower())
                    {
                        case "worried":
                            outputLabel.ForeColor = Color.FromArgb(255, 235, 59); // Amber Alert Warning Yellow
                            break;
                        case "frustrated":
                            outputLabel.ForeColor = Color.FromArgb(255, 235, 59);  
                            break;
                        case "curious":
                            outputLabel.ForeColor = Color.FromArgb(255, 235, 59);  
                            break;
                        default:
                            outputLabel.ForeColor = Color.FromArgb(57, 255, 20);   // Standard Pure Terminal Neon Green
                            break;
                            break;
                        case "error_fallback":
                            outputLabel.ForeColor = Color.FromArgb(212, 143, 56); // Clean Muted Terminal Amber Warning Color
                            break;
                    }
                }

                outputLabel.Text = $"BOT:\n     {message.Replace("\n", "\n     ")}";
            }

            bubbleContainer.Controls.Add(outputLabel);

            // Compute structural sizing dynamically using text dimension graphics libraries
            using (Graphics g = outputLabel.CreateGraphics())
            {
                SizeF renderedBounds = g.MeasureString(outputLabel.Text, outputLabel.Font, outputLabel.Width);
                bubbleContainer.Height = (int)renderedBounds.Height + 14;
            }

            return bubbleContainer;
        }
    }
}
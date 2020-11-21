namespace Pong
{
    partial class PingPongForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.scoreLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.Location = new System.Drawing.Point(230, 182);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(518, 112);
            this.scoreLabel.TabIndex = 1;
            this.scoreLabel.Text = "Player 1 v Player 2\r\n       0 - 0";
            this.scoreLabel.Visible = false;
            // 
            // PingPongForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 550);
            this.Controls.Add(this.scoreLabel);
            this.DoubleBuffered = true;
            this.Name = "PingPongForm";
            this.Text = "PingPongGame";
            this.Load += new System.EventHandler(this.PongForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PingPongForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PingPongForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PingPongForm_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label scoreLabel;
    }
}


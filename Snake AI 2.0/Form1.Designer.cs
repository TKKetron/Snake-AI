using System.Windows.Forms;

namespace Snake_AI_2._0
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.pbCanvas = new System.Windows.Forms.PictureBox();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.genCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bestFit = new System.Windows.Forms.Label();
            this.curFit = new System.Windows.Forms.Label();
            this.genration = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCanvas
            // 
            this.pbCanvas.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.pbCanvas.Location = new System.Drawing.Point(15, 15);
            this.pbCanvas.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pbCanvas.Name = "pbCanvas";
            this.pbCanvas.Size = new System.Drawing.Size(400, 400);
            this.pbCanvas.TabIndex = 0;
            this.pbCanvas.TabStop = false;
            this.pbCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.pbCanvas_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(489, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Generation";
            // 
            // genCount
            // 
            this.genCount.AutoSize = true;
            this.genCount.Location = new System.Drawing.Point(500, 30);
            this.genCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.genCount.Name = "genCount";
            this.genCount.Size = new System.Drawing.Size(13, 15);
            this.genCount.TabIndex = 5;
            this.genCount.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(489, 45);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Best Fitness";
            // 
            // bestFit
            // 
            this.bestFit.AutoSize = true;
            this.bestFit.Location = new System.Drawing.Point(500, 63);
            this.bestFit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.bestFit.Name = "bestFit";
            this.bestFit.Size = new System.Drawing.Size(13, 15);
            this.bestFit.TabIndex = 13;
            this.bestFit.Text = "0";
            // 
            // curFit
            // 
            this.curFit.AutoSize = true;
            this.curFit.Location = new System.Drawing.Point(155, 427);
            this.curFit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.curFit.Name = "curFit";
            this.curFit.Size = new System.Drawing.Size(38, 15);
            this.curFit.TabIndex = 14;
            this.curFit.Text = "label1";
            // 
            // genration
            // 
            this.genration.Location = new System.Drawing.Point(489, 177);
            this.genration.Name = "genration";
            this.genration.Size = new System.Drawing.Size(183, 66);
            this.genration.TabIndex = 15;
            this.genration.Text = "Learn";
            this.genration.UseVisualStyleBackColor = true;
            this.genration.Click += new System.EventHandler(this.genration_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(527, 269);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(83, 19);
            this.checkBox1.TabIndex = 16;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(500, 107);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 49);
            this.button1.TabIndex = 17;
            this.button1.Text = "Kill";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 504);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.genration);
            this.Controls.Add(this.curFit);
            this.Controls.Add(this.bestFit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.genCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pbCanvas);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCanvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCanvas;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label genCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label bestFit;
        private Label curFit;
        private Button genration;
        private CheckBox checkBox1;
        private Button button1;
    }
}


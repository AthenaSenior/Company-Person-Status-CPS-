
namespace Company_Person_Status__CPS_
{
    partial class AdminPanelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminPanelForm));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.DurationResetButton = new System.Windows.Forms.Button();
            this.ımageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ımageList2 = new System.Windows.Forms.ImageList(this.components);
            this.allUsersField = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.addUserButton = new System.Windows.Forms.Button();
            this.trackDurationButton = new System.Windows.Forms.Button();
            this.removeUserButton = new System.Windows.Forms.Button();
            this.editUserButton = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Lucida Bright", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(67, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "CPS Admin Panel";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::Company_Person_Status__CPS_.Properties.Resources.AnkameeLogo;
            this.pictureBox1.Location = new System.Drawing.Point(1, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(69, 74);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // DurationResetButton
            // 
            this.DurationResetButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DurationResetButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.DurationResetButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.DurationResetButton.Image = global::Company_Person_Status__CPS_.Properties.Resources.ClockReset1;
            this.DurationResetButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DurationResetButton.Location = new System.Drawing.Point(512, 12);
            this.DurationResetButton.Name = "DurationResetButton";
            this.DurationResetButton.Size = new System.Drawing.Size(181, 29);
            this.DurationResetButton.TabIndex = 2;
            this.DurationResetButton.Text = "      One-Click Duration Reset";
            this.DurationResetButton.UseVisualStyleBackColor = true;
            this.DurationResetButton.Click += new System.EventHandler(this.DurationResetButton_Click);
            // 
            // ımageList1
            // 
            this.ımageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ımageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.ımageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ımageList2
            // 
            this.ımageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ımageList2.ImageSize = new System.Drawing.Size(16, 16);
            this.ımageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // allUsersField
            // 
            this.allUsersField.BackColor = System.Drawing.Color.Silver;
            this.allUsersField.FormattingEnabled = true;
            this.allUsersField.ItemHeight = 15;
            this.allUsersField.Location = new System.Drawing.Point(48, 145);
            this.allUsersField.Name = "allUsersField";
            this.allUsersField.Size = new System.Drawing.Size(304, 289);
            this.allUsersField.TabIndex = 3;
            this.allUsersField.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Lucida Bright", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(48, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "All Users";
            // 
            // addUserButton
            // 
            this.addUserButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.addUserButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.addUserButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.addUserButton.Image = global::Company_Person_Status__CPS_.Properties.Resources._5363451__1_;
            this.addUserButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addUserButton.Location = new System.Drawing.Point(436, 145);
            this.addUserButton.Name = "addUserButton";
            this.addUserButton.Size = new System.Drawing.Size(181, 47);
            this.addUserButton.TabIndex = 5;
            this.addUserButton.Text = "Add User";
            this.addUserButton.UseVisualStyleBackColor = true;
            this.addUserButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // trackDurationButton
            // 
            this.trackDurationButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.trackDurationButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.trackDurationButton.Enabled = false;
            this.trackDurationButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.trackDurationButton.Image = global::Company_Person_Status__CPS_.Properties.Resources._3063792;
            this.trackDurationButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.trackDurationButton.Location = new System.Drawing.Point(436, 387);
            this.trackDurationButton.Name = "trackDurationButton";
            this.trackDurationButton.Size = new System.Drawing.Size(181, 47);
            this.trackDurationButton.TabIndex = 6;
            this.trackDurationButton.Text = "    Track Away Duration";
            this.trackDurationButton.UseVisualStyleBackColor = true;
            this.trackDurationButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // removeUserButton
            // 
            this.removeUserButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.removeUserButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.removeUserButton.Enabled = false;
            this.removeUserButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.removeUserButton.Image = global::Company_Person_Status__CPS_.Properties.Resources.output_onlinepngtools__4___1_;
            this.removeUserButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.removeUserButton.Location = new System.Drawing.Point(436, 301);
            this.removeUserButton.Name = "removeUserButton";
            this.removeUserButton.Size = new System.Drawing.Size(181, 47);
            this.removeUserButton.TabIndex = 7;
            this.removeUserButton.Text = "Remove User";
            this.removeUserButton.UseVisualStyleBackColor = true;
            this.removeUserButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // editUserButton
            // 
            this.editUserButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.editUserButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.editUserButton.Enabled = false;
            this.editUserButton.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.editUserButton.Image = global::Company_Person_Status__CPS_.Properties.Resources._166256;
            this.editUserButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editUserButton.Location = new System.Drawing.Point(436, 224);
            this.editUserButton.Name = "editUserButton";
            this.editUserButton.Size = new System.Drawing.Size(181, 47);
            this.editUserButton.TabIndex = 8;
            this.editUserButton.Text = "Edit User";
            this.editUserButton.UseVisualStyleBackColor = true;
            this.editUserButton.Click += new System.EventHandler(this.button4_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::Company_Person_Status__CPS_.Properties.Resources.output_onlinepngtools;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(531, 239);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(180, 260);
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // AdminPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(700, 494);
            this.Controls.Add(this.editUserButton);
            this.Controls.Add(this.removeUserButton);
            this.Controls.Add(this.trackDurationButton);
            this.Controls.Add(this.addUserButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.allUsersField);
            this.Controls.Add(this.DurationResetButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AdminPanelForm";
            this.Text = "Admin Panel";
            this.Load += new System.EventHandler(this.AdminPanelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button DurationResetButton;
        private System.Windows.Forms.ImageList ımageList1;
        private System.Windows.Forms.ImageList ımageList2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button trackDurationButton;
        private System.Windows.Forms.Button removeUserButton;
        private System.Windows.Forms.Button editUserButton;
        private System.Windows.Forms.PictureBox pictureBox2;
        public System.Windows.Forms.ListBox allUsersField;
        public System.Windows.Forms.Button addUserButton;
    }
}
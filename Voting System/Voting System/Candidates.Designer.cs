namespace Voting_System
{
    partial class Candidates
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
            this.dgvCandidates = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblCandidateID = new System.Windows.Forms.TextBox();
            this.cmbParty = new System.Windows.Forms.ComboBox();
            this.cmbPosition = new System.Windows.Forms.ComboBox();
            this.lblGrade = new System.Windows.Forms.TextBox();
            this.lblStudentName = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblStudentID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtFndStdnt = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCandidates)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCandidates
            // 
            this.dgvCandidates.BackgroundColor = System.Drawing.Color.White;
            this.dgvCandidates.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCandidates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCandidates.Location = new System.Drawing.Point(344, 131);
            this.dgvCandidates.Name = "dgvCandidates";
            this.dgvCandidates.RowHeadersVisible = false;
            this.dgvCandidates.Size = new System.Drawing.Size(324, 294);
            this.dgvCandidates.TabIndex = 743;
            this.dgvCandidates.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCandidates_CellContentClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Label11);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lblCandidateID);
            this.groupBox1.Controls.Add(this.cmbParty);
            this.groupBox1.Controls.Add(this.cmbPosition);
            this.groupBox1.Controls.Add(this.lblGrade);
            this.groupBox1.Controls.Add(this.lblStudentName);
            this.groupBox1.Controls.Add(this.Label4);
            this.groupBox1.Controls.Add(this.Label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblStudentID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.groupBox1.Location = new System.Drawing.Point(38, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 319);
            this.groupBox1.TabIndex = 742;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Register a Candidate";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Font = new System.Drawing.Font("Segoe Marker", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.Label11.Location = new System.Drawing.Point(11, 74);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(256, 18);
            this.Label11.TabIndex = 803;
            this.Label11.Text = "---------- { Candidate\'s Information } ----------";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.label9.Location = new System.Drawing.Point(14, 121);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 17);
            this.label9.TabIndex = 801;
            this.label9.Text = "Candidate ID :";
            // 
            // lblCandidateID
            // 
            this.lblCandidateID.BackColor = System.Drawing.Color.White;
            this.lblCandidateID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblCandidateID.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCandidateID.ForeColor = System.Drawing.Color.Black;
            this.lblCandidateID.Location = new System.Drawing.Point(117, 121);
            this.lblCandidateID.Multiline = true;
            this.lblCandidateID.Name = "lblCandidateID";
            this.lblCandidateID.ReadOnly = true;
            this.lblCandidateID.Size = new System.Drawing.Size(148, 20);
            this.lblCandidateID.TabIndex = 802;
            // 
            // cmbParty
            // 
            this.cmbParty.DisplayMember = "Class";
            this.cmbParty.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbParty.FormattingEnabled = true;
            this.cmbParty.Items.AddRange(new object[] {
            "The Visionaries",
            "",
            "",
            "The Achievers",
            "",
            "",
            "Future Leaders",
            "",
            "",
            "The Innovators",
            "",
            "",
            "The Mavericks",
            "",
            "",
            "The Pioneers",
            "",
            "",
            "The Trailblazers",
            "",
            "",
            "The Game Changers",
            "",
            "",
            "The Ambassadors",
            "",
            "",
            "The Champions"});
            this.cmbParty.Location = new System.Drawing.Point(117, 228);
            this.cmbParty.Name = "cmbParty";
            this.cmbParty.Size = new System.Drawing.Size(148, 23);
            this.cmbParty.TabIndex = 800;
            this.cmbParty.ValueMember = "Class";
            // 
            // cmbPosition
            // 
            this.cmbPosition.DisplayMember = "Class";
            this.cmbPosition.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPosition.FormattingEnabled = true;
            this.cmbPosition.Items.AddRange(new object[] {
            "President",
            "Vice President",
            "Secretary",
            "Treasurer",
            "JHS Governor",
            "SHS Governor",
            "JHS Vice Governor",
            "SHS Vice Governor"});
            this.cmbPosition.Location = new System.Drawing.Point(117, 199);
            this.cmbPosition.Name = "cmbPosition";
            this.cmbPosition.Size = new System.Drawing.Size(148, 23);
            this.cmbPosition.TabIndex = 799;
            this.cmbPosition.ValueMember = "Class";
            // 
            // lblGrade
            // 
            this.lblGrade.BackColor = System.Drawing.Color.White;
            this.lblGrade.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblGrade.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrade.ForeColor = System.Drawing.Color.Black;
            this.lblGrade.Location = new System.Drawing.Point(117, 173);
            this.lblGrade.Multiline = true;
            this.lblGrade.Name = "lblGrade";
            this.lblGrade.ReadOnly = true;
            this.lblGrade.Size = new System.Drawing.Size(148, 20);
            this.lblGrade.TabIndex = 798;
            // 
            // lblStudentName
            // 
            this.lblStudentName.BackColor = System.Drawing.Color.White;
            this.lblStudentName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblStudentName.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudentName.ForeColor = System.Drawing.Color.Black;
            this.lblStudentName.Location = new System.Drawing.Point(117, 147);
            this.lblStudentName.Multiline = true;
            this.lblStudentName.Name = "lblStudentName";
            this.lblStudentName.ReadOnly = true;
            this.lblStudentName.Size = new System.Drawing.Size(148, 20);
            this.lblStudentName.TabIndex = 797;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.Label4.Location = new System.Drawing.Point(30, 149);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(74, 17);
            this.Label4.TabIndex = 790;
            this.Label4.Text = "Full-Name :";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.Label5.Location = new System.Drawing.Point(29, 96);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(75, 17);
            this.Label5.TabIndex = 791;
            this.Label5.Text = "Student ID :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.label7.Location = new System.Drawing.Point(60, 229);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 17);
            this.label7.TabIndex = 796;
            this.label7.Text = "Party :";
            // 
            // lblStudentID
            // 
            this.lblStudentID.BackColor = System.Drawing.Color.White;
            this.lblStudentID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblStudentID.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudentID.ForeColor = System.Drawing.Color.Black;
            this.lblStudentID.Location = new System.Drawing.Point(117, 95);
            this.lblStudentID.Multiline = true;
            this.lblStudentID.Name = "lblStudentID";
            this.lblStudentID.ReadOnly = true;
            this.lblStudentID.Size = new System.Drawing.Size(148, 20);
            this.lblStudentID.TabIndex = 792;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.label2.Location = new System.Drawing.Point(53, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 17);
            this.label2.TabIndex = 794;
            this.label2.Text = "Grade :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.label6.Location = new System.Drawing.Point(43, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 17);
            this.label6.TabIndex = 795;
            this.label6.Text = "Position :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.label1.Location = new System.Drawing.Point(35, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(581, 32);
            this.label1.TabIndex = 738;
            this.label1.Text = "Fill up all the required information. Make sure all the spellings are typed corre" +
    "ctly before registering.\r\nFor any questions, you may ask the assistance of the F" +
    "acilitator.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Britannic Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(112)))));
            this.label3.Location = new System.Drawing.Point(34, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(212, 23);
            this.label3.TabIndex = 737;
            this.label3.Text = "REGISTER CANDIDATE";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(101)))), ((int)(((byte)(101)))));
            this.Label8.Location = new System.Drawing.Point(341, 106);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(75, 17);
            this.Label8.TabIndex = 740;
            this.Label8.Text = "Student ID :";
            // 
            // txtFndStdnt
            // 
            this.txtFndStdnt.BackColor = System.Drawing.Color.White;
            this.txtFndStdnt.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFndStdnt.ForeColor = System.Drawing.Color.Black;
            this.txtFndStdnt.Location = new System.Drawing.Point(415, 104);
            this.txtFndStdnt.Multiline = true;
            this.txtFndStdnt.Name = "txtFndStdnt";
            this.txtFndStdnt.Size = new System.Drawing.Size(153, 21);
            this.txtFndStdnt.TabIndex = 741;
            this.txtFndStdnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFndStdnt.TextChanged += new System.EventHandler(this.txtFndStdnt_TextChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(112)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(112)))));
            this.btnDelete.Location = new System.Drawing.Point(402, 433);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(86, 26);
            this.btnDelete.TabIndex = 766;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(112)))));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(112)))));
            this.btnEdit.Location = new System.Drawing.Point(310, 433);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(86, 26);
            this.btnEdit.TabIndex = 765;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(112)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(122)))), ((int)(((byte)(112)))));
            this.btnSave.Location = new System.Drawing.Point(218, 433);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 26);
            this.btnSave.TabIndex = 767;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Candidates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(706, 482);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvCandidates);
            this.Controls.Add(this.txtFndStdnt);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Candidates";
            this.Text = "Candidates";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCandidates)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCandidates;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.TextBox txtFndStdnt;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.Button btnEdit;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.TextBox lblCandidateID;
        internal System.Windows.Forms.ComboBox cmbParty;
        internal System.Windows.Forms.ComboBox cmbPosition;
        internal System.Windows.Forms.TextBox lblGrade;
        internal System.Windows.Forms.TextBox lblStudentName;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.TextBox lblStudentID;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Label Label11;
    }
}
namespace SysbotMemoryViewer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GB_Connection = new System.Windows.Forms.GroupBox();
            this.BTN_Connect = new System.Windows.Forms.Button();
            this.TB_Port = new System.Windows.Forms.TextBox();
            this.TB_IP = new System.Windows.Forms.TextBox();
            this.LBL_IP = new System.Windows.Forms.Label();
            this.GB_Search = new System.Windows.Forms.GroupBox();
            this.BTN_Restart = new System.Windows.Forms.Button();
            this.BTN_Undo = new System.Windows.Forms.Button();
            this.BTN_Search = new System.Windows.Forms.Button();
            this.BTN_Dump = new System.Windows.Forms.Button();
            this.BTN_DecHex = new System.Windows.Forms.Button();
            this.NUD_SearchValue = new System.Windows.Forms.NumericUpDown();
            this.CB_Comparer = new System.Windows.Forms.ComboBox();
            this.CB_Condition = new System.Windows.Forms.ComboBox();
            this.CB_Type = new System.Windows.Forms.ComboBox();
            this.LBL_Value = new System.Windows.Forms.Label();
            this.LBL_Type = new System.Windows.Forms.Label();
            this.DGV_Values = new System.Windows.Forms.DataGridView();
            this.PB_Main = new System.Windows.Forms.ProgressBar();
            this.GB_Connection.SuspendLayout();
            this.GB_Search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_SearchValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Values)).BeginInit();
            this.SuspendLayout();
            // 
            // GB_Connection
            // 
            this.GB_Connection.Controls.Add(this.BTN_Connect);
            this.GB_Connection.Controls.Add(this.TB_Port);
            this.GB_Connection.Controls.Add(this.TB_IP);
            this.GB_Connection.Controls.Add(this.LBL_IP);
            this.GB_Connection.Location = new System.Drawing.Point(12, 12);
            this.GB_Connection.Name = "GB_Connection";
            this.GB_Connection.Size = new System.Drawing.Size(373, 45);
            this.GB_Connection.TabIndex = 0;
            this.GB_Connection.TabStop = false;
            this.GB_Connection.Text = "Connection";
            // 
            // BTN_Connect
            // 
            this.BTN_Connect.Location = new System.Drawing.Point(288, 16);
            this.BTN_Connect.Name = "BTN_Connect";
            this.BTN_Connect.Size = new System.Drawing.Size(75, 23);
            this.BTN_Connect.TabIndex = 3;
            this.BTN_Connect.Text = "Connect";
            this.BTN_Connect.UseVisualStyleBackColor = true;
            this.BTN_Connect.Click += new System.EventHandler(this.BTN_Connect_Click);
            // 
            // TB_Port
            // 
            this.TB_Port.Location = new System.Drawing.Point(207, 16);
            this.TB_Port.Name = "TB_Port";
            this.TB_Port.Size = new System.Drawing.Size(75, 23);
            this.TB_Port.TabIndex = 2;
            this.TB_Port.Text = "6000";
            // 
            // TB_IP
            // 
            this.TB_IP.Location = new System.Drawing.Point(65, 16);
            this.TB_IP.Name = "TB_IP";
            this.TB_IP.Size = new System.Drawing.Size(136, 23);
            this.TB_IP.TabIndex = 1;
            this.TB_IP.Text = "127.0.0.1";
            // 
            // LBL_IP
            // 
            this.LBL_IP.AutoSize = true;
            this.LBL_IP.Location = new System.Drawing.Point(6, 19);
            this.LBL_IP.Name = "LBL_IP";
            this.LBL_IP.Size = new System.Drawing.Size(53, 15);
            this.LBL_IP.TabIndex = 0;
            this.LBL_IP.Text = "IP + Port";
            // 
            // GB_Search
            // 
            this.GB_Search.Controls.Add(this.BTN_Restart);
            this.GB_Search.Controls.Add(this.BTN_Undo);
            this.GB_Search.Controls.Add(this.BTN_Search);
            this.GB_Search.Controls.Add(this.BTN_Dump);
            this.GB_Search.Controls.Add(this.BTN_DecHex);
            this.GB_Search.Controls.Add(this.NUD_SearchValue);
            this.GB_Search.Controls.Add(this.CB_Comparer);
            this.GB_Search.Controls.Add(this.CB_Condition);
            this.GB_Search.Controls.Add(this.CB_Type);
            this.GB_Search.Controls.Add(this.LBL_Value);
            this.GB_Search.Controls.Add(this.LBL_Type);
            this.GB_Search.Location = new System.Drawing.Point(12, 63);
            this.GB_Search.Name = "GB_Search";
            this.GB_Search.Size = new System.Drawing.Size(373, 303);
            this.GB_Search.TabIndex = 1;
            this.GB_Search.TabStop = false;
            this.GB_Search.Text = "Search";
            this.GB_Search.Visible = false;
            // 
            // BTN_Restart
            // 
            this.BTN_Restart.Location = new System.Drawing.Point(270, 157);
            this.BTN_Restart.Name = "BTN_Restart";
            this.BTN_Restart.Size = new System.Drawing.Size(93, 57);
            this.BTN_Restart.TabIndex = 9;
            this.BTN_Restart.Text = "Restart";
            this.BTN_Restart.UseVisualStyleBackColor = true;
            this.BTN_Restart.Click += new System.EventHandler(this.BTN_Restart_Click);
            // 
            // BTN_Undo
            // 
            this.BTN_Undo.Location = new System.Drawing.Point(144, 157);
            this.BTN_Undo.Name = "BTN_Undo";
            this.BTN_Undo.Size = new System.Drawing.Size(93, 57);
            this.BTN_Undo.TabIndex = 8;
            this.BTN_Undo.Text = "Undo";
            this.BTN_Undo.UseVisualStyleBackColor = true;
            this.BTN_Undo.Click += new System.EventHandler(this.BTN_Undo_Click);
            // 
            // BTN_Search
            // 
            this.BTN_Search.Location = new System.Drawing.Point(12, 157);
            this.BTN_Search.Name = "BTN_Search";
            this.BTN_Search.Size = new System.Drawing.Size(93, 57);
            this.BTN_Search.TabIndex = 7;
            this.BTN_Search.Text = "Search";
            this.BTN_Search.UseVisualStyleBackColor = true;
            this.BTN_Search.Click += new System.EventHandler(this.BTN_Search_Click);
            // 
            // BTN_Dump
            // 
            this.BTN_Dump.Location = new System.Drawing.Point(12, 239);
            this.BTN_Dump.Name = "BTN_Dump";
            this.BTN_Dump.Size = new System.Drawing.Size(93, 49);
            this.BTN_Dump.TabIndex = 3;
            this.BTN_Dump.Text = "Dump";
            this.BTN_Dump.UseVisualStyleBackColor = true;
            this.BTN_Dump.Click += new System.EventHandler(this.BTN_Dump_Click);
            // 
            // BTN_DecHex
            // 
            this.BTN_DecHex.Location = new System.Drawing.Point(188, 67);
            this.BTN_DecHex.Name = "BTN_DecHex";
            this.BTN_DecHex.Size = new System.Drawing.Size(175, 27);
            this.BTN_DecHex.TabIndex = 6;
            this.BTN_DecHex.Text = "Decimal > Hex";
            this.BTN_DecHex.UseVisualStyleBackColor = true;
            // 
            // NUD_SearchValue
            // 
            this.NUD_SearchValue.Hexadecimal = true;
            this.NUD_SearchValue.Location = new System.Drawing.Point(188, 38);
            this.NUD_SearchValue.Maximum = new decimal(new int[] {
            -1,
            -1,
            0,
            0});
            this.NUD_SearchValue.Name = "NUD_SearchValue";
            this.NUD_SearchValue.Size = new System.Drawing.Size(175, 23);
            this.NUD_SearchValue.TabIndex = 5;
            // 
            // CB_Comparer
            // 
            this.CB_Comparer.FormattingEnabled = true;
            this.CB_Comparer.Location = new System.Drawing.Point(12, 111);
            this.CB_Comparer.Name = "CB_Comparer";
            this.CB_Comparer.Size = new System.Drawing.Size(146, 23);
            this.CB_Comparer.TabIndex = 4;
            // 
            // CB_Condition
            // 
            this.CB_Condition.FormattingEnabled = true;
            this.CB_Condition.Location = new System.Drawing.Point(12, 82);
            this.CB_Condition.Name = "CB_Condition";
            this.CB_Condition.Size = new System.Drawing.Size(146, 23);
            this.CB_Condition.TabIndex = 3;
            // 
            // CB_Type
            // 
            this.CB_Type.FormattingEnabled = true;
            this.CB_Type.Location = new System.Drawing.Point(12, 37);
            this.CB_Type.Name = "CB_Type";
            this.CB_Type.Size = new System.Drawing.Size(146, 23);
            this.CB_Type.TabIndex = 2;
            // 
            // LBL_Value
            // 
            this.LBL_Value.AutoSize = true;
            this.LBL_Value.Location = new System.Drawing.Point(188, 19);
            this.LBL_Value.Name = "LBL_Value";
            this.LBL_Value.Size = new System.Drawing.Size(35, 15);
            this.LBL_Value.TabIndex = 1;
            this.LBL_Value.Text = "Value";
            // 
            // LBL_Type
            // 
            this.LBL_Type.AutoSize = true;
            this.LBL_Type.Location = new System.Drawing.Point(12, 19);
            this.LBL_Type.Name = "LBL_Type";
            this.LBL_Type.Size = new System.Drawing.Size(31, 15);
            this.LBL_Type.TabIndex = 0;
            this.LBL_Type.Text = "Type";
            // 
            // DGV_Values
            // 
            this.DGV_Values.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Values.Location = new System.Drawing.Point(424, 18);
            this.DGV_Values.Name = "DGV_Values";
            this.DGV_Values.RowTemplate.Height = 25;
            this.DGV_Values.Size = new System.Drawing.Size(337, 348);
            this.DGV_Values.TabIndex = 2;
            // 
            // PB_Main
            // 
            this.PB_Main.Location = new System.Drawing.Point(30, 397);
            this.PB_Main.Name = "PB_Main";
            this.PB_Main.Size = new System.Drawing.Size(719, 23);
            this.PB_Main.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PB_Main);
            this.Controls.Add(this.DGV_Values);
            this.Controls.Add(this.GB_Search);
            this.Controls.Add(this.GB_Connection);
            this.Name = "Form1";
            this.Text = "Form1";
            this.GB_Connection.ResumeLayout(false);
            this.GB_Connection.PerformLayout();
            this.GB_Search.ResumeLayout(false);
            this.GB_Search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_SearchValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Values)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox GB_Connection;
        private TextBox TB_Port;
        private TextBox TB_IP;
        private Label LBL_IP;
        private Button BTN_Connect;
        private GroupBox GB_Search;
        private Button BTN_DecHex;
        private NumericUpDown NUD_SearchValue;
        private ComboBox CB_Comparer;
        private ComboBox CB_Condition;
        private ComboBox CB_Type;
        private Label LBL_Value;
        private Label LBL_Type;
        private DataGridView DGV_Values;
        private Button BTN_Dump;
        private Button BTN_Search;
        private Button BTN_Restart;
        private Button BTN_Undo;
        private ProgressBar PB_Main;
    }
}
namespace ZiyangPackagingSoftware
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            groupBox2 = new GroupBox();
            groupBox4 = new GroupBox();
            dataGridView1 = new DataGridView();
            groupBox3 = new GroupBox();
            queryBySN = new Button();
            queryByPackagingNumber = new Button();
            label1 = new Label();
            snSearchInput = new TextBox();
            packagingNumberSearchInput = new TextBox();
            label2 = new Label();
            groupBox1 = new GroupBox();
            rePrimaryPrintInput = new TextBox();
            label6 = new Label();
            groupBox5 = new GroupBox();
            export = new Button();
            label4 = new Label();
            packagingNumberExportInput = new TextBox();
            rePrintLabel = new Button();
            snList = new ListBox();
            printLabelTest = new Button();
            executePackaging = new Button();
            snInput = new TextBox();
            label3 = new Label();
            tabPage2 = new TabPage();
            groupBox8 = new GroupBox();
            groupBox9 = new GroupBox();
            dataGridView2 = new DataGridView();
            groupBox10 = new GroupBox();
            querySecondaryByPackagingNumber = new Button();
            secondaryPackagingCodeinput = new TextBox();
            label8 = new Label();
            groupBox6 = new GroupBox();
            reSecondaryPrintInput = new TextBox();
            label10 = new Label();
            label9 = new Label();
            groupBox7 = new GroupBox();
            secondaryExport = new Button();
            label5 = new Label();
            secondaryPackagingNumberExportInput = new TextBox();
            reSecondaryPrintLabel = new Button();
            primaryPackagingCodeList = new ListBox();
            printSecondaryLabelTest = new Button();
            executeSecondaryPackaging = new Button();
            primaryCodeInput = new TextBox();
            tabPage3 = new TabPage();
            statusStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox3.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox5.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            groupBox10.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip1.Location = new Point(0, 445);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(912, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(0, 17);
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(912, 445);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(904, 415);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Primary";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(505, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(396, 409);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Search";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(dataGridView1);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 119);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(390, 287);
            groupBox4.TabIndex = 7;
            groupBox4.TabStop = false;
            groupBox4.Text = "result";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 19);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(384, 265);
            dataGridView1.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(queryBySN);
            groupBox3.Controls.Add(queryByPackagingNumber);
            groupBox3.Controls.Add(label1);
            groupBox3.Controls.Add(snSearchInput);
            groupBox3.Controls.Add(packagingNumberSearchInput);
            groupBox3.Controls.Add(label2);
            groupBox3.Dock = DockStyle.Top;
            groupBox3.Location = new Point(3, 19);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(390, 100);
            groupBox3.TabIndex = 6;
            groupBox3.TabStop = false;
            groupBox3.Text = "criteria";
            // 
            // queryBySN
            // 
            queryBySN.Location = new Point(309, 26);
            queryBySN.Name = "queryBySN";
            queryBySN.Size = new Size(75, 23);
            queryBySN.TabIndex = 2;
            queryBySN.Text = "Search";
            queryBySN.UseVisualStyleBackColor = true;
            queryBySN.Click += queryBySN_Click;
            // 
            // queryByPackagingNumber
            // 
            queryByPackagingNumber.Location = new Point(309, 56);
            queryByPackagingNumber.Name = "queryByPackagingNumber";
            queryByPackagingNumber.Size = new Size(75, 23);
            queryByPackagingNumber.TabIndex = 5;
            queryByPackagingNumber.Text = "Search";
            queryByPackagingNumber.UseVisualStyleBackColor = true;
            queryByPackagingNumber.Click += queryByPackagingNumber_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(107, 29);
            label1.Name = "label1";
            label1.Size = new Size(28, 17);
            label1.TabIndex = 0;
            label1.Text = "SN:";
            // 
            // snSearchInput
            // 
            snSearchInput.Location = new Point(144, 25);
            snSearchInput.Name = "snSearchInput";
            snSearchInput.Size = new Size(159, 23);
            snSearchInput.TabIndex = 1;
            // 
            // packagingNumberSearchInput
            // 
            packagingNumberSearchInput.Location = new Point(144, 56);
            packagingNumberSearchInput.Name = "packagingNumberSearchInput";
            packagingNumberSearchInput.Size = new Size(159, 23);
            packagingNumberSearchInput.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 58);
            label2.Name = "label2";
            label2.Size = new Size(119, 17);
            label2.TabIndex = 3;
            label2.Text = "PackagingNumber:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rePrimaryPrintInput);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(groupBox5);
            groupBox1.Controls.Add(rePrintLabel);
            groupBox1.Controls.Add(snList);
            groupBox1.Controls.Add(printLabelTest);
            groupBox1.Controls.Add(executePackaging);
            groupBox1.Controls.Add(snInput);
            groupBox1.Controls.Add(label3);
            groupBox1.Dock = DockStyle.Left;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(502, 409);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Binding";
            // 
            // rePrimaryPrintInput
            // 
            rePrimaryPrintInput.Location = new Point(149, 238);
            rePrimaryPrintInput.Name = "rePrimaryPrintInput";
            rePrimaryPrintInput.Size = new Size(159, 23);
            rePrimaryPrintInput.TabIndex = 13;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(18, 240);
            label6.Name = "label6";
            label6.Size = new Size(119, 17);
            label6.TabIndex = 12;
            label6.Text = "PackagingNumber:";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(export);
            groupBox5.Controls.Add(label4);
            groupBox5.Controls.Add(packagingNumberExportInput);
            groupBox5.Dock = DockStyle.Bottom;
            groupBox5.Location = new Point(3, 306);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(339, 100);
            groupBox5.TabIndex = 11;
            groupBox5.TabStop = false;
            groupBox5.Text = "TestReport";
            // 
            // export
            // 
            export.BackColor = Color.White;
            export.Location = new Point(146, 57);
            export.Name = "export";
            export.Size = new Size(129, 31);
            export.TabIndex = 12;
            export.Text = "Export";
            export.UseVisualStyleBackColor = false;
            export.Click += export_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 31);
            label4.Name = "label4";
            label4.Size = new Size(119, 17);
            label4.TabIndex = 11;
            label4.Text = "PackagingNumber:";
            // 
            // packagingNumberExportInput
            // 
            packagingNumberExportInput.Location = new Point(146, 28);
            packagingNumberExportInput.Name = "packagingNumberExportInput";
            packagingNumberExportInput.Size = new Size(159, 23);
            packagingNumberExportInput.TabIndex = 10;
            // 
            // rePrintLabel
            // 
            rePrintLabel.Location = new Point(203, 267);
            rePrintLabel.Name = "rePrintLabel";
            rePrintLabel.Size = new Size(105, 30);
            rePrintLabel.TabIndex = 8;
            rePrintLabel.Text = "RePrint";
            rePrintLabel.UseVisualStyleBackColor = true;
            rePrintLabel.Click += rePrintLabel_Click;
            // 
            // snList
            // 
            snList.Dock = DockStyle.Right;
            snList.FormattingEnabled = true;
            snList.ItemHeight = 17;
            snList.Location = new Point(342, 19);
            snList.Name = "snList";
            snList.Size = new Size(157, 387);
            snList.TabIndex = 7;
            snList.DoubleClick += snList_DoubleClick;
            // 
            // printLabelTest
            // 
            printLabelTest.BackColor = Color.White;
            printLabelTest.Location = new Point(65, 129);
            printLabelTest.Name = "printLabelTest";
            printLabelTest.Size = new Size(111, 51);
            printLabelTest.TabIndex = 6;
            printLabelTest.Text = "PrintTest";
            printLabelTest.UseVisualStyleBackColor = false;
            printLabelTest.Click += printLabelTest_Click;
            // 
            // executePackaging
            // 
            executePackaging.Location = new Point(203, 119);
            executePackaging.Name = "executePackaging";
            executePackaging.Size = new Size(105, 70);
            executePackaging.TabIndex = 5;
            executePackaging.Text = "Execute";
            executePackaging.UseVisualStyleBackColor = true;
            executePackaging.Click += executePrimaryPackaging_ClickAsync;
            // 
            // snInput
            // 
            snInput.Location = new Point(55, 41);
            snInput.Name = "snInput";
            snInput.Size = new Size(159, 23);
            snInput.TabIndex = 3;
            snInput.TextChanged += snInput_TextChanged;
            snInput.KeyPress += snInput_KeyPressAsync;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 45);
            label3.Name = "label3";
            label3.Size = new Size(28, 17);
            label3.TabIndex = 2;
            label3.Text = "SN:";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox8);
            tabPage2.Controls.Add(groupBox6);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(904, 415);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Secondary";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(groupBox9);
            groupBox8.Controls.Add(groupBox10);
            groupBox8.Dock = DockStyle.Fill;
            groupBox8.Location = new Point(505, 3);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(396, 409);
            groupBox8.TabIndex = 2;
            groupBox8.TabStop = false;
            groupBox8.Text = "Search";
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(dataGridView2);
            groupBox9.Dock = DockStyle.Fill;
            groupBox9.Location = new Point(3, 119);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(390, 287);
            groupBox9.TabIndex = 7;
            groupBox9.TabStop = false;
            groupBox9.Text = "result";
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 19);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowTemplate.Height = 25;
            dataGridView2.Size = new Size(384, 265);
            dataGridView2.TabIndex = 0;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(querySecondaryByPackagingNumber);
            groupBox10.Controls.Add(secondaryPackagingCodeinput);
            groupBox10.Controls.Add(label8);
            groupBox10.Dock = DockStyle.Top;
            groupBox10.Location = new Point(3, 19);
            groupBox10.Name = "groupBox10";
            groupBox10.Size = new Size(390, 100);
            groupBox10.TabIndex = 6;
            groupBox10.TabStop = false;
            groupBox10.Text = "criteria";
            // 
            // querySecondaryByPackagingNumber
            // 
            querySecondaryByPackagingNumber.Location = new Point(309, 56);
            querySecondaryByPackagingNumber.Name = "querySecondaryByPackagingNumber";
            querySecondaryByPackagingNumber.Size = new Size(75, 23);
            querySecondaryByPackagingNumber.TabIndex = 5;
            querySecondaryByPackagingNumber.Text = "Search";
            querySecondaryByPackagingNumber.UseVisualStyleBackColor = true;
            querySecondaryByPackagingNumber.Click += querySecondaryByPackagingNumber_Click;
            // 
            // secondaryPackagingCodeinput
            // 
            secondaryPackagingCodeinput.Location = new Point(144, 55);
            secondaryPackagingCodeinput.Name = "secondaryPackagingCodeinput";
            secondaryPackagingCodeinput.Size = new Size(159, 23);
            secondaryPackagingCodeinput.TabIndex = 4;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(16, 61);
            label8.Name = "label8";
            label8.Size = new Size(119, 17);
            label8.TabIndex = 3;
            label8.Text = "PackagingNumber:";
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(reSecondaryPrintInput);
            groupBox6.Controls.Add(label10);
            groupBox6.Controls.Add(label9);
            groupBox6.Controls.Add(groupBox7);
            groupBox6.Controls.Add(reSecondaryPrintLabel);
            groupBox6.Controls.Add(primaryPackagingCodeList);
            groupBox6.Controls.Add(printSecondaryLabelTest);
            groupBox6.Controls.Add(executeSecondaryPackaging);
            groupBox6.Controls.Add(primaryCodeInput);
            groupBox6.Dock = DockStyle.Left;
            groupBox6.Location = new Point(3, 3);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(502, 409);
            groupBox6.TabIndex = 1;
            groupBox6.TabStop = false;
            groupBox6.Text = "Binding";
            // 
            // reSecondaryPrintInput
            // 
            reSecondaryPrintInput.Location = new Point(168, 225);
            reSecondaryPrintInput.Name = "reSecondaryPrintInput";
            reSecondaryPrintInput.Size = new Size(159, 23);
            reSecondaryPrintInput.TabIndex = 15;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 231);
            label10.Name = "label10";
            label10.Size = new Size(163, 17);
            label10.TabIndex = 14;
            label10.Text = "SecondaryPackagingCode:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 42);
            label9.Name = "label9";
            label9.Size = new Size(146, 17);
            label9.TabIndex = 12;
            label9.Text = "PrimaryPackagingCode:";
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(secondaryExport);
            groupBox7.Controls.Add(label5);
            groupBox7.Controls.Add(secondaryPackagingNumberExportInput);
            groupBox7.Dock = DockStyle.Bottom;
            groupBox7.Location = new Point(3, 306);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(339, 100);
            groupBox7.TabIndex = 11;
            groupBox7.TabStop = false;
            groupBox7.Text = "TestReport";
            // 
            // secondaryExport
            // 
            secondaryExport.BackColor = Color.White;
            secondaryExport.Location = new Point(146, 57);
            secondaryExport.Name = "secondaryExport";
            secondaryExport.Size = new Size(129, 31);
            secondaryExport.TabIndex = 12;
            secondaryExport.Text = "Export";
            secondaryExport.UseVisualStyleBackColor = false;
            secondaryExport.Click += secondaryExport_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 31);
            label5.Name = "label5";
            label5.Size = new Size(119, 17);
            label5.TabIndex = 11;
            label5.Text = "PackagingNumber:";
            // 
            // secondaryPackagingNumberExportInput
            // 
            secondaryPackagingNumberExportInput.Location = new Point(146, 28);
            secondaryPackagingNumberExportInput.Name = "secondaryPackagingNumberExportInput";
            secondaryPackagingNumberExportInput.Size = new Size(159, 23);
            secondaryPackagingNumberExportInput.TabIndex = 10;
            // 
            // reSecondaryPrintLabel
            // 
            reSecondaryPrintLabel.Location = new Point(203, 254);
            reSecondaryPrintLabel.Name = "reSecondaryPrintLabel";
            reSecondaryPrintLabel.Size = new Size(105, 27);
            reSecondaryPrintLabel.TabIndex = 8;
            reSecondaryPrintLabel.Text = "RePrint";
            reSecondaryPrintLabel.UseVisualStyleBackColor = true;
            reSecondaryPrintLabel.Click += reSecondaryPrintLabel_Click;
            // 
            // primaryPackagingCodeList
            // 
            primaryPackagingCodeList.Dock = DockStyle.Right;
            primaryPackagingCodeList.FormattingEnabled = true;
            primaryPackagingCodeList.ItemHeight = 17;
            primaryPackagingCodeList.Location = new Point(342, 19);
            primaryPackagingCodeList.Name = "primaryPackagingCodeList";
            primaryPackagingCodeList.Size = new Size(157, 387);
            primaryPackagingCodeList.TabIndex = 7;
            // 
            // printSecondaryLabelTest
            // 
            printSecondaryLabelTest.BackColor = Color.White;
            printSecondaryLabelTest.Location = new Point(41, 149);
            printSecondaryLabelTest.Name = "printSecondaryLabelTest";
            printSecondaryLabelTest.Size = new Size(111, 51);
            printSecondaryLabelTest.TabIndex = 6;
            printSecondaryLabelTest.Text = "PrintTest";
            printSecondaryLabelTest.UseVisualStyleBackColor = false;
            printSecondaryLabelTest.Click += printSecondaryLabelTest_Click;
            // 
            // executeSecondaryPackaging
            // 
            executeSecondaryPackaging.Location = new Point(203, 119);
            executeSecondaryPackaging.Name = "executeSecondaryPackaging";
            executeSecondaryPackaging.Size = new Size(105, 70);
            executeSecondaryPackaging.TabIndex = 5;
            executeSecondaryPackaging.Text = "Execute";
            executeSecondaryPackaging.UseVisualStyleBackColor = true;
            executeSecondaryPackaging.Click += executeSecondaryPackaging_Click;
            // 
            // primaryCodeInput
            // 
            primaryCodeInput.Location = new Point(158, 39);
            primaryCodeInput.Name = "primaryCodeInput";
            primaryCodeInput.Size = new Size(159, 23);
            primaryCodeInput.TabIndex = 3;
            primaryCodeInput.KeyPress += primaryCodeInput_KeyPress;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(904, 415);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Tertiary";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(912, 467);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            Margin = new Padding(4);
            Name = "MainForm";
            Text = "MainForm";
            Load += MainForm_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private Label label1;
        private Button queryBySN;
        private TextBox snSearchInput;
        private Button queryByPackagingNumber;
        private TextBox packagingNumberSearchInput;
        private Label label2;
        private Button printLabelTest;
        private Button executePackaging;
        private TextBox snInput;
        private Label label3;
        private ListBox snList;
        private GroupBox groupBox4;
        private DataGridView dataGridView1;
        private GroupBox groupBox3;
        private Button rePrintLabel;
        private GroupBox groupBox5;
        private TextBox packagingNumberExportInput;
        private Button export;
        private Label label4;
        private GroupBox groupBox8;
        private GroupBox groupBox9;
        private DataGridView dataGridView2;
        private GroupBox groupBox10;
        private Button button5;
        private Button querySecondaryByPackagingNumber;
        private Label label7;
        private TextBox secondaryPackagingCodeinput;
        private TextBox textBox4;
        private Label label8;
        private GroupBox groupBox6;
        private Label label9;
        private GroupBox groupBox7;
        private Button secondaryExport;
        private Label label5;
        private TextBox secondaryPackagingNumberExportInput;
        private Button reSecondaryPrintLabel;
        private ListBox primaryPackagingCodeList;
        private Button printSecondaryLabelTest;
        private Button executeSecondaryPackaging;
        private TextBox primaryCodeInput;
        private TextBox rePrimaryPrintInput;
        private Label label6;
        private TextBox reSecondaryPrintInput;
        private Label label10;
    }
}

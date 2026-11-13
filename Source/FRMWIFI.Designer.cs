namespace WifiOlcer
{
    partial class FRMWIFI
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
            this.LBLCAPTION = new System.Windows.Forms.Label();
            this.GBWIFI = new System.Windows.Forms.GroupBox();
            this.CBSSID = new System.Windows.Forms.ComboBox();
            this.LBLCHANNEL = new System.Windows.Forms.Label();
            this.LBLKANAL = new System.Windows.Forms.Label();
            this.LBLSEVIYE = new System.Windows.Forms.Label();
            this.PBSINYAL = new System.Windows.Forms.ProgressBar();
            this.LBLSINYALSEVIYESI = new System.Windows.Forms.Label();
            this.LBLSSIDNAME = new System.Windows.Forms.Label();
            this.LBLSSID = new System.Windows.Forms.Label();
            this.TMRREFRESH = new System.Windows.Forms.Timer(this.components);
            this.LBLSTATUS = new System.Windows.Forms.Label();
            this.LBLDURUM = new System.Windows.Forms.Label();
            this.GBWIFI.SuspendLayout();
            this.SuspendLayout();
            // 
            // LBLCAPTION
            // 
            this.LBLCAPTION.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LBLCAPTION.Location = new System.Drawing.Point(2, 9);
            this.LBLCAPTION.Name = "LBLCAPTION";
            this.LBLCAPTION.Size = new System.Drawing.Size(519, 71);
            this.LBLCAPTION.TabIndex = 1;
            this.LBLCAPTION.Text = "Wifi Tester";
            this.LBLCAPTION.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GBWIFI
            // 
            this.GBWIFI.Controls.Add(this.LBLSTATUS);
            this.GBWIFI.Controls.Add(this.LBLDURUM);
            this.GBWIFI.Controls.Add(this.CBSSID);
            this.GBWIFI.Controls.Add(this.LBLCHANNEL);
            this.GBWIFI.Controls.Add(this.LBLKANAL);
            this.GBWIFI.Controls.Add(this.LBLSEVIYE);
            this.GBWIFI.Controls.Add(this.PBSINYAL);
            this.GBWIFI.Controls.Add(this.LBLSINYALSEVIYESI);
            this.GBWIFI.Controls.Add(this.LBLSSIDNAME);
            this.GBWIFI.Controls.Add(this.LBLSSID);
            this.GBWIFI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.GBWIFI.Location = new System.Drawing.Point(2, 83);
            this.GBWIFI.Name = "GBWIFI";
            this.GBWIFI.Size = new System.Drawing.Size(525, 347);
            this.GBWIFI.TabIndex = 2;
            this.GBWIFI.TabStop = false;
            this.GBWIFI.Text = "Wifi Tester";
            // 
            // CBSSID
            // 
            this.CBSSID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBSSID.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.CBSSID.FormattingEnabled = true;
            this.CBSSID.Location = new System.Drawing.Point(5, 299);
            this.CBSSID.Name = "CBSSID";
            this.CBSSID.Size = new System.Drawing.Size(514, 28);
            this.CBSSID.TabIndex = 10;
            this.CBSSID.SelectedIndexChanged += new System.EventHandler(this.CBSSID_SelectedIndexChanged);
            // 
            // LBLCHANNEL
            // 
            this.LBLCHANNEL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBLCHANNEL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LBLCHANNEL.Location = new System.Drawing.Point(104, 78);
            this.LBLCHANNEL.Name = "LBLCHANNEL";
            this.LBLCHANNEL.Size = new System.Drawing.Size(415, 37);
            this.LBLCHANNEL.TabIndex = 6;
            this.LBLCHANNEL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LBLKANAL
            // 
            this.LBLKANAL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBLKANAL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LBLKANAL.Location = new System.Drawing.Point(0, 78);
            this.LBLKANAL.Name = "LBLKANAL";
            this.LBLKANAL.Size = new System.Drawing.Size(101, 37);
            this.LBLKANAL.TabIndex = 5;
            this.LBLKANAL.Text = "Kanal";
            this.LBLKANAL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LBLSEVIYE
            // 
            this.LBLSEVIYE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBLSEVIYE.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LBLSEVIYE.Location = new System.Drawing.Point(0, 234);
            this.LBLSEVIYE.Name = "LBLSEVIYE";
            this.LBLSEVIYE.Size = new System.Drawing.Size(519, 40);
            this.LBLSEVIYE.TabIndex = 4;
            this.LBLSEVIYE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PBSINYAL
            // 
            this.PBSINYAL.Location = new System.Drawing.Point(104, 191);
            this.PBSINYAL.Name = "PBSINYAL";
            this.PBSINYAL.Size = new System.Drawing.Size(415, 40);
            this.PBSINYAL.TabIndex = 3;
            // 
            // LBLSINYALSEVIYESI
            // 
            this.LBLSINYALSEVIYESI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBLSINYALSEVIYESI.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LBLSINYALSEVIYESI.Location = new System.Drawing.Point(1, 191);
            this.LBLSINYALSEVIYESI.Name = "LBLSINYALSEVIYESI";
            this.LBLSINYALSEVIYESI.Size = new System.Drawing.Size(101, 40);
            this.LBLSINYALSEVIYESI.TabIndex = 2;
            this.LBLSINYALSEVIYESI.Text = "Sinyal Seviyesi";
            this.LBLSINYALSEVIYESI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LBLSSIDNAME
            // 
            this.LBLSSIDNAME.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBLSSIDNAME.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LBLSSIDNAME.Location = new System.Drawing.Point(104, 19);
            this.LBLSSIDNAME.Name = "LBLSSIDNAME";
            this.LBLSSIDNAME.Size = new System.Drawing.Size(415, 37);
            this.LBLSSIDNAME.TabIndex = 1;
            this.LBLSSIDNAME.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LBLSSID
            // 
            this.LBLSSID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBLSSID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LBLSSID.Location = new System.Drawing.Point(1, 19);
            this.LBLSSID.Name = "LBLSSID";
            this.LBLSSID.Size = new System.Drawing.Size(101, 37);
            this.LBLSSID.TabIndex = 0;
            this.LBLSSID.Text = "Ağ Adı:";
            this.LBLSSID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TMRREFRESH
            // 
            this.TMRREFRESH.Interval = 2;
            this.TMRREFRESH.Tick += new System.EventHandler(this.TMRREFRESH_Tick);
            // 
            // LBLSTATUS
            // 
            this.LBLSTATUS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBLSTATUS.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LBLSTATUS.Location = new System.Drawing.Point(104, 134);
            this.LBLSTATUS.Name = "LBLSTATUS";
            this.LBLSTATUS.Size = new System.Drawing.Size(415, 37);
            this.LBLSTATUS.TabIndex = 12;
            this.LBLSTATUS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LBLDURUM
            // 
            this.LBLDURUM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LBLDURUM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LBLDURUM.Location = new System.Drawing.Point(0, 134);
            this.LBLDURUM.Name = "LBLDURUM";
            this.LBLDURUM.Size = new System.Drawing.Size(101, 37);
            this.LBLDURUM.TabIndex = 11;
            this.LBLDURUM.Text = "Bağlantı Durumu";
            this.LBLDURUM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FRMWIFI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(524, 435);
            this.Controls.Add(this.GBWIFI);
            this.Controls.Add(this.LBLCAPTION);
            this.MaximizeBox = false;
            this.Name = "FRMWIFI";
            this.Text = "Wifi Tester";
            this.Load += new System.EventHandler(this.FRMWIFI_Load);
            this.GBWIFI.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label LBLCAPTION;
        private System.Windows.Forms.GroupBox GBWIFI;
        private System.Windows.Forms.ProgressBar PBSINYAL;
        private System.Windows.Forms.Label LBLSINYALSEVIYESI;
        private System.Windows.Forms.Label LBLSSIDNAME;
        private System.Windows.Forms.Label LBLSSID;
        private System.Windows.Forms.Label LBLSEVIYE;
        private System.Windows.Forms.Label LBLCHANNEL;
        private System.Windows.Forms.Label LBLKANAL;
        private System.Windows.Forms.ComboBox CBSSID;
        private System.Windows.Forms.Timer TMRREFRESH;
        private System.Windows.Forms.Label LBLSTATUS;
        private System.Windows.Forms.Label LBLDURUM;
    }
}
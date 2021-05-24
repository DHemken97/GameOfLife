using System;
using System.Windows.Forms;

namespace GameOfLifeSimulator
{
    public partial class SettingsWindow : Form
    {
        public event EventHandler RestartRequested;
        public event EventHandler RefreshRequested;
        public event EventHandler SettingChanged;

        public int SpawnChance => (int) numericUpDown1.Value;
        public int BoardSize => (int) numericUpDown2.Value;
        public int ChangeChance => (int) numericUpDown3.Value;
        public int Speed => (int) numericUpDown4.Value;
        public bool ShowNew => checkBox1.Checked;
        public bool ShowOld => checkBox2.Checked;
        public int OldAge => (int)numericUpDown5.Value;
        public int Threshold => (int)numericUpDown6.Value;
        public int DeathAge => (int)numericUpDown7.Value;
        public bool TwoLayer => checkBox3.Checked;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RestartRequested?.Invoke(this,EventArgs.Empty);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SettingChanged?.Invoke(this, EventArgs.Empty);

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            label5.Visible = checkBox2.Checked;
            numericUpDown5.Visible = checkBox2.Checked;
            SettingChanged?.Invoke(this, EventArgs.Empty);

        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            SettingChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            SettingChanged?.Invoke(this, EventArgs.Empty);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            SettingChanged?.Invoke(this, EventArgs.Empty);
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            SettingChanged?.Invoke(this, EventArgs.Empty);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}

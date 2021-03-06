﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using Impostor.Patcher.Shared;
using Impostor.Patcher.Shared.Events;

namespace Impostor.Patcher.WinForms.Forms
{
    public partial class FrmMain : Form
    {
        private readonly Configuration _config;
        private readonly AmongUsModifier _modifier;

        public FrmMain()
        {
            InitializeComponent();

            AcceptButton = buttonLaunch;

            _config = new Configuration();
            _modifier = new AmongUsModifier();
            _modifier.Error += ModifierOnError;
            _modifier.Saved += ModifierOnSaved;
        }

        private void ModifierOnError(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.Message, "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            comboIp.Text = "RankedAmongUs NA";
            comboIp.Focus();

            comboIp.Enabled = true;
            buttonLaunch.Enabled = true;
        }

        private void ModifierOnSaved(object sender, SavedEventArgs e)
        {
            MessageBox.Show("The region server settings have been saved\nPlease (re)start Among Us.", "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            var ipText = e.Port == AmongUsModifier.DefaultPort
                ? e.IpAddress
                : $"{e.IpAddress}:{e.Port}";

            buttonLaunch.Enabled = true;

            _config.AddIp(ipText);
            _config.Save();

            /*RefreshComboIps();*/
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            /*_config.Load();

            RefreshComboIps();

            if (_modifier.TryLoadIp(out var ipAddress))
            {
                comboIp.Text = ipAddress;
            }*/

            comboIp.Text = "RankedAmongUs NA";
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            comboIp.Focus();
        }

        private void textIp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.Handled = true;

            buttonLaunch_Click(this, EventArgs.Empty);
        }

        private async void buttonLaunch_Click(object sender, EventArgs e)
        {
            comboIp.Enabled = true;
            buttonLaunch.Enabled = true;

            _modifier.RegionName = $"<color=#00DB19>{comboIp.Text}";

            await _modifier.SaveIpAsync(comboIp.Text == "RankedAmongUs FR" ? "fr1.rankedamongus.com:22028" : comboIp.Text == "RankedAmongUs ASIA" ? "as1.rankedamongus.com" : comboIp.Text == "RankedAmongUs EU" ? "eu1.rankedamongus.com": "na1.rankedamongus.com");
        }

        private void lblUrl_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Impostor/Impostor");
        }

        private void RefreshComboIps()
        {
            comboIp.Items.Clear();

            if (_config.RecentIps.Count > 0)
            {
                foreach (var ip in _config.RecentIps)
                {
                    comboIp.Items.Add(ip);
                }
            }
        }
    }
}

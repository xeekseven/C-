using ConsoleInvoker.src;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleInvoker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ComponentInit();
            
        }
        private void ComponentInit()
        {
            try
            {
                ImageList img = new ImageList();
                String path = Application.StartupPath + "\\StaticConfig\\img";
                img.Images.Add(Image.FromFile(path + "\\closed.ico"));
                img.Images.Add(Image.FromFile(path + "\\folder.ico"));
                treeView.ImageList = img;
                treeView.SelectedImageIndex = 1;
                treeView.ImageIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                //load
                ConfigSetting.GetInstance().InitConfig(AppDomain.CurrentDomain.BaseDirectory + "ConfigApp.json");

                foreach (var configItem in ConfigSetting.GetInstance().ConfigJson.configList)
                {
                    string toolBarName = configItem.ToolBarName;
                    TreeNode parentNode = new TreeNode();
                    parentNode.Text = toolBarName;
                    foreach (var childItem in configItem.ToolChildArray)
                    {
                        TreeNode nodeItem = new TreeNode();
                        nodeItem.Text = childItem.ConsoleName;
                        nodeItem.Tag = new { paramName = childItem.ParamName, paramValue = childItem.ParamValue };
                        nodeItem.ToolTipText = AppDomain.CurrentDomain.BaseDirectory+childItem.ExecFilePath;
                        parentNode.Nodes.Add(nodeItem);

                    }
                    treeView.Nodes.Add(parentNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
            
        }

        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private void StartBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(execFiletxt.Text))
                {
                    MessageBox.Show("找不到此路径的exe文件");
                    return;
                }
                TabPage newPage = new TabPage();
                //newPage.Padding =new Padding(100,100,100,100);
                newPage.Text = titleLabel.Text.Substring(0,4)+"...";
                newPage.ToolTipText = titleLabel.Text;
                ToolTip newToolTip = new ToolTip();
                
                //newPage.Width = 20;
                tabControl.TabPages.Add(newPage);
                newToolTip.SetToolTip(newPage, titleLabel.Text);
                string execFullPath = String.Format("\"{0}\"", execFiletxt.Text);
                var cmd = Process.Start(execFullPath, paramTextTxt.Text);
                newPage.Tag = cmd;
                SpinWait.SpinUntil(() => cmd.MainWindowHandle != (IntPtr)0);
                SetParent(cmd.MainWindowHandle, newPage.Handle);
                //tabControl.SelectedIndex = tabControl.TabPages.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
        private void TreeClick(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = sender as TreeNode;
                execFiletxt.Text = node.ToolTipText;
                dynamic param = node.Tag;

                paramNameTxt.Text = param.paramName;
                paramTextTxt.Text = param.paramValue;
                titleLabel.Text = node.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (treeView.SelectedNode.Nodes.Count > 0) return;
                //MessageBox.Show(treeView.SelectedNode.Text);
                TreeClick(treeView.SelectedNode, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            nowTabPage.Text = tabControl.SelectedTab.ToolTipText;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int index = tabControl.SelectedIndex;
                if (index > 0)
                {
                    Process process = tabControl.SelectedTab.Tag as Process;
                    process.Kill();
                    process.Dispose();
                    tabControl.TabPages.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (TabPage pageItem in tabControl.TabPages)
            {
                try
                {
                    if (pageItem.Tag != null)
                    {
                        Process process = pageItem.Tag as Process;
                        process.Kill();
                        process.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
            }
        }

    }
}

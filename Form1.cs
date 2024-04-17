using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SonarServices
{
    public partial class Form1 : Form
    {
        private Process cmdProcess;
        private string filePath1 = @"C:\sonarqube-9.9.0.65466\logs\ce.log";
        private string filePath2 = @"C:\sonarqube-9.9.0.65466\logs\es.log";
        private string filePath3 = @"C:\sonarqube-9.9.0.65466\logs\sonar.log";
        private string filePath4 = @"C:\sonarqube-9.9.0.65466\logs\web.log";

        private string backupFilePath1 = @"C:\sonarqube-9.9.0.65466\logs\ce-bak.log";
        private string backupFilePath2 = @"C:\sonarqube-9.9.0.65466\logs\es-bak.log";
        private string backupFilePath3 = @"C:\sonarqube-9.9.0.65466\logs\sonar-bak.log";
        private string backupFilePath4 = @"C:\sonarqube-9.9.0.65466\logs\web-bak.log";

        private Timer timer1, timer2, timer3, timer4;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1 = new Timer();
            timer1.Interval = 4000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();

            timer2 = new Timer();
            timer2.Interval = 4000;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Start();

            timer3 = new Timer();
            timer3.Interval = 4000;
            timer3.Tick += new EventHandler(timer3_Tick);
            timer3.Start();

            timer4 = new Timer();
            timer4.Interval = 4000;
            timer4.Tick += new EventHandler(timer4_Tick);
            timer4.Start();


            ProcessStartInfo cmdStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            cmdProcess = new Process // Initialize cmdProcess at the class level
            {
                StartInfo = cmdStartInfo,
                EnableRaisingEvents = true
            };
            cmdProcess.OutputDataReceived += (s, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    AppendTextToRichTextBox(args.Data + Environment.NewLine);
                }
            };
            cmdProcess.Start();
            cmdProcess.BeginOutputReadLine();
        }
        private void SendCommandToCmd(string command)
        {
            try
            {
                if (cmdProcess != null && !cmdProcess.HasExited)
                {
                    cmdProcess.StandardInput.WriteLine(command);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AppendTextToRichTextBox(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AppendTextToRichTextBox(text)));
            }
            else
            {
                richTextBox0.AppendText(text);
            }
        }
        


        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendCommandToCmd("cd C:\\sonarqube-9.9.0.65466\\bin\\windows-x86-64");
            SendCommandToCmd("StartSonar.bat");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string processName = "java";
            KillProcessByName(processName);

            Application.Exit();
        }

        static void KillProcessByName(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                Console.WriteLine($"No {processName} processes found.");
                return;
            }
            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                    Console.WriteLine($"Killed {processName} process with ID {process.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error killing {processName} process: {ex.Message}");
                }
            }
        }

        private bool IsFileInUse(string path)
        {
            try
            {
                using (File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(filePath1))
                {
                    if (IsFileInUse(filePath1))
                    {
                        File.Copy(filePath1, backupFilePath1, true);
                        string fileContents = File.ReadAllText(backupFilePath1);
                        richTextBox1.Text = fileContents;
                        if (File.Exists(backupFilePath1))
                        {
                            File.Delete(backupFilePath1);
                        }
                    }
                    else
                    {
                        string fileContents = File.ReadAllText(filePath1);
                        richTextBox1.Text = fileContents;
                    }
                }
                else
                {
                    richTextBox1.Text = "File not found.";
                }
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "Error reading file: " + ex.Message;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(filePath2))
                {
                    if (IsFileInUse(filePath2))
                    {
                        File.Copy(filePath2, backupFilePath2, true);
                        string fileContents = File.ReadAllText(backupFilePath2);
                        richTextBox2.Text = fileContents;
                        if (File.Exists(backupFilePath2))
                        {
                            File.Delete(backupFilePath2);
                        }
                    }
                    else
                    {
                        string fileContents = File.ReadAllText(filePath2);
                        richTextBox2.Text = fileContents;
                    }
                }
                else
                {
                    richTextBox2.Text = "File not found.";
                }
            }
            catch (Exception ex)
            {
                richTextBox2.Text = "Error reading file: " + ex.Message;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(filePath3))
                {
                    if (IsFileInUse(filePath3))
                    {
                        File.Copy(filePath3, backupFilePath3, true);
                        string fileContents = File.ReadAllText(backupFilePath3);
                        richTextBox3.Text = fileContents;
                        if (File.Exists(backupFilePath3))
                        {
                            File.Delete(backupFilePath3);
                        }
                    }
                    else
                    {
                        string fileContents = File.ReadAllText(filePath3);
                        richTextBox3.Text = fileContents;
                    }
                }
                else
                {
                    richTextBox3.Text = "File not found.";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text = "Error reading file: " + ex.Message;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(filePath4))
                {
                    if (IsFileInUse(filePath4))
                    {
                        File.Copy(filePath4, backupFilePath4, true);
                        string fileContents = File.ReadAllText(backupFilePath4);
                        richTextBox4.Text = fileContents;
                        if (File.Exists(backupFilePath4))
                        {
                            File.Delete(backupFilePath4);
                        }
                    }
                    else
                    {
                        string fileContents = File.ReadAllText(filePath4);
                        richTextBox4.Text = fileContents;
                    }
                }
                else
                {
                    richTextBox4.Text = "File not found.";
                }
            }
            catch (Exception ex)
            {
                richTextBox4.Text = "Error reading file: " + ex.Message;
            }
        }
    }
}

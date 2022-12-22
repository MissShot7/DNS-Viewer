using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {


     

        public Form1()
        {
            InitializeComponent();
            this.Text = "DNS Viewer";
            List<string> DNSlist = new List<string>();

            
            TextBox.AcceptsTab = true;
            // Set WordWrap to true to allow text to wrap to the next line.
            TextBox.WordWrap = true;


            TextBox.Text = string.Join("\n", GetDNS());

            TextBox.ReadOnly = true; //textbox nelze upravit
            OutputTextBox.ReadOnly = true; //OutputTextBox nelze upravit
            TextBox.BackColor = System.Drawing.SystemColors.Window; // textbox barva je bílá
            OutputTextBox.BackColor = System.Drawing.SystemColors.Window; // OutputTextBox barva je bílá




        }

        private void refresh_button_Click(object sender, EventArgs e)
        {
            if (WiewAllData_box.Checked) { TextBox.Text = string.Join("\n", GetDNS(true)); }
            else { TextBox.Text = string.Join("\n", GetDNS(false)); }


        }

        
        private void WiewAllData_box_CheckStateChanged(object sender, EventArgs e)
        {
            if (WiewAllData_box.Checked) { TextBox.Text = string.Join("\n", GetDNS(true));  }
            else { TextBox.Text = string.Join("\n", GetDNS(false)); }
            

            
        }





        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/c ipconfig /flushdns";
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;

                process.StartInfo = startInfo;
                process.Start();


                
                if (WiewAllData_box.Checked) { TextBox.Text = string.Join("\n", GetDNS(true)); }
                else { TextBox.Text = string.Join("\n", GetDNS(false)); }

                OutputTextBox.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + 
                DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString() + "  flushed DNS";
            }
            catch {
                TextBox.Text = string.Join("\n", GetDNS());
                OutputTextBox.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" +
                DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString() + "  didn't flushed DNS";
            }


        }


        private void Search_TextBox_DNS(object sender, EventArgs e)
        {
            //unmark text
            TextBox.SelectionStart = 0;
            TextBox.SelectionLength = TextBox.TextLength;
            TextBox.SelectionBackColor = Color.White;



            string[] words = FindBox.Text.Split(',');    
             foreach(string word in words)    
             {    
                 int startindex = 0;    
                 while(startindex < TextBox.TextLength)    
                 {    
                     int wordstartIndex = TextBox.Find(word, startindex, RichTextBoxFinds.None);    
                     if (wordstartIndex != -1)    
                     {
                        
                        //mark text
                        TextBox.SelectionStart = wordstartIndex;
                        TextBox.SelectionLength = word.Length;
                        TextBox.SelectionBackColor = Color.Red;    
                     }
                    else
                    {
                        break;
                    }
                           
                     startindex += wordstartIndex + word.Length;    
                 }    
             }    
         }    

        

        static List<string> GetDNS(bool vsechnaData = false)
        {
            



            Console.WriteLine();
            List<string> DNSlist = new List<string>();
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd";
            p.StartInfo.Arguments = "/c ipconfig /displaydns";
            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            List<string> lOutput = new List<string>();
            lOutput = output.Split('\n').ToList(); //udělá seznam s output

            if (vsechnaData == false)
            {
                
                lOutput.RemoveRange(0, Math.Min(3, lOutput.Count)); //vymaže první 3 pložky

                //lOutput = lOutput.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                int count = 0;
                foreach (var l in lOutput)
                {
                    //Console.WriteLine(l);
                    if (l.Replace(" ", "").Contains("--"))
                    {
                        //Console.WriteLine(l);
                        int count2 = 0;
                        foreach (var f_ in lOutput)
                        {
                            string f = f_.Replace(" ", "");
                            f = f.Replace("-", "");
                            if (count - 1 == count2)
                            {
                                //Console.WriteLine(f);

                                DNSlist.Add(f);
                            }

                            count2 += 1;
                        }
                    }
                    count += 1;


                }

                DNSlist = DNSlist.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                //Console.WriteLine(string.Join("\n", lOutput));
                return DNSlist;
            }
            return lOutput;

        }

        private void button1_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Search_TextBox_DNS(object sender, KeyEventArgs e)
        {

        }

        private void Search_TextBox_DNS(object sender, KeyPressEventArgs e)
        {

        }

        private void Search_TextBox_DNS(object sender, PreviewKeyDownEventArgs e)
        {

        }
    }


}

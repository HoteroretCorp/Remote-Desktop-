using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace RemoteDesktopFH
{
    public partial class RemoteDesktopFH : Form
    {
        public RemoteDesktopFH()
        {
            InitializeComponent();
        }
        private Dictionary<string, List<string>> Conexiones = new Dictionary<string, List<string>>();
        private List<string> PartidasAbiertas = new List<string>();
        private void RemoteDesktopFH_Load(object sender, EventArgs e)
        {
            XmlDocument objXml = new XmlDocument();
            objXml.Load("Configuracion.xml");
            foreach (XmlNode nodo in objXml.SelectNodes("CONFIGURACION/CONEXION"))
            {
                List<string> temp = new List<string>();
                temp.Add(nodo.SelectSingleNode("@IP")?.InnerText);
                temp.Add(nodo.SelectSingleNode("@PASS")?.InnerText);
                temp.Add(nodo.SelectSingleNode("@USER")?.InnerText);
                Conexiones.Add(nodo.SelectSingleNode("@NOMBRE")?.InnerText, temp);
                listBox1.Items.Add(nodo.SelectSingleNode("@NOMBRE")?.InnerText);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PartidasAbiertas.Contains(listBox1.SelectedItem.ToString()))
            {
                AxMSTSCLib.AxMsTscAxNotSafeForScripting temp = (AxMSTSCLib.AxMsTscAxNotSafeForScripting)tabControl1.TabPages[tabControl1.TabPages.IndexOfKey(listBox1.SelectedItem.ToString())].Controls[0];
                temp.Disconnect();
                tabControl1.TabPages[tabControl1.TabPages.IndexOfKey(listBox1.SelectedItem.ToString())].Controls.Remove(temp);
                tabControl1.TabPages.RemoveByKey(listBox1.SelectedItem.ToString());
                PartidasAbiertas.Remove(listBox1.SelectedItem.ToString());
            }
            else
            {
                List<string> obj = Conexiones.FirstOrDefault(x => x.Key == listBox1.SelectedItem.ToString()).Value;

                CrearComponente(obj[0], obj[1], obj[2]);
                PartidasAbiertas.Add(listBox1.SelectedItem.ToString());
            }
        }

        public void CrearComponente(string ip, string pass, string user)
        {
            try
            {
                TabPage tabPage;
                AxMSTSCLib.AxMsTscAxNotSafeForScripting remoto;
                tabPage = new TabPage();
                remoto = new AxMSTSCLib.AxMsTscAxNotSafeForScripting();
                ComponentResourceManager recursos = new System.ComponentModel.ComponentResourceManager(typeof(RemoteDesktopFH));
                ((System.ComponentModel.ISupportInitialize)(remoto)).BeginInit();
                tabPage.Controls.Add(remoto);
                tabPage.Location = new System.Drawing.Point(4, 22);
                tabPage.Name = listBox1.SelectedItem.ToString();
                tabPage.Padding = new System.Windows.Forms.Padding(3);
                tabPage.Size = new System.Drawing.Size(900, 602);
                tabPage.TabIndex = 2;
                tabPage.Text = listBox1.SelectedItem.ToString();
                tabPage.UseVisualStyleBackColor = true;
                // 
                // RDP
                // 
                remoto.Dock = DockStyle.Fill;
                remoto.Enabled = true;
                remoto.Location = new System.Drawing.Point(3, 3);
                remoto.Name = "RDP";
                remoto.OcxState = ((System.Windows.Forms.AxHost.State)(recursos.GetObject("RDP.OcxState")));
                remoto.Size = new System.Drawing.Size(894, 596);
                remoto.TabIndex = 0;
                this.tabControl1.Controls.Add(tabPage);
                ((System.ComponentModel.ISupportInitialize)(remoto)).EndInit();
                remoto.Server = ip;
                remoto.UserName = user;
                MSTSCLib.IMsTscNonScriptable secured = (MSTSCLib.IMsTscNonScriptable)remoto.GetOcx();
                secured.ClearTextPassword = pass;
                remoto.Connect();
            }
            catch (Exception ex)
            {

            }
        }
    }
}

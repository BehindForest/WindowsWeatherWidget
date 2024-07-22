using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsWeatherWidget
{
    public partial class WeatherWidget : Form
    {
        private int topPanelHeight = 20;
        private Timer timerExpand;
        private Timer timerCollapse;
        private bool expanding;
        private bool collapsing;

        public WeatherWidget()
        {
            InitializeComponent();
            TopPanel.Height = topPanelHeight;
            timerExpand = new Timer();
            timerExpand.Interval = 3; // Czas odświeżania animacji w milisekundach
            timerExpand.Tick += TimerExpand_Tick;

            timerCollapse = new Timer();
            timerCollapse.Interval = 1;
            timerCollapse.Tick += TimerCollapse_Tick;

            CloseButton.Visible = false;
            expanding = false;
            collapsing = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Drag Form
        [DllImport("user32.dll", EntryPoint="ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint="SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void TimerExpand_Tick(object sender, EventArgs e)
        {
            if (TopPanel.Height < topPanelHeight + 25)
            {
                TopPanel.Height += 2; // Zmieniaj wysokość w małych krokach
            }
            else
            {
                timerExpand.Stop();
                CloseButton.Visible = true;
                expanding = false;
            }
        }

        private void TimerCollapse_Tick(object sender, EventArgs e)
        {
            if (TopPanel.Height > topPanelHeight)
            {
                TopPanel.Height -= 2; // Zmieniaj wysokość w małych krokach
            }
            else
            {
                timerCollapse.Stop();
                CloseButton.Visible = false;
                collapsing = false;
            }
        }

        private void TopPanel_MouseEnter(object sender, EventArgs e)
        {
            if (!expanding && !collapsing)
            {
                expanding = true;
                collapsing = false;
                timerExpand.Start();
            }
        }

        private void TopPanel_MouseLeave(object sender, EventArgs e)
        {
            // Sprawdź, czy kursor nie jest nad panelem ani nad przyciskiem
            if (!TopPanel.ClientRectangle.Contains(PointToClient(MousePosition)) && !CloseButton.ClientRectangle.Contains(PointToClient(MousePosition)))
            {
                if (!expanding && !collapsing)
                {
                    collapsing = true;
                    expanding = false;
                    timerCollapse.Start();
                }
            }
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            // Zatrzymanie chowania się panelu gdy kursor jest na przycisku
            if (collapsing)
            {
                timerCollapse.Stop();
                collapsing = false;
            }
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            // Sprawdź, czy kursor nie jest nad panelem ani nad przyciskiem
            if (!TopPanel.ClientRectangle.Contains(PointToClient(MousePosition)) && !CloseButton.ClientRectangle.Contains(PointToClient(MousePosition)))
            {
                if (!expanding && !collapsing)
                {
                    collapsing = true;
                    expanding = false;
                    timerCollapse.Start();
                }
            }
        }

        
    }
}

using System;
using System.Windows.Forms;
using System.Drawing;

namespace MM1QueueSimulator
{
    public partial class Form1 : Form
    {
        private TabControl tabControl;
        private TextBox txtArrivalRate, txtServiceRate;
        private TextBox txtSimArrival, txtSimService, txtSimTime;
        private ListBox lstResults;
        private TextBox txtSimResults;
        private Button btnCalculate, btnRunSim;

        public Form1()
        {
            // Don't call InitializeComponent here - we don't have it
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "MM1 Queue Simulator";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            // Tab 1: Theoretical
            TabPage theoryTab = new TabPage("MM1 Theoretical");
            SetupTheoryTab(theoryTab);

            // Tab 2: Simulation
            TabPage simTab = new TabPage("Simulation");
            SetupSimTab(simTab);

            tabControl.TabPages.Add(theoryTab);
            tabControl.TabPages.Add(simTab);
            this.Controls.Add(tabControl);
        }

        private void SetupTheoryTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(20);

            // Input Group
            GroupBox inputGroup = new GroupBox();
            inputGroup.Text = "Input Parameters (Mean Wise)";
            inputGroup.Size = new Size(400, 200);
            inputGroup.Location = new Point(20, 20);

            Label lblLambda = new Label();
            lblLambda.Text = "Mean Arrival Rate (λ):";
            lblLambda.Location = new Point(20, 40);
            lblLambda.Size = new Size(180, 25);

            txtArrivalRate = new TextBox();
            txtArrivalRate.Text = "2.0";
            txtArrivalRate.Location = new Point(210, 40);
            txtArrivalRate.Size = new Size(100, 25);

            Label lblMu = new Label();
            lblMu.Text = "Mean Service Rate (μ):";
            lblMu.Location = new Point(20, 80);
            lblMu.Size = new Size(180, 25);

            txtServiceRate = new TextBox();
            txtServiceRate.Text = "3.0";
            txtServiceRate.Location = new Point(210, 80);
            txtServiceRate.Size = new Size(100, 25);

            btnCalculate = new Button();
            btnCalculate.Text = "Calculate Performance Measures";
            btnCalculate.Location = new Point(20, 130);
            btnCalculate.Size = new Size(200, 40);
            btnCalculate.BackColor = Color.LightGreen;
            btnCalculate.Click += BtnCalculate_Click;

            inputGroup.Controls.AddRange(new Control[] {
                lblLambda, txtArrivalRate, lblMu, txtServiceRate, btnCalculate
            });

            // Results Group
            GroupBox resultsGroup = new GroupBox();
            resultsGroup.Text = "Performance Measures Results";
            resultsGroup.Size = new Size(420, 500);
            resultsGroup.Location = new Point(440, 20);

            lstResults = new ListBox();
            lstResults.Size = new Size(400, 480);
            lstResults.Location = new Point(10, 25);
            lstResults.Font = new Font("Consolas", 10);

            resultsGroup.Controls.Add(lstResults);

            panel.Controls.Add(inputGroup);
            panel.Controls.Add(resultsGroup);
            tab.Controls.Add(panel);
        }

        private void SetupSimTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(20);

            GroupBox inputGroup = new GroupBox();
            inputGroup.Text = "Simulation Parameters";
            inputGroup.Size = new Size(400, 250);
            inputGroup.Location = new Point(20, 20);

            Label lblLambda = new Label();
            lblLambda.Text = "Mean Arrival Rate (λ):";
            lblLambda.Location = new Point(20, 40);
            lblLambda.Size = new Size(180, 25);

            txtSimArrival = new TextBox();
            txtSimArrival.Text = "2.0";
            txtSimArrival.Location = new Point(210, 40);
            txtSimArrival.Size = new Size(100, 25);

            Label lblMu = new Label();
            lblMu.Text = "Mean Service Rate (μ):";
            lblMu.Location = new Point(20, 80);
            lblMu.Size = new Size(180, 25);

            txtSimService = new TextBox();
            txtSimService.Text = "3.0";
            txtSimService.Location = new Point(210, 80);
            txtSimService.Size = new Size(100, 25);

            Label lblTime = new Label();
            lblTime.Text = "Simulation Time:";
            lblTime.Location = new Point(20, 120);
            lblTime.Size = new Size(180, 25);

            txtSimTime = new TextBox();
            txtSimTime.Text = "10000";
            txtSimTime.Location = new Point(210, 120);
            txtSimTime.Size = new Size(100, 25);

            btnRunSim = new Button();
            btnRunSim.Text = "Run Simulation";
            btnRunSim.Location = new Point(20, 170);
            btnRunSim.Size = new Size(150, 40);
            btnRunSim.BackColor = Color.LightBlue;
            btnRunSim.Click += BtnRunSim_Click;

            inputGroup.Controls.AddRange(new Control[] {
                lblLambda, txtSimArrival, lblMu, txtSimService, lblTime, txtSimTime, btnRunSim
            });

            GroupBox resultsGroup = new GroupBox();
            resultsGroup.Text = "Simulation Results";
            resultsGroup.Size = new Size(420, 500);
            resultsGroup.Location = new Point(440, 20);

            txtSimResults = new TextBox();
            txtSimResults.Multiline = true;
            txtSimResults.Size = new Size(400, 480);
            txtSimResults.Location = new Point(10, 25);
            txtSimResults.Font = new Font("Consolas", 9);
            txtSimResults.ScrollBars = ScrollBars.Vertical;

            resultsGroup.Controls.Add(txtSimResults);

            panel.Controls.Add(inputGroup);
            panel.Controls.Add(resultsGroup);
            tab.Controls.Add(panel);
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                double lambda = double.Parse(txtArrivalRate.Text);
                double mu = double.Parse(txtServiceRate.Text);

                if (lambda <= 0 || mu <= 0)
                {
                    MessageBox.Show("Enter positive values!");
                    return;
                }

                double rho = lambda / mu;

                lstResults.Items.Clear();
                lstResults.Items.Add("==================================================");
                lstResults.Items.Add("     MM1 QUEUE PERFORMANCE MEASURES");
                lstResults.Items.Add("==================================================");
                lstResults.Items.Add("");
                lstResults.Items.Add($"Input: λ = {lambda}, μ = {mu}");
                lstResults.Items.Add("");

                if (rho >= 1)
                {
                    lstResults.Items.Add("⚠️ WARNING: System UNSTABLE! (λ ≥ μ)");
                    lstResults.Items.Add($"ρ = {rho:F4} ≥ 1");
                    return;
                }

                double Lq = (rho * rho) / (1 - rho);
                double L = Lq + rho;
                double Wq = Lq / lambda;
                double W = Wq + (1 / mu);
                double P0 = 1 - rho;

                lstResults.Items.Add($"Server Utilization (ρ)     = {rho:F6}");
                lstResults.Items.Add($"P0 (System Empty)          = {P0:F6}");
                lstResults.Items.Add($"Lq (Avg Queue Length)      = {Lq:F6}");
                lstResults.Items.Add($"L (Avg System Size)        = {L:F6}");
                lstResults.Items.Add($"Wq (Avg Wait in Queue)     = {Wq:F6}");
                lstResults.Items.Add($"W (Avg Time in System)     = {W:F6}");
                lstResults.Items.Add("");
                lstResults.Items.Add("✅ System is STABLE");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnRunSim_Click(object sender, EventArgs e)
        {
            try
            {
                double lambda = double.Parse(txtSimArrival.Text);
                double mu = double.Parse(txtSimService.Text);
                double simTime = double.Parse(txtSimTime.Text);

                if (lambda <= 0 || mu <= 0 || simTime <= 0)
                {
                    MessageBox.Show("Enter positive values!");
                    return;
                }

                if (lambda >= mu)
                {
                    txtSimResults.Text = "⚠️ System unstable! λ must be < μ";
                    return;
                }

                // Run simulation
                Random rand = new Random();
                double currentTime = 0;
                double nextArrival = -Math.Log(1 - rand.NextDouble()) / lambda;
                double nextDeparture = double.MaxValue;
                int queueLength = 0;
                int totalCustomers = 0;
                int customersServed = 0;
                double totalWaitTime = 0;
                double areaUnderQueue = 0;
                double lastEventTime = 0;
                bool serverBusy = false;
                double serverStartTime = 0;
                int maxQueueLength = 0;

                while (currentTime < simTime)
                {
                    if (nextArrival < nextDeparture)
                    {
                        currentTime = nextArrival;
                        totalCustomers++;

                        if (!serverBusy)
                        {
                            serverBusy = true;
                            double serviceTime = -Math.Log(1 - rand.NextDouble()) / mu;
                            nextDeparture = currentTime + serviceTime;
                            serverStartTime = currentTime;
                        }
                        else
                        {
                            queueLength++;
                            areaUnderQueue += queueLength * (currentTime - lastEventTime);
                            if (queueLength > maxQueueLength) maxQueueLength = queueLength;
                        }

                        nextArrival = currentTime + (-Math.Log(1 - rand.NextDouble()) / lambda);
                        lastEventTime = currentTime;
                    }
                    else
                    {
                        currentTime = nextDeparture;
                        customersServed++;

                        if (queueLength > 0)
                        {
                            queueLength--;
                            double serviceTime = -Math.Log(1 - rand.NextDouble()) / mu;
                            nextDeparture = currentTime + serviceTime;
                            totalWaitTime += (currentTime - serverStartTime);
                            serverStartTime = currentTime;
                            areaUnderQueue += queueLength * (currentTime - lastEventTime);
                        }
                        else
                        {
                            serverBusy = false;
                            nextDeparture = double.MaxValue;
                        }
                        lastEventTime = currentTime;
                    }
                }

                double avgQueueLength = areaUnderQueue / currentTime;
                double utilization = (customersServed / mu) / currentTime;
                double avgWaitTime = customersServed > 0 ? totalWaitTime / customersServed : 0;
                double rho = lambda / mu;
                double theoreticalLq = (rho * rho) / (1 - rho);
                double theoreticalWq = theoreticalLq / lambda;

                txtSimResults.Text = "";
                txtSimResults.AppendText("╔════════════════════════════════════════════════╗\r\n");
                txtSimResults.AppendText("║           SIMULATION RESULTS                    ║\r\n");
                txtSimResults.AppendText("╚════════════════════════════════════════════════╝\r\n\r\n");
                txtSimResults.AppendText($"Total Customers Arrived: {totalCustomers}\r\n");
                txtSimResults.AppendText($"Total Customers Served: {customersServed}\r\n");
                txtSimResults.AppendText($"Server Utilization: {utilization:F6}\r\n");
                txtSimResults.AppendText($"Max Queue Length: {maxQueueLength}\r\n");
                txtSimResults.AppendText($"Avg Queue Length (Sim): {avgQueueLength:F6}\r\n");
                txtSimResults.AppendText($"Avg Wait Time (Sim): {avgWaitTime:F6}\r\n");
                txtSimResults.AppendText($"\r\nTheoretical Lq: {theoreticalLq:F6}\r\n");
                txtSimResults.AppendText($"Theoretical Wq: {theoreticalWq:F6}\r\n");
                txtSimResults.AppendText($"\r\nError in Lq: {Math.Abs(avgQueueLength - theoreticalLq) / theoreticalLq * 100:F2}%\r\n");
            }
            catch (Exception ex)
            {
                txtSimResults.Text = $"Error: {ex.Message}";
            }
        }
    }
}

using System.Windows.Forms;
using System.Drawing;

namespace FinalProject_IliaB
{
    public partial class Form1 : Form
    {
        
        public class BudgetEntry
        {
            public string CategoryName { get; set; }
            public decimal Amount { get; set; }
            public DateTime Date { get; set; }
            public string Type { get; set; }

            public BudgetEntry(string categoryName, decimal amount, DateTime date, string type)
            {
                CategoryName = categoryName;
                Amount = amount;
                Date = date;
                Type = type;
            }
        }
        private List<BudgetEntry> budgetEntries = new List<BudgetEntry>();
        
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = budgetEntries;
            label3.Visible= false;
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            BudgetEntry budgetEntry = new BudgetEntry("Food", 50.0m, DateTime.Now, "Outcome");
            budgetEntry.CategoryName = textBox2.Text;
            budgetEntry.Amount = Convert.ToDecimal(textBox1.Text);
            budgetEntry.Date = dateTimePicker1.Value;
            if (radioButton1.Checked)
            {
               budgetEntry.Type = "Income";
            }
            else
            {
               budgetEntry.Type = "Outcome";
            }
            // Add the entry to the list
            budgetEntries.Add(budgetEntry);

            // Clear the input fields
            textBox1.Clear();
            textBox2.Clear();
            // Rebind the budgetEntries list to the dataGridView's DataSource
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = budgetEntries;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Type"].Value.ToString() == "Income")
                {
                    row.DefaultCellStyle.ForeColor = Color.Green;
                }
                if (row.Cells["Type"].Value.ToString() == "Outcome")
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            // Count the total Income
            decimal totalIncome = 0;
            foreach (BudgetEntry entry in budgetEntries)
            {
                if (entry.Type == "Income")
                {
                    totalIncome += entry.Amount;
                }
            }
            // Count the total Outcome
            decimal totalOutcome = 0;
            foreach (BudgetEntry entry in budgetEntries)
            {
                if (entry.Type == "Outcome")
                {
                    totalOutcome += entry.Amount;
                }
            }
            decimal totalAmount = totalIncome - totalOutcome;
            // Display all the totals
            incomeLabel.Text = "Income: " + totalIncome.ToString("ˆ#,##0.00");
            outcomeLabel.Text = "Outcome: " + totalOutcome.ToString("ˆ#,##0.00");
            if (totalAmount < 0)
            {
                labelBalance.ForeColor = Color.Red;
            }
            else
            {
                labelBalance.ForeColor = Color.Green;
            }
            labelBalance.Text = "Balance: " + totalAmount.ToString("ˆ#,##0.00");

        }

        private void DisplayReport(Dictionary<string, decimal> reportData)
        {
            // Calculate the total amount spent
            decimal totalAmount = reportData.Values.Sum();

            // Create a new Bitmap to draw the report on
            Bitmap bmp = new Bitmap(200, 200);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Set the starting angle to 0
                float startingAngle = 0;

                int index = 0;

                // Iterate through the dictionary and draw a slice for each category
                foreach (KeyValuePair<string, decimal> entry in reportData)
                {
                    // Calculate the percentage of the total amount spent for this category
                    float percentage = (float)(entry.Value / totalAmount);

                    // Calculate the sweep angle for this slice
                    float sweepAngle = 360 * percentage;

                    // Set the color for this slice
                    List<Color> colors = new List<Color>
                    {
                        Color.Red,
                        Color.Orange,
                        Color.Yellow,
                        Color.Green,
                        Color.Blue,
                        Color.Indigo,
                        Color.Violet
                    };
                    if (index == 7)
                    {
                        index = 0;
                    }

                    Brush brush = new SolidBrush(colors[index]);
                    

                    // Draw the slice
                    g.FillPie(brush, 50, 50, 100, 100, startingAngle, sweepAngle);

                    // Create a label for this slice and display it next to the slice
                    Label label = new Label();
                    label.Text = entry.Key;
                    label.ForeColor = colors[index];
                    label.BackColor = Color.LightGray;
                    label.Location = new Point(947, (int)(513 + 20 * index)); //947; 468
                    this.Controls.Add(label);

                    // Set the starting angle for the next slice
                    startingAngle += sweepAngle;
                    index++;
                }
            }

            // Display the report in a pictureBox
            pictureBox2.Image = bmp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary<string, decimal> categories = new Dictionary<string, decimal>();

            foreach (var entry in budgetEntries)
            {
                if (entry.Type == "Outcome")
                {
                    if (categories.ContainsKey(entry.CategoryName))
                    {
                        categories[entry.CategoryName] += entry.Amount;
                    }
                    else
                    {
                        categories[entry.CategoryName] = entry.Amount;
                    }
                }
            }
            DisplayReport(categories);

        }
    }
}
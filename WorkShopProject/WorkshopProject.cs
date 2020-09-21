using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class WorkshopProject : Form
    {
        public WorkshopProject()
        {
            InitializeComponent();
            Workshop();
        }

        private void Workshop()
        {
            NpgsqlConnection connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=workshopProject; User Id = postgres; Password=busra123;");
            DataSet dataset = new DataSet();
            DataSet dataset1 = new DataSet();
            DataSet dataset2 = new DataSet();
            DataSet dataset3 = new DataSet();
            DataSet dataset4 = new DataSet();
            DataSet dataset5 = new DataSet();
            try
            {
                connection.Open();
            }
            catch(Exception ex) 
            {
                throw;
            }
            string returning = "SELECT p.name as ProductNames, count(s.IdProduct) as productcount FROM public.\"product\" as p right join public.\"saleproductuser\" as s " +
                "on s.IdProduct = p.Id where p.cost < 0 group by p.name order by count(s.IdProduct) desc limit 5";// Top 5 products that most returning
            NpgsqlDataAdapter addReturn = new NpgsqlDataAdapter(returning, connection);
            addReturn.Fill(dataset);

            dataGridView1.DataSource = dataset.Tables[0];
         
            string sold = "SELECT p.name as ProductNames,count(s.IdProduct) as productcount FROM public.\"product\" as p right join public.\"saleproductuser\" as s " +
                "on s.IdProduct = p.Id where p.cost > 0 group by p.name order by count(s.IdProduct) desc limit 5 ";// Top 5 products that most sold
            NpgsqlDataAdapter addSold = new NpgsqlDataAdapter(sold, connection);
            addSold.Fill(dataset1);
            dataGridView2.DataSource = dataset1.Tables[0];


            string totalcostofsold = " Select sum(totalcost) as Totalprice from public.\"sale\" where Date between timestamp '2020-03-30 00:00:00'::date - extract(dow from timestamp '2020-03-30 00:00:00')::integer - 7" +
                "and timestamp '2020-03-30 00:00:00'::date - extract(DOW from timestamp '2020-03-30 00:00:00')::integer";//Total Cost Of sold in last week

            NpgsqlDataAdapter addtotalcostofsold = new NpgsqlDataAdapter(totalcostofsold, connection);
            addtotalcostofsold.Fill(dataset2);
            dataGridView3.DataSource = dataset2.Tables[0];

            string totalcostofreturning = " Select sum(p.cost) as Totalprice from public.\"saleproductuser\" as s left join public.\"product\" as p " +
                "on p.Id = s.IdProduct left join public.\"sale\" as sa on sa.Id = s.IdSale " +
                "where p.cost<0 and Date between timestamp '2020-03-30 00:00:00'::date - extract(dow from timestamp '2020-03-30 00:00:00')::integer - 7 " +
                "and timestamp '2020-03-30 00:00:00'::date - extract(DOW from timestamp '2020-03-30 00:00:00')::integer";//Total Cost Of Returning in Last Week

            NpgsqlDataAdapter addtotalcostofreturning = new NpgsqlDataAdapter(totalcostofreturning, connection);
            addtotalcostofreturning.Fill(dataset3);
            dataGridView4.DataSource = dataset3.Tables[0];

            string mostemployee = "select concat(u.name,' ',u.lastname) as Employee ,r.Name as RoleName from public.\"users\" as u right join public.\"saleproductuser\" as s " +
                "on s.IdUser = u.IdUser left join public.\"product\" as p on p.Id = s.IdProduct left join public.\"userrole\" as ur " +
                "on ur.UserId = u.IdUser left join public.\"role\" as r " +
                "on r.Id = ur.RoleId where p.cost < 0 group by u.IdUser,r.Name order by count(u.IdUser) desc limit 3; ";//Most Employee with re-turned products

            NpgsqlDataAdapter addmostemployee = new NpgsqlDataAdapter(mostemployee, connection);
            addmostemployee.Fill(dataset4);
            dataGridView5.DataSource = dataset4.Tables[0];

            string mostrole = "select r.Name as RoleName,count(r.name) as rolecount from public.\"users\" as u right join public.\"saleproductuser\" as s " +
                "on s.IdUser = u.IdUser left join public.\"product\" as p on p.Id = s.IdProduct left join public.\"userrole\" as ur " +
                "on ur.UserId = u.IdUser left join public.\"role\" as r " +
                "on r.Id = ur.RoleId where p.cost < 0 group by r.Id,r.Name order by count(r.Id) desc limit 3; ";

            NpgsqlDataAdapter addmostrole = new NpgsqlDataAdapter(mostrole, connection);
            addmostrole.Fill(dataset5);
            dataGridView6.DataSource = dataset5.Tables[0];
            connection.Close();

        }

      
    }
}

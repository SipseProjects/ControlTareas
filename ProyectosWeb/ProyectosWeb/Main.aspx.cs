using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace ProyectosWeb
{
    public partial class Main : System.Web.UI.Page
    {
        private SqlConnection conn = new SqlConnection("Data Source=(local);Integrated Security=true;Initial Catalog=ProyectosGestion");
        String PageIndex,PrevIndex;
        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownListDep.Visible = false;
            if (IsPostBack&&ViewState["Index"]!=null) {
                PageIndex = ViewState["Index"].ToString();
                /**/
            }
            DropDownListDep.Visible = false;
            DropDownListHoras.Visible = false;
            LabelHrs.Visible = false;
            if (IsPostBack && ViewState["PrevIndex"] != null)
            {
                PrevIndex = ViewState["PrevIndex"].ToString();
                /**/
            }

        }

        protected void ProyectoOnClick(object sender, EventArgs e)
        {
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Proyectos";
            LabelNav.Text =PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "IDProyectos";
            ((BoundField)GridView1.Columns[1]).DataField = "ClaveProyectos";
            gvbind();
            LabelDependencia.Text = "";
        }

        protected void RequerimientoOnClick(object sender, EventArgs e)
        {
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Requerimientos";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "ID"+PageIndex;
            ((BoundField)GridView1.Columns[1]).DataField = "Clave"+PageIndex;
            DropDownListDep.Visible = true;
            LabelDependencia.Text = "Proyectos";
            gvbind();
            DropDownBindProyectos();
        }

        protected void CasosUsoOnClick(object sender, EventArgs e)
        {
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "CasosUso";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "ID" + PageIndex;
            ((BoundField)GridView1.Columns[1]).DataField = "Clave" + PageIndex;
            DropDownListDep.Visible = true;
            LabelDependencia.Text = "Requerimientos";
            gvbind();
            DropDownBindRequerimientos();
        }

        protected void ComponenteOnClick(object sender, EventArgs e)
        {
            PrevIndex = PageIndex;
            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Componentes";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "ID" + PageIndex;
            ((BoundField)GridView1.Columns[1]).DataField = "Clave" + PageIndex;
            DropDownListDep.Visible = true;
            LabelDependencia.Text = "Casos de uso";
            gvbind();
            DropDownBindCasosUso();
        }

        protected void TareaOnClick(object sender, EventArgs e)
        {

            ViewState["PrevIndex"] = PrevIndex;
            PageIndex = "Tareas";
            LabelNav.Text = PageIndex;
            ViewState["Index"] = PageIndex;
            ((BoundField)GridView1.Columns[0]).DataField = "ID" + PageIndex;
            ((BoundField)GridView1.Columns[1]).DataField = "Clave" + PageIndex;
            DropDownListDep.Visible = true;
            LabelDependencia.Text = "Componentes";
            gvbind();
            DropDownBindComponentes();
            LabelHrs.Visible = true;
            DropDownListHoras.Visible = true;
        }





        protected void DropDownBindProyectos() {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDProyectos,ClaveProyectos from proyectos where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DropDownListDep.DataSource = ds;
            DropDownListDep.DataValueField = "IDProyectos";
            DropDownListDep.DataTextField = "ClaveProyectos";
            DropDownListDep.DataBind();
        
        }

        protected void DropDownBindRequerimientos()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDRequerimientos,ClaveRequerimientos from Requerimientos where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DropDownListDep.DataSource = ds;
            DropDownListDep.DataValueField = "IDRequerimientos";
            DropDownListDep.DataTextField = "ClaveRequerimientos";
            DropDownListDep.DataBind();
            //Ya no revisamos nada nosotros, entocnes ya quedo confirmado 4 30
        }

        protected void DropDownBindCasosUso()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDCasosUso,ClaveCasosUso from CasosUso where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DropDownListDep.DataSource = ds;
            DropDownListDep.DataValueField = "IDCasosUso";
            DropDownListDep.DataTextField = "ClaveCasosUso";
            DropDownListDep.DataBind();

        }
 
        protected void DropDownBindComponentes()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDComponentes,ClaveComponentes from Componentes where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DropDownListDep.DataSource = ds;
            DropDownListDep.DataValueField = "IDComponentes";
            DropDownListDep.DataTextField = "ClaveComponentes";
            DropDownListDep.DataBind();

        }

        protected void DropDownBindUsuario()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select IDUsuario,Usuario from Usuarios where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            DropDownListDep.DataSource = ds;
            DropDownListDep.DataValueField = "IDComponentes";
            DropDownListDep.DataTextField = "ClaveComponentes";
            DropDownListDep.DataBind();

        }


        protected void gvbind()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select * from "+PageIndex+" where Estado=0", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                GridView1.DataSource = ds;
                GridView1.DataBind();
                int columncount = GridView1.Rows[0].Cells.Count;
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[0].ColumnSpan = columncount;
                GridView1.Rows[0].Cells[0].Text = "Sin registros";
            }

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            gvbind();
            ViewState["Index"]=PageIndex;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ViewState["Index"] = PageIndex;
            gvbind();

        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            ViewState["Index"] = PageIndex;
            gvbind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            TableCell cell = GridView1.Rows[e.RowIndex].Cells[0];
            conn.Open();
            SqlCommand cmd = new SqlCommand("update "+PageIndex+" set Estado='1' where ID"+PageIndex+"="+Convert.ToInt32(cell.Text), conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            ViewState["Index"] = PageIndex;
            gvbind();

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox Clave,Nombre, Desc, Cliente, FechaInicio, FechaFinEstimada, FechaFinReal, Tecnologias;
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            TableCell ID = GridView1.Rows[e.RowIndex].Cells[0];
            Clave = (TextBox)row.Cells[1].Controls[0];
            Nombre = (TextBox)row.Cells[2].Controls[0];
            Desc = (TextBox)row.Cells[3].Controls[0];
            Cliente = (TextBox)row.Cells[4].Controls[0];
            FechaInicio = (TextBox)row.Cells[6].Controls[0];
            FechaFinEstimada = (TextBox)row.Cells[7].Controls[0];
            FechaFinReal = (TextBox)row.Cells[8].Controls[0];
            Tecnologias = (TextBox)row.Cells[9].Controls[0];
            GridView1.EditIndex = -1;
            conn.Open();
            SqlCommand cmd = new SqlCommand("update " + PageIndex + " set Clave" + PageIndex + "='" + Clave.Text + "', Nombre = '" + Nombre.Text + "', Descripcion='" + Desc.Text.ToString() + "', Cliente='" + Cliente.Text + "', FechaInicio=Convert(datetime,'" + FechaInicio.Text + "',111) , FechaFinEstimada=Convert(datetime,'" + FechaFinEstimada.Text + "',111) , FechaFinReal=Convert(datetime,'" + FechaFinReal.Text + "',111) , Tecnologias='"+Tecnologias.Text+"' where ID"+PageIndex+"="+ID.Text+"  ",conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            gvbind();



        }

        protected void Button1_Click(object sender, EventArgs e)
        {


            if (PageIndex != null && PageIndex.Equals("Tareas")) {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into " + PageIndex + " values("+DropDownListDep.SelectedValue.ToString()+",'" + TextBoxClave.Text + "' , '" + TextBoxNombre.Text + "' , '" + TextBoxDescripcion.Text + "' , '" + TextBoxCliente.Text + "' , GETDATE() , CONVERT(datetime,'" + TextBoxFechaInicio.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinEst.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinReal.Text + "',111) ,null,null, '04' , 0  )  ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();


            
            }else 
            if (PageIndex!=null&&PageIndex.Equals("Proyectos"))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into " + PageIndex + " values('" + TextBoxClave.Text + "' , '" + TextBoxNombre.Text + "' , '" + TextBoxDescripcion.Text + "' , '" + TextBoxCliente.Text + "' , GETDATE() , CONVERT(datetime,'" + TextBoxFechaInicio.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinEst.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinReal.Text + "',111) , '04' , 0  )  ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            else {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into " + PageIndex + " values(  "+DropDownListDep.SelectedValue.ToString()+", '" + TextBoxClave.Text + "' , '" + TextBoxNombre.Text + "' , '" + TextBoxDescripcion.Text + "' , '" + TextBoxCliente.Text + "' , GETDATE() , CONVERT(datetime,'" + TextBoxFechaInicio.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinEst.Text + "',111) , CONVERT(datetime,'" + TextBoxFechaFinReal.Text + "',111) , '04' , 0  )  ";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                conn.Open();
                cmd.ExecuteNonQuery();
                 
            }
            
            conn.Close();
            ViewState["Index"] = PageIndex;
            gvbind();

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from "+PageIndex+" where Clave"+PageIndex+" ='"+TextBoxClave.Text+"'    ", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            else
            {
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                GridView1.DataSource = ds;
                GridView1.DataBind();
                int columncount = GridView1.Rows[0].Cells.Count;
                GridView1.Rows[0].Cells.Clear();
                GridView1.Rows[0].Cells.Add(new TableCell());
                GridView1.Rows[0].Cells[0].ColumnSpan = columncount;
                GridView1.Rows[0].Cells[0].Text = "Sin resultados de busqueda";
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            gvbind();
        }







        






    }
}
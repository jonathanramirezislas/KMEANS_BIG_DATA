using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;


namespace KMEANS_BIG_DATA
{
    public partial class Form1 : Form
    {

        //VARIABLES GLOBALES

        public long NumClusters = 0;//se cambio valor a long

        public int NumElementos = 0;
        public int NumDatos = 0;
        double distancia = 0;
        bool cambios = false;

        bool kmeans = false;

       public int numerospositivos;
       public  int numerosnegativos;
        public int numerosinter;

        public Form1()
        {
            InitializeComponent();
            dataGridView2.Columns.Add("ColunmName", "CLÚSTERS");
            dataGridView2.Columns.Add("ColunmName", "ELEMENTOS");
            dataGridView2.Columns.Add("ColunmName", "CENTROIDES");

            dataGridViewCLASTERINICIAL.Columns.Add("ColunmName", "CLÚSTERS");
            dataGridViewCLASTERINICIAL.Columns.Add("ColunmName", "ELEMENTOS");
            dataGridViewCLASTERINICIAL.Columns.Add("ColunmName", "CENTROIDES");


            dataGridViewOPERACIONES.Columns.Add("ColunmName", "CLÚSTERS");
            dataGridViewOPERACIONES.Columns.Add("ColunmName", "ESPECIE");
            dataGridViewOPERACIONES.Columns.Add("ColunmName", "INTERACCION");
            dataGridViewOPERACIONES.Columns.Add("ColunmName", "CLUSTER-ESPECIE");
            dataGridViewOPERACIONES.Columns.Add("ColunmName", "ESPECIE-CLUSTER");
            dataGridViewOPERACIONES.Columns.Add("ColunmName", "DISTANCIA");

            dataGridViewespecie.Columns.Add("ColunmName", "ESPECIE");
            dataGridViewespecie.Columns.Add("ColunmName", "ELEMENTOS");

          
       


            this.chart1.ChartAreas[0].AxisX.LineColor = Color.Red;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Black;
            this.chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;

            this.chart1.ChartAreas[0].AxisY.LineColor = Color.Red;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Black;
            this.chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonAbrir_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            CargarDatos();

            No_Vaciando_Toddo_El_CSV();
        }
 private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridViewOPERACIONES.Rows.Clear();
            dataGridViewOPERACIONES.Refresh();
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            dataGridViewCLASTERINICIAL.Rows.Clear();
            dataGridViewCLASTERINICIAL.Refresh();
            Form1 llamar = new Form1();
    ///        llamar.NumDatos = 1;
     //       llamar.NumElementos = 1;

           
            
            //  double[,] clusters = new double[,] {{ 4, 3},    { 5,9}};
            double[,] clusters;

            double[,] datos;



            //PAAMOS DATAGRIDVIEW A MATRIZ
            llamar.NumElementos = (int)dataGridViewdatos.Rows.Count - 1;
            llamar.NumDatos = dataGridViewdatos.ColumnCount - 1;
            datos = new double[llamar.NumElementos, llamar.NumDatos];

            for (int fila = 0; fila < llamar.NumElementos; fila++)
            {
                for (int columna = 0; columna < llamar.NumDatos; columna++)
                {
                    //MessageBox.Show(Convert.ToString(dataGridView1.Rows[fila].Cells[columna + 1].Value));
                    datos[fila, columna] = Convert.ToDouble(dataGridViewdatos.Rows[fila].Cells[columna + 1].Value);
                }
            }



            if (radioButtonEstablecidos.Checked)//establecidos
            {
                llamar.NumClusters = 3;
               
                llamar.NumDatos = 4;
                llamar.kmeans = true;

    

                clusters = new double[,] {
                             { 5.5, 2.3, 4.0, 1.3},//VERSICOLOR
                   { 4.6, 3.6, 1.0, 0.2 }, //SETOSA
                  { 7.7, 3.8 , 6.7, 2.2}};//VIRGINICA

                bool csv = false;
                mostrandoClustersIniciales(clusters, llamar.NumElementos, llamar.NumClusters, llamar.NumDatos, llamar.kmeans,csv);


            }
            else if (radioButtonAleatorios.Checked)//aleatorios
            {
                llamar.NumClusters = (long)numericUpDownClsuters.Value;
                llamar.NumDatos = dataGridViewdatos.ColumnCount - 1;
                llamar.kmeans = false;
                


                //Se crean numeros aleatorios
                clusters = new double[llamar.NumClusters, llamar.NumDatos];
                Random random = new Random();
                double[] rangosdatos = new double[llamar.NumDatos];
            

                rangosdatos = obtenerrangosparadatos(datos, llamar.NumElementos, llamar.NumDatos);

                for (int fila = 0; fila < llamar.NumClusters; fila++)
                {
                    for (int columna = 0; columna < llamar.NumDatos; columna++)
                    { clusters[fila, columna] = Convert.ToDouble(random.NextDouble() * (rangosdatos[columna] - 0) + 0); }
                }
                bool csv = false;
                
                mostrandoClustersIniciales(clusters, llamar.NumElementos, llamar.NumClusters, llamar.NumDatos, llamar.kmeans,csv);


            }
            else if (radioButtonCarga.Checked)//carga por csv
            {
                llamar.NumClusters = (long)numericUpDownClsuters.Value;
                llamar.NumDatos = dataGridViewdatos.ColumnCount - 1;
                
                llamar.kmeans = false;


                CargarClusters();
                int numclusters = 0;
                numclusters = (int)dataGridViewCLUSTERSCSV.Rows.Count - 1;
               
                llamar.NumClusters = numclusters;//estableciendo el numeor de clusters de cvs
                clusters = new double[numclusters, llamar.NumDatos];

              

               
            
                //llenamos los clusters
                for (int fila = 0; fila < numclusters; fila++)
                {
                    for (int columna = 0; columna < llamar.NumDatos; columna++)
                    {
                        clusters[fila, columna] = Convert.ToDouble(dataGridViewCLUSTERSCSV.Rows[fila].Cells[columna + 1].Value);
                    }
                }
               // MessageBox.Show(Convert.ToString(clusters[0, 0]));

                bool csv = true;
                mostrandoClustersIniciales(clusters, llamar.NumElementos, llamar.NumClusters, llamar.NumDatos, llamar.kmeans, csv);
               
               
            }
            else
            {
                clusters = new double[,] {
                             { 5.5, 2.3, 4.0, 1.3},//VERSICOLOR
                   { 4.6, 3.6, 1.0, 0.2 }, //SETOSA
                  { 7.7, 3.8 , 6.7, 2.2}};//VIRGINICA

            }
           



            double[,] distancias = new double[llamar.NumElementos, llamar.NumClusters];
            double[,] Elementos = new double[llamar.NumClusters, llamar.NumElementos];
            double[,] NuevosElementos = new double[llamar.NumClusters, llamar.NumElementos];

   
            
            int numerointeraciones = 0;

            if (llamar.NumClusters <= llamar.NumElementos)
            {

                do
                {
                    //calculos
                    Console.WriteLine("Calculando .....");
                    distancias = llamar.obtendiendodistancias(datos, clusters, distancias);
                    Elementos = llamar.asginarelementosaclusters(Elementos, distancias);
                    clusters = llamar.nuevosclusters(Elementos, datos);

                    numerointeraciones++;
                    Console.WriteLine("\n\n\n NUMERO DE INTERACIONES " + numerointeraciones + "  \n\n\n");

                } while (llamar.cambios == true);


               mostrandoResultados(Elementos, clusters, llamar.NumElementos, llamar.NumClusters, llamar.NumDatos,llamar.kmeans);
                llenardatagridespecies();
                if (llamar.NumDatos == 2) { graficar2D(clusters, (int)llamar.NumClusters); }
            }
            else
            {
                MessageBox.Show("NO SE PERMITE TENER MÁS NÚMEROS DE CLÚSTERS QUE LA CANTIDAD DE ELEMENTOS ");
            }

            interseccion();

        }

        public void graficar2D(double[,] clusters, int numclus)
        {
            try
            {
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                int rowcount = dataGridViewdatos.RowCount - 1;
                double c1 = 0, c2 = 0;

              
              
                for (int i = 0; i < numclus; i++)
                {         
                    
                        c1 = Convert.ToDouble(clusters[i, 0]);
                        c2 = Convert.ToDouble(clusters[i, 1]);
                   
                    if (!Double.IsNaN(c1))
                    {
                        
                        chart1.Series["Clusters"].Points.AddXY(c1, c2);
                        chart1.Series["Clusters"].Points[i].Color = Color.Blue;
                    }
                    
                        
                }

                
                for (int i = 0; i < rowcount; i++)
                {
                    c1 = Convert.ToDouble(dataGridViewdatos.Rows[i].Cells[1].Value);
                    c2 = Convert.ToDouble(dataGridViewdatos.Rows[i].Cells[2].Value);
                   
                        chart1.Series["Valores"].Points.AddXY(c1, c2);
                    chart1.Series["Valores"].Points[i].Color = Color.Red;
                }

            }catch(Exception e)
            {
                MessageBox.Show(""+e);
            }
        }

        private void CargarDatos()
        {

            String ruta = "";

            OpenFileDialog openfile1 = new OpenFileDialog();
            openfile1.Filter = "Excel Files |*.csv";
            openfile1.Title = "Seleccione un archivo CSV";
            if (openfile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openfile1.FileName.Equals("") == false)
                {
                    ruta = openfile1.FileName;
                }
            }


            if (String.IsNullOrEmpty(ruta))
            {
                MessageBox.Show("Seleccione un archivo CSV por favor");
            }
            else
            {
                try
                {
                    DataTable dt = new DataTable();

                    string[] lines = System.IO.File.ReadAllLines(ruta);
                    if (lines.Length > 0)
                    {
                        //primera linea para crear  header
                        string firstLine = lines[0];
                        string[] headerLabels = firstLine.Split(',');
                        foreach (string headerWord in headerLabels)
                        {
                            dt.Columns.Add(new DataColumn(headerWord));
                        }
                        //Para los datos
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] dataWords = lines[i].Split(',');
                            DataRow dr = dt.NewRow();
                            int columnIndex = 0;
                            foreach (string headerWord in headerLabels)
                            {
                                dr[headerWord] = dataWords[columnIndex++];
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Se esta utilizando archivo ");
                }


            }

        }


        private void CargarClusters()
        {

            String ruta = "";

            OpenFileDialog openfile1 = new OpenFileDialog();
            openfile1.Filter = "Excel Files |*.csv";
            openfile1.Title = "Seleccione un archivo CSV";
            if (openfile1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openfile1.FileName.Equals("") == false)
                {
                    ruta = openfile1.FileName;
                }
            }


            if (String.IsNullOrEmpty(ruta))
            {
                MessageBox.Show("Seleccione un archivo CSV por favor");
            }
            else
            {
                try
                {
                    DataTable dt = new DataTable();

                    string[] lines = System.IO.File.ReadAllLines(ruta);
                    if (lines.Length > 0)
                    {
                        //primera linea para crear  header
                        string firstLine = lines[0];
                        string[] headerLabels = firstLine.Split(',');
                        foreach (string headerWord in headerLabels)
                        {
                            dt.Columns.Add(new DataColumn(headerWord));
                        }
                        //Para los datos
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] dataWords = lines[i].Split(',');
                            DataRow dr = dt.NewRow();
                            int columnIndex = 0;
                            foreach (string headerWord in headerLabels)
                            {
                                dr[headerWord] = dataWords[columnIndex++];
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dataGridViewCLUSTERSCSV.DataSource = dt;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Se esta utilizando archivo ");
                }


            }

        }




        //primer paso
        public double[,] obtendiendodistancias(double[,] arrdatos, double[,] arrclusters, double[,] arrdistancias)
        {
        //  MessageBox.Show("NumClusters" + NumClusters + "\nNumElementos" +NumElementos + "\nNumDatos" + NumDatos);
            try
            {
                /* OBTENIENDO DISTANCIAS  */
                for (int yd = 0; yd < NumElementos; yd++)//Numelementos
                {
                    for (int yc = 0; yc < NumClusters; yc++) //numero de datos
                    {
                        for (int xd = 0; xd < NumDatos; xd++)//Numero de clusters
                        {
                            distancia += Math.Round((Math.Pow((arrdatos[yd, xd] - arrclusters[yc, xd]), 2)));
                        }
                        distancia = Math.Sqrt(distancia);
                        arrdistancias[yd, yc] = distancia;
                        //   MessageBox.Show( Convert.ToString( Math.Round(distancia, 2)));
                        distancia = 0;
                    }
                    Console.WriteLine();
                }

            }catch(Exception e)
            {
                MessageBox.Show("Error en el primer paso"+e);
            }
            return arrdistancias;
        }

        //paso 2
        public double[,] asginarelementosaclusters(double[,] arrElemntos, double[,] arrdistancias)
        {
            try {
                double[,] NuevosElementos = new double[NumClusters, NumElementos];
                cambios = false;
                double minimo = 0;
                int colummMinimo = 0;
                for (int i = 0; i < NumElementos; i++)
                {
                    minimo = arrdistancias[i, 0];
                    colummMinimo = 0;
                    for (int j = 0; j < NumClusters; j++)
                    {
                        if (arrdistancias[i, j] < minimo)
                        {
                            minimo = arrdistancias[i, j]; /* actualizacion */
                            colummMinimo = j;  }         }
                    NuevosElementos[colummMinimo, i] = 1;              }
                if (CompararOldElementosConNuevosElementos(NuevosElementos, arrElemntos)) {
                    Array.Copy(NuevosElementos, 0, arrElemntos, 0, NuevosElementos.Length);
                    Array.Clear(NuevosElementos, 0, NuevosElementos.Length);
                }

                Console.WriteLine("existieron cambios " + cambios);
            }
            catch (Exception)
            {
            }
            return arrElemntos;
            }

        //paso 3
        public double[,] nuevosclusters(double[,] arrElemntos, double[,] arrdatos)
        {
      double Promedio = 0; int Numerodedatos = 0; double[,] clusters = new double[NumElementos, NumDatos];
            try  {
            for (int filaCluster = 0; filaCluster < NumClusters; filaCluster++)
                {
                    for (int ColumElementos = 0; ColumElementos < NumDatos; ColumElementos++)
                    {
                        Promedio = 0;
                        Numerodedatos = 0;
                        for (int FilaElementos = 0; FilaElementos < NumElementos; FilaElementos++)
                        {
                            if (arrElemntos[filaCluster, FilaElementos] == 1)
                            {
                                Promedio += arrdatos[FilaElementos, ColumElementos];
                                Numerodedatos++;        }  }
                        if(Numerodedatos!=0)
                        clusters[filaCluster, ColumElementos] = Promedio / Numerodedatos;
                    }
                }
            }
            catch (Exception)
            {    
            }
            return clusters;

        }


        public bool CompararOldElementosConNuevosElementos(double[,] arrNuervosElementos, double[,] arrElementos)
        {

            bool sondiferentes = false;
            try
            {
                for (int i = 0; i < NumClusters; i++)
                {
                    for (int j = 0; j < NumElementos; j++)
                    {
                        if (arrElementos[i, j] != arrNuervosElementos[i, j])
                        {
                            sondiferentes = true;
                            cambios = true;
                        }
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error en la comparación de los elementos");
            }

            return sondiferentes;
         }

        public void mostrandoResultados(double[,] arrElemntos, double[,] arrclusters, int numeelem, long numclusters, int numdato,bool iskmeans)
        {
            if (iskmeans==true)
            {
                dataGridView2.Rows.Add("VERSICOLOR " );
                dataGridView2.Rows.Add("SETOSA " );
                dataGridView2.Rows.Add("VIRGINICA " );
            }
            //AGREGAMOS LOS ELEMENTOS A CLUSTERES EN DATAGRIDVIEW
            String elementos = "";
            for (int i = 0; i < numclusters; i++)// numero de clusters
            {

                elementos = "";
                for (int j = 0; j < numeelem; j++)  //numero de elementos
                {
                    if (arrElemntos[i, j] == 1)
                        elementos += "" + dataGridViewdatos.Rows[j].Cells[0].Value + ",";
                }
                if (iskmeans == false)
                {
                    dataGridView2.Rows.Add("Cluster " + (i + 1));
                } 
                if (!elementos.Equals(""))
                {
                    elementos = elementos.Remove(elementos.Length - 1);// eliminamos la ultima coma (,)
                    dataGridView2.Rows[i].Cells[1].Value = elementos;
                }
                else
                {
                    dataGridView2.Rows[i].Cells[1].Value = "Vacio";
                }
            }

 //AGREGAMOS LOS CENTROIDES A CLUSTERES EN DATAGRIDVIEW
             String elementos2 = "";
            for (int i = 0; i < numclusters; i++)// numero de clusters
            {

                elementos2 = "";
                for (int j = 0; j < numdato; j++) ///numero de datos
                {
                    if(arrclusters[i, j]!=0)
                    elementos2 += Math.Round(arrclusters[i, j], 3) + ",";
                }
                
                if (!elementos2.Equals("")) {
                    elementos2 = elementos2.Remove(elementos2.Length - 1);// eliminamos la ultima coma (,)
                    dataGridView2.Rows[i].Cells[2].Value = "("+elementos2+")"; }
                else
                {
                    dataGridView2.Rows[i].Cells[2].Value = "/";
                }

            }
                 }


        public void mostrandoClustersIniciales(double[,] arrclusters, int numeelem, long numclusters, int numdato, bool iskmeans,bool csv)
        {
            if (iskmeans == true)
            {
                dataGridViewCLASTERINICIAL.Rows.Add("VERSICOLOR ");
                dataGridViewCLASTERINICIAL.Rows.Add("SETOSA ");
                dataGridViewCLASTERINICIAL.Rows.Add("VIRGINICA ");
            }

            //AGREGAMOS LOS ELEMENTOS A CLUSTERES EN DATAGRIDVIEW

            for (int i = 0; i < numclusters; i++)// numero de clusters
            {   
                if(iskmeans == false)
                {
                    if (csv) { dataGridViewCLASTERINICIAL.Rows.Add(dataGridViewCLUSTERSCSV.Rows[i].Cells[0].Value); }
                    else { dataGridViewCLASTERINICIAL.Rows.Add("Cluster " + (i + 1)); }
                    
                    dataGridViewCLASTERINICIAL.Rows[i].Cells[1].Value = "Vacío";
                }
                              
            }

            //AGREGAMOS LOS CENTROIDES A CLUSTERES EN DATAGRIDVIEW
            String elementos2 = "";
            for (int i = 0; i < numclusters; i++)// numero de clusters
            {

                elementos2 = "";
                for (int j = 0; j < numdato; j++) ///numero de datos
                {
                    if (arrclusters[i, j] != 0)
                        elementos2 += Math.Round(arrclusters[i, j], 3) + ",";
                }

                if (!elementos2.Equals(""))
                {
             //       elementos2 = elementos2.Remove(elementos2.Length - 1);// eliminamos la ultima coma (,)
                  dataGridViewCLASTERINICIAL.Rows[i].Cells[2].Value = "(" + elementos2 + ")";
                }
                else
                {
                    dataGridViewCLASTERINICIAL.Rows[i].Cells[2].Value = "/";
                }

            }
        }
        public double[] obtenerrangosparadatos(double[,] arrdatos,int numeelem, int numdato)
        {
            double[] rangosdatos = new double[numdato];
            double datomayor = 0;
            try
            {

                for (int columna = 0; columna < numdato; columna++)
                {
                    datomayor = arrdatos[0, columna];
                    for (int fila = 0; fila < numeelem; fila++)
                    {

                        if (arrdatos[fila, columna] > datomayor)
                        {
                            datomayor = arrdatos[fila, columna];

                        }

                    }
                    rangosdatos[columna] = datomayor;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Error en obtención de rangos de datos");

                MessageBox.Show("Error obtención de distancia");

            }
                    return rangosdatos;
        }

        private void llenardatagridespecies()
        {
          //  MessageBox.Show("llenardatagridespecies llenando");
            dataGridViewespecie.Rows.Add("SETOSA", "1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50");
            

            dataGridViewespecie.Rows.Add("VERSICOLOR", "51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100");

            dataGridViewespecie.Rows.Add("VIRGINICA", "101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150");

    
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewdatos.SelectedRows)
                if (!row.IsNewRow)
                {
                    dataGridViewdatos.Rows.Remove(row);
                    dataGridViewexcluidos.Rows.Add(row);
                    MessageBox.Show("Se elimino de Elemntos");
                }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void opciondeclusters()
        {
            
        }

        private void dataGridViewCLUSTERSCSV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void interseccion()
        {
            int numclusters = dataGridViewCLASTERINICIAL.RowCount - 1;
            int clustersposision = 0;
            for(int numClu = 0; numClu < numclusters; numClu++)
            {
                for(int numeroespeciers=0; numeroespeciers<3; numeroespeciers++)
                {
                    dataGridViewOPERACIONES.Rows.Add(dataGridView2.Rows[numClu].Cells[0].Value);

                    if (numeroespeciers == 0) {
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[1].Value = "Setosa";
                        string[] setosa = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50" };

                        //cluster interseccion especie
                        string inte =inter(string_a_Array(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value)), setosa);
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[2].Value = inte;

                        ///cluster-especie
                        string restaCLUmenosESP = resta(string_a_Array(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value)), setosa);
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[3].Value = restaCLUmenosESP;

                        ///especie-cluster
                        string restaESPmenosCLU = resta(setosa, string_a_Array(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value)));
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[4].Value = restaESPmenosCLU;

                        double dist = distanciatarimoto(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value), Convert.ToString(dataGridViewespecie.Rows[numeroespeciers].Cells[1].Value),numerosinter);
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[5].Value = Convert.ToString(dist);


                    }
                    else 
                        if(numeroespeciers == 1) {
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[1].Value = "versicolor";
                        string[] veriscolor = { "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "100"};
                  
                        //cluster interseccion especie
                        string inte = inter(string_a_Array(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value)), veriscolor);
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[2].Value = inte;
                 
                        ///cluster-especie
                        string restaCLUmenosESP = resta(string_a_Array(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value)), veriscolor);
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[3].Value = restaCLUmenosESP;
                 

                        ///especie-cluster
                        string restaESPmenosCLU = resta(veriscolor, string_a_Array(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value)));
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[4].Value = restaESPmenosCLU;

                        double dist = distanciatarimoto(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value), Convert.ToString(dataGridViewespecie.Rows[numeroespeciers].Cells[1].Value), numerosinter);
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[5].Value = Convert.ToString(dist);


                    }
                    else 
                        if(numeroespeciers == 2)
                    {

                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[1].Value = "virginica";
                        string[] virginica = { "101", "102", "103", "104", "105", "106", "107", "108", "109", "110", "111", "112", "113", "114", "115", "116", "117", "118", "119", "120", "121", "122", "123", "124", "125", "126", "127", "128", "129", "130", "131", "132", "133", "134", "135", "136", "137", "138", "139", "140", "141", "142", "143", "144", "145", "146", "147", "148", "149", "150" };

                        //cluster interseccion especie
                        string inte = inter(string_a_Array(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value)), virginica);
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[2].Value = inte;

                        ///cluster-especie
                        string restaCLUmenosESP = resta(string_a_Array(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value)), virginica);
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[3].Value = restaCLUmenosESP;

                        ///especie-cluster
                        string restaESPmenosCLU = resta(virginica, string_a_Array(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value)));
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[4].Value = restaESPmenosCLU;

                        double dist = distanciatarimoto(Convert.ToString(dataGridView2.Rows[numClu].Cells[1].Value), Convert.ToString(dataGridViewespecie.Rows[numeroespeciers].Cells[1].Value), numerosinter);
                        dataGridViewOPERACIONES.Rows[clustersposision].Cells[5].Value = Convert.ToString(dist);

                    }

                    clustersposision++;




                }
            }
        }

        
        public string Array_a_Atring(string[] Arraystr)
        {
            string datos = "";
            for (int i = 0; i < Arraystr.Length; i++)
            {
                datos += Arraystr[i] + ",";

            }

            return datos;
        }


        public string[] string_a_Array(string datos)
        {
            
            // Split datos separado por coma y un espacio.  

            string[] DatosList = datos.Split(',');
            foreach (string dat in DatosList)
                Console.WriteLine(dat);

            return DatosList;

        }

        public string inter(string[] elem1, string[] elem2)
        {
            string result="";
            Form1 llamar = new Form1();
            numerosinter = 0;

              var ShowUnion = elem1.Union(elem2);
            IEnumerable<string> union = elem1.Intersect(elem2);

      
                foreach (string num in union)
            {
                result += num + ",";
                numerosinter++;
            }


            return result;

        }
        public string resta(string[] elem1, string[] elem2)
        {
            string result = "";

            Form1 llamar=new Form1();
            numerospositivos = 0;
            numerosnegativos = 0;
            IEnumerable<string> resta = elem1.Except(elem2);


            foreach (string num in resta)
            {
                result += num + ",";
                numerospositivos++;
            }
            numerosnegativos = numerospositivos - elem1.Length;

            return result;

          

        }

        private void dataGridViewOPERACIONES_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public double distanciatarimoto(string datoscluster, string datosespecie,int numerosinterseccion)
        {
            double distancia = 0;
            double NumeroELemntosCluster = numerodedatos(datoscluster);
           // MessageBox.Show("#DATOS de cluster" + NumeroELemntosCluster);
            double Numeroespecie = numerodedatos(datosespecie);
           // MessageBox.Show("#DATOS de especie" + Numeroespecie);
         
     
             
            double intersec = Convert.ToDouble(numerosinterseccion);
            distancia = ((NumeroELemntosCluster + Numeroespecie) - (2* intersec)) / ((NumeroELemntosCluster + Numeroespecie) - (intersec));


            return distancia;

        }

        public int numerodedatos(string datos)
        {

            // Split datos separado por coma y un espacio.  
            int numerodedatos = 0;
            string[] DatosList = datos.Split(',');

            if (datos != "Vacio")
            {

                foreach (string dat in DatosList)
                    numerodedatos++;
            }
                

            
            

            return numerodedatos;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private double[,] VaciarDatosA_MATRIZDATOS_COMPLETO()
        {
            

            dataGridViewdatos.DataSource = dataGridView1.DataSource;

            int NumElementos,NumDatos;
            NumElementos = (int)dataGridViewdatos.Rows.Count - 1;
            NumDatos = dataGridViewdatos.ColumnCount - 1;
            double[,] datos;
            
            datos = new double[NumElementos,NumDatos];

            for (int fila = 0; fila < NumElementos; fila++)
            {
                
                for (int columna = 0; columna < NumDatos; columna++)
                {
                    //MessageBox.Show(Convert.ToString(dataGridView1.Rows[fila].Cells[columna + 1].Value));
                    datos[fila, columna] = Convert.ToDouble(dataGridViewdatos.Rows[fila].Cells[columna + 1].Value);
                }
               
            }



            return datos;

        }



        private double[,] No_Vaciando_Toddo_El_CSV()
        {
            dataGridViewdatos.Columns.Clear();
            dataGridViewdatos.Rows.Clear();
            dataGridViewdatos.Refresh();
            dataGridViewdatos.Columns.Add("ColunmName", "Id");
            dataGridViewdatos.Columns.Add("ColunmName", "SepalLengthCm");
            dataGridViewdatos.Columns.Add("ColunmName", "SepalWidthCm");
            dataGridViewdatos.Columns.Add("ColunmName", "PetalLengthCm");
            dataGridViewdatos.Columns.Add("ColunmName", "PetalWidthCm");


            dataGridViewexcluidos.Columns.Clear();
            dataGridViewexcluidos.Rows.Clear();
            dataGridViewexcluidos.Refresh();
            dataGridViewexcluidos.Columns.Add("ColunmName", "Id");
            dataGridViewexcluidos.Columns.Add("ColunmName", "SepalLengthCm");
            dataGridViewexcluidos.Columns.Add("ColunmName", "SepalWidthCm");
            dataGridViewexcluidos.Columns.Add("ColunmName", "PetalLengthCm");
            dataGridViewexcluidos.Columns.Add("ColunmName", "PetalWidthCm");


            int NumE, NumD;
            NumE = (int)dataGridViewdatos.Rows.Count - 1;
            NumD = dataGridViewdatos.ColumnCount;
            double[,] datos;
            datos = new double[NumElementos, NumDatos];






            Random random = new Random();



            int[] abc1 = new int[50];//50 es el numero de setosa 
            int[] abc2 = new int[50];//50 es el numero de virginica 
            int[] abc3 = new int[50];//50 es el numero de setosa 

            int indicedatos = 0; //saber en que indice nos quedamos datos a usar
            int indicedatos_excluidos = 0;//saber en que indice nos quedamos datos excluidos

            //50 elementos de setosa
            for (int i = 0; i < 50; i++)abc1[i] = i;
   
            //obtenemos 40 numeros aleatorios del 0 -50
            var arr1 = Enumerable.Range(0, abc1.Length).OrderBy(x => random.Next()).Take(40).ToArray();

            //pasamos los 40 elelemntos aleatorios a datos a usar
            for (int j = 0; j < arr1.Length; j++)
            {
                dataGridViewdatos.Rows.Add();
                for (int x = 0; x < NumD; x++)
                {        dataGridViewdatos.Rows[indicedatos].Cells[x].Value = dataGridView1.Rows[arr1[j]].Cells[x].Value.ToString(); 
                }

                indicedatos++;
            }

           //obtenemos los datos que no se utilizaran 
            var diferentes = abc1.Except(arr1);
            
            //pasamos al datagrisvies de datos exluidos los elelemtos a no utilizar
            foreach (int res in diferentes)
            {
                dataGridViewexcluidos.Rows.Add("");
                for (int x = 0; x < NumD; x++)
                {
                  dataGridViewexcluidos.Rows[indicedatos_excluidos].Cells[x].Value = dataGridView1.Rows[res].Cells[x].Value.ToString();
                }
                indicedatos_excluidos++;
            }

            ////////////////                                  //////////////////
            /*                SEGUNDA                  ESPECIE                  */

            string control = "";
            //50 elementos de VERSICOLOR
            for (int i = 0, j = 50; i < 50; i++, j++)
            {
                abc2[i] = j;
              
            }
           

            //obtenemos 40 numeros aleatorios del 50 -99  suponiendo que indice[50]=51 y indice[99]=100
            var arr2 = Enumerable.Range(50, abc1.Length).OrderBy(x => random.Next()).Take(40).ToArray();

            //pasamos los 40 elelemntos aleatorios a datos a usar
            for (int j = 0; j < arr2.Length; j++)
            {
                dataGridViewdatos.Rows.Add();
                for (int x = 0; x < NumD; x++)
                {
                    dataGridViewdatos.Rows[indicedatos].Cells[x].Value = dataGridView1.Rows[arr2[j]].Cells[x].Value.ToString();
                    
                }
                control += arr2[j] + ",";
                indicedatos++;
            }

            //obtenemos los datos que no se utilizaran 
            var diferentes2 = abc2.Except(arr2);

            //pasamos al datagrisvies de datos exluidos los elelemtos a no utilizar
            foreach (int res in diferentes2)
            {
                dataGridViewexcluidos.Rows.Add("");
                for (int x = 0; x < NumD; x++)
                {
                    dataGridViewexcluidos.Rows[indicedatos_excluidos].Cells[x].Value = dataGridView1.Rows[res].Cells[x].Value.ToString();
                }
                indicedatos_excluidos++;
            }



            ////////////////                                  //////////////////
            /*                TERCERA                  ESPECIE                  */

            
            //50 elementos de VIRGINICA
            for (int i = 0, j = 50; i < 50; i++, j++)
            {
                abc3[i] = j;

            }


            //obtenemos 40 numeros aleatorios del 100 -149  suponiendo que indice[100]=101 y indice[149]=150
            var arr3 = Enumerable.Range(100, abc1.Length).OrderBy(x => random.Next()).Take(40).ToArray();

            //pasamos los 40 elelemntos aleatorios a datos a usar
            for (int j = 0; j < arr3.Length; j++)
            {
                dataGridViewdatos.Rows.Add();
                for (int x = 0; x < NumD; x++)
                {
                    dataGridViewdatos.Rows[indicedatos].Cells[x].Value = dataGridView1.Rows[arr3[j]].Cells[x].Value.ToString();

                }
                control += arr3[j] + ",";
                indicedatos++;
            }

            //obtenemos los datos que no se utilizaran 
            var diferentes3 = abc3.Except(arr3);

            //pasamos al datagrisvies de datos exluidos los elelemtos a no utilizar
            foreach (int res in diferentes3)
            {
                dataGridViewexcluidos.Rows.Add("");
                for (int x = 0; x < NumD; x++)
                {
                    dataGridViewexcluidos.Rows[indicedatos_excluidos].Cells[x].Value = dataGridView1.Rows[res].Cells[x].Value.ToString();
                }
                indicedatos_excluidos++;
            }








            return datos;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewexcluidos.SelectedRows)
                if (!row.IsNewRow)
                {
                    dataGridViewexcluidos.Rows.Remove(row);
                    dataGridViewdatos.Rows.Add(row);
                    MessageBox.Show("Se agrego elemento");
                }
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            VScrollBar vScrollBar1 = new VScrollBar();

            // Dock the scroll bar to the right side of the form.
            vScrollBar1.Dock = DockStyle.Right;

            // Add the scroll bar to the form.
            Controls.Add(vScrollBar1);
        }

        private void dataGridViewexcluidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

   




}

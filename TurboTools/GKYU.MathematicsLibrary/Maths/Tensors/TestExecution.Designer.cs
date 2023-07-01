﻿
namespace GKYU.MathLibrary.Tensors
{
    partial class TestExecution
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartTraining = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.btnXORTest = new System.Windows.Forms.Button();
            this.WorkerThread = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.chartTraining)).BeginInit();
            this.SuspendLayout();
            // 
            // chartTraining
            // 
            chartArea1.Name = "Loss";
            chartArea2.Name = "Metric";
            this.chartTraining.ChartAreas.Add(chartArea1);
            this.chartTraining.ChartAreas.Add(chartArea2);
            legend1.Name = "Legend1";
            this.chartTraining.Legends.Add(legend1);
            this.chartTraining.Location = new System.Drawing.Point(334, 76);
            this.chartTraining.Name = "chartTraining";
            this.chartTraining.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.BorderWidth = 2;
            series1.ChartArea = "Loss";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Loss";
            series2.BorderWidth = 2;
            series2.ChartArea = "Metric";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Metric";
            this.chartTraining.Series.Add(series1);
            this.chartTraining.Series.Add(series2);
            this.chartTraining.Size = new System.Drawing.Size(844, 689);
            this.chartTraining.TabIndex = 0;
            this.chartTraining.Text = "Training Result";
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(0, 76);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(328, 689);
            this.txtConsole.TabIndex = 1;
            // 
            // btnXORTest
            // 
            this.btnXORTest.Location = new System.Drawing.Point(12, 22);
            this.btnXORTest.Name = "btnXORTest";
            this.btnXORTest.Size = new System.Drawing.Size(115, 23);
            this.btnXORTest.TabIndex = 2;
            this.btnXORTest.Text = "Run XOR";
            this.btnXORTest.UseVisualStyleBackColor = true;
            this.btnXORTest.Click += new System.EventHandler(this.BtnXORTest_Click);
            // 
            // WorkerThread
            // 
            this.WorkerThread.WorkerReportsProgress = true;
            this.WorkerThread.WorkerSupportsCancellation = true;
            this.WorkerThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.WorkerThread_DoWork);
            this.WorkerThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.WorkerThread_ProgressChanged);
            this.WorkerThread.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.WorkerThread_RunWorkerCompleted);
            // 
            // TestExecution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 770);
            this.Controls.Add(this.btnXORTest);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.chartTraining);
            this.Name = "TestExecution";
            this.Text = "NNet Executer";
            ((System.ComponentModel.ISupportInitialize)(this.chartTraining)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartTraining;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.Button btnXORTest;
        private System.ComponentModel.BackgroundWorker WorkerThread;
    }
}
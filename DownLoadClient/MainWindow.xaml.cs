// ***********************************************************************
// Assembly         : DownLoadClient
// Author           : S
// Created          : 01-27-2017
//
// Last Modified By : S
// Last Modified On : 01-27-2017
// ***********************************************************************
// <copyright file="MainWindow.xaml.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using winForms = System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Timers;

namespace DownLoadClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {

        /// <summary>
        /// The timer
        /// </summary>
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        /// <summary>
        /// The second
        /// </summary>
        int second = 0;
        /// <summary>
        /// The minute
        /// </summary>
        int minute = 0;
        /// <summary>
        /// The hours
        /// </summary>
        int hours = 0;
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Tick += (TimeOfDownload);
             
        }
        /// <summary>
        /// The path of dounload
        /// </summary>
        private string PathOfDounload;
        /// <summary>
        /// По нажатию Кнопки проверяет есть ли файл с таким именем в указанной дирректории, если есть заменяет файл. Если его нет то скачивает файл.
        /// </summary>
        /// <param name="sender">объект класса object</param>
        /// <param name="e">- объект класса RoutedEventArg, передается событие по нажатию кнопки.</param>
        /// <returns>Функия void ничего не возвращает.</returns>
        /// .
        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            if (IsTrueNameOrPath(textBox.Text, textBox1.Text))
            {
                if (File.Exists(PathOfDounload))
                {
                    File.Delete(PathOfDounload);
                }
                try
                {
                    timer.Start();
                    PathOfDounload = @"" + textBox1.Text + textBox.Text.Substring(textBox.Text.LastIndexOf("/"));
                    label.Content = "Идет загрузка...";
                    WebClient webclient = new WebClient();
                    webclient.DownloadFileAsync(new Uri(textBox.Text),
                      PathOfDounload);
                    webclient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webclient_DownloadProgressChanged);
                    webclient.DownloadFileCompleted += new AsyncCompletedEventHandler(webclient_DownloadFileCompleted);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (checkBox1.IsChecked==true)
                {
                    SetPath.AddPath = textBox1.Text;
                    SetPath setpath = new SetPath();
                    setpath.PathWriter();

                }
            }
        }
        /// <summary>
        /// Заполняет ProgressBar. Отображает информацию по состоянию загрузки файла.
        /// </summary>
        /// <param name="sender">объект класса object</param>
        /// <param name="e">объект класса DownloadProgressChangedEventArgs, передается событие загрузки файла.</param>
        /// <returns>Функия void ничего не возвращает.</returns>
        /// .
        void webclient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                progressBar.Maximum = (int)e.TotalBytesToReceive / 100;
                progressBar.Value = (int)e.BytesReceived / 100;
                persentdownload.Content = "Загружено: " + Convert.ToInt32((double)e.BytesReceived / (double)e.TotalBytesToReceive * 100) + "%";

            } catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        /// <summary>
        /// Выводит информацию о том что файл заагружен.
        /// </summary>
        /// <param name="sender">объект класса object</param>
        /// <param name="e">объект класса  AsyncCompletedEventArgs, передается событие об асинхронной заругке файла, о том что файл загружен.</param>
        /// <returns>Функия void ничего не возвращает.</returns>
        /// .

        void webclient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            timer.Stop();
            label.Content = "Загрузка завершена";
            MessageBox.Show("Файл загружен"+"\nВремя загрузки файла: "+hours+":"+minute+":"+second);
        }
        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                SetPath setPath = new SetPath();
               textBox1.Text= setPath.PathReader();
                
            }
            else
            {
                var pathName = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = pathName.ShowDialog();
                textBox1.Text = pathName.SelectedPath.ToString() + @"\";
            }
            
        }
        /// <summary>
        /// Проверяет правильность запонения TextBox-ов. Правильно ли заполнен путь и название файла.
        /// </summary>
        /// <param name="s1">объект типа string. Передается первая строка textBox</param>
        /// <param name="s2">объект типа string.Передается вторая строка textBox</param>
        /// <returns>Функия bool Возвращает true если все верно запонено, и false если нет..</returns>
        static bool IsTrueNameOrPath(string s1,string s2)
        {
            if (s1.Length < 1)
            {
                MessageBox.Show("Введите ссылку, для скачивания файла");
                    return false;
            }
            else if (s2.Length < 1)
            {
                MessageBox.Show("Выберете папку для хранения файла");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Handles the Click event of the button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void button2_Click(object sender, RoutedEventArgs e)
        { 
            try
            {
                System.Windows.Forms.OpenFileDialog openDownloadDialog = new winForms.OpenFileDialog();
                openDownloadDialog.FileName = textBox1.Text;
                Process.Start(openDownloadDialog.FileName);
            } catch (Exception ex) { MessageBox.Show(ex.Message+"\n Задайте путь к желаемому файлу "); }
        }

        /// <summary>
        /// Handles the Click event of the button3 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        /// <summary>
        /// Расччитыввает время загрузки файла
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <returns>Функия void ничего не возвращает.</returns>

        void TimeOfDownload(object o,EventArgs e)
        {
            second++;
            if (second % 60 == 0)
            {
                second = 00;
                minute++;
                if (minute % 60 == 0)
                {
                    second = 0;
                    minute = 0;
                    hours++;
                }
            }
        }
        /// <summary>
        /// Устанавливает путь куда будет загружаться файл по умолчание в textBox
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        /// <returns>Функия void ничего не возвращает.</returns>

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                SetPath setPath = new SetPath();
                textBox1.Text = setPath.PathReader();

            }
        }
        /// <summary>
        /// Устанавливает путь куда будет загружаться файл по умолчание в textBox
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        /// <returns>Функия void ничего не возвращает.</returns>

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true)
            {
                SetPath.AddPath = textBox1.Text;
                SetPath setpath = new SetPath();
                setpath.PathWriter();

            }
        }
    }
}

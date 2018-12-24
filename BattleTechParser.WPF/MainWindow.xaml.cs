using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BattleTechParser.WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _OriginalCSVFilePath;
        private string _TMProJsonFolderPath;
        private string _BattleTech_DataFolderPath;
        private string _TranslatedCSVFilePath;
        private string _ResultsFolderPath;

        public string OriginalCSVFilePath { get => _OriginalCSVFilePath; set {
                _OriginalCSVFilePath = value;
                OnPropertyChanged("OriginalCSVFilePath");
            } }
        public string TMProJsonFolderPath { get => _TMProJsonFolderPath; set
            {
                _TMProJsonFolderPath = value;
                OnPropertyChanged("TMProJsonFolderPath");
            }
        }
        public string BattleTech_DataFolderPath { get => _BattleTech_DataFolderPath; set
            {
                _BattleTech_DataFolderPath = value;
                OnPropertyChanged("BattleTech_DataFolderPath");
            }
        }
        public string TranslatedCSVFilePath { get => _TranslatedCSVFilePath; set
            {
                _TranslatedCSVFilePath = value;
                OnPropertyChanged("TranslatedCSVFilePath");
            }
        }
        public string ResultsFolderPath { get => _ResultsFolderPath; set
            {
                _ResultsFolderPath = value;
                OnPropertyChanged("ResultsFolderPath");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void BtnOriginalCSVFileSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() {
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*",
                FilterIndex = 1,
            };
            if(openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                if(System.IO.File.Exists(filePath))
                {
                    OriginalCSVFilePath = filePath;
                } else
                {
                    OriginalCSVFilePath = "파일을 찾을 수 없습니다.";
                }
            }

        }

        private void BtnTMProJsonFolderSelect_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                AllowNonFileSystemItems = false,
                IsFolderPicker = true,
                Title = "TMPro.TMP_Dropdown, TMPro.TMP_InputField, TMPro.TextMeshProUGUI 등의 json 파일이 있는 폴더 경로"
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string folderPath = dialog.FileName;
                if (System.IO.Directory.Exists(folderPath))
                {
                    TMProJsonFolderPath = folderPath;
                } else
                {
                    TMProJsonFolderPath = "폴더를 찾을 수 없습니다.";
                }
            }
        }

        private void BtnBattleTech_DataFolderSelect_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                AllowNonFileSystemItems = false,
                IsFolderPicker = true,
                Title = @"BATTLETECH\BattleTech_Data\ 폴더 경로"
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string folderPath = dialog.FileName;
                if (System.IO.Directory.Exists(folderPath))
                {
                    string sADataFolderPath = System.IO.Path.Combine(folderPath, @"StreamingAssets\data\");
                    string GameTipsPath = System.IO.Path.Combine(folderPath, @"StreamingAssets\GameTips\general.txt");
                    string transactionsPath = System.IO.Path.Combine(folderPath, @"StreamingAssets\MDD\data\transactions.sql");
                    string tagdataPath = System.IO.Path.Combine(folderPath, @"StreamingAssets\MDD\data\tagdata.sql");
                    if (System.IO.Directory.Exists(sADataFolderPath) &&
                        System.IO.File.Exists(GameTipsPath) && 
                        System.IO.File.Exists(transactionsPath) &&
                        System.IO.File.Exists(tagdataPath))
                    {
                        BattleTech_DataFolderPath = folderPath;
                    }
                    else
                    {
                        BattleTech_DataFolderPath = "BattleTech_Data 폴더가 아닌 것 같습니다. 경로를 확인하거나 게임 폴더의 내용물을 확인하세요.";
                    }
                }
                else
                {
                    BattleTech_DataFolderPath = "폴더를 찾을 수 없습니다.";
                }
            }
        }

        private void BtnTranslatedCSVFileSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*",
                FilterIndex = 1,
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                if (System.IO.File.Exists(filePath))
                {
                    TranslatedCSVFilePath = filePath;
                }
                else
                {
                    TranslatedCSVFilePath = "파일을 찾을 수 없습니다.";
                }
            }
        }

        private void BtnResultsFolderSelect_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog
            {
                EnsurePathExists = true,
                EnsureFileExists = false,
                AllowNonFileSystemItems = false,
                IsFolderPicker = true,
                Title = "결과물이 저장될 폴더 경로"
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string folderPath = dialog.FileName;
                if (System.IO.Directory.Exists(folderPath))
                {
                    ResultsFolderPath = folderPath;
                }
                else
                {
                    ResultsFolderPath = "폴더를 찾을 수 없습니다.";
                }
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            string sADataFolderPath = "";
            string GameTipsPath = "";
            string transactionsPath = "";
            string tagdataPath = "";
            if (!System.IO.File.Exists(OriginalCSVFilePath))
            {
                OriginalCSVFilePath = "파일을 찾을 수 없습니다.";
                return;
            }
            if (!System.IO.Directory.Exists(TMProJsonFolderPath))
            {
                TMProJsonFolderPath = "폴더를 찾을 수 없습니다.";
                return;
            }
            if (!System.IO.Directory.Exists(BattleTech_DataFolderPath))
            {
                BattleTech_DataFolderPath = "폴더를 찾을 수 없습니다.";
            }
            else
            {
                sADataFolderPath = System.IO.Path.Combine(BattleTech_DataFolderPath, @"StreamingAssets\data\");
                GameTipsPath = System.IO.Path.Combine(BattleTech_DataFolderPath, @"StreamingAssets\GameTips\general.txt");
                transactionsPath = System.IO.Path.Combine(BattleTech_DataFolderPath, @"StreamingAssets\MDD\data\transactions.sql");
                tagdataPath = System.IO.Path.Combine(BattleTech_DataFolderPath, @"StreamingAssets\MDD\data\tagdata.sql");
                if (!(System.IO.Directory.Exists(sADataFolderPath) &&
                    System.IO.File.Exists(GameTipsPath) &&
                    System.IO.File.Exists(transactionsPath) &&
                    System.IO.File.Exists(tagdataPath)))
                {
                    BattleTech_DataFolderPath = "BattleTech_Data 폴더가 아닌 것 같습니다. 경로를 확인하거나 게임 폴더의 내용물을 확인하세요.";
                    return;
                }
            }
            if (!System.IO.File.Exists(TranslatedCSVFilePath))
            {
                TranslatedCSVFilePath = "파일을 찾을 수 없습니다.";
                return;
            }
            if (!System.IO.Directory.Exists(ResultsFolderPath))
            {
                ResultsFolderPath = "폴더를 찾을 수 없습니다.";
                return;
            }

            BackgroundWorker backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
            };
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 200;
            backgroundWorker.DoWork += (object worker, DoWorkEventArgs doWorkEventArgs)=>
            {
                BackgroundWorker localBackgroundWorker = worker as BackgroundWorker;
                
                BattleTechParser.Lib.Parser parserFromOriginalCSV = new Lib.Parser();
                BattleTechParser.Lib.Parser parserFromJsonFiles = new Lib.Parser();
                BattleTechParser.Lib.Parser parserFromTranslatedCSV = new Lib.Parser();
                BattleTechParser.Lib.Parser parserFromGameTips = new Lib.Parser();
                BattleTechParser.Lib.Parser parserFromSQLs = new Lib.Parser();
                parserFromOriginalCSV.AddCSVFromFilePath(OriginalCSVFilePath);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(10);
                parserFromJsonFiles.AddJsonFromFolderPath(TMProJsonFolderPath);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(20);
                parserFromJsonFiles.AddJsonFromFolderPath(sADataFolderPath);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(30);
                parserFromGameTips.AddGameTipsFromFilePath(GameTipsPath);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(40);
                parserFromSQLs.AddSQLFromFilePath(transactionsPath);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(50);
                parserFromSQLs.AddSQLFromFilePath(tagdataPath);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(60);
                BattleTechParser.Lib.Parser parserEnglish = (parserFromJsonFiles + parserFromGameTips + parserFromSQLs);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(70);
                parserFromTranslatedCSV.AddCSVFromFilePath(TranslatedCSVFilePath);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(80);
                BattleTechParser.Lib.Parser mergedParser = parserFromOriginalCSV.Merge(parserEnglish);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(90);
                BattleTechParser.Lib.Parser mergedParser2 = mergedParser.Merge(parserFromTranslatedCSV);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(100);
                string tempCSV = mergedParser2.ToCSVFormat();
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(110);
                string tempCSV2 = parserEnglish.ToCSVFormat();
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(120);
                string tempCSV3 = (parserFromTranslatedCSV - parserFromOriginalCSV).ToCSVFormat();
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(130);
                string tempCSV4 = (parserFromOriginalCSV - parserEnglish - parserFromTranslatedCSV).ToCSVFormat();
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(140);
                string tempCSV5 = (parserEnglish - parserFromOriginalCSV).ToCSVFormat();
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(150);
                System.IO.File.WriteAllText(System.IO.Path.Combine(ResultsFolderPath, "1. strings_fr-FR.csv"), tempCSV);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(160);
                System.IO.File.WriteAllText(System.IO.Path.Combine(ResultsFolderPath, "2. strings_fr-FR.csv"), tempCSV2);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(170);
                System.IO.File.WriteAllText(System.IO.Path.Combine(ResultsFolderPath, "3. strings_fr-FR.csv"), tempCSV3);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(180);
                System.IO.File.WriteAllText(System.IO.Path.Combine(ResultsFolderPath, "4. strings_fr-FR.csv"), tempCSV4);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(190);
                System.IO.File.WriteAllText(System.IO.Path.Combine(ResultsFolderPath, "5. strings_fr-FR.csv"), tempCSV5);
                if (localBackgroundWorker != null) localBackgroundWorker.ReportProgress(200);
            };

            backgroundWorker.ProgressChanged += (object worker, ProgressChangedEventArgs progressChangedEventArgs) =>
            {
                progressBar1.Value = progressChangedEventArgs.ProgressPercentage;
            };

            backgroundWorker.RunWorkerCompleted += (object worker, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs) =>
            {
                if(runWorkerCompletedEventArgs.Error != null)
                {
                    MessageBox.Show("예외가 발생하였습니다. 다음에 표시되는 예외를 개발자에게 전해주세요.", "예외가 발생하였습니다.");
                    MessageBox.Show(runWorkerCompletedEventArgs.Error.Message);
                }
                else if(runWorkerCompletedEventArgs.Cancelled)
                {
                    MessageBox.Show("작업이 취소되었습니다.");
                }else
                {
                    MessageBox.Show("추출이 완료되었습니다.");
                }
            };

            backgroundWorker.RunWorkerAsync();
        }

    }
}

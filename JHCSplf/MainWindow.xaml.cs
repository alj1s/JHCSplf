using com.ibm.as400.access;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using AppSettings = JHCSplf.Properties.Settings;

namespace JHCSplf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ObservableCollection<SpooledFileWrapper> Files
        {
            get { return (ObservableCollection<SpooledFileWrapper>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }

        public static readonly DependencyProperty FilesProperty =
            DependencyProperty.Register("Files", typeof(ObservableCollection<SpooledFileWrapper>), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            LoadSpoolFiles();
            DataContext = this;
        }

        private void LoadSpoolFiles()
        {
            Files = new ObservableCollection<SpooledFileWrapper>();

            var tracey = new AS400(AppSettings.Default.SystemName, AppSettings.Default.Username, AppSettings.Default.Password);
            var list = new SpooledFileList(tracey);
            list.setUserFilter(AppSettings.Default.Username);
            list.openSynchronously();
            var files = list.getObjects();

            while (files.hasMoreElements())
            {
                Files.Add(new SpooledFileWrapper((SpooledFile)files.nextElement()));
            }

        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (splf.SelectedItem != null)
            {
                Show((SpooledFileWrapper)splf.SelectedItem);
            }
        }

        private void Show(SpooledFileWrapper file)
        {
            var window = new Display(file);
            window.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

using com.ibm.as400.access;

using java.io;

namespace JHCSplf
{


    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    public partial class Display
    {
        private readonly List<string> pages = new List<string>();

        public Display(SpooledFileWrapper wrapper)
        {
            GetPages(wrapper.file);
            InitializeComponent();
            InitialiseViewer();
            DataContext = this;

        }

        private void InitialiseViewer()
        {
            var document = new FixedDocument();
            viewer.Document = document;

            foreach (var page in pages)
            {
                var fixedPage = new FixedPage();
                var pageText = new TextBlock { Text = page, FontFamily = new FontFamily("Lucida Console"), TextWrapping = TextWrapping.Wrap };
                fixedPage.Children.Add(pageText);
                var pageContent = new PageContent();
                ((IAddChild)pageContent).AddChild(fixedPage);
                document.Pages.Add(pageContent);
            }
        }

        private void GetPages(SpooledFile file)
        {
            pages.Clear();

            var printParms = new PrintParameterList();
            printParms.setParameter(PrintObject.ATTR_WORKSTATION_CUST_OBJECT, "/QSYS.LIB/QWPDEFAULT.WSCST");
            printParms.setParameter(PrintObject.ATTR_MFGTYPE, "*WSCST");

            var stream = file.getPageInputStream(printParms);
            var reader = new BufferedReader(new InputStreamReader(stream));
            var data = string.Empty;
            var page = new StringBuilder();

            do
            {
                page.Clear();

                while ((data = reader.readLine()) != null)
                {
                    page.Append(data);
                    page.Append(Environment.NewLine);
                }

                pages.Add(page.ToString());

            } while (stream.nextPage());
        }
    }
}

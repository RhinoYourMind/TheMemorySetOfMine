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
using System.Xml;

namespace TreeViewTest
{
    public partial class MainWindow : Window
    {
        List<string[]> datasList = new List<string[]>();

        public MainWindow()
        {
            InitializeComponent();
            TreeViewItemData(out datasList);// 初始化TreeViewItem
            SetDateToTreeViewItem(TreeView1, datasList);
        }

        private void TreeViewItemData(out List<string[]> datasList)// 初始化TreeViewItem
        {

            List<string[]> lists = new List<string[]>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Properties.Resources.TreeViewItemInfo);//读取Xml文件，注意要先把Xml文件加入到Properties下的Resources资源文件中
            XmlNode xRoot = xml.SelectSingleNode("Root");
            XmlNodeList xNlist = xRoot.ChildNodes;
            for (int i = 0; i < xNlist.Count; i++)
            {
                List<string> list = new List<string>();
                XmlNodeList node = xNlist[i].ChildNodes;
                for (int j = 0; j < node.Count; j++)
                {
                    list.Add(node[j].InnerText);
                }
                lists.Add(list.ToArray());
            }
            datasList = lists;
        }//private void TreeViewItemSetData() 初始化TreeViewItem

        private void SetDateToTreeViewItem(TreeView treeView, List<string[]> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                TreeViewItem item = new TreeViewItem();
                item.Margin = new Thickness(5);
                item.FontSize = 12;
                for (int j = 0; j < lists[i].Length; j++)
                {
                    if (j == 0)
                    {
                        item.Header = lists[i][0];
                    }
                    else
                    {
                        item.Items.Add(lists[i][j]);
                    }
                }
                treeView.Items.Add(item);
            }
        }

    }
}

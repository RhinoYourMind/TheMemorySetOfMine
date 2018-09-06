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
            TreeViewItemData(out datasList); // 初始化TreeViewItem
            SetDateToTreeViewItem(TreeView1, datasList);
        }

        private void TreeViewItemData(out List<string[]> datasList) // 初始化TreeViewItem
        {

            List<string[]> lists = new List<string[]>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Properties.Resources.TreeViewItemInfo); //读取Xml文件，注意要先把Xml文件加入到Properties下的Resources资源文件中
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
        } //private void TreeViewItemSetData() 初始化TreeViewItem

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

        private void TreeView1_OnSelected(object sender, RoutedEventArgs e)
        {
            TextBlock1.Text = (e.OriginalSource as TreeViewItem).Header.ToString();
        }


        private void ComboBox1_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            List<string[]> lists = new List<string[]>(datasList);
            List<string[]> lists2 = new List<string[]>();
            if (ComboBox1.Text.Trim() == String.Empty)
            {
                TreeView1.Items.Clear();
                this.SetDateToTreeViewItem(TreeView1, datasList);
                this.CloseAllTreeViewItems(TreeView1);
                TextBlock2.Text = "进到String.Empty";
            }
            else
            {
                char[] cc = ComboBox1.Text.Trim().ToCharArray();
                for (int i = 0; i < lists.Count; i++)
                {
                    List<string> sList = new List<string>();
                    int t = 0;
                    for (int j = 1; j < lists[i].Length; j++)
                    {
                        int count = 0;
                        for (int k = 0; k < cc.Length; k++)
                        {
                            if (lists[i][j].Contains(cc[k]))
                            {
                                count += 1;
                            }
                        }

                        if (count == cc.Length)
                        {
                            if (t == 0)
                            {
                                sList.Add(lists[i][0]);
                                sList.Add(lists[i][j]);
                                t = 1;
                            }
                            else
                            {
                                sList.Add(lists[i][j]);
                            }
                        }
                    }

                    if (sList.Count > 0)
                    {
                        lists2.Add(sList.ToArray());
                    }
                }// for (int i = 0; i < lists.Count; i++)数据处理

                if (lists2.Count > 0)
                {
                    TreeView1.Items.Clear();
                    this.SetDateToTreeViewItem(TreeView1, lists2);
                    this.OpenAllTreeViewItems(TreeView1);
                    TextBlock2.Text = "进入lists2.Count > 0 + 数量：" + (ComboBox1.Text.Trim() == String.Empty);
                    //this.CloseAllTreeViewItems();
                }
                else
                {
                    TreeView1.Items.Clear();
                    TreeViewItem item = new TreeViewItem();
                    item.Margin = new Thickness(5);
                    item.FontSize = 12;
                    item.Header = "搜索无结果";
                    TreeView1.Items.Add(item);
                    TextBlock2.Text = ComboBox1.Text.Trim();
                }
            }//if (ComboBox1.Text.Trim() == String.Empty)的else部分代码
        }// private void ComboBox1_OnTextChanged(object sender, TextChangedEventArgs e)事件函数


        // TreeView全部收缩
        private void CloseAllTreeViewItems(TreeView treeView)
        {
            foreach (var item in treeView.Items)
            {
                DependencyObject dObject = treeView.ItemContainerGenerator.ContainerFromItem(item);
                CollapseTreeviewItems((TreeViewItem)dObject, treeView);
            }
        }

        // TreeView全部展开
        private void OpenAllTreeViewItems(TreeView treeView)
        {
            foreach (var item in treeView.Items)
            {
                DependencyObject dObject = treeView.ItemContainerGenerator.ContainerFromItem(item);
                ((TreeViewItem)dObject).ExpandSubtree();
            }
        }

        // TreeView全部展开和收缩用函数
        private void CollapseTreeviewItems(TreeViewItem items, TreeView treeView)
        {
            items.IsExpanded = false;

            foreach (var item in items.Items)
            {
                DependencyObject dObject = treeView.ItemContainerGenerator.ContainerFromItem(item);

                if (dObject != null)
                {
                    ((TreeViewItem)dObject).IsExpanded = false;

                    if (((TreeViewItem)dObject).HasItems)
                    {
                        CollapseTreeviewItems((TreeViewItem)dObject, treeView);
                    }
                }
            }
        }// private void CollapseTreeviewItems(TreeViewItem items, TreeView treeView)

    }
}

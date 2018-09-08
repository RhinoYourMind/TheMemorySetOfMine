using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TreeViewTest.ClassFiles;

namespace TreeViewTest
{
    public partial class MainWindow : Window
    {
        ObservableCollection<ClassFiles.ListViewDatas> datasListView = new ObservableCollection<ClassFiles.ListViewDatas>();
        List<string[]> datasList = new List<string[]>();
        List<string[]> datasList02 = new List<string[]>();

        public MainWindow()
        {
            InitializeComponent();
            TreeViewItemData(); // 初始化TreeViewItem
            SetDateToTreeViewItem(TreeView1, datasList, datasList02);
            ListView1.ItemsSource = datasListView;
        }

        private void TreeViewItemData() // Xml文件导入数据，数据初始化。
        {

            List<string[]> lists = new List<string[]>();
            List<string[]> lists02 = new List<string[]>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(Properties.Resources.TreeViewItemInfo); //读取Xml文件，注意要先把Xml文件加入到Properties下的Resources资源文件中
            XmlNode xRoot = xml.SelectSingleNode("Root");
            XmlNodeList xNlist = xRoot.ChildNodes;
            for (int i = 0; i < xNlist.Count; i++)
            {
                List<string> list = new List<string>();
                List<string> list02 = new List<string>();
                XmlNodeList node = xNlist[i].ChildNodes;
                list.Add(node[0].InnerText);

                XmlNodeList node2 = node[1].ChildNodes;
                for (int j = 0; j < node2.Count; j++)
                {
                    list.Add(node2[j].ChildNodes[0].InnerText);//数组0为第一个值，数组1为第二个值 
                    list02.Add(node2[j].ChildNodes[1].InnerText);//数组0为第一个值，数组1为第二个值 
                }
                lists.Add(list.ToArray());
                lists02.Add(list02.ToArray());
            }
            this.datasList = lists;
            this.datasList02 = lists02;
        } //private void TreeViewItemSetData()   Xml文件导入数据，数据初始化。

        private void SetDateToTreeViewItem(TreeView treeView, List<string[]> lists, List<string[]> lists02)
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

                        ClassFiles.ListViewDatas listViewDatas = new ListViewDatas();
                        listViewDatas.ImageName = lists[i][j];
                        listViewDatas.ImagePath = "ImageFilses/" + lists[i][j] + ".png";
                        listViewDatas.Ditail = lists02[i][j - 1];
                        datasListView.Add(listViewDatas);
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

            List<string[]> lists3 = new List<string[]>(datasList02);
            List<string[]> lists4 = new List<string[]>();

            if (ComboBox1.Text.Trim() == String.Empty)
            {
                TreeView1.Items.Clear();
                datasListView.Clear();

                this.SetDateToTreeViewItem(TreeView1, datasList, datasList02);
                this.CloseAllTreeViewItems(TreeView1);
                TextBlock2.Text = "进到String.Empty";
            }
            else
            {
                char[] cc = ComboBox1.Text.Trim().ToCharArray();
                for (int i = 0; i < lists.Count; i++)
                {
                    List<string> sList = new List<string>();
                    List<string> sList2 = new List<string>();
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
                            ClassFiles.ListViewDatas listViewDatas = new ListViewDatas();
                            if (t == 0)
                            {
                                sList.Add(lists[i][0]);
                                sList.Add(lists[i][j]);
                                sList2.Add(lists3[i][j - 1]);

                                t = 1;
                            }
                            else
                            {
                                sList.Add(lists[i][j]);
                                sList2.Add(lists3[i][j - 1]);
                            }
                        }
                    }

                    if (sList.Count > 0)
                    {
                        lists2.Add(sList.ToArray());
                        lists4.Add(sList2.ToArray());

                    }
                }// for (int i = 0; i < lists.Count; i++)数据处理

                if (lists2.Count > 0)
                {
                    TreeView1.Items.Clear();
                    datasListView.Clear();

                    this.SetDateToTreeViewItem(TreeView1, lists2, lists4);
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
                    datasListView.Clear();
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

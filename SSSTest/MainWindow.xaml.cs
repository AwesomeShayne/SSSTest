using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

namespace SSSTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static OneDimensionPacker ODP = new OneDimensionPacker();
        static TwoDimensionPacker TDP = new TwoDimensionPacker();
        List<GridBin> Bins2D = new List<GridBin>();
        List<Bitmap> VisualizerImages = new List<Bitmap>();
        int Dimensions;
        int Length;
        int[] Max;
        int[,] Inputs;

        public MainWindow()
        {
            InitializeComponent();
            SolutionMethodComboBox.Items.Add("NextFit");
            SolutionMethodComboBox.Items.Add("HighLowNextFit");
            SolutionMethodComboBox.Items.Add("LowHighNextFit");
            SolutionMethodComboBox.Items.Add("FirstFit");
            SolutionMethodComboBox.Items.Add("HighLowFirstFit");
            SolutionMethodComboBox.Items.Add("LowHighFirstFit");
            SolutionMethodComboBox.Items.Add("WorstFit");
            SolutionMethodComboBox.Items.Add("HighLowWorstFit");
            SolutionMethodComboBox.Items.Add("LowHighWorstFit");
            SolutionMethodComboBox.Items.Add("BestFit");
            SolutionMethodComboBox.Items.Add("HighLowBestFit");
            SolutionMethodComboBox.Items.Add("LowHighBestFit");
            LoadInputFileButton.Click += LoadInputFileButton_Click;
            CalculateSolutionButton.Click += CalculateSolutionButton_Click;
        }

        private void LoadInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                StepBox.Inlines.Add(String.Concat("\nOpened File: ", openFileDialog.FileName));
                GetFileInformation(openFileDialog.FileName);
            }
        }

        private void GetFileInformation(string _FileName)
        {
            var _Inputs = File.ReadAllLines(_FileName);
            Dimensions = (_Inputs[0].Split(',')).Length;
            StepBox.Inlines.Add(String.Concat("\nFile Dimensions: ", Dimensions.ToString()));
            var _DimMax = (_Inputs[0].Split(':')[1].Split(','));
            switch (Dimensions)
            {
                default:
                    Max = new int[1];
                    Max[0] = Convert.ToInt16(_DimMax[0]);
                    StepBox.Inlines.Add(String.Concat("\nMaximum Bin Dimensions: ", Max[0].ToString()));
                    var _Contents = _Inputs[1].Split(':')[1].Split(';');
                    Length = _Contents.Length;
                    StepBox.Inlines.Add(String.Concat("\nNumber of Items: ", Length.ToString()));
                    Inputs = new int[Length, 1];
                    for (int _Index = 0; _Index < Length - 1; _Index++)
                    {
                        Inputs[_Index, 0] = Convert.ToInt32(_Contents[_Index]);
                    }
                    break;
                case 2:
                    Max = new int[2];
                    Max[0] = Convert.ToInt16(_DimMax[0]);
                    Max[1] = Convert.ToInt16(_DimMax[1]);
                    StepBox.Inlines.Add(String.Concat("\nMaximum Bin Dimensions: ", Max[0].ToString(), ",", Max[1].ToString()));
                    Length = Convert.ToInt16((_Inputs[1].Split(':')[1].Split(';')).Length);
                    StepBox.Inlines.Add(String.Concat("\nNumber of Items: ", Length.ToString()));
                    var _FormattedInputs = _Inputs[1].Split(':')[1];
                    var _SeparatedInputs= _FormattedInputs.Split(';');
                    Inputs = new int[Length, 2];
                    for (int _Index = 0; _Index < Length - 1; _Index++)
                    {
                        var _ShapeDims = _SeparatedInputs[_Index].Split(',');
                        Inputs[_Index, 0] = Convert.ToInt32(_ShapeDims[0]);
                        Inputs[_Index, 1] = Convert.ToInt32(_ShapeDims[1]);
                    }
                        break;
                case 3:
                    Max = new int[3];
                    Max[0] = Convert.ToInt16(_DimMax[0]);
                    Max[1] = Convert.ToInt16(_DimMax[1]);
                    Max[2] = Convert.ToInt16(_DimMax[1]);
                    StepBox.Inlines.Add(String.Concat("\nMaximum Bin Dimensions: ", Max[0].ToString(), ",", Max[1].ToString(), ",", Max[2].ToString()));
                    Length = Convert.ToInt16((_Inputs[1].Split(':')[1].Split(';')).Length);
                    StepBox.Inlines.Add(String.Concat("\nNumber of Items: ", Length.ToString()));
                    break;
            }
        }

        private void CalculateSolutionButton_Click(object sender, RoutedEventArgs e)
        {
            string _Method = SolutionMethodComboBox.SelectedItem.ToString();
            switch (Dimensions )
            {
                default:
                    var lines = ODP.Pack(Inputs, Max, _Method);
                    foreach (var line in lines)
                    StepBox.Inlines.Add(line);
                    break;
                case 2:
                    Bins2D = TDP.Pack(Inputs, Max[0], Max[1], _Method);
                    StepBox.Inlines.Add(String.Concat("\nThis test used ", Bins2D.Count, " bins"));
                    VisualizerImages = new List<Bitmap>();
                    foreach (var _Bin in Bins2D)
                    {
                        VisualizerImages.Add(_Bin.BinImage());
                        
                    }
                    StartVisualizer();
                    break;
            }
        }

        private void StartVisualizer()
        {
            StepSlider.Maximum = VisualizerImages.Count - 1;
            StepSlider.Minimum = 0;
            StepSlider.ValueChanged += StepSlider_ValueChanged;
        }

        private void StepSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var _Image = VisualizerImages[(int)e.NewValue];
            var rect = new System.Drawing.Rectangle(0, 0, _Image.Width, _Image.Height);
            var size = (rect.Width * rect.Height) * 4;
            var bitmapData = _Image.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            VisualizedImage.Source = BitmapSource.Create(
                _Image.Width, _Image.Height, _Image.HorizontalResolution, _Image.VerticalResolution, PixelFormats.Bgra32, null, bitmapData.Scan0, size, bitmapData.Stride);
            _Image.UnlockBits(bitmapData);
        }

    }
}

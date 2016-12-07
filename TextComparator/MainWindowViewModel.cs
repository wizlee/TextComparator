using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextComparator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _orginalText;
        private string _modifiedText;

        #region Bind To View
        public string OriginalText
        {
            get
            {
                return _orginalText;
            }

            set
            {
                SetField(ref _orginalText, value);
            }
        }

        public string ModifiedText
        {
            get
            {
                return _modifiedText;
            }

            set
            {
                SetField(ref _modifiedText, value);
            }
        } 

        public void SaveDifferences(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd.Title = "Please Choose a file to save differences";
            bool? dialogResult = sfd.ShowDialog();
            sfd.OverwritePrompt = true;
            if (dialogResult != null && dialogResult == true)
            {
                File.WriteAllText(sfd.FileName, GetDifferences());
            }
        }

        public void SaveSimilarities(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd.Title = "Please Choose a file to save differences";
            bool? dialogResult = sfd.ShowDialog();
            sfd.OverwritePrompt = true;
            if (dialogResult != null && dialogResult == true)
            {
                File.WriteAllText(sfd.FileName, GetSimiliarities());
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region Support Method
        private string GetDifferences()
        {
            StringBuilder sb = new StringBuilder();
            List<string> originalTextLines = OriginalText?.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> modifiedTextLines = ModifiedText?.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (originalTextLines == null)
            {
                originalTextLines = new List<string>();
            }
            if (modifiedTextLines == null)
            {
                modifiedTextLines = new List<string>();
            }
            
            foreach (string originalLine in originalTextLines)
            {
                bool isLineMatch = false;
                foreach (var modifiedLine in modifiedTextLines)
                {
                    if (originalLine == modifiedLine)
                    {
                        isLineMatch = true;
                        break;
                    }
                }
                if (!isLineMatch)
                {
                    sb.AppendLine(originalLine);
                }
            }
            return sb.ToString();
        }

        private string GetSimiliarities()
        {
            StringBuilder sb = new StringBuilder();
            List<string> originalTextLines = OriginalText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> modifiedTextLines = ModifiedText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string originalLine in originalTextLines)
            {
                foreach (var modifiedLine in modifiedTextLines)
                {
                    if (originalLine == modifiedLine)
                    {
                        sb.AppendLine(originalLine);
                        break;
                    }
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
